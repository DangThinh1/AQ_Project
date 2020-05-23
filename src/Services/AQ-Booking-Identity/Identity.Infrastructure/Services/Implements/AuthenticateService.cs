                            using System;
using System.Net.Http;
using Newtonsoft.Json;
using APIHelpers.Response;
using Identity.Core.Helpers;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Identity.Infrastructure.Helpers;
using Identity.Infrastructure.Database;
using Identity.Core.Models.Authentications;
using Identity.Infrastructure.Database.Entities;
using Identity.Infrastructure.Services.Interfaces;
using Identity.Core.Enums;
using AutoMapper;

namespace Identity.Infrastructure.Services.Implements
{
    public class AuthenticateService : IdentityServiceBase, IAuthenticateService
    {
        private const string FACEBOOK_GET_USER_API_URL = "https://graph.facebook.com/{0}?fields=name,last_name,first_name,email&access_token={1}";
        private const string FACEBOOK_BASE_URL = "https://graph.facebook.com/v4.0/";
        private readonly int TOKEN_MINUTE_TIME_LIFE = 6000000; //=> TODO: Helper Get Time Life on app setting
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AuthenticateService(IAccountService accountService, IdentityDbContext db, UserManager<Users> userManager, IMapper mapper) : base(db, userManager)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        //For AQ Account
        private AuthenticateViewModel GetAuthResponseModel(Users user)
        {
            try 
            {
                if (user == null)
                    return null;
                var authModel = _mapper.Map<Users, AuthenticateViewModel>(user);
                var refreshToken = UniqueIDHelper.GenarateRandomString(12);
                user.RefreshToken = refreshToken;
                var saveSucceed = _accountService.UpdateUser(user).IsSuccessStatusCode;
                if (saveSucceed)
                {
                    authModel.RefreshToken = refreshToken;
                    authModel.TokenEffectiveDate = DateTime.Now.ToString();
                    authModel.TokenEffectiveTimeStick = DateTime.Now.Ticks.ToString();
                    authModel.AccountTypeFid = ((int)AccountTypeEnum.Admin).ToString();
                    var result = JwtTokenHelper.GenerateJwtTokenModel(authModel, TOKEN_MINUTE_TIME_LIFE);
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        public BaseResponse<AuthenticateViewModel> AuthenticateByEmail(string email, string password)
        {
            try
            {
                var user = LoadRelated(FindByEmail(email));
                if (user == null)
                    return BaseResponse<AuthenticateViewModel>.NotFound();
                if (IsNotDeleted(user) && IsActivated(user) && IsValidated(user, password))
                {
                    var authResponseModel = GetAuthResponseModel(user);
                    if(authResponseModel != null)
                        return BaseResponse<AuthenticateViewModel>.Success(authResponseModel);
                }

                return BaseResponse<AuthenticateViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<AuthenticateViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<AuthenticateViewModel> AuthenticateByFaceBook(string facebookAccessToken, string userId)
        {
            try
            {
                if(string.IsNullOrEmpty(facebookAccessToken) || string.IsNullOrEmpty(userId))
                    return BaseResponse<AuthenticateViewModel>.BadRequest();
                var facebookGetUserInfoApi = string.Format(FACEBOOK_GET_USER_API_URL, userId, facebookAccessToken);
                var httpClient = new HttpClient();
                httpClient = new HttpClient
                {
                    BaseAddress = new Uri(FACEBOOK_BASE_URL)
                };
                httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.GetAsync(facebookGetUserInfoApi).Result;
                if (!response.IsSuccessStatusCode)
                    return BaseResponse<AuthenticateViewModel>.BadRequest();
                var result = response.Content.ReadAsStringAsync().Result;
                var facebookAuthModel = JsonConvert.DeserializeObject<FacebookAuthenticateModel>(result);
                var syncSucceed = SyncFacebookUserProfileIfNotExisted(facebookAuthModel);
                if(syncSucceed)
                {
                    var user = LoadRelated(FindByEmail(facebookAuthModel.Email)); ;
                    if (user == null)
                        return BaseResponse<AuthenticateViewModel>.NotFound();
                    var authResponseModel = GetAuthResponseModel(user);
                    if (authResponseModel != null)
                        return BaseResponse<AuthenticateViewModel>.Success(authResponseModel);
                }
                return BaseResponse<AuthenticateViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<AuthenticateViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<AuthenticateViewModel> AuthenticateByGoogle(string googleAccessToken)
        {
            try
            {
                return BaseResponse<AuthenticateViewModel>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<AuthenticateViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<AuthenticateViewModel> AuthenticateByWechat(string wechatAccessToken)
        {
            throw new NotImplementedException();
        }
    }
}
