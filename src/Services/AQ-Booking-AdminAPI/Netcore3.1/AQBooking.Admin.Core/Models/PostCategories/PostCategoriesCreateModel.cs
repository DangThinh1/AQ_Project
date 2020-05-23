using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PostCategories
{
    public class PostCategoriesCreateModel
    {
        #region Field
        public int Id { get; set; }      
        public string DefaultName { get; set; }
        public int? ParentFid { get; set; }
        public double? OrderBy { get; set; }
        public bool IsActivated { get; set; }
        public int LanguageFid { get; set; }
        public string Name { get; set; }
        public int PostCateDetailId { get; set; }
        public List<PostCategoryDetailCreateModel> postCateDetails { get; set; }
        #endregion
        #region Ctor
        public PostCategoriesCreateModel()
        {
            postCateDetails = new List<PostCategoryDetailCreateModel>();
        }
        #endregion
    }
}
