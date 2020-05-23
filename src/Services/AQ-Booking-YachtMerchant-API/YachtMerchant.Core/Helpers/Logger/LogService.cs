using APIHelpers;
using AQBooking.Core.Helpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YachtMerchant.Core.Helpers.Logger
{
    public class LogService : AQLogger
    {

        public LogService(string logAPIUrl, string apiName) : base(logAPIUrl, apiName)
        {
        }
        public override string GetIPAddress()
        {
            try
            {
                return HttpHelper.GetCurrentIpAddress();
            }
            catch(Exception ex)
            {
                ex.StackTrace.ToString();
                return string.Empty;
            }
           
        }
        public override string GetUserName()
        {
            try
            {
                var userEmail = JwtAccessTokenHelper.GetClaimValue(ClaimTypes.Email);
                return userEmail;
            }
            catch
            {
                return string.Empty;
            }

        }

        public async override Task InsertLog(string module, string function, LogType logType, string shortMsg, string fullMsg)
        {
            try
            {
                await base.InsertLog(module, function, logType, shortMsg, fullMsg);
            }
            catch (Exception ex)
            {
                await Task.FromResult(false);
            }
        }
    }
}
