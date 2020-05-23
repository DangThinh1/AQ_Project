using APIHelpers.Request;
using APIHelpers.Response;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace APIHelpers
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

        public APIExcute(AuthenticationType authenticationType, string acceptType)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", acceptType);
            _authenticationType = authenticationType;
        }

        public virtual async Task<BaseResponse<TResponse>> GetData<TResponse>(string url, Dictionary<string, object> requestParams = null, string token = null)
        {
            BaseResponse<TResponse> result = new BaseResponse<TResponse>();
            try
            {
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
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_authenticationType.ToString(), token);
                }

                HttpResponseMessage response = await httpClient.GetAsync(url);
                string responseData = await response.Content.ReadAsStringAsync();
                result.FullResponseString = responseData;

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseData);

                }
                else
                {
                    result.StatusCode = response.StatusCode;


                }
                result.FullResponseString = responseData;
            }
            catch (Exception ex)
            {
                result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                result.Message = ex.ToString();
            }
            return result;
        }

        public virtual async Task<BaseResponse<TResponse>> PostData<TResponse, TRequest>(string url, HttpMethodEnum method = HttpMethodEnum.POST, BaseRequest<TRequest> requestParams = null, string token = null)
        {
            BaseResponse<TResponse> result = new BaseResponse<TResponse>();
            try
            {
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_authenticationType.ToString(), token);
                }

                HttpResponseMessage response = null;
                if (requestParams != null)
                {
                    StringContent data = new StringContent(JsonConvert.SerializeObject(requestParams.RequestData), Encoding.UTF8, "application/json");
                    response = await PostDataAsync(method, url, data);
                }
                else
                {
                    response = await PostDataAsync(method, url, null);
                }
                string responseData = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseData);

                }
                else
                {
                    result.StatusCode = response.StatusCode;
                }
                result.FullResponseString = responseData;
            }
            catch (Exception ex)
            {
                result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                result.Message = ex.ToString();
            }
            return result;
        }

        public virtual async Task<BaseResponse<TResponse>> PostData<TResponse, TRequest>(string url, HttpMethodEnum method = HttpMethodEnum.POST, Dictionary<string, object> urlParams = null, BaseRequest<TRequest> requestParams = null, string token = null)
        {
            BaseResponse<TResponse> result = new BaseResponse<TResponse>();
            try
            {
                if (urlParams != null)
                {
                    if (!url.Contains("?"))
                        url += "?";
                    foreach (KeyValuePair<string, object> k in urlParams)
                    {
                        url += $"{k.Key}={k.Value}&";
                    }
                    url = url.TrimEnd('&');

                }
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_authenticationType.ToString(), token);
                }

                HttpResponseMessage response = null;
                if (requestParams != null)
                {
                    StringContent data = new StringContent(JsonConvert.SerializeObject(requestParams.RequestData), Encoding.UTF8, "application/json");
                    response = await PostDataAsync(method, url, data);
                }
                else
                {
                    response = await PostDataAsync(method, url, null);
                }
                string responseData = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseData);
                }
                else
                {
                    result.StatusCode = response.StatusCode;
                }
                result.FullResponseString = responseData;
            }
            catch (Exception ex)
            {
                result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                result.Message = ex.ToString();
            }
            return result;

        }

        public virtual async Task<BaseResponse<TResponse>> PostData<TResponse, TRequest>(string url, BaseRequest<TRequest> requestParams = null, string token = null)
        {
            return await PostData<TResponse, TRequest>(url, HttpMethodEnum.POST, requestParams, token);
        }
        public virtual async Task<BaseResponse<TResponse>> PostDataMedia<TResponse>(string url, string fileParamName, byte[] fileData, string fileName, object requestParams = null, string token = null)
        {
            BaseResponse<TResponse> result = new BaseResponse<TResponse>();
            try
            {

                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_authenticationType.ToString(), token);
                }

                new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);


                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {


                    if (requestParams != null)
                    {
                        content.Add(new StringContent(JsonConvert.SerializeObject(requestParams), Encoding.UTF8, "application/json"));
                    }
                    content.Add(new StreamContent(new MemoryStream(fileData))
                    {
                        Headers =
                        {
                            ContentLength = fileData.Length,
                            ContentType = new MediaTypeHeaderValue(contentType)
                        }
                    }, fileParamName, fileName);
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseData);
                    }
                    else
                    {
                        result.StatusCode = response.StatusCode;
                    }
                    result.FullResponseString = responseData;
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                result.Message = ex.ToString();
            }
            return result;
        }

        private Task<HttpResponseMessage> PostDataAsync(HttpMethodEnum method, string url, HttpContent content)
        {
            switch (method)
            {
                case HttpMethodEnum.POST:
                    return httpClient.PostAsync(url, content);
                case HttpMethodEnum.PUT:
                    return httpClient.PutAsync(url, content);
                case HttpMethodEnum.DELETE:
                    return httpClient.DeleteAsync(url);
            }
            return httpClient.PostAsync(url, content);
        }

        public string GetBasicAuthToken(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        }

        public Dictionary<string, object> ConvertModelToDictionary(object obj)
        {
            var result = new Dictionary<string, object>();
            var json = JsonConvert.SerializeObject(obj);
            result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            return result;
        }
    }

    public enum AuthenticationType
    {
        Bearer,
        Basic
    }

}
