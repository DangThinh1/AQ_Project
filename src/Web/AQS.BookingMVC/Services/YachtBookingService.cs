using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQS.BookingMVC.Interfaces;
using Identity.Core.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services
{
    public class YachtBookingService : ServiceBase, IYachtBookingService
    {
        #region Fields

        #endregion

        #region Ctor
        public YachtBookingService() : base()
        {

        }
        #endregion

        #region Methods
        public YachtCharteringViewModel GetCharterByReservationEmail(string email)
        {
            //var req = new BaseRequest<UserCreateModel>(registerModel);
            var apiResult = _apiExcute.GetData<YachtCharteringViewModel>($"http://localhost:220/api/Yachts/YachtCharterings/GetCharterByReservationEmail?email={email}", null, _token).Result;
            if (apiResult.IsSuccessStatusCode)
                return apiResult.ResponseData;
            else
                return null;
        }

        public bool SaveYachtBookingtoRedis(RedisCachesModel yachtBookingModel)
        {
            var requestModel = new BaseRequest<RedisCachesModel>(yachtBookingModel);
            var apiResult = _apiExcute.PostData<object, RedisCachesModel>("http://localhost:220/api/RedisCache/RedisCache/SimpleKey/Model/Set", requestModel, _token).Result;
            if (apiResult.IsSuccessStatusCode) return true;
            else return false;
        }
        #endregion

    }
}
