using APIHelpers.Response;
using System.Collections.Generic;
using Identity.Core.Portal.Models.SSOAuthentication;
using Identity.Core.Models.Authentications;

namespace Identity.Core.Portal.Services.Interfaces
{
    public interface ISSOAuthenticationRequestService
    {
        BaseResponse<bool> DeleteById(string id);
        BaseResponse<string> Create(List<SSOAuthenticationCreateModel> createModel);
        BaseResponse<SSOAuthenticationViewModel> FindByDomainId(string id, string domainId);
        BaseResponse<List<SSOAuthenticationViewModel>> FindById(string id);
        BaseResponse<bool> Clear(string uid);
        BaseResponse<AuthenticateViewModel> GetProfile(string authId, string domainId);
    }
}