using Accommodation.Core.Models.HotelInformationDetails;
using System.Collections.Generic;
using System;

namespace Accommodation.Core.Models.HotelInformations
{
    public class HotelInformationViewModel
    {
        public int Id { get; set; }
        public int HotelFid { get; set; }
        public string UniqueId { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
        public List<HotelInformationDetailViewModel> hotelInforDetailLst { get; set; }
    }
}
