using Identity.Core.Models.JwtToken;

namespace Identity.Core.Helpers
{
    public class JwtTokenBuilderHelper
    {
        private static JwtTokenBuilder _jwtSecurityKeyBuilder;

        public static JwtTokenBuilder DefaultBuilder
        {
            get => _jwtSecurityKeyBuilder != null ? _jwtSecurityKeyBuilder : CreateDefaultBuilder();
            set => _jwtSecurityKeyBuilder = value;
        }

        private static JwtTokenBuilder CreateDefaultBuilder() => new JwtTokenBuilder(JwtTokenOptionHelper.AppSettingOption);
    }
}
