using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Infrastructure.Helps
{
    public class ConfigUrlHelper
    {
        public static string GetLogAPIUrl(int Server, IConfiguration configuration = null)
        {
            var config = configuration;
            switch (Server)
            {
                case 1:
                    return config.GetValue<string>("LogAPI:AQBooking_LogAPI_VN");
                case 2:
                    return config.GetValue<string>("LogAPI:AQBooking_LogAPI_LIVE");
                case 3:
                    return config.GetValue<string>("LogAPI:AQBooking_LogAPI_BETA");
                default:
                    throw new Exception("Please check the LOG API Url in appsettings.json");

            }
        }

        public static string GetConnectionName(int server)
        {
            switch (server)
            {
                case 1:
                    return "AQ_FileStreams_VN";
                case 2:
                    return "AQ_FileStreams_BETA";
                case 3:
                    return "AQ_FileStreams_LIVE";
                default:
                    throw new Exception("Please check the connection string in appsettings.json");
            }
        }
    }
}
