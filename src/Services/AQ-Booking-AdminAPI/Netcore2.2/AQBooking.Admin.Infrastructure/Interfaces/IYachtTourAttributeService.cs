using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtTourAttribute;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtTourAttributeService
    {
        IPagedList<YachtTourAttributeViewModel> SearchYachtTourAttributes(YachtTourAttributeSearchModel searchModel);
        YachtTourAttributeCreateModel GetYachtTourAttributeById(int id);
        Task<BasicResponse> CreateOrUpdateYachtTourAttribute(YachtTourAttributeCreateModel model);
        Task<BasicResponse> DeleteYachtTourAttribute(int id);
    }
}
