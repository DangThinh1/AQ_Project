using APIHelpers.Response;
using AQBooking.Admin.Core.Models.Subscriber;
using AQS.BookingAdmin.Infrastructure.AQPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.Posts
{
   public interface IEmailSubcriberService
    {
        Task<BaseResponse<PagedListClient<SubscriberViewModel>>> SearchEmailSubcriber(SubscriberSearchModel searchModel);
        Task<List<SubscriberViewModel>> GetLstSubcriToExport(SubscriberSearchModel searchModel);
    }
}
