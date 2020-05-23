using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQBooking.YachtPortal.Core.Models.Yachts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtCharteringService
    {
        BaseResponse<List<YachtCharteringDetailViewModel>> GetCharteringDetailByCharteringFId(string charteringFId);
        BaseResponse<YachtCharteringViewModel> GetCharteringByCharteringFId(string charteringFId);
        BaseResponse<YachtCharteringViewModel> GetCharteringByUniqueId(string uniqueId);
        BaseResponse<SaveCharterPaymentResponseViewModel> UpdateStatusCharterPrivatePayment(CharteringUpdateStatusModel charteringModel);
        BaseResponse<YachtCharteringViewModel> GetChartering(YachtCharteringRequestModel RequestModel);
        /*** Using in CartPayment page**/
        BaseResponse<SaveCharterPaymentResponseViewModel> SaveChartering(SaveBookingRequestModel requestModel, string PaymentMethod);
        BaseResponse<YachtCharteringViewModel> GetCharterByReservationEmail(string email);
    }
}
