using AQBooking.Admin.Core.Paging;
using System;
namespace AQBooking.Admin.Core.Models.Post
{
    public class PostSearchModel : PagableModel
    {
        public int? CategoryFID { get; set; }
        public int LanguageFID { get; set; }
        public bool? IsActived { get; set; }
        public Guid? CreatedBy { get; set; }
        public string Title { get; set; }
        public int CurrentPostId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
