using System;
using System.Threading.Tasks;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtNonBusinessDay;
using YachtMerchant.Infrastructure.Interfaces;
using Identity.Core.Conts;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtNonBusinessDayController : ControllerBase
    {
        private readonly IYachtNonBusinessDaysService _yachtNonBusinessDaysService;
        public YachtNonBusinessDayController(IYachtNonBusinessDaysService yachtNonBusinessDaysService)
        {
            _yachtNonBusinessDaysService = yachtNonBusinessDaysService;
        }

        /// <summary>
        /// Create Non Bussiness Day 
        /// </summary>
        /// <param name="model">YachtNonBusinessDayCreateModel object to create</param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtNonOperationDay")]
        public async Task<IActionResult> CreateAsync([FromBody] YachtNonBusinessDayCreateModel model)
        {
            if (model == null || model.StartDate==null  || model.EndDate==null)
                return BadRequest();
            var result =  await _yachtNonBusinessDaysService.CreateAsync(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        [HttpPut]
        [Route("YachtNonOperationDay")]
        public IActionResult Update(YachtNonBusinessDayUpdateModel model)
        {
            if (model == null || model.YachtFid < 0 || model.StartDate == null || model.EndDate == null)
                return BadRequest();
            var result =  _yachtNonBusinessDaysService.Update(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        /// <summary>
        /// Create range of Non Business Day 
        /// </summary>
        /// <param name="createModel">YachtNonBusinessDayCreateRangeModel object to create</param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtNonOperationDays")]
        public IActionResult CreateRangeAsync([FromBody]YachtNonBusinessDayCreateRangeModel createModel)
        {
            if (createModel == null || string.IsNullOrEmpty(createModel.NonBusinessDay))
                return BadRequest();
            var result = _yachtNonBusinessDaysService.CreateRangeAsync(createModel).Result;
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        


        /// <summary>
        /// Get Yacht non business day information by Yacht Id
        /// </summary>
        /// <param name="id">Yacht id in int format</param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtNonOperationDays/{id}")]
        public IActionResult GetNonBusinessDaysByYachtID(int id)
        {
            var result = _yachtNonBusinessDaysService.GetNonBusinessDaysByYachtID(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);

            return BadRequest();
            
        }

        [HttpGet]
        [Route("YachtNonOperationDays/GetYachtNonBusinessDay/{id}")]
        public IActionResult GetYachtNonBusinessDaysByID(int id)
        {
            var result = _yachtNonBusinessDaysService.GetYactNonBusinessDayById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);

            return BadRequest();
           
        }

        /// <summary>
        /// Delete Yacht non business day information by id
        /// </summary>
        /// <param name="id">Non business day id in int format</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtNonOperationDays/{id}")]
        public IActionResult DeleteNonBusinessDayByIdAsync(int id)
        {
            var result =  _yachtNonBusinessDaysService.DeleteNonBusinessDayByIdAsync(id).Result;
            if (result.IsSuccessStatusCode)
                return Ok(result);

            return BadRequest();
           
        }
    }
}