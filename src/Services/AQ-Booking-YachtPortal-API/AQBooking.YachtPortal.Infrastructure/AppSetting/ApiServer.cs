using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.AppSetting
{
    public class ApiServer
    {
        public int Server { get; set; }
        public Server ConfigurationApi { get; set; }
        public Server ConnectionString { get; set; }
        public RedisCacheSrv RedisCacheSrv { get; set; }
    }

    public class Server
    {
        public string Server_LOCAL { get; set; }
        public string Server_VN { get; set; }
        public string Server_BETA { get; set; }
        public string Server_LIVE { get; set; }
    }

    public class RedisCacheSrv
    {
        public RedisCache Server_LOCAL { get; set; }
        public RedisCache Server_VN { get; set; }
        public RedisCache Server_BETA { get; set; }
        public RedisCache Server_LIVE { get; set; }

        public class RedisCache
        {
            public string Host { get; set; }
            public string Port { get; set; }
            public string InstanceName { get; set; }
            public string Password { get; set; }
        }

    }
}
