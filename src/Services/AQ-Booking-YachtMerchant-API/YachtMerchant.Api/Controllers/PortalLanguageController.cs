using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortalLanguageController : ControllerBase
    {
        private readonly IPortalLanguageService _portalLanguageService;
        public PortalLanguageController(IPortalLanguageService portalLanguageService)
        {
            _portalLanguageService = portalLanguageService;
            _portalLanguageService.InitController(this);
            
        }

        /// <summary>
        /// API support get all language support for Portal,
        /// API allow anonymous access
        /// </summary>
        /// <param name="id">Id of domain portal support both Id number and Unique Id </param>
        /// <returns>List language support with Text and Value </returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllLanguageSupportByPortalId/{id}")]
        public IActionResult GetAllLanguageSupportByPortalId(string id)
        {
            var data = _portalLanguageService.GetAllLanguageSupportByPortalId(id);
            if (data.Count > 0)
                return Ok(data);
            else
                return NoContent();
        }
    }
}