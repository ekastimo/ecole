﻿using System;
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
    [Route("api/chc/location")]
    public class LocationController:BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocationRepository _repository;
        private readonly ILogger<LocationController> _logger;
        private readonly IDocService _docService;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public LocationController(IHttpContextAccessor httpContextAccessor, ILocationRepository repository, ILogger<LocationController> logger,
            IDocService docService,IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _logger = logger;
            _docService = docService;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetch church locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<LocationViewModel>))]
        public async Task<IEnumerable<LocationViewModel>> Create([FromQuery] SearchBase request)
        {
            _logger.LogInformation("fetch.locations");
            var data = await _repository.SearchAsync(request);
            return _mapper.Map<IEnumerable<LocationViewModel>>(data);
        }

        /// <summary>
        /// Creates Church Location
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(LocationViewModel))]
        public async Task<LocationViewModel> Create([FromBody] LocationViewModel model)
        {
            _logger.LogInformation("add.location");
            var toSave = _mapper.Map<Location>(model);
            var saved = await _repository.CreateAsync(toSave);
            _logger.LogInformation($"added.location {saved.Id}");
            return _mapper.Map<LocationViewModel>(saved);
        }

        /// <summary>
        /// Updates Church Location
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(LocationViewModel))]
        public async Task<LocationViewModel> Update([FromBody] LocationViewModel model)
        {
            Tk.AssertValidIds(model.Id);
            _logger.LogInformation("update.location");
            var toSave = _mapper.Map<Location>(model);
            var saved = await _repository.UpdateAsync(toSave);
            _logger.LogInformation($"updated.location {saved.Id}");
            return _mapper.Map<LocationViewModel>(saved);
        }

        /// <summary>
        /// Delete Church Location
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces(typeof(LocationViewModel))]
        public async Task<object> Delete([FromRoute] Guid id)
        {
            _logger.LogInformation($"delete.location {id}");
            await _repository.DeleteAsync(id);
            _logger.LogInformation($"deleted.location {id}");
            return new
            {
                Massage="Deleted Location"
            };
        }

    }
}