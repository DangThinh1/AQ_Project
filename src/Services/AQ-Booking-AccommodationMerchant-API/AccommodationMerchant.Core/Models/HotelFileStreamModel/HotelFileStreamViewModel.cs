using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationMerchant.Core.Models.HotelFileStreamModel
{
    public class HotelFileStreamViewModel : HotelFileStreamCreateUpdateModel
    {
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
