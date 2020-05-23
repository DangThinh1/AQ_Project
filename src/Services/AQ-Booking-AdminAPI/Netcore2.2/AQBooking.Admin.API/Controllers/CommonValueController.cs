using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.CommonValue;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommonValueController : ControllerBase
    {
        private readonly ICommonValueService _commonValueService;
        public CommonValueController(ICommonValueService commonValueService)
        {
            _commonValueService = commonValueService;
        }
        [Route("SearchCommonValue")]
        [HttpGet]
        public IActionResult SearchCommonValue([FromQuery]CommonValueSearchModel model)
        {
            try
            {
                var res = _commonValueService.GetAllCommonValuesPaging(model);
                if (res != null)
                    return Ok(res);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [Route("CommonValues")]
        [HttpGet]
        public IActionResult GetAllCommonValue()
        {
            try
            {
                var res = _commonValueService.GetAllCommonValues();
                if (res.Any())
                    return Ok(res);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [Route("CommonValue")]
        [HttpPost]
        public IActionResult CreateCommonValue([FromBody]CommonValueCreateModel model)
        {
            try
            {
                var flag = _commonValueService.CreateNewCommonValues(model);
                if (flag.Succeeded)
                    return Ok(flag);

                return BadRequest(flag.Errors[0].Description);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [Route("CommonValue")]
        [HttpPut]
        public IActionResult UpdateCommonValue([FromBody]CommonValueUpdateModel model)
        {
            try
            {
                var flag = _commonValueService.UpdateCommonValues(model);
                if (flag.Succeeded)
                    return Ok(flag);

                return BadRequest(flag.Errors[0].Description);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        [Route("CommonValue/{Id}")]
        [HttpGet]
        public IActionResult GetByIdCommonValue(int Id)
        {
            var res = _commonValueService.GetById(Id);
            if (res != null)
                return Ok(res);
            return NotFound();
        }
        [Route("CommonValue/GetListStringDDL")]
        [HttpGet]
        public IActionResult GetValueGroupLst()
        {
            var res = _commonValueService.GetValueGroupDDL();
            if (res != null)
                return Ok(res);
            return NotFound();
        }
        [Route("CommonValue/GetAllYachtAttributeCategory")]
        [HttpGet]
        public IActionResult GetAllYachtAttributeCategory()
        {
            var res = _commonValueService.GetAllYachtAttributeCategory();
            if (res != null)
                return Ok(res);
            return NotFound();
        }
    }
}