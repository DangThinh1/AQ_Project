using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class SelectListController : ControllerBase
    {
        #region Fields
        private readonly IRestaurantMerchantService _restaurantMerchantService;
        private readonly IYachtMerchantService _yachtMerchantService;
        private readonly IEVisaMerchantService _evisaMerchantService;
        #endregion

        #region Ctor
        public SelectListController(
            IRestaurantMerchantService restaurantMerchantService,
            IYachtMerchantService yachtMerchantService,
            IEVisaMerchantService evisaMerchantService)
        {
            this._restaurantMerchantService = restaurantMerchantService;
            this._yachtMerchantService = yachtMerchantService;
            this._evisaMerchantService = evisaMerchantService;
        }
        #endregion

        /// <summary>
        /// Get restaurant merchant selectlist don't have manager
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/RestaurantMerChantNoManager")]
        public IActionResult GetRestaurantMerchantNoManagerSelectList()
        {
            try
            {
                var result = _restaurantMerchantService.GetRestaurantMerchantWithoutManagerSelectList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get restaurant merchant select list don't have user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/RestaurantMerChantNoUser")]
        public IActionResult GetRestaurantMerchantNoUserSelectList()
        {
            try
            {
                var result = _restaurantMerchantService.GetRestaurantMerchantWithoutAccountSelectList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get all restaurant merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/AllRestaurantMerchant")]
        public IActionResult GetAllRestaurantMerchantSelectList()
        {
            try
            {
                var result = _restaurantMerchantService.GetAllRestaurantMerchantSelectList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get yacht merchant don't have manager
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/YachtMerchantNoManager")]
        public IActionResult GetYachtMerchantNoManagerSelectList()
        {
            try
            {
                var result = _yachtMerchantService.GetYachtMerchantWithoutManagerSelectList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get yacht merchant selectlist don't have user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/YachtMerchantNoUser")]
        public IActionResult GetYachtMerchantNoUserSelectList()
        {
            try
            {
                var result = _yachtMerchantService.GetYachtMerchantWithoutAccountSelectList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get all yacht merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/AllYachtMerchant")]
        public IActionResult GetAllYachtMerchantSelectList()
        {
            try
            {
                var result = _yachtMerchantService.GetAllYachtMerchantSelectList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get yacht merchant account management has merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Lists/YachtAccMgtHasMerchant")]
        public IActionResult GetYachtAccMgtHasMerchant()
        {
            try
            {
                var result = _yachtMerchantService.GetListYamHasMerchant();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get yacht merchant account has merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Lists/YachtAccHasMerchant")]
        public IActionResult GetYachtAccHasMerchant()
        {
            try
            {
                var result = _yachtMerchantService.GetListYmHasMerchant();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get restaurant merchant account management has merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Lists/RestaurantAccMgtHasMerchant")]
        public IActionResult GetResAccMgtHasMerchant()
        {
            try
            {
                var result = _restaurantMerchantService.GetListDamHasMerchant();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get restaurant merchant account has merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Lists/RestaurantAccHasMerchant")]
        public IActionResult GetResAccHasMerchant()
        {
            try
            {
                var result = _restaurantMerchantService.GetListDmHasMerchant();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get evisa merchant selectlist don't have user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/EVisaMerchantNoUsers")]
        public IActionResult GetEVisaMerchantNoUserSll()
        {
            var result = new List<SelectListModel>();
            result = _evisaMerchantService.GetEVisaMerchantNoUserSll();
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

        /// <summary>
        /// Get all evisa merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectLists/AllEVisaMerchants")]
        public IActionResult GetAllEVisaMerchant()
        {
            var result = new List<SelectListModel>();
            result = _evisaMerchantService.GetAllEVisaMerchantSll();
            if (result != null)
                return Ok(result);
            return BadRequest();
        }
    }
}