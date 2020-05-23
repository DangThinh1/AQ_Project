using AccommodationMerchant.Core.Models.HotelMerchants;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MerchantsController : ControllerBase
    {
        private readonly IHotelMerchantService _hotelMerchantService;

        public MerchantsController(IHotelMerchantService hotelMerchantService)
        {
            _hotelMerchantService = hotelMerchantService;
        }
        /// <summary>
        /// Get UniqueId of Merchant from Id of Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUniquedId/{id}")]
        public async Task<IActionResult> GetUniquedId(int id)
        {
            var response = await _hotelMerchantService.GetMerchantUniqueID(id);
            return Ok(response);
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
            var response = _hotelMerchantService.GetMerchantBasicInfoByMerchantId(id);
            return Ok(response);
        }

        

        /// <summary>
        /// Get all merchant manager by user have role Accomodation Account Manager ( AAM )
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> list merchant use for select list</returns>
        [HttpGet]
        [Route("GetAllMerchantOfAccountManager/{userId}")]
        public IActionResult GetListMerchantOfAccommodationAccountManager(string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                var response = _hotelMerchantService.GetListMerchantOfAccommodationAccountManager(userId);
                return Ok(response);
            }
            return Ok(BaseResponse<List<Merchant>>.BadRequest());
        }


        /// <summary>
        /// Get merchant of user have role Accommodation Merchant (AM)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> one merchant </returns>
        [HttpGet]
        [Route("GetMerchantOfUserRoleAccommodationMerchant/{userId}")]
        public IActionResult GetMerchantOfUserRoleAccommodationMerchant(string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                var response = _hotelMerchantService.GetMerchantOfUserRoleAccommodationMerchant(userId);
                return Ok(response);
            }
            return Ok(BaseResponse<Merchant>.BadRequest());
        }

        /// <summary>
        /// Get all hotel of merchant
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns>list hotel for select list</returns>
        [HttpGet]
        [Route("GetAllHotelOfMerchant/{merchantId}")]
        public IActionResult GetListHotelOfMerchant(int merchantId)
        {
            var response = _hotelMerchantService.GetListHotelOfMerchant(merchantId);
            return Ok(response);
        }


        /// <summary>
        /// Get Merchant Id by hotelId
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMerchantIdByHotelId/{hotelId}")]
        public async Task<IActionResult> GetMerchantByHotelId(int hotelId)
        {
            var response = await _hotelMerchantService.GetMerchantIdByHotelId(hotelId);
            return Ok(response);
        }



    }
}