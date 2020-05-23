using StackExchange.Redis;
using System;

namespace Identity.Core.Portal.Helpers
{
    public static class RedisCacheHelper
    {
        public static ConnectionMultiplexer ConnectToRedisServer(string host, string port)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
                return null;
            try
            {
                return ConnectionMultiplexer.Connect($"{host}:{port}");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
