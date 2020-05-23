using APIHelpers.Request;
using AQS.BookingMVC.Services.Interfaces;
using Identity.Core.Models.Users;

namespace AQS.BookingMVC.Services.Implements
{
    public class UserService : ServiceBase, IUserService
    {

        public UserService() : base()
        {

        }

        public bool Resigster(UserCreateModel registerModel)
        {
            var req = new BaseRequest<UserCreateModel>(registerModel);
            var apiResult = _apiExcute.PostData<object, UserCreateModel>("http://localhost:71/api/Accounts/Register", req, _token).Result;
            if (!apiResult.IsSuccessStatusCode)
                return apiResult.IsSuccessStatusCode;
            else
                return apiResult.IsSuccessStatusCode;
        }
    }
}
