using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.PortLocation
{
    public class PortLocationViewModel : PortLocationUpdateModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
