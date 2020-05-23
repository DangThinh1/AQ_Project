using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtTourCategory;
using AQBooking.YachtPortal.Core.Models.YachtTourCharter;
using AQBooking.YachtPortal.Core.Models.YachtTours;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtTourController : ControllerBase
    {
        #region Fields
        private readonly IYachtTourService _yachtTourService;
        private readonly IYachtTourCategoryService _yachtTourCategoryService;
        #endregion

        #region Ctor
        public YachtTourController(
            IYachtTourService yachtTourService,
            IYachtTourCategoryService yachtTourCategoryService)
        {
            this._yachtTourService = yachtTourService;
            this._yachtTourCategoryService = yachtTourCategoryService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("YachtTours")]
        public IActionResult SearchYachtTour([FromQuery]YachtTourSearchModel model)
        {
            var result = _yachtTourService.SearchYachtTour(model);
            var response = new BaseResponse<IPagedList<YachtTourViewModel>>();
            if (result != null)
            {
                response = BaseResponse<IPagedList<YachtTourViewModel>>.Success(result);
                return Ok(response);
            }

            return Ok(BaseResponse<IPagedList<YachtTourViewModel>>.BadRequest());
        }

        [HttpGet]
        [Route("YachtTourDetails/{tourId}/Langs/{langId}")]
        public IActionResult GetYachtTourDetail(int tourId, int langId)
        {
            var result = _yachtTourService.GetYachtTourDetail(tourId, langId);
            var response = new BaseResponse<YachtTourDetailModel>();
            if (result != null)
            {
                response = BaseResponse<YachtTourDetailModel>.Success(result);
                return Ok(response);
            };

            return Ok(BaseResponse<YachtTourDetailModel>.BadRequest());
        }

        [HttpGet]
        [Route("YachtTourFileStreams/{tourId}/Categories/{fileCategoryFid}")]
        public IActionResult GetYachtTourFileStream(int tourId, int fileCategoryFid)
        {
            var result = _yachtTourService.GetYachtTourFileStream(tourId, fileCategoryFid);
            var response = new BaseResponse<List<YachtTourFileStreamModel>>();
            if (result != null)
            {
                response = BaseResponse<List<YachtTourFileStreamModel>>.Success(result);
                return Ok(response);
            }

            return Ok(BaseResponse<List<YachtTourFileStreamModel>>.BadRequest());
        }

        [HttpGet]
        [Route("YachtTourAttributes/{tourId}")]
        public IActionResult GetYachtTourAttribute(int tourId)
        {
            var result = _yachtTourService.GetYachtTourAttribute(tourId);
            var response = new BaseResponse<List<YachtTourAttributeModel>>();
            if (result != null)
            {
                response = BaseResponse<List<YachtTourAttributeModel>>.Success(result);
                return Ok(response);
            }

            return Ok(BaseResponse<List<YachtTourAttributeModel>>.BadRequest());
        }

        [HttpGet]
        [Route("YachtTourCategories/{langId}")]
        public IActionResult GetYachtTourCategory(int langId)
        {
            var result = _yachtTourCategoryService.GetAllYachtTourCategory(langId);
            var response = new BaseResponse<List<YachtTourCategoryViewModel>>();
            if (result != null)
            {
                response = BaseResponse<List<YachtTourCategoryViewModel>>.Success(result);
                return Ok(response);
            }

            return Ok(BaseResponse<List<YachtTourCategoryViewModel>>.BadRequest());
        }

        [HttpGet]
        [Route("YachtTourYachts/{yachtId}/Langs/{langId}")]
        public IActionResult GetYachtTourYacht(int yachtId, int langId)
        {
            var result = _yachtTourService.GetYachtTourYacht(yachtId, langId);
            var response = new BaseResponse<YachtTourYachtModel>();
            if (result != null)
            {
                response = BaseResponse<YachtTourYachtModel>.Success(result);
                return Ok(response);
            }

            return Ok(BaseResponse<YachtTourYachtModel>.BadRequest());
        }

        [HttpGet]
        [Route("YachtTourPrices/{tourId}/Yachts/{yachtId}/Paxs/{pax}")]
        public IActionResult GetTourPricing(int tourId, int yachtId, int pax)
        {
            var tourPrice = _yachtTourService.GetYachtTourPrice(tourId, yachtId, pax);
            var response = new BaseResponse<YachtTourPriceResultModel>();
            response = BaseResponse<YachtTourPriceResultModel>.Success(tourPrice);
            return Ok(response);
        }

        [HttpGet]
        [Route("YachtTourCharters/{uniqueId}")]
        public IActionResult GetYachtTourCharterByUniqueId(string uniqueId)
        {
            var result = _yachtTourService.GetYachtTourCharterByUniqueId(uniqueId);
            var response = new BaseResponse<YachtTourCharterViewModel>();
            response = BaseResponse<YachtTourCharterViewModel>.Success(result);
            return Ok(response);
        }

        [HttpPost]
        [Route("YachtTourCharters")]
        public IActionResult CreateYachtTourCharter(YachtTourCharterCreateModel model)
        {
            var result = _yachtTourService.CreateYachtTourCharter(model);
            var response = new BaseResponse<YachtTourCharterResultModel>();
            response = BaseResponse<YachtTourCharterResultModel>.Success(result);
            return Ok(response);
        }

        [HttpPut]
        [Route("YachtTourCharters")]
        public IActionResult UpdateYachtTourPayment(YachtTourCharterUpdateModel model)
        {
            var result = _yachtTourService.UpdateYachtTourCharter(model);
            var response = new BaseResponse<bool>();
            response = BaseResponse<bool>.Success(result);
            return Ok(response);
        }
        #endregion
    }
}