using System;
using System.Security.Claims;
using Identity.Core.Services.Interfaces;
using Identity.Core.Models.Authentications;

namespace Identity.Core.Helpers
{
    public class UserContextHelper
    {
        private static readonly IUsersContext _userContext = IdentityInjectionHelper.GetService<IUsersContext>();

        public static ClaimsIdentity UserClaims
        {
            get
            {
                ThrowIfIUsersContextNull();
                return _userContext.UserClaims;
            }
        }
        public static ClaimsPrincipal UserPrincipal
        {
            get
            {
                ThrowIfIUsersContextNull();
                return _userContext.UserPrincipal;
            }
        }
        public static AuthenticateViewModel UserProfiles
        {
            get
            {
                ThrowIfIUsersContextNull();
                ThrowIfUserProfileNull();
                return _userContext.UserProfiles;
            }
        }
        public static string Email => UserProfiles.Email;
        public static string RoleId => UserProfiles.RoleId;
        public static string NameOfUser => UserProfiles.Name;
        public static string UserRole => UserProfiles.UserRole;
        public static string UserUniqueId => UserProfiles.UniqueId;
        public static Guid UserId => Guid.Parse(UserProfiles.UserId);
        public static string AccessToken => UserProfiles.AccessToken;
        public static string RefreshToken => UserProfiles.RefreshToken;
        public static string AccountTypeFid => UserProfiles.AccountTypeFid;
        public static string GetClaimValue(string key)
        {
            try
            {
                var claim = UserClaims.FindFirst(key);
                return claim != null ? claim.Value : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        } 
        #region Private Methods
        private static void ThrowIfIUsersContextNull()
        {
            if (_userContext == null)
                throw new Exception("IUsersContext is null, please inject IUsersContext interface into the startup.cs");
        }
        private static void ThrowIfUserProfileNull()
        {
            if (_userContext.UserProfiles == null)
                throw new Exception("UserProfile is null");
        }
        #endregion Private Methods
    }
}