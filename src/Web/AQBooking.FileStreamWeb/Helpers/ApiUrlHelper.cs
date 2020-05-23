using AQBooking.FileStreamWeb.AppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AQBooking.FileStream.Core.Helpers;

namespace AQBooking.FileStreamWeb.Helpers
{
    public class ApiUrlHelper
    {
        #region Fields

        #endregion

        #region Ctor
        #endregion

        #region Methods
        public static ApiUrlConfig GetListApiUrl()
        {
            return DependencyInjectionHelper.GetService<IOptions<ApiUrlConfig>>().Value;
        }

        public static IOptions<ServerConfig> GetServer()
        {
            return DependencyInjectionHelper.GetService<IOptions<ServerConfig>>();
        }

        public static string FileStreamApiUrl
        {
            get
            {
                var apiServer = GetServer().Value;
                var server = apiServer.FileStreamApi;
                switch (apiServer.Server)
                {
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default:
                        throw new Exception("Please check server variable in appsetting.json");
                }
            }
        }
        #endregion
    }
}
