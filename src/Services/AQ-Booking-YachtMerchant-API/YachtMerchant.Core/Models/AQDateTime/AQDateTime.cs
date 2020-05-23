using System;

namespace YachtMerchant.Core.Models.AQDateTime
{
    public  class AQDateTime
    {
        public bool IsValid { get; set; }
        public string Format { get; set; }
        public DateTime Value {get;set; }
    }
}