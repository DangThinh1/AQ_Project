using System.Threading.Tasks;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtCharterings;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api")]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtCharteringsController : ControllerBase
    {
        private readonly IYachtCharteringService _yachtCharteringService;
        public YachtCharteringsController(IYachtCharteringService yachtCharteringService)
        {
            _yachtCharteringService = yachtCharteringService;
        }


        #region Basic method Yacht Chartering


        /// <summary>
        /// Get caculate total reservation of Merchant By MerchantId 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/GetTotalReservationOfMerchantByMerchantId")]
        public IActionResult GetTotalReservationOfMerchantByMerchantId([FromQuery]GetTotalReservationOfMerchantModel model)
        {
            var result = _yachtCharteringService.CalculateTotalReservationOfMerchantByMerchantId(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get  total amount reseravation for Merchant with  ReservationItemType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/GetTotalAmountReservationOfMerchantByMerchantId")]
        public IActionResult GetTotalAmountReservationOfMerchantByMerchantId([FromQuery]GetTotalAmountReservationOfMerchantWithItemTypeModel model)
        {
            var result = _yachtCharteringService.CalculateTotalAmountReservationOfMerchantByMerchantId(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get infomation of Yacht Chartering by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/GetInfomationYachtChartering/{id}")]
        public IActionResult GetInfomationYachtCharteringById(long id)
        {
            var result = _yachtCharteringService.GetInfomationYachtCharteringById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Create new Yacht Chartering from Origin source AQBooking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtCharterings/CreateCharteringFromOriginSource")]
        public async Task<IActionResult> CreateYachtCharteringFromOriginSourceAsync([FromBody]YachtCharteringsCreateModel model)
        {
            var result = await _yachtCharteringService.CreateCharteringFromOriginSource(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Create new Yacht Chartering from Other source (outsite aqbooking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtCharterings/CreateCharteringFromOtherSource")]
        public async Task<IActionResult> CreateYachtCharteringFromOtherSourceAsync([FromBody]CreateCharteringFromOtherSourceModel model)
        {
            var result = await _yachtCharteringService.CreateCharteringFromOtherSource(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Change status of Yacht Chartering reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtCharterings/ConfirmStatus")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody]YachtCharteringsConfirmStatusModel model)
        {
            var result = await _yachtCharteringService.UpdateStatusAsync(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Delete Yacht Chartering reservation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtCharterings/{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var result = await _yachtCharteringService.DeleteAsync(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        #endregion


        #region  Search Yacht Chartering  ===> is Running 

        /// <summary>
        /// Search all Yacht Chartering reservation of Merchant by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/SearchAllYachtCharteringReservationOfMerchant")]
        public IActionResult SearchAllYachtCharteringReservationsForMerchant([FromQuery]YachtCharteringsOfMerchantSearchPagingModel model)
        {
            var result = _yachtCharteringService.SearchAllYachtCharteringOfMerchantPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }


        /// <summary>
        /// Search all Yacht Chartering reservation of Yacht by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/SearchAllYachtCharteringReservationOfYacht")]
        public IActionResult SearchAllYachtCharteringReservationsForYacht([FromQuery]YachtCharteringsOfYachtSearchPagingModel model)
        {
            var result = _yachtCharteringService.SearchAllYachtCharteringOfYachtPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }



        /// <summary>
        /// Search all Yacht Chartering reservation by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/SearchAllYachtCharteringReservation")]
        public IActionResult SearchAllYachtCharteringReservationsPaging([FromQuery]YachtCharteringsSearchPagingModel model)
        {
            var result = _yachtCharteringService.SearchAllYachtCharteringPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        #endregion



        #region Yacht Chartering Detail  ==> Is Running

        /// <summary>
        /// Get detail Yacht Chartering reservation No Paging
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/{id}")]
        public IActionResult GetYachtCharteringReservationDetail(long id)
        {
            var result = _yachtCharteringService.GetYachtCharteringDetail(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get detail Yacht Chartering reservation with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/Detail")]
        public IActionResult GetYachtCharteringReservationDetail([FromQuery]YachtCharteringsDetailSearchPagingModel model)
        {
            var result = _yachtCharteringService.GetYachtCharteringDetails(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        #endregion


        #region Search Yacht Chartering  ===> old version

        /// <summary>
        /// Get all Yacht Chartering reservation by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/GetAllYachtCharteringReservationByTypeNoPaging")]
        public IActionResult GetAllYachtCharteringReservationByTypeNoPaging([FromQuery]YachtCharteringsSearchModel model)
        {
            var result = _yachtCharteringService.GetAllYachtCharteringByTypeNoPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }


        /// <summary>
        /// Get all Yacht Chartering reservation of Merchant by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/GetAllYachtCharteringReservationOfMerchantByTypeNoPaging")]
        public IActionResult GetAllYachtCharteringReservationOfMerchantByTypeNoPaging([FromQuery]YachtCharteringsOfMerchantSearchModel model)
        {
            var result = _yachtCharteringService.GetAllYachtCharteringOfMerchantByTypeNoPaging(model);

            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get all Yacht Chartering reservation of Yacht by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/GetAllYachtCharteringReservationOfYachtByTypeNoPaging")]
        public IActionResult GetAllYachtCharteringReservationOfYachtByTypeNoPaging([FromQuery]YachtCharteringsOfYachtSearchModel model)
        {
            var result = _yachtCharteringService.GetAllYachtCharteringOfYachtByTypeNoPaging(model);

            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }
        #endregion


        #region Show infomation chartering to home dashboard

        /// <summary>
        /// Show infomation in dashboard about Reservation Info in date in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/ShowDashboardReservationInfo/{yachtId}")]
        public IActionResult ShowDashboardReservationInfo([FromQuery]int yachtId)
        {
            var result = _yachtCharteringService.ShowDashboardReservationInfo(yachtId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        /// <summary>
        /// Show infomation in dashboard about Request process reservation in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/ShowDashboardRequestProcessReservations/{yachtId}")]
        public IActionResult ShowDashboardRequestProcessReservations([FromQuery]int yachtId)
        {
            var result = _yachtCharteringService.ShowDashboardRequestProcessReservations(yachtId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        /// <summary>
        /// Show infomation in dashboard about recent payment reservation in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharterings/ShowDashboardRecentPaymentReservations/{yachtId}")]
        public IActionResult ShowDashboardRecentPaymentReservations([FromQuery]int yachtId)
        {
            var result = _yachtCharteringService.ShowDashboardRecentPaymentReservations(yachtId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        #endregion







    }
}