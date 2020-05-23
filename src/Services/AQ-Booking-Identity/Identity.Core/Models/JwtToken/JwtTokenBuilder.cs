using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Core.Models.JwtToken
{
    public sealed class JwtTokenBuilder
    {
        private readonly JwtTokenBuilderParams _jwtSecurityKeyBuilderParams;

        #region Ctor
        public JwtTokenBuilder(string securityKey, string issuer, string audience)
        {
            _jwtSecurityKeyBuilderParams = new JwtTokenBuilderParams(securityKey, issuer, audience);
        }
        public JwtTokenBuilder(JwtTokenOption jwtTokenOption)
        {
            _jwtSecurityKeyBuilderParams = new JwtTokenBuilderParams(jwtTokenOption);
        }
        public JwtTokenBuilder(JwtTokenBuilderParams param)
        {
            _jwtSecurityKeyBuilderParams = param;
        }

        #endregion

        #region Method
        public string GetIssuer()
        {
            return _jwtSecurityKeyBuilderParams.Issuer;
        }
        public string GetAudience()
        {
            return _jwtSecurityKeyBuilderParams.Audience;
        }
        public string GetSecurityKey()
        {
            return _jwtSecurityKeyBuilderParams.SecurityKey;
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
        #endregion
    }
}
