using APIHelpers;
using System.Threading.Tasks;

namespace YachtMerchant.Core.Helpers.Logger
{
    public static class LogHelper
    {
        private static LogService _logService;

        public static void InsertLog(string module, string function, LogType logType, string shortMsg, string fullMsg)
        {
            if (_logService == null)
                _logService = DependencyInjectionHelper.GetService<LogService>();

            string IpAddress = _logService.GetIPAddress();
            Task.Run(() => _logService.InsertLog(module, function, logType, shortMsg, fullMsg));
        }
    }
}
