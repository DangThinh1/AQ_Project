using AQBooking.Admin.Core.Models.Post;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Posts
{
    public class PostSearchViewModel:PostSearchModel
    {
        public PostSearchViewModel()
        {
            Categories = new List<SelectListItem>();
            IsDeleted = false;
        }
        public List<SelectListItem> Categories { get; set; }
    }
}
