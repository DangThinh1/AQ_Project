using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharters
{
    public class GetTotalReservationOfMerchantModel
    {
        public int MerchantId { get; set; }
        public string EffectiveStartDate { get; set; }
    }
}
