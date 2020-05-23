using System;
using APIHelpers;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using APIHelpers.Response;
using Identity.Core.Helpers;
using System.Net.Http.Headers;
using Identity.Core.Services.Interfaces;

namespace Identity.Core.Services.Implements
{
    public partial class RequestServiceBase : IRequestServiceBase
    {
        private IUsersContext _usersContext;
        private string _baseToken { get; set; }
        public string BaseUrl { get; set; }
        public string BaseToken {
            get => string.IsNullOrEmpty(_baseToken) ? _usersContext.UserProfiles.AccessToken : _baseToken;
            set => _baseToken = value;
        }
        
        public RequestServiceBase(IUsersContext usersContext)
        {
            _usersContext = usersContext;
            BaseToken = string.Empty;
            BaseUrl = IdentityInjectionHelper.GetBaseUrl();
        }

        #region protected method
        protected virtual BaseResponse<TResponse> Get<TResponse>(ApiRequestModel model) => ApiExcute<TResponse>(model, HttpMethodEnum.GET);
        protected virtual BaseResponse<TResponse> Put<TResponse>(ApiRequestModel model) => ApiExcute<TResponse>(model, HttpMethodEnum.PUT);
        protected virtual BaseResponse<TResponse> Post<TResponse>(ApiRequestModel model) => ApiExcute<TResponse>(model, HttpMethodEnum.POST);
        protected virtual BaseResponse<TResponse> Delete<TResponse>(ApiRequestModel model) => ApiExcute<TResponse>(model, HttpMethodEnum.DELETE);
        #endregion protected method

        #region private method
        private BaseResponse<TResponse> ApiExcute<TResponse>(ApiRequestModel model, HttpMethodEnum method)
        {
            try
            {
                if(model == null || string.IsNullOrEmpty(GenerateApiUrl(model.Host, model.Api)))
                    return BaseResponse<TResponse>.BadRequest();
                var url = GenerateApiUrl(model.Host, model.Api);
                var httpClient = GenerateHttpClient(model.TokenScheme, model.Token);
                switch (method)
                {
                    case HttpMethodEnum.GET:
                        return ConvertToBaseResponse<TResponse>(httpClient.GetAsync(url).Result);
                    case HttpMethodEnum.POST:
                        var postBody = model.RequestBody != null ? GenerateHttpJsonContent(model.RequestBody) : null;
                        return ConvertToBaseResponse<TResponse>(httpClient.PostAsync(url, postBody).Result);
                    case HttpMethodEnum.PUT:
                        var putBody = model.RequestBody != null ? GenerateHttpJsonContent(model.RequestBody) : null;
                        return ConvertToBaseResponse<TResponse>(httpClient.PutAsync(url, putBody).Result);
                    case HttpMethodEnum.DELETE:
                        return ConvertToBaseResponse<TResponse>(httpClient.DeleteAsync(url).Result);
                }
                return BaseResponse<TResponse>.InternalServerError();
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        private HttpClient GenerateHttpClient(string scheme, string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, string.IsNullOrEmpty(token) ? BaseToken : token);
            return httpClient;
        }
        private BaseResponse<TResponse> ConvertToBaseResponse<TResponse>(HttpResponseMessage response)
        {
            var responseData = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var succeedResponse = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseData);
                succeedResponse.FullResponseString = responseData;
                return succeedResponse;
            }

            return new BaseResponse<TResponse>()
            {
                StatusCode = response.StatusCode,
                FullResponseString = responseData,
                Message = response.ToString(),
            };
        }
        private string GenerateApiUrl(string hostUrl, string apiUrl)
        {
            hostUrl = string.IsNullOrEmpty(hostUrl) ? BaseUrl : hostUrl;
            if (string.IsNullOrEmpty(hostUrl) || string.IsNullOrEmpty(apiUrl))
                return string.Empty;
            var host = hostUrl.EndsWith("/") ? hostUrl.TrimEnd('/') : hostUrl;
            var api = apiUrl.StartsWith("/") ? apiUrl.TrimStart('/') : apiUrl;
            return $"{host}/{api}";
        }
        private StringContent GenerateHttpJsonContent(object obj)
         => new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        #endregion private method
    }

    public class ApiRequestModel
    {
        public string Host { get; set; }
        public string Api { get; set; }
        public object RequestBody { get; set; }
        public string Token { get; set; }
        public string TokenScheme { get; set; } = "Bearer";
    }
}