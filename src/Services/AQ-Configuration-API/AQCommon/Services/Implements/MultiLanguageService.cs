using System;
using System.Linq;
using Identity.Core.Common;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using AQConfigurations.Core.Helper;
using Microsoft.Extensions.Caching.Memory;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.CommonResources;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AQConfigurations.Core.Services.Implements
{
    public partial class MultiLanguageService : IMultiLanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = ConfigurationsInjectionHelper.GetRequiredService<IHttpContextAccessor>() 
                                                             ?? throw new ArgumentNullException("IHttpContextAccessor is null");
        protected const string DEFAULT_SCHEME = "AQ_RESOURCE_CACHE";
        protected const int DEFAULT_LANGUAGE_ID = 1;
        public int CurrentLaguageId
        {
            get
            {
                try
                {
                    return GetLanguageIdFromClaim() ?? GetLanguageIdFromCookie();
                }
                catch
                {
                    return DEFAULT_LANGUAGE_ID;
                }
            }
        }
        protected int _cacheTimeInMinute { get; set; }
        protected readonly string _cacheScheme;
        protected readonly IMemoryCache _memoryCache;
        protected readonly ICommonResourceRequestService _commonResourceRequestService;
        protected readonly IActionContextAccessor _actionContextAccessor= ConfigurationsInjectionHelper.GetRequiredService<IActionContextAccessor>() 
                                                             ?? throw new ArgumentNullException("IActionContextAccessor is null");

        protected List<CommonResourceViewModel> ResourcePool
        {
            get
            {
                try
                {
                    if (_memoryCache.TryGetValue(_cacheScheme, out List<CommonResourceViewModel> listResourceCache))
                        return listResourceCache;
                    return new List<CommonResourceViewModel>();
                }
                catch
                {
                    return new List<CommonResourceViewModel>();
                }
            }
        }

        public MultiLanguageService(IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            ICommonResourceRequestService commonResourceRequestService,
            int cacheTimeInMinute = 10, string cacheScheme = DEFAULT_SCHEME)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException("IMemoryCache is null");
            _commonResourceRequestService = commonResourceRequestService ?? throw new ArgumentNullException("ICommonResourceRequestService is null");
            _cacheTimeInMinute = cacheTimeInMinute >= 10 ? cacheTimeInMinute : 10;
            _cacheScheme = !string.IsNullOrEmpty(cacheScheme) ? cacheScheme : DEFAULT_SCHEME;
            InitCache();
        }

        #region Private Methods
        private int GetLangIdFromSession()
        {
            try
            {
               
                var langCookieValue = _httpContextAccessor.HttpContext.Session.GetString("lang_id");
                var langId = int.Parse(langCookieValue);
                return langId;
            }
            catch
            {
                return DEFAULT_LANGUAGE_ID;
            }
        }
        private int GetLangIdFromQuery()
        {
            try
            {
                
                var langCookieValue = _httpContextAccessor.HttpContext.Request.Query["lang_id"].ToString();
                var langId = int.Parse(langCookieValue);
                return langId;
            }
            catch
            {
                return GetLangIdFromSession();
            }
        }
        private int GetLanguageIdFromCookie()
        {
            try
            {
               
                var langCookieValue = _httpContextAccessor.HttpContext.Request.Cookies["lang"].ToString();
                var langId = int.Parse(langCookieValue);
                return langId;
            }
            catch
            {
                return GetLangIdFromQuery();
            }
            
               
           
        }

        private int? GetLanguageIdFromClaim()
        {
            try
            {
                return int.Parse(UserContextHelper.GetClaimValue(ClaimConstant.LangId));
            }
            catch
            {
                return null;
            }
        }
        

        protected virtual void InitCache(bool allowClearCache = false)
        {
            if (!_memoryCache.TryGetValue(_cacheScheme, out List<CommonResourceViewModel> listInCache) || allowClearCache)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_cacheTimeInMinute));
                _memoryCache.Set(_cacheScheme, new List<CommonResourceViewModel>(), cacheEntryOptions);
            }
        }
        protected virtual bool IsExisted(string resourceKey, int languageId)
            => ResourcePool.Any(k => k.LanguageFid == languageId && k.ResourceKey.ToUpper() == resourceKey.ToUpper());
        protected virtual void Insert(CommonResourceViewModel model)
        {
            if (!IsExisted(model.ResourceKey, model.LanguageFid))
                ResourcePool.Add(model);
        }
        protected virtual void Update(CommonResourceViewModel model)
        {
            var resourceCache = Find(model.ResourceKey, model.LanguageFid);
            if (resourceCache != null)
            {
                resourceCache.ResourceValue = model.ResourceValue;
            }
        }
        protected virtual CommonResourceViewModel Find(string resourceKey, int languageId)
            => ResourcePool.FirstOrDefault(k => k.LanguageFid == languageId && k.ResourceKey.ToUpper() == resourceKey.ToUpper());
        protected virtual void Delete(string resourceKey, int languageId)
        {
            var resourceCache = Find(resourceKey, languageId);
            if (resourceCache != null)
            {
                ResourcePool.Remove(resourceCache);
            }
        }

        #endregion Private Methods

        #region Public Methods
        public virtual List<CommonResourceViewModel> GetCurrentResources()
        {
            return ResourcePool;
        }
        public void Clear()
        {
            ResourcePool.Clear();
        }

        public virtual string GetResource(string resourceKey)
        {

            if (string.IsNullOrEmpty(resourceKey))
                return string.Empty;

            resourceKey = resourceKey.ToUpper();

            if (IsExisted(resourceKey, CurrentLaguageId)) //=> Get resource from cache
            {
                var resourceValueCache = Find(resourceKey, CurrentLaguageId).ResourceValue;
                return resourceValueCache ?? resourceKey;
            }
            else//=> Load resource from api
            {
                var apiResourceResponse = _commonResourceRequestService.GetResourceValue(CurrentLaguageId, resourceKey);
                if (apiResourceResponse.IsSuccessStatusCode)
                {
                    Insert(new CommonResourceViewModel()
                    {
                        LanguageFid = CurrentLaguageId,
                        ResourceKey = resourceKey,
                        ResourceValue = apiResourceResponse.ResponseData
                    });
                    return apiResourceResponse.ResponseData;
                }

            }
            return resourceKey;
        }

        #endregion Public Methods
    }
}