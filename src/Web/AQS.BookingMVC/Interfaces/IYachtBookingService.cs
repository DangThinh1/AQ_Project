using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Interfaces
{
    public interface IYachtBookingService
    {
        public YachtCharteringViewModel GetCharterByReservationEmail(string email);
        public bool SaveYachtBookingtoRedis(RedisCachesModel yachtBookingModel);
    }
}
