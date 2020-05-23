using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtMerchantFileStream;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtMerchantFileStreamService
    {
        List<YachtMerchantFileStreamViewModel> GetAllYachtMerchantFileStream();
        YachtMerchantFileStreamViewModel GetYachtMerchantFileStreamById(int id);
        List<YachtMerchantFileStreamViewModel> GetYachtMerchantFileStreamByType(int fileType);
        BasicResponse CreateYachtMerchantFileStream(YachtMerchantFileStreamCreateModel model);
        BasicResponse UpdateYachtMerchantFileStream(YachtMerchantFileStreamUpdateModel model);
        BasicResponse DeleteYachtMerchantFileStream(int id);
    }
}
