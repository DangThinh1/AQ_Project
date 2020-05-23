using System;
using Microsoft.Extensions.Options;
using Identity.Infrastructure.AppSetting;
using Identity.Core.Portal.Helpers;

namespace Identity.Infrastructure.Helpers
{
    public class ApiServerHelper
    {
        private static IOptions<ApiServer> GetApiServer()
        {
            try
            {
                return DependencyInjectionHelper.GetService<IOptions<ApiServer>>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string FileStreamApiUrl
        {
            get
            {
                var apiServer = GetApiServer().Value;
                var server = apiServer.FileStreamApi;
                switch (apiServer.Enviroment)
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

        public static string LoggingApiUrl
        {
            get
            {
                var apiServer = GetApiServer().Value;
                var server = apiServer.LoggingApi;
                switch (apiServer.Enviroment)
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
                var apiServer = GetApiServer().Value;
                var server = apiServer.ConnectionString;
                switch (apiServer.Enviroment)
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
