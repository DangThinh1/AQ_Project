using Newtonsoft.Json;
using AQConfigurations.Core.Helper;
using Microsoft.Extensions.Configuration;

namespace AQConfigurations.Core.Models.AppSettings.Helpers
{
    public class AppSettingHelper
    {
        private static readonly IConfiguration _configuration = ConfigurationsInjectionHelper.GetRequiredService<IConfiguration>();
        public static T GetValue<T>(string name) where T : struct
        {
            try
            {
                var value = _configuration.GetSection(name).Value;
                var result = JsonConvert.DeserializeObject<T>(value);
                return result;
            }
            catch
            {
                return default(T);
            }
        }
        public static string GetValue(string name)
        {
            try
            {
                var value = _configuration.GetSection(name).Value;
                return value;
            }
            catch
            {
                return null;
            }
        }
    }
} 