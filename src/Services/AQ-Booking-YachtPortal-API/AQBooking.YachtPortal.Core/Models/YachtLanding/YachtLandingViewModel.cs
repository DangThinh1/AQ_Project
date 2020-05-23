using AQBooking.YachtPortal.Core.Models.YachtMerchants;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtLanding
{
    public class YachtLandingViewModel
    {
        public int ID { get; set; }
        public string IdEncrypt { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CharterTypeReskey { get; set; }
        public int CharterCategoryFid { get; set; }
        public int Cabins { get; set; }
        public int CharterTypeFid { get; set; }
        public double LengthMeters { get; set; }
        public int MaxPassenger { get; set; }
        public double MaxSpeed { get; set; }        
        public string Name { get; set; }
        public string EngineGenerators { get; set; }
        public int? YachtFileStreamId { get; set; }
        public int? MerchantFileStreamId { get; set; }
        public int MerchantFid { get; set; }
        public string MerchantName { get; set; }
        public int LandingPageOptionFid { get; set; }
        public string Address1 { get; set; }        
        public string MerchantCountry { get; set; }
        public string MerchantCity { get; set; }
        public string MerchantContactNum1 { get; set; }
        public string MerchantContactNum2 { get; set; }
        public string MerchantEmail1 { get; set; }
        public string MerchantEmail2 { get; set; }
        public string MerchantFIDEncypt { get; set; }
    }
}
