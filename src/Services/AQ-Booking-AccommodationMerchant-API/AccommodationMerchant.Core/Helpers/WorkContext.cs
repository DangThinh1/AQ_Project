using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Linq;
using Identity.Core.Common;

namespace AccommodationMerchant.Core.Helpers
{
    public class WorkContext:IWorkContext
    {
        private string _token;
        public string _userRole;
        public string _userGuid;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }


        public string UserToken
        {
            get
            {
                if (_token != null)
                    return _token;

                if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated==false)
                    return string.Empty;

                ClaimsIdentity identity = _httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;

                var claim = identity?.Claims?.FirstOrDefault(c => c.Type == ClaimConstant.AccessToken);

                _token = claim?.Value??string.Empty;

                return _token;
            }
        }


        public string UserRole
        {
            get
            {
                if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ==false)
                    _userRole = string.Empty;

                ClaimsIdentity identity = _httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;

                var claim = identity?.Claims?.FirstOrDefault(c => c.Type == ClaimConstant.RoleId);

                _userGuid =claim?.Value??string.Empty;

                return _userRole;
            }
        }

        public Guid UserGuid
        {
            get
            {
                if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated==false)
                    _userGuid = string.Empty;

                ClaimsIdentity identity = _httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;

                var claim = identity?.Claims?.FirstOrDefault(c => c.Type == ClaimConstant.UserId);

                _userGuid = claim?.Value??string.Empty;

                if(!string.IsNullOrEmpty(_userGuid))
                    return new Guid(_userGuid);
                return new Guid();
            }
        }
    }
}
