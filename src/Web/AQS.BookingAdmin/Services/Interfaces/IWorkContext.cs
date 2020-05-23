using AQBooking.Admin.Core.Models.AuthModel;

namespace AQS.BookingAdmin.Interfaces.User
{
    public interface IWorkContext
    {
        bool IsAuthentication { get;}
        UserInfoModel CurrentUser { get; }
        string UserToken { get; }
    }
}
