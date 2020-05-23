using AccommodationMerchant.Core.Models.HotelMerchants;
using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelMerchantService
    {

        /// <summary>
        /// Get Merchant UniqueId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MerchantUniqueId</returns>
        Task<BaseResponse<string>> GetMerchantUniqueID(int id);
        /// <summary>
        /// Get infomation basic of Merchant with Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        BaseResponse<HotelMerchantBasicInfoModel> GetMerchantBasicInfoByMerchantId(int merchantId);

        
        /// <summary>
        /// Get all merchant of user have role Accommodation Account Manager ( AAM ) with UserId
        /// </summary>
        /// <param name="userId">B7A7A1E2-7F34-43E2-BE21-746F2271ECEC</param>
        /// <returns></returns>
        BaseResponse<List<Merchant>> GetListMerchantOfAccommodationAccountManager(string userId);

        /// <summary>
        /// Get Merchant of user have role Accommodation Merchant ( AM ) with UserId
        /// </summary>
        /// <param name="userId">B7A7A1E2-7F34-43E2-BE21-746F2271ECEC</param>
        /// <returns></returns>
        BaseResponse<Merchant> GetMerchantOfUserRoleAccommodationMerchant(string userId);

        /// <summary>
        /// Get all Hotel of Merchant 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        BaseResponse<List<SelectListItem>> GetListHotelOfMerchant(int merchantId);

        /// <summary>
        /// Get MerchantId by HotelId
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns>MerchantId</returns>
        Task<BaseResponse<int>> GetMerchantIdByHotelId(int hotelId);

    }
}
