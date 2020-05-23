namespace Identity.Core.Models.JwtToken
{
    public sealed class JwtTokenBuilderParams
    {
        public string SecurityKey;
        public string Issuer;
        public string Audience;
        public JwtTokenBuilderParams(JwtTokenOption option)
        {
            SecurityKey = option.SecurityKey;
            Issuer = option.Issuer;
            Audience = option.Audience;
        }
        public JwtTokenBuilderParams(string securityKey, string issuer, string audience)
        {
            SecurityKey = securityKey;
            Issuer = issuer;
            Audience = audience;
        }
    }
}
