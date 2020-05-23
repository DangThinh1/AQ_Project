using APIHelpers;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQBooking.YachtPortal.Infrastructure.Services;
using System.Threading.Tasks;

namespace AQBooking.YachtPortal.API.Helpers
{
    public static class LogHelper
    {
        private static LogService _logService;

        public static void InsertLog(string module, string function, LogType logType, string shortMsg, string fullMsg)
        {
            if (_logService == null)
                _logService = DependencyInjectionHelper.GetService<LogService>();
            Task.Run(() => _logService.InsertLog(module, function, logType, shortMsg, fullMsg));
        }
    }
}
