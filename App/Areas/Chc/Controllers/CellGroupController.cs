using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Chc.Models;
using App.Areas.Chc.Repositories;
using App.Areas.Chc.ViewModel;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using App.Areas.Doc.Services;
using App.Areas.Events.Controllers;
using AutoMapper;
using Core.Controllers;
using Core.Extensions;
using Core.Helpers;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Areas.Chc.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// All end-points dealing with contacts 
    /// </summary>
    [AreaName("Chc")]
    [Route("api/chc/cellgroup")]
    public class CellGroupController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICellGroupRepository _repository;
        private readonly ILogger<CellGroupController> _logger;
        private readonly IDocService _docService;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public CellGroupController(IHttpContextAccessor httpContextAccessor, ICellGroupRepository repository, ILogger<CellGroupController> logger,
            IDocService docService,IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _logger = logger;
            _docService = docService;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetch church cellgroups
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<CellGroupViewModel>))]
        public async Task<IEnumerable<CellGroupViewModel>> Create([FromQuery] SearchBase request)
        {
            _logger.LogInformation("fetch.cellgroups");
            var data = await _repository.SearchAsync(request);
            return _mapper.Map<IEnumerable<CellGroupViewModel>>(data);
        }

        /// <summary>
        /// Creates Church CellGroup
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(CellGroupViewModel))]
        public async Task<CellGroupViewModel> Create([FromBody] CellGroupViewModel model)
        {
            _logger.LogInformation("add.cellgroup");
            var toSave = _mapper.Map<CellGroup>(model);
            var saved = await _repository.CreateAsync(toSave);
            _logger.LogInformation($"added.cellgroup {saved.Id}");
            return _mapper.Map<CellGroupViewModel>(saved);
        }

        /// <summary>
        /// Updates Church CellGroup
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(CellGroupViewModel))]
        public async Task<CellGroupViewModel> Update([FromBody] CellGroupViewModel model)
        {
            _logger.LogInformation("update.cellgroup");
            var toSave = _mapper.Map<CellGroup>(model);
            var saved = await _repository.UpdateAsync(toSave);
            _logger.LogInformation($"updated.cellgroup {saved.Id}");
            return _mapper.Map<CellGroupViewModel>(saved);
        }

        /// <summary>
        /// Delete Church CellGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces(typeof(CellGroupViewModel))]
        public async Task<object> Delete([FromRoute] string id)
        {
            _logger.LogInformation($"delete.cellgroup {id}");
            await _repository.DeleteAsync(id);
            _logger.LogInformation($"deleted.cellgroup {id}");
            return new
            {
                Massage="Deleted CellGroup"
            };
        }

    }
}
