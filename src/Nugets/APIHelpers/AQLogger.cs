using APIHelpers.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIHelpers
{
    public abstract class AQLogger
    {
        private readonly APIExcute _aPIExcute;
        private readonly string _logAPIUrl;
        private string APIName;
        public AQLogger(string logAPIUrl,string APIName)
        {
            _aPIExcute = new APIExcute(AuthenticationType.Basic);
            _logAPIUrl = logAPIUrl;
            this.APIName = APIName;
        }
        public virtual string GetIPAddress()
        {
            return null;
        }
        public virtual string GetUserName()
        {
            return null;
        }
        public virtual async Task InsertLog(string module,string function,LogType logType,string shortMsg,string fullMsg)
        {
            var request = new BaseRequest<object>
            {
                RequestData = new
                {
                    logType=logType.ToString(),
                    apiName=APIName,
                    user=GetUserName(),
                    action=function,
                    module=module,
                    ipAddress=GetIPAddress(),
                    shortMessage=shortMsg,
                    fullMessage=fullMsg

                }
            };
            string token = _aPIExcute.GetBasicAuthToken("AESAPI", "Sysadmin@2019$$");
            var response =await _aPIExcute.PostData<object, object>(_logAPIUrl + "api/Log/create-log",request,token);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Cannot insert log , "+response.FullResponseString);
        }
        public Task Log(string module,string function,string Msg,string data)
        {
            return InsertLog(module, function, LogType.Info, Msg, data);
        }
        public Task Error(string module,string function,string msg,string data=null)
        {
            return InsertLog(module, function, LogType.Error, msg, data);
        }
        public Task Error(string module, string function,Exception ex)
        {
            return InsertLog(module, function, LogType.Error, ex.Message, ex.ToString());
        }
    }
    public enum LogType
    {
        Error,
        Info,
        Warning
    }
   
}
