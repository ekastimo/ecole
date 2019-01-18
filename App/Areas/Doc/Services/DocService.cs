using System.IO;
using System.Threading.Tasks;
using App.Areas.Doc.Controllers;
using App.Areas.Doc.Repositories;
using App.Areas.Doc.ViewModels;
using AutoMapper;
using Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Areas.Doc.Services
{
    public class DocService : IDocService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDocRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DocController> _logger;
        private readonly string DocStore = "Uploads";

        public DocService(IHttpContextAccessor httpContextAccessor, IDocRepository repository,
            IMapper mapper, ILogger<DocController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DocViewModel> Upload(UploadRequest request)
        {
            var fileData = request.File;
            var document = new Models.Doc
            {
                ContentType = request.File.ContentType,
                Description = request.Details,
                OriginalFileName = request.File.FileName,
                FileName = request.Name,
                SizeInMbs = request.File.Length,
                CreatedBy = _httpContextAccessor.GetUser().userId,
                IsPrimary = request.IsPrimary
            };
            //Save record to db
            var resp = await _repository.CreateAsync(document);
            //create server filename
            var extension = Path.GetExtension(request.File.FileName);
            var filePath = Path.Combine(DocStore, $"{resp.Id}.{extension}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileData.CopyToAsync(stream);
            }

            return _mapper.Map<DocViewModel>(resp);
        }
    }
}