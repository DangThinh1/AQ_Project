using AQBooking.Admin.Core.Models.AuthModel;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingAdmin.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AQS.BookingMVC.Services
{
    public class WorkContext : IWorkContext
    {

        private UserInfoModel _currentUser =null;
        private string _token;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;

        public WorkContext(IHttpContextAccessor httpContextAccessor,
            IAuthService authService
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }      

      

        public UserInfoModel CurrentUser =>GetCurrentUser();

        public bool IsAuthentication => CurrentUser!=null;
        public string UserToken
        {
            get

            {
                if (_token != null)
                    return _token;

                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                ClaimsIdentity identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                _token = identity.FindFirst(ClaimTypes.Rsa).Value;
                return _token;
            }
        }


        private UserInfoModel GetCurrentUser()
        {
            if (_currentUser != null)
                return _currentUser;
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;
            _currentUser = _authService.GetUser(UserToken).Result;
            return _currentUser;
        }
       

       
    }
}
