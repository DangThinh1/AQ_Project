using System;
using System.ComponentModel.DataAnnotations;

namespace AQBooking.Admin.Core.Models.Post
{
    public class PostCreateModel
    {
        #region Field
        public long Id { get; set; }
        
        public int PostCategoryFid { get; set; }
        public string PostCategoryResKey { get; set; }
        [MaxLength(255)]
        public string DefaultTitle { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public short TimeToRead { get; set; }
        public bool IsActivated { get; set; }
        #endregion
    }
       
}
