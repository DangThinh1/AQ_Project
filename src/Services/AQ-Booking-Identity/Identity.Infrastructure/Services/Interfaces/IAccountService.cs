using APIHelpers.Response;
using Identity.Core.Models.Users;
using Identity.Core.Models.Paging;
using Identity.Infrastructure.Database.Entities;
namespace Identity.Infrastructure.Services.Interfaces
{
    public interface IAccountService
    {
        BaseResponse<bool> UpdateUser(Users user);
        BaseResponse<bool> VerifyEmailForCreate(string email);
        BaseResponse<bool> VerifyEmailForSignIn(string email);
        BaseResponse<UserProfileViewModel> GetUserProfile(string key);
        BaseResponse<bool> ChangePassword(string key, string password);
        BaseResponse<bool> UpdateProfile(string key, UserProfileUpdateModel model);
        BaseResponse<PagedList<UserProfileViewModel>> Search(UserSearchModel model);
        BaseResponse<bool> Resigster(UserCreateModel registerModel, string creatorId="");
        BaseResponse<bool> UpdateProperty(string key, string propertyName, object value);
    }
}
