using AQBooking.YachtPortal.Core.Enum;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQBooking.YachtPortal.Core.Helpers
{
    public static class GlobalMethod
    {
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //*****using for calculation fee on Service and Payment page.
        public static int BookingDayNumber(string checkIn, string checkOut)
        {
            int dayNumber = 0;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(checkIn) && !string.IsNullOrWhiteSpace(checkOut))
            {
                fromDate = DateTime.Parse(checkIn);
                toDate = DateTime.Parse(checkOut);
            }
            dayNumber = (toDate.Date.Subtract(fromDate.Date)).Days;
            dayNumber += 1;
            return dayNumber;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //*****using for calculation fee on Service and Payment page.
        public static void GetPriceRecuse(List<YachtPricingPlanDetailViewModel> lstprice, int daynumber, ref double total)
        {
            int surDay = 0;
            YachtPricingPlanDetailViewModel objPricew = lstprice.Where(x => x.RealDayNumber <= daynumber).OrderByDescending(x => x.RealDayNumber).FirstOrDefault();
            if (objPricew != null)
            {
                surDay = daynumber % objPricew.RealDayNumber;
                int inDay = daynumber / objPricew.RealDayNumber;
                total += inDay * objPricew.Price;
                if (surDay > 0)
                {
                    GetPriceRecuse(lstprice, surDay, ref total);
                }
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //*****using for calculation fee on Service and Payment page.
        public static double PackageTotal(double price, int quantity)
        {
            return price * quantity;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //*****using on Search,Page Slide, Service ,Payment pages.
        public static List<int> GetYachtSoldOutStatus()
        {
            List<int> lstStatusId = new List<int> {
                     Convert.ToInt32(YachtCharterStatusEnum.Paid)
                    ,Convert.ToInt32(YachtCharterStatusEnum.Pending)
                    ,Convert.ToInt32(YachtCharterStatusEnum.Accepted) };
            return lstStatusId;
        }
    }
}
