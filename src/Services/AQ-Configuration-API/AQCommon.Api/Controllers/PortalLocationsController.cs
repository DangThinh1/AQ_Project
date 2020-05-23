using Microsoft.AspNetCore.Mvc;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Conts;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class PortalLocationsController : ControllerBase
    {
        private readonly IPortalLocationControlService _portalLocationControlService;

        public PortalLocationsController(IPortalLocationControlService portalLocationControlService)
        {
            _portalLocationControlService = portalLocationControlService;
        }
        
        [HttpGet("PortalLocations/PortalUniqueId/{portalUniqueId}")]        
        public IActionResult GetLocationsByPortalUniqueId(string portalUniqueId)
        {
            var result = _portalLocationControlService.GetLocationsByPortalUniqueId(portalUniqueId);
            return Ok(result);
        }

        [HttpGet("PortalLocations/PortalUniqueId/{portalUniqueId}/CountryCode/{countryCode}")]
        public IActionResult GetLocationsByPortalUniqueIdAndCountryCode(string portalUniqueId, int countryCode)
        {
            var result = _portalLocationControlService.GetLocationsByPortalUniqueIdAndCountryCode(portalUniqueId, countryCode);
            return Ok(result);
        }

        [HttpGet("PortalLocations/PortalUniqueId/{portalUniqueId}/CountryName/{countryName}")]
        public IActionResult GetLocationsByPortalUniqueIdAndCountryName(string portalUniqueId, string countryName)
        {
            var result = _portalLocationControlService.GetLocationsByPortalUniqueIdAndCountryName(portalUniqueId, countryName);
            return Ok(result);
        }
    }
}