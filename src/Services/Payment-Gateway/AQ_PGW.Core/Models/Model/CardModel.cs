using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.Models.Model
{
    public class CardModel
    {
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public int Exp_Month { get; set; }
        public int Exp_Year { get; set; }
        public string CVC { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Type { get; set; }
    }
}
