using AQBooking.Core.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace YachtMerchant.Core.Helpers
{
    public class WebHelperCustomize : WebHelper
    {

        public static T GetCache<T>(string key, int timeInMinute, Func<T> result)
        {
            var _cache = DependencyInjectionHelper.GetService<IMemoryCache>();
            T cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = result();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(timeInMinute));

                // Save data in cache.
                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }
            return cacheEntry;
        }
        public static void ClearCached(string key)
        {
            var _cache = DependencyInjectionHelper.GetService<IMemoryCache>();
            _cache.Remove(key);
        }

    }
}
