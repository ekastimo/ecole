using System;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(500)]
    public class BaseController : Controller
    {
        protected void AssertValidIds(params Guid[] ids)
        {
            foreach (var guid in ids)
            {
                if (guid == Guid.Empty)
                    throw new ClientFriendlyException($"Invalid record id:{guid}");
            }
        }
    }
}
