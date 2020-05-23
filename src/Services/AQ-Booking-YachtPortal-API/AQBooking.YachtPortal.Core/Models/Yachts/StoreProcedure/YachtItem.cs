using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure
{
   public class YachtItem
    {
        public YachtItem()
        {
            CustomProperties = new Dictionary<string, string>();
        }
        public int YachtID { get; set; }
        public string Name { get; set; }
        public string UniqueID { get; set; }
        public double LengthMeters { get; set; }
        public int Cabins { get; set; }
        public int MaxPassenger { get; set; }
        public double MaxSpeed { get; set; }
        public int CharterTypeFID { get; set; }
        public string CharterTypeReskey { get; set; }
        public string EngineGenerators { get; set; }
        public int? FileStreamFid { get; set; }
        public int YachtTypeFID { get; set; }
        public string YachtTypeResKey { get; set; }
        public double? FromPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int? PricingTypeFId { get; set; }
        public string PricingTypeResKey { get; set; }
        #region Ext Properties
        public Dictionary<string, string> CustomProperties { get; set; }
 
        #endregion
    }
}
