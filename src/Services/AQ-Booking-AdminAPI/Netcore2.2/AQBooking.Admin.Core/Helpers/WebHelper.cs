using AQBooking.Admin.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;

namespace AQBooking.Admin.Core.Helpers
{
    public class WebHelper
    {
        public static int Server;

        public static void Init(int server)
        {
            Server = server;
        }

        public static bool IsLive()
        {
            return Server == 1;
        }


        public static string GetIPAddress()
        {
            try
            {
                var _accessor = EngineerContext.GetService<IHttpContextAccessor>();
                return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static bool IsImage(string ext)
        {
            var regex = new Regex(@"(.*?)\.(jpg|jpeg|JPG|JPEG|PNG|png)$");
            return regex.IsMatch(ext);
        }

        public static bool IsFile(string ext)
        {
            var regex = new Regex(@"(.*?)\.(ppt|PPTX|xls|XLS|xlsx|XLSX|doc|DOC|docx|DOCX|pdf|PDF)$");
            return regex.IsMatch(ext);
        }

        public static bool IsPDF(string ext)
        {
            var regex = new Regex(@"(.*?)\.(pdf|PDF)$");
            return regex.IsMatch(ext);
        }

        public static bool IsValidFileSize(byte[] fileData, int fileMb)
        {
            return (fileData.Length / 1000000) <= fileMb;
        }

        public static bool IsValidFileSize(int contentLenth, int fileMb)
        {
            return (contentLenth / 1000000) <= fileMb;
        }

        public static string GetContentType(string fileName)
        {
            string contentType = null;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            return contentType;
        }

        public static DateTime ConvertUnixTimeToDate(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


        public static T GetCache<T>(string key, int timeInMinute, Func<T> result)
        {
            var _cache = EngineerContext.GetService<IMemoryCache>();
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
            var _cache = EngineerContext.GetService<IMemoryCache>();
            _cache.Remove(key);
        }
    }
}