using Newtonsoft.Json;

namespace Identity.Core.Models.Authentications
{
    public class FacebookAuthenticateModel
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonProperty("id")]
        public string UserId { get; set; }
    }
}
