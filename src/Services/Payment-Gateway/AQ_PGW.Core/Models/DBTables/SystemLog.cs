using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class SystemLogs
    {
        public decimal ID { get; set; }
        public DateTime? LogDate { get; set; }
        public string UrlRequest { get; set; }
        public string DataURL { get; set; }
        public string DataBody { get; set; }
        public string TypeRequest { get; set; }
        public string DataReponse { get; set; }
        public string TypePayment { get; set; }
        public int StatusCode { get; set; }
    }
}
