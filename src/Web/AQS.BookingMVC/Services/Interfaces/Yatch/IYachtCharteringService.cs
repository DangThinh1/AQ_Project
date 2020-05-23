using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtCharteringPaymentLogs;
using AQS.BookingMVC.Areas.Yacht.Models;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Yatch
{
    public interface IYachtCharteringService
    {
        Task<BaseResponse<SaveCharterPaymentResponseViewModel>> SaveCharterInformation(YachtSavePackageServiceModel request, string paymentMethod);

        Task<BaseResponse<TResponse>> GetCharterInformation<TResponse>(string uniqueId);

        Task<BaseResponse<SaveCharterPaymentResponseViewModel>> UpdateCharterStatus(string charteringUniqueId, int status);

        Task<BaseResponse<YachtCharteringPaymentLogViewModel>> GetCharteringPaymentLogByCharteringUniqueId(string charteringUniqueId, int statusFid);

        Task<BaseResponse<YachtCharteringPaymentLogViewModel>> UpdateCharterPrivatePaymentLogByCharteringUniqueId(YachtCharteringPaymentLogViewModel paymentNewModel, string charteringUniqueId, string apiUrl = "");
    }
}
