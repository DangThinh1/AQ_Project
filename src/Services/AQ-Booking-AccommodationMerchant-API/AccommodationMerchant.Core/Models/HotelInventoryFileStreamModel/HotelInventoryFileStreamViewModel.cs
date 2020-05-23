using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationMerchant.Core.Models.HotelInventoryFileStreamModel
{
    public class HotelInventoryFileStreamViewModel : HotelInventoryFileStreamCreateUpdateModel
    {
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
