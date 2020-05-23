using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.API.Helpers;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    [LogHelper]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtController : ControllerBase
    {
        private readonly IYachtService _yachtService;

        public YachtController(IYachtService yachtService, IPortLocationService portLocationService)
        {
            _yachtService = yachtService;
        }

        [HttpGet]
        [Route("Yachts")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult Search([FromQuery]YachtSearchModel searchModel)
        {
            var result = _yachtService.PrivateCharterSearchStoreProcedure(searchModel);           
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/PrivateCharter")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult PrivateCharter([FromQuery]YachtSearchModel searchModel)
        {
            var result = _yachtService.PrivateCharterSearch(searchModel);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Yachts/YachtsByMerchantId")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetRestaurantsByMerchantFId([FromQuery]SearchYachtWithMerchantIdModel searchModel)
        {
            var result = _yachtService.GetYachtsByMerchantFId(searchModel);
            return Ok(result);
        }

        [HttpPost]
        [Route("Yachts/CharterPrivatePayment/Save/PaymentMethod/{PaymentMethod}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult SaveCharterPrivatePayment(YachtSavePackageServiceModel yachtPackageModel, string PaymentMethod)
        {
            var result = _yachtService.SaveCharterPrivatePayment(yachtPackageModel, PaymentMethod);
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/YachtFindingById/yachtFId/{yachtFId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult YachtFindingById(string yachtFId)
        {
            var result = _yachtService.YachtFindingById(yachtFId);
            return Ok(result);
        }

        //----create by TuanTran
        [HttpGet]
        [Route("Yachts/YachtFindingById2/yachtFId/{yachtFId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult YachtFindingById2(string yachtFId)
        {
            var result = _yachtService.YachtFindingByIdActiveFalse(yachtFId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/YachtAnyNew/showAmount/{showAmount}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetYachtAnyNew(int showAmount)
        {
            var result = _yachtService.GetYachtAnyNew(showAmount);
            return Ok(result);
        }
        [HttpGet]
        [Route("Yachts/GetYachtAnyPromotion/showAmount/{showAmount}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetYachtAnyPromotion(int showAmount)
        {
            var result = _yachtService.GetYachtAnyPromotion(showAmount);
            return Ok(result);
        }
        [HttpGet]
        [Route("Yachts/YachtAnyExclusive")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetYachtAnyExclusive([FromQuery]YachtImageSlideSearchModel searchModel)
        {
            var result = _yachtService.GetYachtAnyExclusive(searchModel);
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/CharterName")]
        public IActionResult GetListYachtName([FromQuery]YachtSearchModel searchModel)
        {
            //LogHelper.InsertLog()
            var result = _yachtService.GetListYachtName(searchModel);
            return Ok(result);
        }

        [HttpGet]
        [Route("YachtsLst")]
        public IActionResult GetlstYachts()
        {
            var result = _yachtService.GetListYacht();
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/GetAllMerchantYachts/{id}")]
        public IActionResult GetAllMerchantYacht(int id)
        {
            var result = _yachtService.GetAllMerchantYacht(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("Yachts/Profile/Booking/List")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetBookingList(YachtbookingRequestModel requestModel)
        {
            var result = _yachtService.GetBookingList(requestModel);
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/Profile/Booking/Detail/charteringId/{charteringId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetBookingDetail(string charteringId)
        {
            var result = _yachtService.GetBookingDetail(charteringId);
            return Ok(result);
        }


        [HttpGet]
        [Route("Yachts/Profile/Booking/uniqueId/{uniqueId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetBookingByCharteringUniqueId(string uniqueId)
        {
            var result = _yachtService.GetBookingByCharteringUniqueId(uniqueId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/CartStorage/{hashKey}/{key}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetYachtCharterCartStorage(string hashKey, string key)
        {
            var result = _yachtService.GetRedisCartStorageAll( hashKey, key);
            return Ok(result);
        }
        [HttpGet]
        [Route("Yachts/CartStorage/Detail/{hashKey}/{key}/{yachtFId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetYachtCharterCartStorageDetail(string hashKey, string key,string yachtFId)
        {
            var result = _yachtService.GetRedisCartStorageDetail(hashKey,key, yachtFId);
            return Ok(result);
        }
        [HttpPost]
        [Route("Yachts/CartStorage/Delete")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult YachtCartDelete(RedisCacheYacthRequestModel requestModel)
        {
            var result = _yachtService.DeleteRedisCartStorage(requestModel);
            return Ok(result);
        }

        [HttpPost("Yachts/CartStorage/TotalFee")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetCartTotalFee(BookingRequestModel requestModel)
        {
            var result = _yachtService.GetRedisCartStorageTotalFee(requestModel);
            return Ok(result);
        }
        #region New Yacht Function
        [HttpGet]
        [Route("Yachts/SimilarYacht")]
     
        public IActionResult SearchSimilarYacht([FromQuery]YachtSimilarSearchModel searchModel)
        {
            var result = _yachtService.YachtSearchSimilarStoreProcedure(searchModel);
            return Ok(result);
        }
        #endregion
    }
}