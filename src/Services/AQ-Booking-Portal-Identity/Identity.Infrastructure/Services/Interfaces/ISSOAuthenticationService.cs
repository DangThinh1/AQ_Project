using APIHelpers.Response;
using System.Collections.Generic;
using Identity.Core.Portal.Models.SSOAuthentication;

namespace Identity.Infrastructure.Services.Interfaces
{
    public interface ISSOAuthenticationService
    {
        BaseResponse<bool> DeleteById(string id);
        BaseResponse<SSOAuthenticationViewModel> FindByDomainId(string id, string domainId);
        BaseResponse<string> Create(List<SSOAuthenticationCreateModel> createModel);
        BaseResponse<List<SSOAuthenticationViewModel>> FindById(string id);
        BaseResponse<bool> DeleteByUserUid(string uid);
    }
}