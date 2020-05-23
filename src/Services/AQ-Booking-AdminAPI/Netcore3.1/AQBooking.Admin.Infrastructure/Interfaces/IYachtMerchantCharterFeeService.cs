using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtMerchantCharterFee;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtMerchantCharterFeeService
    {
        List<YachtMerchantCharterFeeViewModel> GetAllYachtMerchantCharterFee();
        List<YachtMerchantCharterFeeViewModel> GetYachtMerchantCharterFeeByMerchantId(int merchantId);
        YachtMerchantCharterFeeViewModel GetYachtMerchantCharterFeeById(int id);
        BasicResponse CreateYachtMerchantCharterFee(YahctMerchantCharterFeeCreateModel model);
        BasicResponse UpdateYachtMerchantCharterFee(YachtMerchantCharterFeeUpdateModel model);
        BasicResponse DeleteYachtMerchantCharterFee(int id);
    }
}
