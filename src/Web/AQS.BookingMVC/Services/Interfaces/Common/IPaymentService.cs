using APIHelpers.Response;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Common
{
    public interface IPaymentService
    {
        Task<BaseResponse<string>> GetToken();

        Task<BaseResponse<TResponse>> CreateTransactionInformation<TRequest, TResponse>(TRequest request, string aqToken);

        Task<BaseResponse<TResponse>> GetTokenFromApiPayment<TRequest, TResponse>(TRequest request, string aqToken);

        Task<BaseResponse<TResponse>> ProcessPayment<TRequest, TResponse>(TRequest request, string aqToken, string paymentType);

        Task<BaseResponse<TResponse>> GetTransaction<TResponse>(string transactionId, string apToken);
    }
}
