using AQEncrypts;
using AQS.BookingMVC.Areas.Yacht.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.Helpers
{
    public class CommonHelper
    {
        public static string EncryptObj(object obj)
        {
            return Terminator.Encrypt(JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
        public static T DecryptObj<T>(string encryptedStr)
        {
            try
            {
                var decryptStr = Terminator.Decrypt(encryptedStr);
                return JsonConvert.DeserializeObject<T>(decryptStr);
            }
            catch
            {
                return default(T);
            }
        }
        #region Query Helper
        public static string ConvertToUrlParameter(object param, string paramName = "")
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
        private static string ListValueToParam(object listValue, string paramName = "")
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
        private static string ModelToParam(object model)
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
        private static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
        #endregion
    }
}
