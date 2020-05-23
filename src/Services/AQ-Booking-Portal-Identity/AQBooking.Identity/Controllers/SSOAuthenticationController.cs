using AQEncrypts;
using APIHelpers.Response;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Identity.Core.Models.Authentications;
using Identity.Infrastructure.Services.Interfaces;
using Identity.Core.Portal.Models.SSOAuthentication;

namespace Identity.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class SSOAuthenticationController : ControllerBase
    {
        private readonly ISSOAuthenticationService _sSOAuthenticationService;

        public SSOAuthenticationController(ISSOAuthenticationService sSOAuthenticationService)
        {
            _sSOAuthenticationService = sSOAuthenticationService;
        }

        [HttpPost("SSOAuthentication")]
        public IActionResult Create([FromBody]List<SSOAuthenticationCreateModel> createModel)
        {
            var result = _sSOAuthenticationService.Create(createModel);
            return Ok(result);
        }

        [HttpGet("SSOAuthentication/Profile/AuthId/{authId}/DomainId/{domainId}")]
        public IActionResult GetProfileByAuthId(string authId, string domainId)
        {
            var authIdDecrypted = authId.Decrypt();
            var result = _sSOAuthenticationService.FindByDomainId(authIdDecrypted, domainId);
            if(result.IsSuccessStatusCode)
            {
                var authModel = JwtTokenHelper.DecodeJwtToken(result.ResponseData.Token);
                if(authModel!= null)
                {
                    authModel.AccessToken = result.ResponseData.Token;
                    return Ok(BaseResponse<AuthenticateViewModel>.Success(authModel));
                }
                    
            }
            return Ok(BaseResponse<string>.BadRequest());
        }

        [HttpGet("SSOAuthentication/Id/{id}")]
        public IActionResult FindById(string id)
        {
            var result = _sSOAuthenticationService.FindById(id);
            return Ok(result);
        }

        [HttpGet("SSOAuthentication/Id/{id}/DomainId/{domainid}")]
        public IActionResult FindByDomainId(string id, string domainid)
        {
            var result = _sSOAuthenticationService.FindByDomainId(id, domainid);
            return Ok(result);
        }

        [HttpDelete("SSOAuthentication/Id/{id}")]
        public IActionResult DeleteById(string id)
        {
            var result = _sSOAuthenticationService.DeleteById(id);
            return Ok(result);
        }

        [HttpDelete("SSOAuthentication/UserUid/{uid}")]
        public IActionResult Clear(string uid)
        {
            var result = _sSOAuthenticationService.DeleteByUserUid(uid);
            return Ok(result);
        }
    }
}