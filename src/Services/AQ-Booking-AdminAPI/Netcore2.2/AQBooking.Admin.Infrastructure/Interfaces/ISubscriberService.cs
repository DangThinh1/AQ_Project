using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.Subscriber;
using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface ISubscriberService
    {
        ApiActionResult CreateNewSubscriber(SubscriberCreateModel model);
        IPagedList<SubscriberViewModel> SearchSubscriber(SubscriberSearchModel searchModel);
    }
}
