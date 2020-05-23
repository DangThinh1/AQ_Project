using System;
using System.Threading.Tasks;
using AQBooking.Core.Helpers;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonLanguaguesController : ControllerBase
    {
        private readonly ICommonLanguagesServices _commonLanguagesServices;
        public CommonLanguaguesController(ICommonLanguagesServices commonLanguagesServices)
        {
            _commonLanguagesServices = commonLanguagesServices;
            _commonLanguagesServices.InitController(this);
        }

        [HttpGet]
        [Route("GetAllLanguagues")]
        public async Task<IActionResult> GetAllLang()
        {
            try
            {
                var res = await _commonLanguagesServices.GetAllAsync();
                return Ok(res);                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());

            }
        }
    }
}