using AQBooking.Admin.Core.Models.AuthModel;
using AQBooking.Admin.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace AQBooking.Admin.Infrastructure.Helpers
{
    public class WorkContext : IWorkContext
    {
        private string _token;
        public string _userRole;
        public string _userGuid;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string GET_PROFILE_URL = "api/Accounts/profile";
        private readonly APIExcute _aPIExcute;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            _aPIExcute = new APIExcute(AuthenticationType.Bearer);
        }
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

        public string UserRoleName
        {
            get
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                ClaimsIdentity identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                _userRole = identity.FindFirst(ClaimTypes.Role).Value;
                return _userRole;
            }
        }

        public int UserRoleId
        {
            get
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return 0;
                }

                ClaimsIdentity identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                _userRole = identity.FindFirst("RoleId").Value;
                return Convert.ToInt32(_userRole);
            }
        }

        public string UserRoleGuidId
        {
            get
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                ClaimsIdentity identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                _userRole = identity.FindFirst("RoleGuidId").Value;
                return _userRole;
            }
        }

        public Guid UserGuid
        {
            get
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    _userGuid = "";
                }

                ClaimsIdentity identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                _userGuid = identity.FindFirst("UID").Value;
                return new Guid(_userGuid);
            }
        }
    }
}
