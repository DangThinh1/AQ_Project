using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtMerchantUsers;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtMerchantUsersService
    {

        /// <summary>
        /// This is method use get infomation detail of  user for merchant control 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<YachtMerchantUsersViewModel> GetInfomationOfMerchantUserById(int id);


        /// <summary>
        /// This is method use get all user of merchant control by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtMerchantUsersViewModel>> GetAllUserOfMerchantByMerchantId(int merchantId);

        /// <summary>
        /// This is method use get dropdlownlist  user of merchant control by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<SelectListItem>> GetDropdownListUserOfMerchantByMerchantId(int merchantId);

        /// <summary>
        /// This is method use get all user of merchant control by Roles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtMerchantUsersViewModel>> GetAllUserOfMerchantByRole(YachtMerchantUsersRequestGetAllUserWithRolesOfMerchantModel model);

        /// <summary>
        /// This is method use create new user of Merchant 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateYachtMerchantUser(YachtMerchantUsersCreateModel model);

        /// <summary>
        /// This is method use update infomation of User YachtMerchantUsers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateYachtMerchantUsers(YachtMerchantUsersUpdateModel model);


        /// <summary>
        /// This is method use  when want delete YachtMerchantUsers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteYachtMerchantUsers(int id);
    }
}
