using Identity.Core.Models.JwtToken;
using Microsoft.Extensions.Options;

namespace Identity.Core.Helpers
{
    public class JwtTokenOptionHelper
    {
        public static JwtTokenOption AppSettingOption => GetAppSettingOption() != null ? GetAppSettingOption().Value : null;

        private static IOptions<JwtTokenOption> GetAppSettingOption()
        {
            try
            {
                var test = IdentityInjectionHelper.GetService<IOptions<JwtTokenOption>>();
                return IdentityInjectionHelper.GetService<IOptions<JwtTokenOption>>();
            }
            catch
            {
                return null;
            }
        }
    }
}
