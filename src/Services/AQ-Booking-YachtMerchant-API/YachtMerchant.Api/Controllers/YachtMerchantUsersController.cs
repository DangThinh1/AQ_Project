using System.Threading.Tasks;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtMerchantUsers;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtMerchantUsersController : ControllerBase
    {
        private readonly IYachtMerchantUsersService _yachtMerchantUsersService;
        public YachtMerchantUsersController(IYachtMerchantUsersService yachtMerchantUsersService)
        {
            _yachtMerchantUsersService = yachtMerchantUsersService;
        }

        /// <summary>
        /// Get infomation of user for merchant control by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantUsers/GetInfomationOfMerchantUserById/{id}")]
        public IActionResult GetInfomationOfMerchantUserById(int id)
        {
            var result = _yachtMerchantUsersService.GetInfomationOfMerchantUserById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get all user of merchant control by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantUsers/GetAllUserOfMerchantByMerchantId/{id}")]
        public IActionResult GetAllUserOfMerchantByMerchantId(int id)
        {
            var result = _yachtMerchantUsersService.GetAllUserOfMerchantByMerchantId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get Dropdownlist user of merchant control by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantUsers/GetDropdownListUserOfMerchantByMerchantId/{id}")]
        public IActionResult GetDropdowwnListUserOfMerchantByMerchantId(int id)
        {
            var result = _yachtMerchantUsersService.GetDropdownListUserOfMerchantByMerchantId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get all user of Merchant control by Role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantUsers/GetAllUserOfMerchantByRole")]
        public IActionResult GetAllUserOfMerchantByRole([FromQuery]YachtMerchantUsersRequestGetAllUserWithRolesOfMerchantModel model)
        {
            var result = _yachtMerchantUsersService.GetAllUserOfMerchantByRole(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Create new user of control by Merchant
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtMerchantUsers")]
        public async Task<IActionResult> CreateYachtMerchantUserAsync([FromBody]YachtMerchantUsersCreateModel model)
        {

            var result = await _yachtMerchantUsersService.CreateYachtMerchantUser(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        /// <summary>
        /// Update infomation of user control by Merchant
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtMerchantUsers")]
        public async Task<IActionResult> UpdateYachtMerchantUserAsync([FromBody]YachtMerchantUsersUpdateModel model)
        {

            var result = await _yachtMerchantUsersService.UpdateYachtMerchantUsers(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();

        }

        /// <summary>
        /// Delete user control by Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtMerchantUsers/{id}")]
        public async Task<IActionResult> DeleteYachtMerchantUsersAsync(int id)
        {
            var result = await _yachtMerchantUsersService.DeleteYachtMerchantUsers(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

    }
}