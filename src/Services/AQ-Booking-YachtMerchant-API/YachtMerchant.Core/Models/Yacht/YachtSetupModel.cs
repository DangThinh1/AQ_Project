using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.Yacht
{
    public class YachtSetupModel
    {
        public YachtSetupInfo Setup { get; set; }
        public bool Valid { get; set; }
        public string Message { get; set; }

        public YachtSetupModel()
        {
        }

        public YachtSetupModel(bool valid, string message)
        {
            Valid = valid;
            Message = message;
            Setup = null;
        }

        public YachtSetupModel(bool valid, string message, YachtSetupInfo setupInfo)
        {
            Valid = valid;
            Message = message;
            Setup = setupInfo;
        }
    }

    public class YachtSetupInfo
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public string CultureCode { get; set; }
        public string CurrencyResourceKey { get; set; }
    }
}
