using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using APIHelpers.Response;
using System.Net.Http.Headers;
using Identity.Core.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Identity.Core.Services.Base
{
    public class RequestServiceBase
    {
        private readonly HttpClient _httpClient;
        private const string DEFAULT_BASIC_AUTH_USER = "";
        private const string DEFAULT_BASIC_AUTH_PASS = "";

        public RequestServiceBase()
        {
            _httpClient = new HttpClient();
            UseJWTAuthorization();
        }

        protected virtual BaseResponse<TResponse> Get<TResponse>(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();

                var response = _httpClient.GetAsync(url).Result;
                var result = DeserializeToBaseResponse<TResponse>(response).Result;
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        protected virtual BaseResponse<TResponse> Get<TResponse>(string url, Dictionary<string, object> requestParams = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();
                if (requestParams != null)
                {
                    if (!url.Contains("?"))
                        url += "?";
                    foreach (KeyValuePair<string, object> k in requestParams)
                    {
                        url += $"{k.Key}={k.Value}&";
                    }
                    url = url.TrimEnd('&');
                }
                var response = _httpClient.GetAsync(url).Result;
                var result = DeserializeToBaseResponse<TResponse>(response).Result;
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        protected virtual async Task<BaseResponse<TResponse>> GetAsync<TResponse>(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();

                var response = await _httpClient.GetAsync(url);
                var result = await DeserializeToBaseResponse<TResponse>(response);
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        protected virtual async Task<BaseResponse<TResponse>> GetAsync<TResponse>(string url, Dictionary<string, object> requestParams = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();

                if (requestParams != null)
                {
                    if (!url.Contains("?"))
                        url += "?";
                    foreach (KeyValuePair<string, object> k in requestParams)
                    {
                        url += $"{k.Key}={k.Value}&";
                    }
                    url = url.TrimEnd('&');
                }
                var response = await _httpClient.GetAsync(url);
                var result = await DeserializeToBaseResponse<TResponse>(response);
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        protected virtual BaseResponse<TResponse> Post<TResponse>(string url, object sendData)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();
                var data = new StringContent(JsonConvert.SerializeObject(sendData), Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync(url, data).Result;
                var result = DeserializeToBaseResponse<TResponse>(response).Result;
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        protected virtual async Task<BaseResponse<TResponse>> PostAsync<TResponse>(string url, object sendData)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();
                var data = new StringContent(JsonConvert.SerializeObject(sendData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, data);
                var result = await DeserializeToBaseResponse<TResponse>(response);
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        protected virtual BaseResponse<TResponse> Put<TResponse>(string url, object sendData)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();
                var data = new StringContent(JsonConvert.SerializeObject(sendData), Encoding.UTF8, "application/json");
                var response = _httpClient.PutAsync(url, data).Result;
                var result = DeserializeToBaseResponse<TResponse>(response).Result;
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        protected virtual async Task<BaseResponse<TResponse>> PutAsync<TResponse>(string url, object sendData)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();
                var data = new StringContent(JsonConvert.SerializeObject(sendData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, data);
                var result = await DeserializeToBaseResponse<TResponse>(response);
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        protected virtual BaseResponse<TResponse> Delete<TResponse>(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();
                var response = _httpClient.DeleteAsync(url).Result;
                var result = DeserializeToBaseResponse<TResponse>(response).Result;
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        protected virtual async Task<BaseResponse<TResponse>> DeleteAsync<TResponse>(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();
                var response = await _httpClient.DeleteAsync(url);
                var result = await DeserializeToBaseResponse<TResponse>(response);
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        private async Task<BaseResponse<TResponse>> DeserializeToBaseResponse<TResponse>(HttpResponseMessage response)
        {
            try
            {
                if (response == null)
                    return BaseResponse<TResponse>.BadRequest(message: "Response is null");

                if (!response.IsSuccessStatusCode)
                    return BaseResponse<TResponse>.BadRequest(message: response.ToString());

                var responseData = await response.Content.ReadAsStringAsync();
                var baseResponse = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseData);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        protected virtual void UseAuthorization(string scheme, string token)
        {
            if(!string.IsNullOrEmpty(scheme) && !string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        }
        protected virtual void UseJWTAuthorization(string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = UserContextHelper.AccessToken;
            UseAuthorization("Bearer", token);
        }
        protected virtual void UseBasicAuthorization(string user = DEFAULT_BASIC_AUTH_USER, string password = DEFAULT_BASIC_AUTH_PASS)
        {
            var basicToken = GenerateBasicToken(user, password);
            UseAuthorization("Basic", basicToken);
        }
        protected virtual void UseNonAuthorization() => _httpClient.DefaultRequestHeaders.Authorization = null;

        protected virtual string GenerateBasicToken(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        }
    }
}