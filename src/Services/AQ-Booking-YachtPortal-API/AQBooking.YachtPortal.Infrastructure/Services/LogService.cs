using APIHelpers;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class LogService : AQLogger
    {
        public LogService(string logAPIUrl, string apiName) : base(logAPIUrl, apiName)
        {
        }

        public async override Task InsertLog(string module, string function, LogType logType, string shortMsg, string fullMsg)
        {
            try
            {
                await base.InsertLog(module, function, logType, shortMsg, fullMsg);
            }
            catch
            {
                await Task.FromResult(false);
            }
        }
    }
}
