using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtFileStreams;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Conts;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtFileStreamController : ControllerBase
    {
        #region Fields
        private readonly IYachtFileStreamService _yachtFileStreamService; 
        #endregion

        #region Ctor
        public YachtFileStreamController(IYachtFileStreamService yachtFileStreamService)
        {
            _yachtFileStreamService = yachtFileStreamService;
        }
        #endregion

        /// <summary>
        /// API Search yacht file stream
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        #region Methods
        [HttpGet]
        [Route("YachtFileStreams")]
        public IActionResult SearchYachtFileStream([FromQuery]  YachtFileStreamSearchModel model)
        {
            var res = _yachtFileStreamService.SearchYachtFileStream(model);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
           
        }

        [HttpGet]
        [Route("YachtGallery")]
        public IActionResult SearchYachtGallery([FromQuery]YachtFileStreamSearchModel model)
        {
            var res = _yachtFileStreamService.SearchYachtGallery(model);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        /// <summary>
        /// API Get yacht file stream by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtFileStreams/{id}")]
        public IActionResult GetYachtFileStreamById(int id)
        {
            var res = _yachtFileStreamService.GetYachtFileStreamById(id);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
           
        }

        /// <summary>
        /// API Create new yacht filestreams
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtFileStreams")]
        public async Task<IActionResult> CreateYachtFileStream(YachtFileStreamCreateModel model)
        {
            var res = await _yachtFileStreamService.CreateYachtFileStream(model);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        /// <summary>
        /// API Update yacht filestreams
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtFileStreams")]
        public async Task<IActionResult> UpdateYachtFileStream(YachtFileStreamUpdateModel model)
        {
            var res = await _yachtFileStreamService.UpdateYachtFileStream(model);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        /// <summary>
        /// API Delete yacht filestreams
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtFileStreams/{id}")]
        public async Task<IActionResult> DeleteYachtFileStream(int id)
        {
            var res = await _yachtFileStreamService.DeleteYachtFileStream(id);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
            
        }
        #endregion
    }
}