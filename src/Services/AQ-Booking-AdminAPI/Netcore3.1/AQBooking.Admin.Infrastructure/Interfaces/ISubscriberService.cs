using APIHelpers.Response;
using AQBooking.Admin.Core.Models.Subscriber;
using AQBooking.Admin.Core.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface ISubscriberService
    {
        Task<BaseResponse<bool>> CreateNewSubscriber(SubscriberCreateModel model);
        IPagedList<SubscriberViewModel> SearchSubscriber(SubscriberSearchModel searchModel);
        List<SubscriberViewModel> GetListSubToExport(SubscriberSearchModel searchModel);
    }
}
