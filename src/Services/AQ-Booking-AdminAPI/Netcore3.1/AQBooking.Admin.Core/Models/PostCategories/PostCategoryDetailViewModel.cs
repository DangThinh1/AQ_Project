using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PostCategories
{
    public class PostCategoryDetailViewModel
    {
        public int Id { get; set; }
        public int PostCategoryFid { get; set; }
        public string Name { get; set; }
        public int LanguageFid { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
    }
}
