using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Web.AppConfig
{
    public class ApiServer
    {
        public int Enviroment { get; set; }
        public Server IdentityApi { get; set; }
        public Server FileStreamApi { get; set; }
        public Server ConfigurationApi { get; set; }
        public SigninControl SigninControls { get; set; }
        public RedisCacheSrv RedisCacheSrv { get; set; }
        public Server AQBaseDomainPortal { get; set; }
        public Server CertificatePFX { get; set; }
        public string CertificateSercurityKey { get; set; }
    }

    public class Server
    {
        public string Server_LOCAL { get; set; }
        public string Server_VN { get; set; }
        public string Server_BETA { get; set; }
        public string Server_LIVE { get; set; }
    }


    
    public class SigninControl
    {
        public PortalControl Server_LOCAL { get; set; }
        public PortalControl Server_VN { get; set; }
        public PortalControl Server_BETA { get; set; }
        public PortalControl Server_LIVE { get; set; }
        public class PortalControl
        {
            public List<Portal> Portals { get; set; }
            public List<Flow> SignFlow { get; set; }

            public class Flow
            {
                public string FromDomainId { get; set; }
                public string ToDomainId { get; set; }
            }

            public class Portal
            {
                public string DomainName { get; set; }
                public string DomainId { get; set; }
                public string Host { get; set; }
                public string LoginNextPath { get; set; }
                public string LoginEndPath { get; set; }
                public string LogoutNextPath { get; set; }
                public string LogoutEndPath { get; set; }
                public string ErrorPath { get; set; }
                public string HomePath { get; set; }
            }
        }
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