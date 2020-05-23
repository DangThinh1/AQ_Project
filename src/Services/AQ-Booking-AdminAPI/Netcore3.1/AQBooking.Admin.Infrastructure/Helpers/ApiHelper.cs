using Microsoft.Extensions.Configuration;
using System;

namespace AQBooking.Admin.Infrastructure.Helpers
{
    public class ApiHelper
    {
        private static int _server = 0;
        private static IConfiguration _configuration = null;

        public static void Init(IConfiguration configuration)
        {
            _configuration = configuration;
            _server = _configuration.GetSection("WebSetting").GetValue<int>("Server");
        }

        public static int GetServer()
        {
            return _server;
        }

        public static string GetConnectionStringConfig()
        {
            switch (_server)
            {
                case 1:
                    return _configuration.GetConnectionString("ConnectionStringConfig_VN");
                case 2:
                    return _configuration.GetConnectionString("ConnectionStringConfig_BETA");
                case 3:
                    return _configuration.GetConnectionString("ConnectionStringConfig_LIVE");
                default:
                    throw new Exception("Please check the connection string in appsettings.json");
            }
        }

        public static string GetConnectionStringDining()
        {
            switch (_server)
            {
                case 1:
                    return _configuration.GetConnectionString("ConnectionStringDining_VN");
                case 2:
                    return _configuration.GetConnectionString("ConnectionStringDining_BETA");
                case 3:
                    return _configuration.GetConnectionString("ConnectionStringDining_LIVE");
                default:
                    throw new Exception("Please check the connection string in appsettings.json");
            }
        }

        public static string GetConnectionStringYacht()
        {
            switch (_server)
            {
                case 1:
                    return _configuration.GetConnectionString("ConnectionStringYacht_VN");
                case 2:
                    return _configuration.GetConnectionString("ConnectionStringYacht_BETA");
                case 3:
                    return _configuration.GetConnectionString("ConnectionStringYacht_LIVE");
                default:
                    throw new Exception("Please check the connection string in appsettings.json");
            }
        }

        public static string GetConnectionStringEvisa()
        {
            switch (_server)
            {
                case 1:
                    return _configuration.GetConnectionString("ConnectionStringEVisa_VN");
                case 2:
                    return _configuration.GetConnectionString("ConnectionStringEvisa_BETA");
                case 3:
                    return _configuration.GetConnectionString("ConnectionStringEvisa_LIVE");
                default:
                    throw new Exception("Please check the connection string in appsettings.json");
            }
        }

        public static string GetConnectionStringHotel()
        {
            switch (_server)
            {
                case 1:
                    return _configuration.GetConnectionString("ConnectionStringHotel_VN");
                case 2:
                    return _configuration.GetConnectionString("ConnectionStringHotel_BETA");
                case 3:
                    return _configuration.GetConnectionString("ConnectionStringHotel_LIVE");
                default:
                    throw new Exception("Please check the connection string in appsettings.json");
            }
        }

        public static string GetConnectionStringCMS()
        {
            switch (_server)
            {
                case 1:
                    return _configuration.GetConnectionString("ConnectionStringCMS_VN");
                case 2:
                    return _configuration.GetConnectionString("ConnectionStringCMS_BETA");
                case 3:
                    return _configuration.GetConnectionString("ConnectionStringCMS_LIVE");
                default:
                    throw new Exception("Please check the connection string in appsettings.json");
            }
        }
        public static string GetIdentitServiceyUrl()
        {
            return "";
        }

       

        
    }
}