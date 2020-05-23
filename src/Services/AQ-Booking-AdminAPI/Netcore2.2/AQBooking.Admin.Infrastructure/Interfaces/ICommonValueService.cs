using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.CommonValue;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface ICommonValueService
    {
        ApiActionResult CreateNewCommonValues(CommonValueCreateModel model);
        List<CommonValues> GetAllCommonValues();
        ApiActionResult UpdateCommonValues(CommonValueUpdateModel model);
        Task<bool> DeleteCommonValues(int Id);
        CommonValues GetById(int Id);
        IPagedList<CommonValueViewModel> GetAllCommonValuesPaging(CommonValueSearchModel model);
        List<string> GetValueGroupDDL();
        List<CommonValues> GetAllYachtAttributeCategory();

    }
}