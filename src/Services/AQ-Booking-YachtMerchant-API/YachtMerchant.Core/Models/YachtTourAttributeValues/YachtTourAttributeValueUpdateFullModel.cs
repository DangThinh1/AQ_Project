using System.Collections.Generic;

namespace YachtMerchant.Core.Models.YachtTourAttributeValues
{
    public class YachtTourAttributeValueUpdateFullModel
    {
        public string TourId { get; set; }
        public List<int> ListAttributeId { get; set; }
        public List<string> ListAttributeValue { get; set; }
    }
}