using System;
using System.Collections.Generic;
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
    public class CommonResourcesController : ControllerBase
    {
        private readonly ICommonResourcesPortalServices _commonResourcesServices;
        public CommonResourcesController(ICommonResourcesPortalServices commonResourcesServices)
        {
            _commonResourcesServices = commonResourcesServices;
            _commonResourcesServices.InitController(this);
        }


        [HttpPost]
        [Route("GetListResourcers/{langId}")]
        public async Task<IActionResult> GetListAsync(int langId,[FromBody] List<string> type = null)
        {
            try
            {
                var resultResourcesLst = await  _commonResourcesServices.GetAllResource(langId, type);
                return Ok(resultResourcesLst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
    }
}