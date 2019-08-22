using System.Threading.Tasks;
using App.Areas.Chc.Repositories;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using App.Areas.Doc.Services;
using Core.Controllers;
using Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Areas.Crm.Controllers
{
    [AreaName("Crm")]
    [Route("api/crm/person")]
    public class MetaDataController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _logger;
        private readonly IDocService _docService;
        private readonly ILocationRepository _locationRepository;
        private readonly ICellGroupRepository _cellGroupRepository;

        public MetaDataController(IHttpContextAccessor httpContextAccessor, IContactService contactService, ILogger<ContactController> logger,
            IDocService docService, ILocationRepository locationRepository, ICellGroupRepository cellGroupRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _contactService = contactService;
            _logger = logger;
            _docService = docService;
            _locationRepository = locationRepository;
            _cellGroupRepository = cellGroupRepository;
        }

        /// <summary>
        /// Update Location of a contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("chc")]
        [Produces(typeof(ContactChcViewModel))]
        public async Task<ContactChcViewModel> ContactLocation([FromBody] ContactChcViewModel model)
        {
            _logger.LogInformation("update.contact.chc");
            var data = await _contactService.UpdateChcInformation(model);
            _logger.LogInformation($"updated..contact.chc {data.ContactId}");
            return data;
        }
    }
}
