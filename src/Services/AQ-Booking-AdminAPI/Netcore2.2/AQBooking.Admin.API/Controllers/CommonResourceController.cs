using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.CommonResource;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommonResourceController : ControllerBase
    {
        private readonly ICommonResourceService _commonResourcesServices;

        public CommonResourceController(ICommonResourceService commonResourcesServices)
        {
            _commonResourcesServices = commonResourcesServices ?? throw new ArgumentNullException(nameof(commonResourcesServices));
        }

        [Route("CommonResource")]
        [HttpGet]
        public IActionResult GetallCommonResources()
        {
            var res = _commonResourcesServices.GetAll();
            if (res.Any())
                return Ok(res);
            return NotFound();
        }

        [Route("CommonResource")]
        [HttpPost]
        public IActionResult CreateNewValues([FromBody]CommonResourcesCreateModel model)
        {
            var res = _commonResourcesServices.Create(model);
            if (!res.Succeeded)
                return NotFound();
            return Ok(res);
        }

        [Route("CommonResource")]
        [HttpPut]
        public IActionResult UpdateValues([FromBody]CommonResourcesUpdateModel model)
        {
            var res = _commonResourcesServices.Update(model);
            if (!res.Succeeded)
                return BadRequest("Can not update data");
            return Ok(res);
        }

        [Route("CommonResourceById")]
        [HttpGet]
        public IActionResult GetById([FromQuery]string resKey,[FromQuery] int langId)
        {
            //Remove ResourceId => can not search by Id
            var res = _commonResourcesServices.GetById(resKey, langId);
            if (res != null)
                return Ok(res);
            return BadRequest("Not found data");
        }

        [Route("SearchCommonResources")]
        [HttpGet]
        public IActionResult SearchResources([FromQuery]CommonResourcesSearchModel model)
        {
            try
            {
                var result = _commonResourcesServices.GetAllCommonResourcesPaging(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPost]
        [Route("CommonResourcesMultiLang/{langId}")]
        public IActionResult GetListAsync(int langId, [FromBody]List<string> type = null)
        {
            var result = _commonResourcesServices.GetLstResource(langId, type);
            if (result != null)
                return Ok(result);
            return NoContent();
        }

        [HttpPost]
        [Route("CommonResourcesLst")]
        public IActionResult CreateLst([FromBody]List<CommonResourcesCreateModel> model)
        {
            var res = _commonResourcesServices.CreateNewList(model);
            if (!res.Succeeded)
                return BadRequest("Can not create new data");
            return Ok(res);
        }

        [HttpGet]
        [Route("CommonResourceByLang/{langId}")]
        public IActionResult GetLstByLangID(int langId)
        {
            var res = _commonResourcesServices.GetLstByLangId(langId);
            if (res != null)
                return Ok(res);
            return BadRequest("Can not create new data");
        }

        [HttpPost]
        [Route("CommonResourceDelete")]
        public IActionResult Delete([FromBody]CommonResourcesCreateModel model)
        {
            var res = _commonResourcesServices.Delete(model);
            if (res != null)
                return Ok(res);
            return BadRequest("Delete failed!!");
        }
    }
}