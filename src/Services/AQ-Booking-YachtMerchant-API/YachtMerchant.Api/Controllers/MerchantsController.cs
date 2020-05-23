using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MerchantsController : ControllerBase
    {
        private readonly IYachtMerchantService _yachtMerchantService;

        public MerchantsController(IYachtMerchantService yachtMerchantService)
        {
            _yachtMerchantService = yachtMerchantService;
        }
        /// <summary>
        /// Get UniqueId of Merchant from Id of Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUniquedId/{id}")]
        public IActionResult GetUniquedId(int id)
        {
            var result = _yachtMerchantService.GetMerchantUniqueID(id).Result;
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get Basic Infomation ( Id, UniqueId ) of Merchant By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProfile/{id}")]
        public IActionResult GetProfile(int id)
        {
            var result = _yachtMerchantService.GetMerchantBasicInfoByMerchantId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get Basic Infomation ( Id, UniqueId ) of Merchant By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list merchant use for select list</returns>
        [HttpGet]
        [Route("GetListMerchant")]
        public IActionResult GetListMerchant()
        {
            var result = _yachtMerchantService.GetListYachtMerchants();
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get all merchant manager by user have role Yacht Account Manager ( YAM )
        /// </summary>
        /// <param name="yamId"></param>
        /// <returns> list merchant use for select list</returns>
        [HttpGet]
        [Route("GetAllMerchantOfYachtAccountManager/{yamId}")]
        public IActionResult GetListMerchantOfYachtAccountManager(string yamId)
        {
            var result = _yachtMerchantService.GetListMerchantOfYachtAccountManager(yamId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get merchant of user have role Yacht Merchant ( YM )
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> one merchant </returns>
        [HttpGet]
        [Route("GetMerchantOfUserRoleYachtMerchant/{userId}")]
        public IActionResult GetMerchantOfUserRoleYachtMerchant(string userId)
        {
            var result = _yachtMerchantService.GetMerchantOfUserRoleYachtMerchant(userId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get all Yacht manager by user have role Yacht Merchant (YM)
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns>list yacht use for select list</returns>
        [HttpGet]
        [Route("GetAllYachtOfMerchant/{merchantId}")]
        public IActionResult GetListYachtOfMerchant(int merchantId)
        {
            var result = _yachtMerchantService.GetListYachtOfMerchant(merchantId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }


        /// <summary>
        /// Get all Yacht manager by user have role Yacht Merchant (YM)
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns>list yacht use for select list</returns>
        [HttpGet]
        [Route("GetAllYachtActiveForOperationOfMerchant/{merchantId}")]
        public IActionResult GetListYachtActiveForOperationOfMerchant(int merchantId)
        {
            var result = _yachtMerchantService.GetListYachtActiveForOperationOfMerchant(merchantId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Get Merchant Id by YachtId
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMerchantIdByYachId/{yachtId}")]
        public IActionResult GetMerchantByYachtId(int yachtId)
        {
            var result = _yachtMerchantService.GetMerchantIdByYachtId(yachtId).Result;
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }



    }
}