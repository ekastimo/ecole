using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Doc.Repositories;
using App.Areas.Doc.Services;
using App.Areas.Doc.ViewModels;
using AutoMapper;
using Core.Controllers;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Areas.Doc.Controllers
{
    /// <summary>
    /// Documents API
    /// </summary>
    [AreaName("Docs")]
    [Route("api/docs")]
    public class DocController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDocRepository _repository;
        private readonly ILogger<DocController> _logger;
        private readonly IDocService _docService;
        private readonly string DocStore = "Uploads";

        /// <summary>
        /// Controller for addresses
        /// </summary>
        public DocController(IHttpContextAccessor httpContextAccessor, IDocRepository repository,
            IMapper mapper, ILogger<DocController> logger, IDocService docService)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _logger = logger;
            _docService = docService;
            Directory.CreateDirectory(DocStore);
        }

        /// <summary>
        /// Searches files
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<Models.Doc>))]
        public async Task<IEnumerable<Models.Doc>> Search(DocSearchRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"search.docs ${json}");
            var data = (await _repository.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.docs {data.Count}");
            return data;
        }

        /// <summary>
        /// Get Doc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(typeof(IEnumerable<Models.Doc>))]
        public async Task<Models.Doc> Get(Guid id)
        {
            _logger.LogInformation($"download.docs ${id}");
            var data = await _repository.GetByIdAsync(id);
            if (data == null)
                throw new ClientFriendlyException($"Invalid document {id}");
            return data;
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            _logger.LogInformation($"download.docs ${id}");
            var data = await _repository.GetByIdAsync(id);
            if (data == null)
                throw new ClientFriendlyException($"Invalid document {id}");

            var extension = Path.GetExtension(data.OriginalFileName);
            var filePath = Path.Combine(DocStore, $"{data.Id}.{extension}");
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(filePath), data.OriginalFileName);
        }

        /// <summary>
        /// Upload Doc
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ActionResult> Upload(UploadRequest request)
        {
            var resp = await _docService.Upload(request);
            return Ok(new {Message = "Upload Successful", Data = resp});
        }


        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}