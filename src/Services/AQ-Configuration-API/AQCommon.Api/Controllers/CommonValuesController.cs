using AQConfigurations.Core.Models.CommonValues;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class CommonValuesController : Controller
    {
        private readonly ICommonValueService _commonValueService;
        public CommonValuesController(ICommonValueService commonValueService)
        {
            _commonValueService = commonValueService;
        }

        #region Old Service => must be remove in the future
        [HttpGet("CommonValues/ValueGroup/{valueGroup}/ValueInt/{valueInt}")]
        public IActionResult GetCommonValueByGroupInt(string valueGroup, int valueInt)
        {
            var result = _commonValueService.GetCommonValueByGroupInt(valueGroup, valueInt);
            return Ok(result);
        }
        [HttpGet("CommonValues/ValueGroup/{valueGroup}/ValueString/{valueString}")]
        public IActionResult GetCommonValueByValueString(string valueGroup, string valueString)
        {
            var result = _commonValueService.GetCommonValueByGroupString(valueGroup, valueString);
            return Ok(result);
        }
        [HttpGet("CommonValues/ValueGroup/{valueGroup}/ValueDouble/{valueDouble}")]
        public IActionResult GetCommonValueByValueDouble(string valueGroup, double valueDouble)
        {
            var result = _commonValueService.GetCommonValueByGroupDouble(valueGroup, valueDouble);
            return Ok(result);
        }
        [HttpGet("CommonValues/ValueGroup/{valueGroup}")]
        public IActionResult GetListCommonValueByGroup(string valueGroup)
        {
            var result = _commonValueService.GetListCommonValueByGroup(valueGroup);
            return Ok(result);
        }
        [HttpGet("CommonValues")]
        public IActionResult GetAllCommonValue()
        {
            var result = _commonValueService.GetAllCommonValue();
            return Ok(result);
        }
        #endregion Old Service => must be remove in the future

        //****************************************Support Lang id*************************************************//
        [HttpGet("CommonValues/ValueGroup/{valueGroup}/ValueInt/{valueInt}/Lang/{lang}")]
        public IActionResult GetCommonValueByGroupInt(string valueGroup, int valueInt, int lang = 1)
        {
            var result = _commonValueService.GetCommonValueByGroupInt(valueGroup, valueInt, lang);
            return Ok(result);
        }
        [HttpGet("CommonValues/ValueGroup/{valueGroup}/ValueString/{valueString}/Lang/{lang}")]
        public IActionResult GetCommonValueByValueString(string valueGroup, string valueString, int lang = 1)
        {
            var result = _commonValueService.GetCommonValueByGroupString(valueGroup, valueString, lang);
            return Ok(result);
        }
        [HttpGet("CommonValues/ValueGroup/{valueGroup}/ValueDouble/{valueDouble}/Lang/{lang}")]
        public IActionResult GetCommonValueByValueDouble(string valueGroup, double valueDouble, int lang = 1)
        {
            var result = _commonValueService.GetCommonValueByGroupDouble(valueGroup, valueDouble, lang);
            return Ok(result);
        }
        [HttpGet("CommonValues/ValueGroup/{valueGroup}/Lang/{lang}")]
        public IActionResult GetListCommonValueByGroup(string valueGroup, int lang = 1)
        {
            var result = _commonValueService.GetListCommonValueByGroup(valueGroup, lang);
            return Ok(result);
        }
        [HttpGet("CommonValues/Lang/{lang}")]
        public IActionResult GetAllCommonValue(int lang = 1)
        {
            var result = _commonValueService.GetAllCommonValue(lang);
            return Ok(result);
        }
        [HttpGet("CommonValues/{id}/Lang/{lang}")]
        public IActionResult Find(int id, int lang = 1)
        {
            var result = _commonValueService.Find(id, lang);
            return Ok(result);
        }
        [HttpPost("CommonValues")]
        public IActionResult Create([FromBody]CommonValueCreateModel model)
        {
            var result = _commonValueService.Create(model);
            return Ok(result);
        }
    }
}