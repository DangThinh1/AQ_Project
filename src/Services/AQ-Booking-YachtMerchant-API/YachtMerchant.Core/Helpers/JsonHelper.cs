using Newtonsoft.Json;


namespace YachtMerchant.Core.Helpers
{
    public static class JsonHelper
    {

        public static T Deserialize<T>(object json)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(json));
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
