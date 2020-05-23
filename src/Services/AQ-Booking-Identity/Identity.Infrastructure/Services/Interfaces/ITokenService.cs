using APIHelpers.Response;
using Identity.Core.Models.Authentications;
namespace Identity.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        BaseResponse<AuthenticateViewModel> DecodeJwtToken(string token);
        BaseResponse<string> GenerateJwtToken(AuthenticateViewModel authModel, int expiredInMinute);
        BaseResponse<AuthenticateViewModel> GenerateJwtTokenModel(AuthenticateViewModel authModel, int expiredInMinute);
    }
}
