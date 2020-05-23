using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Identity.Core.Common;
using YachtMerchant.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using YachtMerchant.Core.Helpers;

namespace YachtMerchant.Infrastructure.Services
{
    public class ServiceBase
    {
        protected readonly YachtOperatorDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected ControllerBase _controller;

        public ServiceBase(YachtOperatorDbContext context)
        {
            _context = context;
            _httpContextAccessor = DependencyInjectionHelper.GetService<IHttpContextAccessor>();
        }


        protected Guid GetUserGuidId() => new Guid(GetUserIdFromHttpContext() ?? GetUserIdFromControllerBase());

        private string GetUserIdFromHttpContext()
        {
            
            try
            {
                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.UserId);
                var value = claim != null ? claim.Value : string.Empty;
                return value;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetUserIdFromControllerBase()
        {
            
            try
            {
                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.UserId);
                var value = claim != null ? claim.Value : string.Empty;
                return value;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
