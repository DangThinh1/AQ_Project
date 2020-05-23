using Microsoft.AspNetCore.Mvc;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Conts;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class CommonLanguaguesController : ControllerBase
    {
        private readonly ICommonLanguagesServices _commonLanguagesServices;
        public CommonLanguaguesController(ICommonLanguagesServices commonLanguagesServices)
        {
            _commonLanguagesServices = commonLanguagesServices;
        }

        [HttpGet("CommonLanguagues")]
        public IActionResult GetAllLang()
        {
            var res = _commonLanguagesServices.GetAllLanguages();
            return Ok(res);
        }

        [HttpGet("CommonLanguagues/GetAllLanguagues")]
        public IActionResult GetAllLangV2()
        {
            var res = _commonLanguagesServices.GetAllLanguages();
            return Ok(res);
        }


    }
}