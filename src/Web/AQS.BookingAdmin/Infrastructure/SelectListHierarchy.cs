using AQBooking.Admin.Core.Models.PostCategories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Infrastructure
{
  public static class SelectListHierarchy
  {
    public static List<PostCategoriesViewModel> GetChildren(this List<PostCategoriesViewModel> sources, int parentId, string parentName = null)
    {
      return sources
               .Where(c => c.ParentFid == parentId)
               .Select(c => new PostCategoriesViewModel
               {
                 Id = c.Id,
                 DefaultName = parentName + " >> " + c.DefaultName,
                 ParentFid = c.ParentFid,
                 ChildrenLst = GetChildren(sources, c.Id, parentName + " >> " + c.DefaultName)
               })
               .ToList();
    }
    public static List<SelectListItem> HierarchyHandle(List<PostCategoriesViewModel> parentSources, List<SelectListItem> list = null)
    {
      if (list == null) list = new List<SelectListItem>();
      if (parentSources != null)
      {
        foreach (var item in parentSources)
        {
          list.Add(new SelectListItem { Text = item.DefaultName, Value = item.Id.ToString() });
          HierarchyHandle(item.ChildrenLst, list);
        }
      }
      return list;
    }
  }
}
