using AQBooking.Core.Helpers;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.Yacht;
using YachtMerchant.Core.Models.YachtPort;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtController : ControllerBase
    {
        #region Fields

        private readonly IYachtService _yachtService;

        #endregion Fields

        #region Ctor

        public YachtController(
            IYachtService yachtService)
        {
            _yachtService = yachtService;
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// Get Yacht basic profile with Yacht Id
        /// </summary>
        /// <param name="yachtId">yachtId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Yacht/GetBasicProfile/{yachtId}")]
        public  IActionResult GetBasicProfile(int yachtId)
        {
            var result = _yachtService.GetYachtBasicProfile(yachtId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("Yacht")]
        public IActionResult CreateYacht(YachtCreateModel model)
        {
            var result = _yachtService.CreateYacht(model);
            if( result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("Yacht/CheckActiveYacht/{yachtId}")]
        public IActionResult CheckYachtActive(int yachtId)
        {
            
            if (yachtId < 0)
                return BadRequest();
            var result = _yachtService.ActiveYacht(yachtId);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
           
        }

        [HttpPost]
        [Route("Yacht/SetActiveYacht/{yachtId}/{isActiveOperation}")]
        public IActionResult SetYachtActive(int yachtId, bool isActiveOperation)
        {
            if (yachtId < 0)
                return BadRequest();
            var result = _yachtService.SetActiveYacht(yachtId, isActiveOperation);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        [HttpGet]
        [Route("Yacht/GetInfoYacht/{id}")]
        public IActionResult GetInfoYacht(int id)
        {
            if (id < 1)
                return BadRequest();
            var result = _yachtService.GetYachtInfoYacht(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("Yacht")]
        public IActionResult UpdateYachtInfo(YachtUpdateModel model)
        {
            if (model.Id < 0 && model == null)
                return BadRequest();
            var result = _yachtService.UpdateYachtInfo(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        [HttpPut]
        [Route("Yacht/YachtPort")]
        public IActionResult UpdateYachtPort(YachtPortViewModel model)
        {
            if (model.YachtPortId < 1 && model.YachtPortIdNew < 1 && model.YachtPortIdNew < 1)
                return BadRequest();
            var result = _yachtService.UpdateYachtPort(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        #endregion Methods
    }
}