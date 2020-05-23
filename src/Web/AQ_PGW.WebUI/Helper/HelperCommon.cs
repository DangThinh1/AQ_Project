using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AQ_PGW.WebUI.Helper
{
    public static class HelperCommon
    {
        public static T DeserializeObject<T>(string result)
        {
            dynamic rs = JsonConvert.DeserializeObject(result);
            if (rs != null && rs.result.data.ToString() != "")
            {
                var obj = JsonConvert.DeserializeObject<T>(rs.result.data.ToString());
                return obj;
            }
            
            return default(T);
        }
        public static List<T> DeserializeObjects<T>(string result)
        {
            dynamic rs = JsonConvert.DeserializeObject(result);
            if (rs != null && rs.result.data.ToString() != "[]")
            {
                var obj = JsonConvert.DeserializeObject<List<T>>(rs.result.data.ToString());
                return obj;
            }
            return null;
        }
    }
}
