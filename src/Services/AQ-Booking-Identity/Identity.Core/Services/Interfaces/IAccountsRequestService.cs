using APIHelpers.Response;
using Identity.Core.Models.Users;
using Identity.Core.Models.Paging;

namespace Identity.Core.Services.Interfaces
{
    public interface IAccountsRequestService : IRequestServiceBase
    {
        BaseResponse<UserProfileViewModel> MyProfile(string host = "", string api = "");
        BaseResponse<UserProfileViewModel> UserProfile(string key, string host = "", string api = "");
        BaseResponse<PagedList<UserProfileViewModel>> AllProfiles(string host = "", string api = "");
        BaseResponse<PagedList<UserProfileViewModel>> SearchProfiles(UserSearchModel model, string host = "", string api = "");
        BaseResponse<bool> Create(UserCreateModel model, string host = "", string api = "");
        BaseResponse<bool> Register(UserCreateModel model, string host = "", string api = "");
        BaseResponse<bool> UpdateProfile(string key, UserProfileUpdateModel model, string host = "", string api = "");
        BaseResponse<bool> ChangeLanguage(string key, int lang, string host = "", string api = "");
        BaseResponse<bool> UpdateAvatar(string key, int imageId, string host = "", string api = "");
        BaseResponse<bool> ChangeStatus(string key, bool status, string host = "", string api = "");
        BaseResponse<bool> ChangePassword(string key, string password, string host = "", string api = "");
        BaseResponse<bool> Delete(string key, string host = "", string api = "");
        BaseResponse<bool> VerifyEmailForCreate(string email, string host = "", string api = "");
        BaseResponse<bool> VerifyEmailForSignIn(string email, string host = "", string api = "");
    }
}
