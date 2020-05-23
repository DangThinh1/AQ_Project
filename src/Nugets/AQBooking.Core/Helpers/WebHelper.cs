using System;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.StaticFiles; 

namespace AQBooking.Core.Helpers
{
    public class WebHelper
    {
        public static int Server;
        public static void Init(int server)
        {
            Server = server;
        }
        public static bool IsLive()
        {
            return Server == 1;
        }
        
        public static string GetIPAddress()
        {
            try
            {
                var _accessor = EngineerContext.GetService<IHttpContextAccessor>();
                return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }
         }
        
        public static bool IsImage(string ext)
        {
            var regex = new Regex(@"(.*?)\.(jpg|jpeg|JPG|JPEG|PNG|png)$");
            return regex.IsMatch(ext);
        }
        public static bool IsFile(string ext)
        {
            var regex = new Regex(@"(.*?)\.(ppt|PPTX|xls|XLS|xlsx|XLSX|doc|DOC|docx|DOCX|pdf|PDF)$");
            return regex.IsMatch(ext);
        }
        public static bool IsPDF(string ext)
        {
            var regex = new Regex(@"(.*?)\.(pdf|PDF)$");
            return regex.IsMatch(ext);
        }
        public static bool IsValidFileSize(byte[] fileData, int fileMb)
        {
            return (fileData.Length / 1000000) <= fileMb;
        }
        public static bool IsValidFileSize(int contentLenth, int fileMb)
        {
            return (contentLenth / 1000000) <= fileMb;
        }
        public static string GetContentType(string fileName)
        {
            string contentType = null;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            return contentType;
        }
        public static DateTime ConvertUnixTimeToDate(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }     

    }
}
