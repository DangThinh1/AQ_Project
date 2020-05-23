using System.Collections.Generic;
using APIHelpers.Response;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtTourPreview;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class PreviewYachtTourDetailController : Controller
    {
        #region
        private readonly IPreviewYachtTourDetail _previewYachtTourDetail;
    #endregion

        #region Ctor
        public PreviewYachtTourDetailController(
            IPreviewYachtTourDetail previewYachtTourDetail)
        {
            _previewYachtTourDetail = previewYachtTourDetail;
        }
        #endregion

        #region Methods
       
        [HttpGet]
        [Route("YachtTourDetails/{tourId}/Langs/{langId}")]
        public IActionResult GetYachtTourDetail(int tourId, int langId)
        {
            var result = _previewYachtTourDetail.GetYachtTourDetail(tourId, langId);
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
            var result = _previewYachtTourDetail.GetYachtTourFileStream(tourId, fileCategoryFid);
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
            var result = _previewYachtTourDetail.GetYachtTourAttribute(tourId);
            var response = new BaseResponse<List<YachtTourAttributeModel>>();
            if (result != null)
            {
                response = BaseResponse<List<YachtTourAttributeModel>>.Success(result);
                return Ok(response);
            }

            return Ok(BaseResponse<List<YachtTourAttributeModel>>.BadRequest());
        }

        [HttpGet]
        [Route("YachtTourYachts/{yachtId}/Langs/{langId}")]
        public IActionResult GetYachtTourYacht(int yachtId, int langId)
        {

            var result = _previewYachtTourDetail.GetYachtTourYacht(yachtId, langId);
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
            var tourPrice = _previewYachtTourDetail.GetYachtTourPrice(tourId, yachtId, pax);
            var response = new BaseResponse<string>();
            response = BaseResponse<string>.Success(tourPrice);
            return Ok(response);
        }

        #endregion
    }
}