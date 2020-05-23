using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtCharteringPaymentLogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtCharteringPaymentLogService
    {
        BaseResponse<YachtCharteringPaymentLogViewModel> GetCharteringPaymentLogBycharteringFId(string charteringFId, int statusFId = 1);
        BaseResponse<YachtCharteringPaymentLogViewModel> GetCharteringPaymentLogByCharteringUniqueId(string charteringUniqueId, int statusFId = 1);
        BaseResponse<YachtCharteringPaymentLogViewModel> UpdateCharterPrivatePaymentLog(YachtCharteringPaymentLogViewModel paymentNewModel);
        BaseResponse<YachtCharteringPaymentLogViewModel> UpdateCharterPrivatePaymentLogByCharteringUniqueId(YachtCharteringPaymentLogViewModel paymentNewModel, string charteringUniqueId);
    }
}
