namespace Identity.Core.Models.JwtToken
{
    public sealed class JwtTokenOption
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}