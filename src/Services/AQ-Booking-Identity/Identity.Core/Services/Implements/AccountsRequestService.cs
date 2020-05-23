using System;
using APIHelpers.Response;
using Identity.Core.Models.Users;
using Identity.Core.Models.Paging;
using Identity.Core.Services.Interfaces;

namespace Identity.Core.Services.Implements
{
    public class AccountsRequestService : RequestServiceBase, IAccountsRequestService
    {
        public AccountsRequestService(IUsersContext usersContext) : base(usersContext)
        {
        }

        public BaseResponse<UserProfileViewModel> MyProfile(string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? "api/Accounts/MyProfile" : api,
                };
                var response = Get<UserProfileViewModel>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<UserProfileViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<UserProfileViewModel> UserProfile(string key, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/{key}" : api,
                };
                var response = Get<UserProfileViewModel>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<UserProfileViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<UserProfileViewModel>> AllProfiles(string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts" : api,
                };
                var response = Get<PagedList<UserProfileViewModel>>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<UserProfileViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<UserProfileViewModel>> SearchProfiles(UserSearchModel model,string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/Searching" : api,
                    RequestBody = model
                };
                var response = Post<PagedList<UserProfileViewModel>>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<UserProfileViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Create(UserCreateModel model, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? "api/Accounts" : api,
                    RequestBody = model
                };
                var response = Post<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Register(UserCreateModel model, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? "api/Accounts/Register" : api,
                    RequestBody = model
                };
                var response = Post<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> VerifyEmailForCreate(string email, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/VerifyEmailForCreate/{email}" : api,
                    RequestBody = null
                };
                var response = Get<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> VerifyEmailForSignIn(string email, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/VerifyEmailForsignIn/{email}" : api,
                    RequestBody = null
                };
                var response = Get<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateProfile(string key, UserProfileUpdateModel model, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/{key}" : api,
                    RequestBody = model
                };
                var response = Put<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> ChangeLanguage(string key, int lang, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/{key}/Language/{lang}" : api,
                    RequestBody = null
                };
                var response = Put<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateAvatar(string key, int imageId, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/{key}/Avatar/{imageId}" : api,
                    RequestBody = null
                };
                var response = Put<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> ChangeStatus(string key, bool status, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/{key}/Status/{status}" : api,
                    RequestBody = null
                };
                var response = Put<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> ChangePassword(string key, string password, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/Password/{password}" : api,
                    RequestBody = null
                };
                var response = Put<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(string key, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Accounts/{key}" : api,
                    RequestBody = null
                };
                var response = Delete<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
