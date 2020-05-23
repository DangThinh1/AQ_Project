using APIHelpers.Response;
namespace Identity.Infrastructure.Services.Interfaces
{
    public interface ISignInService
    {
        BaseResponse<bool> IsAllowedToken(string token);
        BaseResponse<bool> SignOutAllDevice(string key);
    }
}
