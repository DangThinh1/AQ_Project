using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtTourCharterProcessingFees;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourCharterProcessingFeesController : ControllerBase
    {
        private readonly IYachtTourCharterProcessingFeesService _yachtTourCharterProcessingFeesService;
        public YachtTourCharterProcessingFeesController(IYachtTourCharterProcessingFeesService yachtTourCharterProcessingFeesService)
        {
            _yachtTourCharterProcessingFeesService = yachtTourCharterProcessingFeesService;
        }


        /// <summary>
        /// Get infomation of Procesing Fee for Charter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharterProcessingFees/{id}")]
        public IActionResult GetYachtTourCharterProcessingFees(long id)
        {
            var result = _yachtTourCharterProcessingFeesService.GetYachtTourCharterProcessingFeesByCharterId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Create new Procesing Fee of Charter 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtTourCharterProcessingFees")]
        public async Task<IActionResult> CreateYachtTourCharterProcessingFeesAsync([FromBody]YachtTourCharterProcessingFeesCreateModel requestModel)
        {
            var result = await _yachtTourCharterProcessingFeesService.CreateYachtTourCharterProcessingFees(requestModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Create new Procesing Fee of Charter and Change Status Reservation
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtTourCharterProcessingFees/CreateYachtTourCharterProcessingFeesAndChangeStatusReservation")]
        public async Task<IActionResult> CreateYachtTourCharterProcessingFeesAndChangeStatusReservationAsync([FromBody]YachtTourCharterProcessingFeeWithChangeStatusReservationCreateModel requestModel)
        {
            var result = await _yachtTourCharterProcessingFeesService.CreateYachtTourCharterProcessingFeesAndChangeStatusReservationTransaction(requestModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Update Processing Fee of  Chartering 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtTourCharterProcessingFees")]
        public async Task<IActionResult> UpdateYachtTourCharterProcessingFeesAsync([FromBody]YachtTourCharterProcessingFeesUpdateModel requestModel)
        {
            var result = await _yachtTourCharterProcessingFeesService.UpdateYachtTourCharterProcessingFees(requestModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }


    

}