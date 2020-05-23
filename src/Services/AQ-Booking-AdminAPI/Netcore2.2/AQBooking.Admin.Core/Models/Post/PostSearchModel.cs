using AQBooking.Admin.Core.Paging;
using System;
namespace AQBooking.Admin.Core.Models.Post
{
    public class PostSearchModel : PagableModel
    {
        public string DefaultTitle { get; set; }
        public short TimeToRead { get; set; }
        public bool Deleted { get; set; }
        public bool? IsActivated { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }

    }
}
