using APIHelpers.Response;
using Identity.Core.Models.Emails;
namespace Identity.Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        BaseResponse<bool> Send(SendMailModel sendModel);
    }
}
