using APIHelpers.Response;
using AQBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.DTO;
using YachtMerchant.Core.Models.YachtMerchant;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtMerchantService
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
        BaseResponse<YachtMerchantBasicInfoModel> GetMerchantBasicInfoByMerchantId(int merchantId);

        /// <summary>
        /// Get all Yacht Merchant
        /// </summary>
        /// <returns></returns>
        BaseResponse<List<Merchant>> GetListYachtMerchants();


        /// <summary>
        /// Get all merchant of user have role Yacht Account Manager ( YAM ) with UserId
        /// </summary>
        /// <param name="yamId">B7A7A1E2-7F34-43E2-BE21-746F2271ECEC</param>
        /// <returns></returns>
        BaseResponse<List<Merchant>> GetListMerchantOfYachtAccountManager(string yamId);

        /// <summary>
        /// Get Merchant of user have role Yacht Merchant ( YM ) with UserId
        /// </summary>
        /// <param name="userId">B7A7A1E2-7F34-43E2-BE21-746F2271ECEC</param>
        /// <returns></returns>
        BaseResponse<Merchant> GetMerchantOfUserRoleYachtMerchant(string userId);

        /// <summary>
        /// Get all Yacht of Merchant 
        /// </summary>
        /// <param name="MerchantId"></param>
        /// <returns></returns>
        BaseResponse<List<DTODropdownItem>> GetListYachtOfMerchant(int merchantId);

        /// <summary>
        /// Get MerchantId by YachtId
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns>MerchantId</returns>
        Task<BaseResponse<int>> GetMerchantIdByYachtId(int yachtId);

        BaseResponse<bool> UpdateLandingPage(YachtMerchantViewModel model);
        BaseResponse<List<DTODropdownItem>> GetListYachtActiveForOperationOfMerchant(int merchantId);
    }
}
