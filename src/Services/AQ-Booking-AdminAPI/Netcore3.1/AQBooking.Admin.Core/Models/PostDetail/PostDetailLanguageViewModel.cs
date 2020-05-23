using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PostDetail
{
   public class PostDetailLanguageViewModel
    {
        public int LanguageId { get; set; }
        public bool IsActivated { get; set; }
        public long PostDetailId { get; set; }
    }
}
