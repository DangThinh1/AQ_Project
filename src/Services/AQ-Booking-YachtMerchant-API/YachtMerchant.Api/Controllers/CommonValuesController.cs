using AQBooking.Core.Helpers;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YachtMerchant.Api.Controllers
{
    [AllowAnonymous]
    [Route("api")]
    [ApiController]
    public class CommonValuesController : ControllerBase
    {
        private readonly ICommonValueService _commonValueService;

        public CommonValuesController(ICommonValueService commonValueService)
        {
            _commonValueService = commonValueService;
        }

        /// <summary>
        /// Get Common value
        /// </summary>
        /// <param name="valueGroup"> Value group in string format </param>
        /// <param name="valueInt">Value Int in int format</param>
        /// <returns>CommonValue{Id, UniqueId, ValueGroup, ValueInt, ValueString, ValueDouble}</returns>
        [HttpGet]
        [Route("CommonValues/ValueGroup/{valueGroup}/ValueInt/{valueInt}")]
        public IActionResult GetCommonValueByValueInt(string valueGroup, int valueInt)
        {
            var data =  _commonValueService.GetCommonValueByGroupIntAsync(valueGroup, valueInt);
            if(data != null)
            {
                return Ok(data);
            }

            return NoContent();
        }

        /// <summary>
        /// Get Common value
        /// </summary>
        /// <param name="valueGroup"> Value group in string format </param>
        /// <param name="valueString">Value String in string format</param>
        /// <returns>CommonValue{Id, UniqueId, ValueGroup, ValueInt, ValueString, ValueDouble}</returns>
        [HttpGet]
        [Route("CommonValues/ValueGroup/{valueGroup}/ValueString/{valueString}")]
        public IActionResult GetCommonValueByValueString(string valueGroup, string valueString)
        {
            var data =  _commonValueService.GetCommonValueByGroupStringAsync(valueGroup, valueString);
            if (data != null)
            {
                return Ok(data);
            }
            return NoContent();
        }

        /// <summary>
        /// Get Common value
        /// </summary>
        /// <param name="valueGroup"> Value group in string format </param>
        /// <param name="valueDouble">Value Double in double format</param>
        /// <returns>CommonValue{Id, UniqueId, ValueGroup, ValueInt, ValueString, ValueDouble}</returns>
        [HttpGet]
        [Route("CommonValues/ValueGroup/{valueGroup}/ValueDouble/{valueDouble}")]
        public IActionResult GetCommonValueByValueDouble(string valueGroup, double valueDouble)
        {
            var data =  _commonValueService.GetCommonValueByGroupDoubleAsync(valueGroup, valueDouble);
            if (data != null)
            {
                return Ok(data);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("CommonValues/ValueGroup/{valueGroup}")]
        public IActionResult GetListCommonValuByValueGroupAsync(string valueGroup)
        {
            var data =  _commonValueService.GetListCommonValueByGroupAsync(valueGroup);
            if (data != null)
            {
                return Ok(data);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("CommonValues")]
        public IActionResult GetAllCommonValueAsync()
        {
            var data =  _commonValueService.GetAllCommonValueAsync();
            if (data != null)
            {
                return Ok(data);
            }
            return NoContent();
        }
    }
}