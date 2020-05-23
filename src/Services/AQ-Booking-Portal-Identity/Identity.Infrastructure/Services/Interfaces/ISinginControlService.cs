using APIHelpers.Response;
using System.Collections.Generic;
using Identity.Core.Portal.Models.SigninControls;

namespace Identity.Infrastructure.Services.Interfaces
{
    public interface ISinginControlService
    {
        BaseResponse<List<SigninControlViewModel>> FindByDomainUid(string domainUid);
    }
}