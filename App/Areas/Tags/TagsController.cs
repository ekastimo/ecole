using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Controllers;
using Core.Extensions;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Areas.Tags
{
    /// <summary>
    /// Tags Controller
    /// </summary>
    [AreaName("Tags")]
    [Route("api/tags")]
    public class TagsController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITagRepository _repository;
        private readonly ILogger<TagsController> _logger;

        public TagsController(
            IHttpContextAccessor httpContextAccessor, 
            ITagRepository repository,
            ILogger<TagsController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Searches tags
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<Tag>))]
        public async Task<IEnumerable<Tag>> Search(TagSearchRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"tags.by.query ${json}");
            var data = (await _repository.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.tags {data.Count}");
            return data;
        }

        /// <summary>
        /// Create a tag
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(Tag))]
        public async Task<Tag> Create([FromBody] Tag model)
        {
            model.CreatedBy = _httpContextAccessor.GetContactId();
            _logger.LogInformation($"create.tag ${model.Name}");
            var data = await _repository.CreateAsync(model);
            _logger.LogInformation($"created.tag ${data.Id}");
            return data;
        }

        /// <summary>
        /// Update a tag
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(Tag))]
        public async Task<Tag> Update([FromBody] Tag model)
        {
            _logger.LogInformation($"update.tag ${model.Id}");
            Tk.AssertValidIds(model.Id);
            var data = await _repository.UpdateAsync(model);
            _logger.LogInformation($"updated.tag ${data.Id}");
            return data;
        }

        /// <summary>
        /// Delete a tag
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(Guid id)
        {
            _logger.LogInformation($"delete.tag ${id}");
            Tk.AssertValidIds(id);
            var data = await _repository.DeleteAsync(id);
            _logger.LogInformation($"delete.tag ${id} result: {data}");
            return new
            {
                Message = "Operation successful"
            };
        }
    }
}
