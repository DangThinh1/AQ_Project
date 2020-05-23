using AutoMapper;
using AccommodationMerchant.Infrastructure.Databases;
using Microsoft.AspNetCore.Http;
using AccommodationMerchant.Core.Helpers;
using System;
using System.Linq;
using Identity.Core.Common;
using Identity.Core.Helpers;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class ServiceBase
    {
        protected AccommodationContext _db;
        protected readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ServiceBase(AccommodationContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = DependencyInjectionHelper.GetService<IHttpContextAccessor>();
        }

        protected Guid GetUserGuidId() => UserContextHelper.UserId;

        private string GetUserIdFromHttpContext()
        {
            var claim = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimConstant.UserId);
            var value = claim != null ? claim.Value : string.Empty;
            return value;
        }

        private string GetUserIdFromControllerBase()
        {
            var claim = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimConstant.UserId);
            var value = claim != null ? claim.Value : string.Empty;
            return value;
        }
    }
}