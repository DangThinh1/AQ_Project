using AQBooking.Admin.Core.Models.YachtTourCategory;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtTourCategoryService
    {
        IPagedList<YachtTourCategoryViewModel> Search(YachtTourCategorySearchModel model);
        bool Create(YachtTourCategoryCreateModel model);
        bool Update(YachtTourCategoryCreateModel model);
        bool IsActivated(int id, bool value);
        YachtTourCategoryCreateModel FindInfoDetailById(long id);
        bool Delete(int id);
        List<YachtTourCategorySupportModel> GetInfoDetailSupportedByTourCategoryId(int tourCateId);
    }
}