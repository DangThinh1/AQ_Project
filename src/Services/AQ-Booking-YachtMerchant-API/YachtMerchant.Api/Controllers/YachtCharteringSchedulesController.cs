using System.Threading.Tasks;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtCharteringSchedules;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtCharteringSchedulesController : ControllerBase
    {
        private readonly IYachtCharteringSchedulesService _yachtCharteringSchedulesService;
        public YachtCharteringSchedulesController(IYachtCharteringSchedulesService yachtCharteringSchedulesService)
        {
            _yachtCharteringSchedulesService = yachtCharteringSchedulesService;
        }

        /// <summary>
        /// Check exist user set in schedules for reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringSchedules/CheckExistUserSetInSchedules")]
        public IActionResult CheckExistUserSetInSchedules([FromQuery]CheckDuplicateUserSchedulesModel model)
        {
            var result = _yachtCharteringSchedulesService.CheckExistUserSetInSchedules(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Check exist role set in schedules for reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringSchedules/CheckExistRoleSetInSchedules")]
        public IActionResult CheckExistRoleSetInSchedules([FromQuery]CheckDuplicateRoleSchedulesModel model)
        {
            var result = _yachtCharteringSchedulesService.CheckExistRoleSetInSchedules(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Check exist schedule set user and role for reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringSchedules/CheckExistUserRoleInSchedules")]
        public IActionResult CheckExistUserRoleInSchedules([FromQuery]CheckDuplicateSchedulesModel model)
        {
            var result = _yachtCharteringSchedulesService.CheckExistUserRoleInSchedules(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get detail of schedule by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringSchedules/{id}")]
        public IActionResult GetInfomationOfYachtCharteringSchedules(long id)
        {
            var result = _yachtCharteringSchedulesService.GetYachtCharteringSchedulesById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get list schedules has set for chartering By CharteringId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringSchedules/GetAllSchedulesByCharteringId/{id}")]
        public IActionResult GetAllYachtCharteringSchedules(long id)
        {
            var result = _yachtCharteringSchedulesService.GetYachtCharteringSchedulesByCharteringId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get all schedule has set for chartering By CharteringId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringSchedules/GetAllSchedulesSetOnCharteringByCharteringId/{id}")]
        public IActionResult GetAllSchedulesSetOnCharteringByCharteringId(long id)
        {
            var result = _yachtCharteringSchedulesService.GetAllScheduleSetOnCharteringSchedulesByCharteringId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Create new schedules for Chartering
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtCharteringSchedules")]
        public async Task<IActionResult> CreateYachtCharteringSchedulesAsync([FromBody]YachtCharteringSchedulesCreateModel model)
        {

            var result = await _yachtCharteringSchedulesService.CreateYachtCharteringSchedules(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }


        /// <summary>
        /// Update schedules for Chartering
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtCharteringSchedules")]
        public async Task<IActionResult> UpdateYachtCharteringSchedulesAsync([FromBody]YachtCharteringSchedulesUpdateModel model)
        {

            var result = await _yachtCharteringSchedulesService.UpdateYachtCharteringSchedules(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        /// <summary>
        /// Delete schedules set for Chartering by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtCharteringSchedules/{id}")]
        public async Task<IActionResult> YachtCharteringSchedulesAsync(long id)
        {
            var result = await _yachtCharteringSchedulesService.DeleteYachtCharteringSchedules(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}