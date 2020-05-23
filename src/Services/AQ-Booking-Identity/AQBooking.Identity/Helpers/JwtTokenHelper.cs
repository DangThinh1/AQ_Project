using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AQBooking.Identity.Helpers
{
    public class JwtTokenHelper
    {
        private string _securityKey = "sd4OIfg+KJH9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        private string _issuer = "AQIdentityServer";

        private string _audience = "AQIdentityServer";
        
        public JwtTokenHelper(string securityKey, string issuer, string audience)
        {
            _securityKey = securityKey;
            _issuer = issuer;
            _audience = audience;
        }

        public string GetIssuer()
        {
            return _issuer;
        }
        public string GetAudience()
        {
            return _audience;
        }
        public string GetSecurityKey()
        {
            return _securityKey;
        }
        public SigningCredentials GetSigningCredentials()
        {
            var result = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            return result;
        }
        public byte[] GetSymmetricSecurityKeyAsBytes()
        {
            var issuerSigningKey = GetSecurityKey();
            byte[] data = Encoding.UTF8.GetBytes(issuerSigningKey);
            return data;
        }
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            byte[] data = GetSymmetricSecurityKeyAsBytes();
            var result = new SymmetricSecurityKey(data);
            return result;
        }
        public string GetCorsOrigins()
        {
            string result = "";
            return result;
        }
        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = GetIssuer(),
                ValidAudience = GetAudience(),
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
        public TokenValidationParameters GetValidationRefreshTokenParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = GetIssuer(),
                ValidAudience = GetAudience(),
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }
}
