using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Conts;
using AQConfigurations.Core.Models.CommonResources;
using APIHelpers.Response;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class CommonResourcesController : ControllerBase
    {
        private readonly ICommonResourceService _commonResourcesServices;

        public CommonResourcesController(ICommonResourceService commonResourcesServices)
        {
            _commonResourcesServices = commonResourcesServices;
        }

        [HttpPost("CommonResources/{langId}")]
        public IActionResult GetListAsync(int langId, [FromBody]List<string> type = null)
        {
            if (type == null)
                type = new List<string>() { "COMMON" };
            var result = _commonResourcesServices.GetAllResource(langId, type);
            return Ok(result);
        }

        [HttpPut("CommonResources")]
        public IActionResult Update(CommonResourceUpdateModel model)
        {
            var entry = _commonResourcesServices.GetResourceValue(model.LanguageFid, model.ResourceKey);
            var response = BaseResponse<bool>.BadRequest();
            if (entry.IsSuccessStatusCode)
                response = _commonResourcesServices.Update(model);
            else
                response = _commonResourcesServices.Create(model);
            return Ok(response);
        }

        [HttpPost("CommonResources/ResourceValue/{langId}")]
        public IActionResult GetResourceValue(int langId, [FromBody] string resourceKey)
        {
            var result = _commonResourcesServices.GetResourceValue(langId, resourceKey);
            return Ok(result);
        }
    }
}