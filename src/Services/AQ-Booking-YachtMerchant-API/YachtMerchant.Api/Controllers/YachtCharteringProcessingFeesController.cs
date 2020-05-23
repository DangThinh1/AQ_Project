using System.Threading.Tasks;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtCharteringProcessingFees;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtCharteringProcessingFeesController : ControllerBase
    {
        private readonly IYachtCharteringProcessingFeesService _yachtCharteringProcessingFeesService;
        public YachtCharteringProcessingFeesController(IYachtCharteringProcessingFeesService yachtCharteringProcessingFeesService)
        {
            _yachtCharteringProcessingFeesService = yachtCharteringProcessingFeesService;
            
        }


        /// <summary>
        /// Get infomation of Procesing Fee for Chartering
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringProcessingFees/{id}")]
        public IActionResult GetYachtCharteringProcessingFees(long id)
        {
            var result = _yachtCharteringProcessingFeesService.GetYachtCharteringProcessingFeesByCharteringId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Create new Procesing Fee of Chartering 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtCharteringProcessingFees")]
        public async Task<IActionResult> CreateYachtCharteringProcessingFeesAsync([FromBody]YachtCharteringProcessingFeesCreateModel requestModel)
        {
           
            var result = await _yachtCharteringProcessingFeesService.CreateYachtCharteringProcessingFees(requestModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }


        /// <summary>
        /// Create new Procesing Fee of Chartering  and Change Status Reservation
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtCharteringProcessingFees/CreateYachtCharteringProcessingFeesAndChangeStatusReservation")]
        public async Task<IActionResult> CreateYachtCharteringProcessingFeesAndChangeStatusReservationAsync([FromBody]YachtCharteringProcessingFeeWithChangeStatusReservationCreateModel requestModel)
        {

            var result = await _yachtCharteringProcessingFeesService.CreateYachtCharteringProcessingFeesAndChangeStatusReservationTransaction(requestModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return Ok(result);

        }


        /// <summary>
        /// Update Processing Fee of  Chartering 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtCharteringProcessingFees")]
        public async Task<IActionResult> UpdateYachtCharteringProcessingFeesAsync([FromBody]YachtCharteringProcessingFeesUpdateModel requestModel)
        {
                
            var result = await _yachtCharteringProcessingFeesService.UpdateYachtCharteringProcessingFees(requestModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
                

        }
    }
}