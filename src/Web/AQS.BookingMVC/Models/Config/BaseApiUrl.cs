using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Models.Config
{
    public class BaseApiUrl
    {
        public CommonValuesApi CommonValues { get; set; }
       
    }
    public class CommonValuesApi
    {
        public string GetAllCommonValue { get; set; }
        public string GetCommonValueByGroupInt { get; set; }
        public string GetCommonValueByValueString { get; set; }
        public string GetCommonValueByValueDouble { get; set; }
        public string GetListCommonValueByGroup { get; set; }
    }
}
