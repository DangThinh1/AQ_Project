using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PostCategories
{
    public class PostCategoriesViewModel
    {
        #region Field
        public int Id { get; set; }
        public string DefaultName { get; set; }
        public int? ParentFid { get; set; }
        public double? OrderBy { get; set; }
        public bool IsActivated { get; set; }
        public List<PostCategoriesViewModel> ChildrenLst { get; set; }
        public Dictionary<string, object> CustomProperties { get; set; }
        #endregion
        #region Ctor
        public PostCategoriesViewModel()
        {
            CustomProperties = new Dictionary<string, object>();
        }
        #endregion


    }
}
