using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Request;
using AQBooking.Admin.Core.Response;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Core.Helpers
{
    public class APIExcute
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationType _authenticationType;

        public APIExcute(AuthenticationType authenticationType)
        {
            httpClient = new HttpClient();
            _authenticationType = authenticationType;
        }
        
        public string GetBasicAuthToken(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        }
    }

    public enum AuthenticationType
    {
        Bearer,
        Basic
    }
}