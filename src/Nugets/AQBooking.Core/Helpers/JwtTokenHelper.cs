using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Core.Helpers
{
    public class JwtTokenHelper
    {
        private static IConfiguration _configuration = null;

        public static void Init(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetSecurityKey()
        {
            if (_configuration != null)
            {
                return _configuration.GetValue<string>("Credential:SecurityKey");
            }
            throw new NullReferenceException("Jwt SecurityKey can not be null or empty");
        }

        public static string GetIssuer()
        {
            if (_configuration != null)
            {
                return _configuration.GetValue<string>("Credential:Issuer");
            }
            throw new NullReferenceException("Jwt Issuer can not be null or empty");
        }

        public static string GetAudience()
        {
            if (_configuration != null)
            {
                return _configuration.GetValue<string>("Credential:Audience");
            }
            throw new NullReferenceException("Jwt Audience can not be null or empty");
        }

        public static SigningCredentials GetSigningCredentials()
        {
            var result = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            return result;
        }

        public static byte[] GetSymmetricSecurityKeyAsBytes()
        {
            var issuerSigningKey = GetSecurityKey();
            byte[] data = Encoding.UTF8.GetBytes(issuerSigningKey);
            return data;
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            byte[] data = GetSymmetricSecurityKeyAsBytes();
            var result = new SymmetricSecurityKey(data);
            return result;
        }

        public static string GetCorsOrigins()
        {
            string result = "";
            return result;
        }

        public static TokenValidationParameters GetValidationParameters()
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

    }
}
