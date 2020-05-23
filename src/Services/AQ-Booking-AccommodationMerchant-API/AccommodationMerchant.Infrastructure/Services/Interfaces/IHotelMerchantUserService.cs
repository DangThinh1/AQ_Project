using AccommodationMerchant.Core.Models.HotelMerchantUsers;
using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelMerchantUserService
    {

        /// <summary>
        /// This is method use get infomation detail of  user for merchant control 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<HotelMerchantUserViewModel> GetInfomationOfMerchantUserById(int id);


        /// <summary>
        /// This is method use get all user of merchant control by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<HotelMerchantUserViewModel>> GetAllUserOfMerchantByMerchantId(int merchantId);

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
        BaseResponse<List<HotelMerchantUserViewModel>> GetAllUserOfMerchantByRole(HotelMerchantUserRequestGetAllUserWithRolesOfMerchantModel model);

        /// <summary>
        /// This is method use create new user of Merchant 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateMerchantUser(HotelMerchantUserCreateModel model);

        /// <summary>
        /// This is method use update infomation of User HotelMerchantUsers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateMerchantUser(HotelMerchantUserUpdateModel model);


        /// <summary>
        /// This is method use  when want delete HotelMerchantUsers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteMerchantUser(int id);
    }
}
