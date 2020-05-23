using System.Threading.Tasks;
using AccommodationMerchant.Core.Models.HotelMerchantUsers;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HotelMerchantUsersController : ControllerBase
    {
        private readonly IHotelMerchantUserService _hotelMerchantUsersService;
        public HotelMerchantUsersController(IHotelMerchantUserService hotelMerchantUsersService)
        {
            _hotelMerchantUsersService = hotelMerchantUsersService;
        }

        /// <summary>
        /// Get infomation of user for merchant control by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelMerchantUsers/GetInfomationOfMerchantUserById/{id}")]
        public IActionResult GetInfomationOfMerchantUserById(int id)
        {
            var response = _hotelMerchantUsersService.GetInfomationOfMerchantUserById(id);
            return Ok(response);
        }

        /// <summary>
        /// Get all user of merchant control by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelMerchantUsers/GetAllUserOfMerchantByMerchantId/{id}")]
        public IActionResult GetAllUserOfMerchantByMerchantId(int id)
        {
            var response = _hotelMerchantUsersService.GetAllUserOfMerchantByMerchantId(id);
            return Ok(response);
        }

        /// <summary>
        /// Get Dropdownlist user of merchant control by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelMerchantUsers/GetDropdownListUserOfMerchantByMerchantId/{id}")]
        public IActionResult GetDropdowwnListUserOfMerchantByMerchantId(int id)
        {
            var response = _hotelMerchantUsersService.GetDropdownListUserOfMerchantByMerchantId(id);
            return Ok(response);
        }

        /// <summary>
        /// Get all user of Merchant control by Role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelMerchantUsers/GetAllUserOfMerchantByRole")]
        public IActionResult GetAllUserOfMerchantByRole([FromQuery]HotelMerchantUserRequestGetAllUserWithRolesOfMerchantModel model)
        {
            var response = _hotelMerchantUsersService.GetAllUserOfMerchantByRole(model);
            return Ok(response);
        }

        /// <summary>
        /// Create new user of control by Merchant
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelMerchantUsers")]
        public async Task<IActionResult> CreateMerchantUserAsync([FromBody]HotelMerchantUserCreateModel model)
        {
            var response = await _hotelMerchantUsersService.CreateMerchantUser(model);
            return Ok(response);
        }

        /// <summary>
        /// Update infomation of user control by Merchant
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("HotelMerchantUsers")]
        public async Task<IActionResult> UpdateMerchantUserAsync([FromBody]HotelMerchantUserUpdateModel model)
        {
            var response = await _hotelMerchantUsersService.UpdateMerchantUser(model);
            return Ok(response);
        }

        /// <summary>
        /// Delete user control by Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("HotelMerchantUsers/{id}")]
        public async Task<IActionResult> DeleteMerchantUserAsync(int id)
        {
            var response = await _hotelMerchantUsersService.DeleteMerchantUser(id);
            return Ok(response);
        }

    }
}