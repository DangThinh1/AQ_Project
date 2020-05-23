using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PostCategories
{
    public class PostCategoryDetailCreateModel
    {
     
        public int Id { get; set; }
        public int PostCategoryFid { get; set; }
        public string Name { get; set; }
        public int LanguageFid { get; set; }      
        public bool IsActivated { get; set; }
        public string FriendlyUrl { get; set; }

    }
}
