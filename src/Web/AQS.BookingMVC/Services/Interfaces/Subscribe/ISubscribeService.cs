using APIHelpers.Response;
using AQBooking.Admin.Core.Models.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Subscribe
{
    public interface ISubscribeService
    {
        Task<BaseResponse<bool>> Subscribe(SubscriberCreateModel createModel);
    }
}
