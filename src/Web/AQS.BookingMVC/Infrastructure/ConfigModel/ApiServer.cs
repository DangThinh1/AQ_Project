using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.ConfigModel
{
    public class ApiServer
    {
        public int Server { get; set; }
        public string PermissionKey { get; set; }
        public string ProtectedEnviroment { get; set; }
        public Server PaymentApi { get; set; }
     //   public Server LoggingApi { get; set; }
        public Server IdentityApi { get; set; }
        public Server FileStreamApi { get; set; }
        public Server DiningPortalApi { get; set; }
        public Server YachtPortalApi { get; set; }
        public Server ConfigurationApi { get; set; }
        public Server SSOPortal { get; set; }
        public Server YachtPortal { get; set; }
        public Server DiningPortal { get; set; }
        public RedisCacheSrv RedisCacheSrv { get; set; }
        public Server AQBaseDomainPortal { get; set; }
        public Server CertificatePFX { get; set; }
        public Server AdminApi { get; set; }
        public string CertificateSercurityKey { get; set; }
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

    public class CommonSettings
    {
        public int LimitRequestPerMin { get; set; }
    }
}
