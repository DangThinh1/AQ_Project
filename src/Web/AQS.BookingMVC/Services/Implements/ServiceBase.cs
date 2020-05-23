using APIHelpers;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Infrastructure.ConfigModel;
using System;
using System.Collections.Generic;
using System.Collections;

namespace AQS.BookingMVC.Services
{
    public class ServiceBase
    {
        protected APIExcute _apiExcute;
        //private readonly HTTPClient _httpClient;
        protected string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiS2lldCBUdWFuIiwiRW1haWwiOiJvbmV5YW1AZ21haWwuY29tIiwiVUlEIjoiMmZjNjkxNTctOGM1ZS00OThjLTk4OGEtNGNmZTViNjAzMWIzIiwiUm9sZUlkIjoiMCIsIlJvbGUiOiIiLCJVbmlxdWVJZCI6IkZFVjk0MjcyNEQ4NyIsIkFjY2Vzc1Rva2VuIjoiIiwiUmVmcmVzaFRva2VuIjoiQlBDNTk1UVA1TkFZIiwiQWNjb3VudFR5cGUiOiIwIiwiVG9rZW5FZmZlY3RpdmVEYXRlIjoiNC8yMS8yMDIwIDU6NTc6MTkgUE0iLCJUb2tlbkVmZmVjdGl2ZVRpbWVTdGljayI6IjYzNzIzMDg4NjM5NDI3ODc5OSIsIm5iZiI6MTU4NzQ2NjYzOSwiZXhwIjoxOTQ3NDY2NjM5LCJpYXQiOjE1ODc0NjY2MzksImlzcyI6IkFRSWRlbnRpdHlTZXJ2ZXIiLCJhdWQiOiJBUUlkZW50aXR5U2VydmVyIn0.Dz_WF_PRxVYxOLDzbYxiMhEgzVLyrwf1cqURif8dlmE";
        protected string basicToken;
        protected string _baseConfigurationApi = ApiUrlHelper.ConfigurationApi;
        protected string _baseFileStreamAPIUrl = ApiUrlHelper.FileStreamApiUrl;
        protected string _basePaymentApi = ApiUrlHelper.PaymentApi;
        protected string _baseAdminApiUrl = ApiUrlHelper.AdminApiUrl;
        protected ApiServer _apiServer = ApiUrlHelper.ApiServer;
   //   protected string _baseUtilitiesAPIUrl = ApiUrlHelper.LoggingApiUrl;
        public ServiceBase()
        {

            _apiExcute = new APIExcute(AuthenticationType.Bearer);
            basicToken = _apiExcute.GetBasicAuthToken(BasicAccountConstant.UserName, BasicAccountConstant.Password);
        }
        #region Helper Methods
        protected string ConvertToUrlParameter(object param, string paramName = "")
        {
            try
            {
                if (param == null)
                    return string.Empty;
                string urlParameter = string.Empty;

                var paramType = param.GetType();
                var typeInt = typeof(List<int>);
                var typeLong = typeof(List<long>);
                var typeLDouble = typeof(List<double>);
                var typeDecimal = typeof(List<decimal>);
                var typeString = typeof(List<string>);

                //if input is a list of value type
                if (paramType.Equals(typeInt) || paramType.Equals(typeLong) || paramType.Equals(typeLong) || paramType.Equals(typeLDouble) || paramType.Equals(typeDecimal) || paramType.Equals(typeString))
                {
                    urlParameter = ListValueToParam(param, paramName);
                }
                else
                {

                    urlParameter = ModelToParam(param);
                }

                return urlParameter;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string ListValueToParam(object listValue, string paramName = "")
        {
            try
            {
                var list = listValue as IList;
                string urlParam = string.Empty;
                if (!urlParam.Contains("?"))
                    urlParam += "?";
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        urlParam += string.Format("{0}={1}&", paramName, item);
                    }
                }

                return urlParam.TrimEnd('&');
            }
            catch
            {

                return string.Empty;
            }
        }

        private string ModelToParam(object model)
        {
            var listProperties = model.GetType().GetProperties();
            string urlParam = string.Empty;
            foreach (var prop in listProperties)
            {
                var type = prop.PropertyType;
                var typeInt = typeof(List<int>);
                var typeLong = typeof(List<long>);
                var typeLDouble = typeof(List<double>);
                var typeDecimal = typeof(List<decimal>);
                var typeString = typeof(List<string>);

                if (type.Equals(typeInt) || type.Equals(typeLong) || type.Equals(typeLong) || type.Equals(typeLDouble) || type.Equals(typeDecimal) || type.Equals(typeString))
                {
                    if (!urlParam.Contains("?"))
                        urlParam += "?";
                    var list = prop.GetValue(model, null) as IList;
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            urlParam += string.Format("{0}={1}&", prop.Name, item);
                        }
                    }
                }
                else
                {
                    if (!urlParam.Contains("?"))
                        urlParam += "?";
                    var value = prop.GetValue(model, null);

                    if (value != null && !value.Equals(DefaultForType(type)))
                    {
                        urlParam += string.Format("{0}={1}&", prop.Name, value);
                    }
                }
            }
            return urlParam.TrimEnd('&');
        }

        private object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        public void SetToken(string jwtToken)
        {
            _token = jwtToken;
        }
        #endregion
    }
}
