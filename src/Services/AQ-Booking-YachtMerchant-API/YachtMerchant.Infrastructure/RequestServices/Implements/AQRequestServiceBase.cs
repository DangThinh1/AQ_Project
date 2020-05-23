using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using APIHelpers.Response;
using YachtMerchant.Core.Helpers;
using Identity.Core.Services.Interfaces;

namespace YachtMerchant.Infrastructure.RequestServices.Implements
{
    public class AQRequestServiceBase
    {
        private string _token;
        private string _scheme;
        private readonly HttpClient _httpClient;
        private readonly IUsersContext _usersContext;

        public AQRequestServiceBase()
        {
            _usersContext = DependencyInjectionHelper.GetService<IUsersContext>();
            _httpClient = new HttpClient();
            UseJWTAuthorization();
        }

        protected virtual BaseResponse<TResponse> Get<TResponse>(string url)
        {
            try
            {
                if(string.IsNullOrEmpty(url))
                    return BaseResponse<TResponse>.BadRequest();

                var response = _httpClient.GetAsync(url).Result;
                var result = DeserializeToBaseResponse<TResponse>(response);
                return result;
            }
            catch(Exception ex)
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
                var result = DeserializeToBaseResponse<TResponse>(response);
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
                var result = DeserializeToBaseResponse<TResponse>(response);
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
                var result = DeserializeToBaseResponse<TResponse>(response);
                return result;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        private BaseResponse<TResponse> DeserializeToBaseResponse<TResponse>(HttpResponseMessage response)
        {
            try
            {
                if(response == null)
                    return BaseResponse<TResponse>.BadRequest(message:"Response is null");

                if (!response.IsSuccessStatusCode)
                    return BaseResponse<TResponse>.BadRequest(message: response.ToString());

                var responseData = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<TResponse>(responseData);
                return BaseResponse<TResponse>.Success(data);
            }
            catch(Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        protected virtual void UseAuthorization(string scheme = "", string token = "")
        {
            _scheme = scheme ?? string.Empty;
            _token = token ?? string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_scheme, _token);
        }
        protected virtual void UseJWTAuthorization(string token = "") => UseAuthorization("Bearer", token);
        protected virtual void UseBasicAuthorization(string token = "") => UseAuthorization("Basic", token);
    }
}