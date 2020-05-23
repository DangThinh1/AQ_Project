using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Core.Models.YachtInformationDetails;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure;
using AQS.BookingMVC.Infrastructure.AQPagination;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Interfaces.Yatch;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Yatch
{
    public class YatchService: ServiceBase, IYatchService
    {
        #region Fields
        private readonly YachtPortalApiUrl _yachtPortalApiUrl;
        private string _baseYatchApiUrl = ApiUrlHelper.YachtPortalApi;
        #endregion

        #region Ctor
        public YatchService(IOptions<YachtPortalApiUrl> yachtPortalApiUrlOption) : base()
        {
            _yachtPortalApiUrl = yachtPortalApiUrlOption.Value;
        }
        #endregion

        #region Methods
        #region Yatch Search
        public async Task<BaseResponse<PagedListClient<YachtSearchItem>>> Search(YachtSearchModel searchModel)
        {
            try
            {
                string url = _baseYatchApiUrl + _yachtPortalApiUrl.Yatchs.Search;
                string paramater = ConvertToUrlParameter(searchModel);
                url = url + paramater;
                var response =await _apiExcute.GetData<PagedListClient<YachtSearchItem>>(url, null);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedListClient<YachtSearchItem>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<List<YachtSimilarItem>>> SearchSimilar(YachtSimilarSearchModel searchModel)
        {
            try
            {
                string url = _baseYatchApiUrl + _yachtPortalApiUrl.Yatchs.SearchSimilar;
                string paramater = ConvertToUrlParameter(searchModel);
                url = url + paramater;
                var response = await _apiExcute.GetData<List<YachtSimilarItem>>(url, null);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtSimilarItem>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion

        #region Yatch Detail
        /// <summary>
        /// Find yatch by id (id already encrypt)
        /// </summary>
        /// <param name="yachtFId">Encrypt yatch id</param>
        /// <returns>BaseResponse Object</returns>
        public async Task<BaseResponse<YachtSingleViewModel>> YachtFindingById(string yachtFId)
        {
            try
            {
                var apiUrl = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.Yatchs.FindingById, yachtFId);
                var response = await base._apiExcute.GetData<YachtSingleViewModel>(url: apiUrl);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtSingleViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Get Yatch Overview
        /// </summary>
        /// <param name="yachtFId">Encrypt yatch id</param>
        /// <param name="lang">Languge id</param>
        /// <returns></returns>
        public async Task<BaseResponse<YachtInformationDetailViewModel>> GetYatchOverview(string yachtFId, int lang)
        {
            try
            {
                var url = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.YachtInformationDetails.YachtInformationDetailByYachtFId, yachtFId, lang);
                var response = await base._apiExcute.GetData<YachtInformationDetailViewModel>(url: url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtInformationDetailViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Get Yatch Detail
        /// </summary>
        /// <param name="yachtFId">Encrypt yatch id</param>
        /// <param name="categoryFId">Encrypt category id</param>
        /// <param name="isInclude"></param>
        /// <param name="attributeName">List attribute name</param>
        /// <returns>Yatch Detail</returns>
        public async Task<BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>> GetYatchDetail(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName)
        {
            try
            {
                var url = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.YachtAttributevalues.CharterGeneral, yachtFId, categoryFId, isInclude);
                var response = await base._apiExcute.GetData<YachtAttributeValueCharterPrivateGeneralViewModel>(url: url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Get Yatch Amenities
        /// </summary>
        /// <param name="yachtFId"></param>
        /// <param name="categoryFId"></param>
        /// <param name="isInclude"></param>
        /// <param name="attributeName"></param>
        /// <returns>Amenities of Yatch</returns>
        public async Task<BaseResponse<List<YachtAttributeValueViewModel>>> GetYatchAmenities(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName)
        {
            try
            {
                var url = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.YachtAttributevalues.CharterPrivate, yachtFId, categoryFId, isInclude);
                var response = await base._apiExcute.GetData<List<YachtAttributeValueViewModel>>(url: url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtAttributeValueViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion

        #region Yatch file stream
        /// <summary>
        /// Get Yacht File Stream
        /// </summary>
        /// <param name="yachtFId">Yatch encrypt id</param>
        /// <param name="fileTypeFId">File type</param>
        /// <returns>List file stream</returns>
        public async Task<BaseResponse<List<YachtFileStreamViewModel>>> GetYachtFileStream(string yachtFId, int fileTypeFId)
        {
            try
            {
                var url = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.YachtFileStreams.FileStream, yachtFId, fileTypeFId);
                var response = await base._apiExcute.GetData<List<YachtFileStreamViewModel>>(url: url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtFileStreamViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }

            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
