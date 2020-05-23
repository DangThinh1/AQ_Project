using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.FileStreamWeb.AppConfig
{
    public class ServerConfig
    {
        public int Server { get; set; }
        public Server FileStreamApi { get; set; }
        public Server LoggingApi { get; set; }
    }

    public class Server
    {
        public string Server_VN { get; set; }
        public string Server_BETA { get; set; }
        public string Server_LIVE { get; set; }
    }
}
