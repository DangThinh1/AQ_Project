using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Configuration;

namespace AQ_PGW.WinService
{
    public class RequestResult
    {
        public bool Status { get; set; } = false;
        public string Content { get; set; }
    }

    public enum ContentTypeRequest
    {
        Json,
        Form
    }

    public static class RestAPI
    {
        static readonly string stringkey = ConfigurationManager.AppSettings["SERVER_ENVIRONMENT"].ToString();
        //string HostApi = "http://172.16.10.138/aq-pgw-api/";


        static string getUrl()
        {
            string url = "";
            switch (stringkey)
            {
                case "VN":
                    url = "http://172.16.10.138/aq-pgw-api/";
                    break;
                case "LIVE":
                    break;
                case "BETA":
                    break;
                default:
                    url = "http://172.16.10.138/aq-pgw-api/";
                    break;
            }
            return url;
        }

        public static RequestResult SendPostRequest(string URL, object data, ContentTypeRequest contentTypeRequest = ContentTypeRequest.Json, Dictionary<string, string> headers = null)
        {
            //dynamic result = new System.Dynamic.ExpandoObject();

            RequestResult result = new RequestResult();
            string HostApi = getUrl() + URL;
            var request = WebRequest.Create(HostApi) as HttpWebRequest;

            string contentType = "application/json; charset=utf-8";
            if (contentTypeRequest != ContentTypeRequest.Json)
                contentType = "application/x-www-form-urlencoded";

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = contentType; //"application/x-www-form-urlencoded"; "application/json; charset=utf-8";

            if (headers != null)
            {
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);
            }

            string param = string.Empty;
            if (contentType.Contains("application/json") == true)
                param = JsonConvert.SerializeObject(data);
            else
            {
                string dataTypeName = data.GetType().Name;
                if (dataTypeName == "String")
                    param = data.ToString();
                else
                {
                    Type dataType = data.GetType();
                    bool isDict = dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Dictionary<,>);
                    if (isDict == true)
                    {
                        List<string> formData = new List<string>();
                        IDictionary idict = (IDictionary)data;
                        IEnumerable<string> keys = idict.Keys.Cast<string>();
                        foreach (var key in keys)
                            formData.Add(string.Format("{0}={1}", key, idict[key]));

                        param = string.Join("&", formData);
                    }
                    else
                    {
                        var serialized = JsonConvert.SerializeObject(data);
                        var deserialized = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
                        param = deserialized.Select((kvp) => kvp.Key.ToString() + "=" + Uri.EscapeDataString(kvp.Value)).Aggregate((p1, p2) => p1 + "&" + p2);
                    }
                }
            }


            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;
            bool flagSuccess = false;
            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        flagSuccess = true;
                    }
                }
            }
            catch (WebException ex)
            {
                responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            result.Content = responseContent;
            result.Status = flagSuccess;

            return result;
        }
        public static RequestResult SendGetRequest(string URL, string data = null, Dictionary<string, string> headers = null)
        {
            //dynamic result = new System.Dynamic.ExpandoObject();

            RequestResult result = new RequestResult();
            var url = getUrl() + URL;
            if (data != null)
                url += data;

            var request = WebRequest.Create(url) as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "GET";


            if (headers != null)
            {
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);
            }


            string responseContent = null;
            bool flagSuccess = false;
            try
            {

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        flagSuccess = true;
                    }
                }
            }
            catch (WebException ex)
            {
                responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            result.Content = responseContent;
            result.Status = flagSuccess;

            return result;
        }
        //public static RequestResult PostAuthenBearer(string url, string token, object data)
        //{
        //    RequestResult result = new RequestResult();

        //    try
        //    {
        //        var client = new HttpClient();

        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


        //        string myContent = string.Empty;
        //        if (data.GetType().Name != "String")
        //            myContent = JsonConvert.SerializeObject(data);
        //        else
        //            myContent = data + "";

        //        var byteContent = new StringContent(myContent, Encoding.UTF8, "application/json");

        //        var authorizedResponse = client.PostAsync(HostApi + url, byteContent).Result;

        //        result.Content = authorizedResponse.Content.ReadAsStringAsync().Result;
        //        result.Status = true;

        //    }
        //    catch (WebException ex)
        //    {
        //        result.Content = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        //        result.Status = false;
        //    }

        //    return result;
        //}
    }
}
