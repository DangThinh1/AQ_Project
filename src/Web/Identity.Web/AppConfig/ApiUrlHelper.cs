using Identity.Core.Portal.Helpers;
using Microsoft.Extensions.Options;
using System;
using static Identity.Web.AppConfig.RedisCacheSrv;
using static Identity.Web.AppConfig.SigninControl;

namespace Identity.Web.AppConfig
{
    public static class ApiUrlHelper
    {
        public static ApiServer ApiServer => DependencyInjectionHelper.GetService<IOptions<ApiServer>>().Value;

        public static int CurrentServer => ApiServer.Enviroment;


        public static string CertificateSercurityKey
        {
            get
            {
                var value = ApiServer.CertificateSercurityKey;
                if (string.IsNullOrEmpty(value))
                    throw new Exception("CertificateSercurityKey not provide");
                return value;
            }
        }

        public static string CertificatePFX
        {
            get
            {
                var server = ApiServer.CertificatePFX;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }
        public static string AQBaseDomainPortal
        {
            get
            {
                var server = ApiServer.AQBaseDomainPortal;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static PortalControl SigninControls
        {
            get
            {
                var server = ApiServer.SigninControls;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string ConfigurationApi
        {
            get
            {
                var server = ApiServer.ConfigurationApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string IdentityApiUrl
        {
            get
            {
                var server = ApiServer.IdentityApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string FileStreamApiUrl
        {
            get
            {
                var server = ApiServer.FileStreamApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static RedisCache RedisCacheSrv
        {
            get
            {
                var server = ApiServer.RedisCacheSrv;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }
    }
}
