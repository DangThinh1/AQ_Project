using AQBooking.YachtPortal.Core.Models.YachtTourCategory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtTourCategoryService
    {
        List<YachtTourCategoryViewModel> GetAllYachtTourCategory(int langId);
    }
}
