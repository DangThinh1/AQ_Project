using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtTourCharters;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourChartersController : ControllerBase
    {
        private readonly IYachtTourCharterService _yachtTourCharterService;
        public YachtTourChartersController(IYachtTourCharterService yachtTourCharterService)
        {
            _yachtTourCharterService = yachtTourCharterService;
        }


        #region Basic method Yacht Chartering


        /// <summary>
        /// Get caculate total reservation of Merchant By MerchantId 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/GetTotalReservationOfMerchantByMerchantId")]
        public IActionResult GetTotalReservationOfMerchantByMerchantId([FromQuery]GetTotalReservationOfMerchantModel model)
        {
            var result = _yachtTourCharterService.CalculateTotalReservationOfMerchantByMerchantId(model);
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
        [Route("YachtTourCharters/GetTotalAmountReservationOfMerchantByMerchantId")]
        public IActionResult GetTotalAmountReservationOfMerchantByMerchantId([FromQuery]GetTotalAmountReservationOfMerchantWithItemTypeModel model)
        {
            var result = _yachtTourCharterService.CalculateTotalAmountReservationOfMerchantByMerchantId(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get infomation of Yacht Tour Charter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/GetInfomationYachtTourCharterById/{id}")]
        public IActionResult GetInfomationYachtTourCharterById(long id)
        {
            var result = _yachtTourCharterService.GetInfomationYachtTourCharterById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Create new Yacht Tour Charter from Origin source AQBooking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtTourCharters/CreateTourCharterFromOriginSource")]
        public async Task<IActionResult> CreateTourCharterFromOriginSourceAsync([FromBody]YachtTourCharterCreateModel model)
        {
            var result = await _yachtTourCharterService.CreateCharterFromOriginSource(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Create new Yacht Tour Charter from Other source (outsite aqbooking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtTourCharters/CreateTourCharterFromOtherSource")]
        public async Task<IActionResult> CreateYachtTourCharterFromOtherSourceAsync([FromBody]CreateTourCharterFromOtherSourceModel model)
        {
            var result = await _yachtTourCharterService.CreateCharterFromOtherSource(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Change status of Yacht Tour Charter reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtTourCharters/ConfirmStatus")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody]YachtTourCharterConfirmStatusModel model)
        {
            var result = await _yachtTourCharterService.UpdateStatusAsync(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Delete Yacht Tour Charter reservation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtTourCharters/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _yachtTourCharterService.DeleteAsync(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        #endregion


        #region  Search Yacht Tour Charter  ===> is Running 


        /// <summary>
        /// Search all Yacht Tour Charter reservation by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/SearchAllCharterPaging")]
        public IActionResult SearchAllCharterPaging([FromQuery]YachtTourCharterSearchPagingModel model)
        {
            var result = _yachtTourCharterService.SearchAllCharterPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }


        /// <summary>
        /// Search all Yacht Tour Charter reservation of Merchant by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/SearchAllYachtTourCharterReservationOfMerchant")]
        public IActionResult SearchAllYachtTourCharterReservationsForMerchant([FromQuery]YachtTourCharterOfMerchantSearchPagingModel model)
        {
            var result = _yachtTourCharterService.SearchAllCharterOfMerchantPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result );
            return BadRequest();

        }


        /// <summary>
        /// Search all Yacht Tour Charter reservation of Tour by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/SearchAllYachtTourCharterReservationOfTour")]
        public IActionResult SearchAllYachtTourChasrterReservationsForTour([FromQuery]YachtTourCharterOfTourSearchPagingModel model)
        {
            var result = _yachtTourCharterService.SearchAllCharterOfTourPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }



        

        
        #endregion



        #region Yacht Tour Charter Detail  ==> Is Running

        /// <summary>
        /// Get detail Yacht Tour Charter reservation No Paging
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/{id}")]
        public IActionResult GetYachtTourCharterReservationDetail(long id)
        {
            var result = _yachtTourCharterService.GetCharterDetail(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get detail Yacht Tour Charter reservation with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/Detail")]
        public IActionResult GetYachtTourCharterReservationDetaiPaging([FromQuery]YachtTourCharterDetailSearchPagingModel model)
        {
            var result = _yachtTourCharterService.GetCharterDetailPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        #endregion


        #region Search Yacht Tour Charter  ===> get all result no paging

        /// <summary>
        /// Get all Yacht Tour Charter reservation by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/GetAllYachtTourCharterReservationByTypeNoPaging")]
        public IActionResult GetAllYachtTourCharterReservationByTypeNoPaging([FromQuery]YachtTourCharterSearchModel model)
        {
            var result = _yachtTourCharterService.GetAllCharterByTypeNoPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get all Yacht Tour Charter reservation of [Merchant] by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/GetAllYachtTourCharterReservationOfMerchantByTypeNoPaging")]
        public IActionResult GetAllYachtTourCharterReservationOfMerchantByTypeNoPaging([FromQuery]YachtTourCharterOfMerchantSearchModel model)
        {
            var result = _yachtTourCharterService.GetAllCharterOfMerchantByTypeNoPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get all Yacht Tour Charter reservation of [Tour] by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/GetAllYachtTourCharterReservationOfTourByTypeNoPaging")]
        public IActionResult GetAllYachtTourCharterReservationOfTourByTypeNoPaging([FromQuery]YachtTourCharterOfTourSearchModel model)
        {
            var result = _yachtTourCharterService.GetAllCharterOfTourByTypeNoPaging(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        #endregion


        #region Show infomation Yacht Tour Charter to home dashboard

        /// <summary>
        /// Show infomation in dashboard about Reservation Info in date in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharters/ShowDashboardReservationInfo/{yachtId}")]
        public IActionResult ShowDashboardReservationInfo([FromQuery]int yachtId)
        {
            var result = _yachtTourCharterService.ShowDashboardReservationInfo(yachtId);
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
        [Route("YachtTourCharters/ShowDashboardRequestProcessReservations/{yachtId}")]
        public IActionResult ShowDashboardRequestProcessReservations([FromQuery]int yachtId)
        {
            var result = _yachtTourCharterService.ShowDashboardRequestProcessReservations(yachtId);
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
        [Route("YachtTourCharters/ShowDashboardRecentPaymentReservations/{yachtId}")]
        public IActionResult ShowDashboardRecentPaymentReservations([FromQuery]int yachtId)
        {
            var result = _yachtTourCharterService.ShowDashboardRecentPaymentReservations(yachtId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        #endregion
    }
}