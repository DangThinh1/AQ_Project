using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.PortalLocation;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IPortalLocationService
    {
        IPagedList<PortalLocationViewModel> SearchPortalLocation(PortalLocationSearchModel model);

        PortalLocationViewModel GetPortalLocationById(int id);

        Task<BasicResponse> CreatePortalLocation(PortalLocationCreateModel model);

        Task<BasicResponse> UpdatePortalLocation(PortalLocationCreateModel model);

        Task<BasicResponse> DeletePortalLocation(int id);

        Task<BasicResponse> DisablePortalLocation(int id);
    }
}