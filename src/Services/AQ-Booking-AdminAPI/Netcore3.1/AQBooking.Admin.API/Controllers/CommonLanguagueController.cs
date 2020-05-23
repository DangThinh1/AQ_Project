using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.CommonLanguague;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommonLanguagueController : ControllerBase
    {
        private readonly ICommonLanguagueService _commonLanguagueService;
        public CommonLanguagueController(ICommonLanguagueService commonLanguagueService)
        {
            _commonLanguagueService = commonLanguagueService;
        }
        [Route("SearchCommonLanguague")]
        [HttpGet]
        public IActionResult SearchCommonLanguague([FromQuery]CommonLanguaguesSearchModel model)
        {
            try
            {
                var res = _commonLanguagueService.GetAllCommonLangsPaging(model);
                if (res != null)
                    return Ok(res);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [Route("CommonLanguague")]
        [HttpPut]
        public IActionResult UpdateValues([FromBody]CommonLanguaguesUpdateModel model)
        {
            try
            {
                var res = _commonLanguagueService.Update(model);
                if (!res.Succeeded)
                    return NoContent();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        
        [Route("CommonLanguague/{Id}")]
        [HttpGet]
        public IActionResult GetById(int Id)
        {
            var res = _commonLanguagueService.GetById(Id);
            if (res != null)
                return Ok(res);
            return NoContent();
        }

        [Route("CommonLanguague")]
        [HttpGet]
        public IActionResult GetallCommonLang()
        {
            var res = _commonLanguagueService.GetAll();
            if (!res.Any())
                return NotFound();
            return Ok(res);
        }

        [Route("CommonLanguague")]
        [HttpPost]
        public IActionResult CreateNewValues([FromBody]CommonLanguaguesCreateModel model)
        {
            var res = _commonLanguagueService.Create(model);
            if (!res.Succeeded)
                return NotFound();
            return Ok(res);
        }

        [Route("CommonLanguague/GetListStringDDL/{param}")]
        [HttpGet]
        public IActionResult GetLangDDL(string param)
        {
            //HttpContext.Request
            var res = _commonLanguagueService.GetLangLstDDL(param);
            if (res != null)
                return Ok(res);
            return NotFound();
        }
    }
}