using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AQS.BookingMVC.Infrastructure.Extensions
{
    public static class SessionExtension
    {
        public static void SetObject(this ISession session, string key, object value) => session.SetString(key, JsonConvert.SerializeObject(value));
        public static T GetObject<T>(this ISession session, string key) => session.GetString(key) == null ? default(T) : JsonConvert.DeserializeObject<T>(session.GetString(key));
    }
}
