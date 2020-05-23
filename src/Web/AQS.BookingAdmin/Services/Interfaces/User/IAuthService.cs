using AQBooking.Admin.Core.Models.AuthModel;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.User
{
    public interface IAuthService
    {
        Task<LoginResultModel> AuthorizeUser(LoginModel model);
        Task<UserInfoModel> GetUser(string token);
    }
}
