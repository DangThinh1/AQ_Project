using AQBooking.YachtPortal.Core.Models.Yachts;
using AQS.BookingMVC.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Areas.Yacht.Models
{
    public class YachtPassParamsModel
    {
        public YachtPassParamsModel()
        {

        }
        public YachtPassParamsModel(double price, int pricingTypeId, YachtSearchModel searchModel)
        {
            CheckIn = searchModel.CheckIn;
            CheckOut = searchModel.CheckOut;
            City = searchModel.City;
            Passengers = searchModel.Passengers;
            YachtName = searchModel.YachtName;
            YachtTypeFID = searchModel.YachtTypeFID;
            Price = price;
            PricingTypeId = pricingTypeId;
            PortFID = searchModel.PortFID;
            Country = searchModel.Country;
            

        }
        public YachtSearchModel GetYachtSearchModel()
        {
            return new YachtSearchModel
            {
                CheckIn = CheckIn,
                CheckOut = CheckOut,
                City = City,
                Passengers = Passengers,
                YachtName = YachtName,
                YachtTypeFID = YachtTypeFID
            };
        }
        public string EncryptData()
        {
            return CommonHelper.EncryptObj(this);
        }
        //public static YachtPassParamsModel GetModelFromEncryptString(string encryptedStr)
        //{
        //    return CommonHelper.DecryptObj<YachtPassParamsModel>(encryptedStr);
        //}
        public string YatchId { get;internal set; }
        public int Passengers { get; set; }

        public string CheckOut { get; set; }
        public string CheckIn { get; set; }

        public int PortFID { get; set; }

        public int YachtTypeFID { get; set; }
        public string City { get; set; }

        public string Country { get; set; }
        public string YachtName { get; set; }

        public double Price { get; set; }
        public double PricingTypeId { get; set; }
    }
}
