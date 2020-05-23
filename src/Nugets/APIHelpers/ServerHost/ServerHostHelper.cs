using System;
using System.Collections.Generic; 

namespace APIHelpers.ServerHost
{
    public class ServerHostHelper
    {
        private static Dictionary<string, string> _listServerHost = new Dictionary<string, string>();

        public static bool AddServerHostByName(string serverName, string serverUrl)
            => _listServerHost.TryAdd(serverName, serverUrl);

        public static string GetServerHostByName(string serverName)
        {
            try
            {
                string serverUrl;
                var success = _listServerHost.TryGetValue(serverName, out serverUrl);
                return success ? serverUrl : throw new Exception($"Server Host with name {serverName} is null please setup on startup.cs");
            }
            catch
            {
                throw new Exception($"Server Host with name {serverName} is null please setup on startup.cs");
            }
        }
    }
}
