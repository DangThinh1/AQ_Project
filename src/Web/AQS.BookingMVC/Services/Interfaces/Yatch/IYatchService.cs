using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Core.Models.YachtInformationDetails;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure;
using AQS.BookingMVC.Infrastructure.AQPagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Yatch
{
    public interface IYatchService
    {
        #region Yatch Search
        /// <summary>
        /// Search Yactch 
        /// </summary>
        /// <param name="searchModel">Search parameter model</param>
        /// <param name="apiUrl">ApiUrl</param>
        /// <returns></returns>
        Task<BaseResponse<PagedListClient<YachtSearchItem>>> Search(YachtSearchModel searchModel);
        Task<BaseResponse<List<YachtSimilarItem>>> SearchSimilar(YachtSimilarSearchModel searchModel);

        #endregion

        #region Yatch Detail
        /// <summary>
        /// Find yatch with Id
        /// </summary>
        /// <param name="yachtFId">Yatch encrypt id</param>
        /// <returns>Yacht Single View Model</returns>
        Task<BaseResponse<YachtSingleViewModel>> YachtFindingById(string yachtFId);

        /// <summary>
        /// Get Yatch Detail
        /// </summary>
        /// <param name="yachtFId"></param>
        /// <param name="categoryFId"></param>
        /// <param name="isInclude"></param>
        /// <param name="attributeName"></param>
        /// <returns>Return yatch detail</returns>
        Task<BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>> GetYatchDetail(
            string yachtFId,
            int categoryFId,
            bool isInclude,
            List<string> attributeName);

        /// <summary>
        /// Get yatch overview
        /// </summary>
        /// <param name="yachtFId">Yatch encrypt id</param>
        /// <param name="langugeId">Languge id</param>
        /// <returns>Yatch overview</returns>
        Task<BaseResponse<YachtInformationDetailViewModel>> GetYatchOverview(string yachtFId, int langugeId);
        #endregion

        /// <summary>
        /// Get Yatch Amenities
        /// </summary>
        /// <param name="yachtFId"></param>
        /// <param name="categoryFId"></param>
        /// <param name="isInclude"></param>
        /// <param name="attributeName"></param>
        /// <returns>Amenities of Yatch</returns>
        Task<BaseResponse<List<YachtAttributeValueViewModel>>> GetYatchAmenities(
            string yachtFId,
            int categoryFId,
            bool isInclude,
            List<string> attributeName);

        #region Yatch file stream
        /// <summary>
        /// Get Yacht File Stream
        /// </summary>
        /// <param name="yachtFId">Yatch encrypt id</param>
        /// <param name="fileTypeFId">File type</param>
        /// <returns>List file stream</returns>
        Task<BaseResponse<List<YachtFileStreamViewModel>>> GetYachtFileStream(string yachtFId, int fileTypeFId);

        /// <summary>
        /// Get File Stream Paging
        /// </summary>
        /// <param name="searchModel">Search Model</param>
        /// <returns>List file stream</returns>
        //Task<BaseResponse<PagedListClient<YachtFileStreamViewModel>>> GetFileStreamPaging(YachtFileStreamSearchModel searchModel);
        #endregion
    }
}
