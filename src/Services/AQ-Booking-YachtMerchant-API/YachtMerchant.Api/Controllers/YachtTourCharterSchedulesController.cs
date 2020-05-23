using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtTourCharterSchedules;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourCharterSchedulesController : ControllerBase
    {
        private readonly IYachtTourCharterSchedulesService _yachtTourCharterSchedulesService;
        public YachtTourCharterSchedulesController(IYachtTourCharterSchedulesService yachtTourCharterSchedulesService)
        {
            _yachtTourCharterSchedulesService = yachtTourCharterSchedulesService;
        }

        /// <summary>
        /// Check exist user set in schedules for reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharterSchedules/CheckExistUserSetInSchedules")]
        public IActionResult CheckExistUserSetInSchedules([FromQuery]CheckDuplicateUserSchedulesModel model)
        {
            var result = _yachtTourCharterSchedulesService.CheckExistUserSetInSchedules(model);
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
        [Route("YachtTourCharterSchedules/CheckExistRoleSetInSchedules")]
        public IActionResult CheckExistRoleSetInSchedules([FromQuery]CheckDuplicateRoleSchedulesModel model)
        {
            var result = _yachtTourCharterSchedulesService.CheckExistRoleSetInSchedules(model);
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
        [Route("YachtTourCharterSchedules/CheckExistUserRoleInSchedules")]
        public IActionResult CheckExistUserRoleInSchedules([FromQuery]CheckDuplicateSchedulesModel model)
        {
            var result = _yachtTourCharterSchedulesService.CheckExistUserRoleInSchedules(model);
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
        [Route("YachtTourCharterSchedules/{id}")]
        public IActionResult GetYachtTourCharterSchedulesById(long id)
        {
            var result = _yachtTourCharterSchedulesService.GetYachtTourCharterSchedulesById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get list schedules has set for c9harter By CharterId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharterSchedules/GetAllSchedulesByCharterId/{id}")]
        public IActionResult GetYachtTourCharterSchedulesByCharterId(long id)
        {
            var result = _yachtTourCharterSchedulesService.GetYachtTourCharterSchedulesByCharterId(id);
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
        [Route("YachtTourCharterSchedules/GetAllScheduleSetOnCharterSchedulesByCharterId/{id}")]
        public IActionResult GetAllScheduleSetOnCharterSchedulesByCharterId(long id)
        {
            var result = _yachtTourCharterSchedulesService.GetAllScheduleSetOnCharterSchedulesByCharterId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Create new schedules for Chater
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtTourCharterSchedules")]
        public async Task<IActionResult> CreateYachtTourCharterSchedulesAsync([FromBody]YachtTourCharterSchedulesCreateModel model)
        {

            var result = await _yachtTourCharterSchedulesService.CreateYachtTourCharterSchedules(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }


        /// <summary>
        /// Update schedules for Charter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtTourCharterSchedules")]
        public async Task<IActionResult> UpdateYachtTourCharterSchedulesAsync([FromBody]YachtTourCharterSchedulesUpdateModel model)
        {

            var result = await _yachtTourCharterSchedulesService.UpdateYachtTourCharterSchedules(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        /// <summary>
        /// Delete schedules set for Charter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtTourCharterSchedules/{id}")]
        public async Task<IActionResult> YachtTourCharterSchedulesAsync(long id)
        {
            var result = await _yachtTourCharterSchedulesService.DeleteYachtTourCharterSchedules(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

    }
}