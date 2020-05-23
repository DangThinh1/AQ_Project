using APIHelpers.Response;

namespace AQBooking.Identity.Responses
{
    public class OkResponse<T> : BaseResponse<T>
    {
        private const string _message = "Success";

        public OkResponse(T data, string message = _message)
        {
            StatusCode = System.Net.HttpStatusCode.OK;
            ResponseData = data;
            Message = message;
        }
    }
}
