using AQConfigurations.Infrastructure.Services.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class PortalLanguagesController : ControllerBase
    {
        private readonly IPortalLanguageService _portalLanguageService;

        public PortalLanguagesController(IPortalLanguageService portalLanguageService)
        {
            _portalLanguageService = portalLanguageService;
        }

        /// <summary>
        /// Get List portal language support as List SelectItem
        /// </summary>
        /// <param name="portalUniqueId">uniqueId of Portal</param>
        /// <returns> List portal language support as List SelectItem </returns>
        [HttpGet("PortalLanguages/{portalUniqueId}")]
        public IActionResult GetListLanguageByProtalUID(string portalUniqueId)
        {
            var data = _portalLanguageService.GetLanguagesByPortalUID(portalUniqueId);
            return Ok(data);
        }

        [HttpGet("PortalLanguages/DomainId/{domainId}")]
        public IActionResult GetListLanguageByDomainId(int domainId)
        {
            var data = _portalLanguageService.GetLanguagesByDomainId(domainId);
            return Ok(data);
        }
    }
}