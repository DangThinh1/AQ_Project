using System;
using YachtMerchant.Core.Helpers;
using Microsoft.Extensions.Options;
using YachtMerchant.Infrastructure.AppSetting;

namespace YachtMerchant.Infrastructure.Helpers
{
    public static class ApiUrlHelper
    {
        public static ApiServer apiServer => DependencyInjectionHelper.GetService<IOptions<ApiServer>>().Value;
        public static int CurrentServer => apiServer.Server;


        private static IOptions<ApiServer> GetApiServer()
        {
            return DependencyInjectionHelper.GetService<IOptions<ApiServer>>();
        }

        public static string ServerName
        {
            get
            {
                var apiServer = GetApiServer().Value;
                return apiServer.ServerName;
            }
        }

        public static int Server
        {
            get
            {
                var apiServer = GetApiServer().Value;
                return apiServer.Server;
            }
        }
        public static string IdentityApiUrl
        {
            get
            {
                var server = apiServer.IdentityApi;
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
                var server = apiServer.ConfigurationApi;
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
        public static string AdminApiUrl
        {
            get
            {
                var server = apiServer.AdminApi;
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
                var server = apiServer.FileStreamApi;
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

        public static string ConnectionString
        {
            get
            {
                var server = apiServer.ConnectionString;
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
