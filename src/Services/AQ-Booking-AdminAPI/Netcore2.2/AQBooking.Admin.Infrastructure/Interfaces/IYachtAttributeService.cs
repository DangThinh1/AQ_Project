using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtAttribute;
using AQBooking.Admin.Core.Paging;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtAttributeService
    {
        IPagedList<YachtAttributeViewModel> SearchYachtAttributes(YachtAttributeSearchModel searchModel);
        YachtAttributeCreateModel GetYachtAttributeById(int id);
        Task<BasicResponse> CreateOrUpdateYachtAttribute(YachtAttributeCreateModel model);
        Task<BasicResponse> DeleteYachtAttribute(int id);
    }
}
