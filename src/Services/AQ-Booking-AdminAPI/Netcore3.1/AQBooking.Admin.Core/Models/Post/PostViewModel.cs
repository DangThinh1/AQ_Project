using System;
using System.Collections.Generic;
namespace AQBooking.Admin.Core.Models.Post
{

    public class PostViewModel
    {
        #region Ctor
        public PostViewModel()
        {
            CustomProperties = new Dictionary<string, object>();
        }
        #endregion

        #region Fields
        public long PostID { get; set; }
        public int PostCategoryFID { get; set; }
        public string DefaultTitle { get; set; }
        public string Title { get; set; }
        public string PostCategoryName { get; set; }
        public short TimeToRead { get; set; }
        public int FileStreamFID { get; set; }
        public int FileTypeFID { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActivated { get; set; }
        public bool Deleted { get; set; }
        public string FriendlyUrl { get; set; }
        public long PostDetailID { get; set; }
        public int TotalRows { get; set; }
        public string ShortDescription { get; set; }
        public Dictionary<string,object> CustomProperties { get; set; }
        #endregion

    }
}
