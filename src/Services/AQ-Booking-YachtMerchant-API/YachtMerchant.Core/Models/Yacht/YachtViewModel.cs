using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.Yacht
{
    public class YachtViewModel : YachtUpdateModel
    {
        public string UniqueId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
