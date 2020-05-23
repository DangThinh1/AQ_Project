using AQBooking.Admin.Core.Models.YachtTourCategory;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class YachtTourCategoryController : ControllerBase
    {
        #region Fields

        private readonly IYachtTourCategoryService _yachtTourCategoryService;

        #endregion Fields

        #region Ctor

        public YachtTourCategoryController(IYachtTourCategoryService yachtTourCategoryService)
        {
            _yachtTourCategoryService = yachtTourCategoryService;
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// API Search Yacht merchant
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCategory")]
        public IActionResult SearchYachtMerchants([FromQuery]YachtTourCategorySearchModel searchModel = null)
        {
            try
            {
                var result = _yachtTourCategoryService.Search(searchModel);
                if (result.Data != null)
                    return Ok(result);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API get Yacht merchant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCategory/{id}")]
        public IActionResult FindInfoDetail(int id)
        {
            try
            {
                var result = _yachtTourCategoryService.GetInfoDetailSupportedByTourCategoryId(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Create Yacht merchant
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtTourCategory")]
        public IActionResult Create([FromBody]YachtTourCategoryCreateModel createModel)
        {
            try
            {
                var result = _yachtTourCategoryService.Create(createModel);
                if (result)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Update Yacht merchant
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtTourCategory")]
        public IActionResult UpdateDetail([FromBody]YachtTourCategoryCreateModel updateModel)
        {
            try
            {
                var result = _yachtTourCategoryService.Update(updateModel);
                if (result)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Delete Yacht merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtTourCategory/{id}")]
        public IActionResult DeleteYachtMerchant(int id)
        {
            try
            {
                var result = _yachtTourCategoryService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        #endregion Methods
    }
}