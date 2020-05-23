using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Posts
{
  public class PostCategoryCreateModel
  {
    public string Name { get; internal set; }
    public List<SelectListItem> Languages { get; set; }
    public bool IsActived { get; set; }
    public int OrderBy { get; set; }
  }
}
