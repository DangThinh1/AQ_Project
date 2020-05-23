using System;
using Microsoft.Extensions.Options;
using Identity.Infrastructure.AppSetting;

namespace Identity.Infrastructure.Helpers
{
    public class ApiServerHelper
    {
        public static ApiServer apiServer => DependencyInjectionHelper.GetService<IOptions<ApiServer>>().Value;

        public static int CurrentServer => apiServer.Server;

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
