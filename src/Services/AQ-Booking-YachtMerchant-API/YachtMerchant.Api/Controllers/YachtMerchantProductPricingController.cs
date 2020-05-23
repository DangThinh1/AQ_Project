using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtMerchantProductInventories;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class YachtMerchantProductPricingController : ControllerBase
    {
        private readonly IYachtMerchantProductInventoryService _yachtMerchantProductInventoryService;
        public YachtMerchantProductPricingController(IYachtMerchantProductInventoryService yachtMerchantProductInventoryService)
        {
            _yachtMerchantProductInventoryService = yachtMerchantProductInventoryService;
            _yachtMerchantProductInventoryService.InitController(this);
        }

        [HttpGet]
        [Route("YachtMerchantProductPricing/{id}")]
        public IActionResult GetYachtMerchantProduct(int id)
        {
            try
            {
                var res = _yachtMerchantProductInventoryService.GetProductPricingById(id);
                return this.OkResponse(res);
            }
            catch (Exception ex)
            {
                return this.BadRequestResponse(ex.StackTrace.ToString());
            }
        }

        [HttpPost]
        [Route("YachtMerchantProductPricing")]
        public IActionResult CreateYachtMerchantProductPricing(YachtMerchantProductPricingCreateOrUpdateModel model)
        {
            try
            {
                var res = _yachtMerchantProductInventoryService.CreateYachtMerchantProductPricing(model);
                return this.OkResponse(res);
            }
            catch (Exception ex)
            {
                return this.BadRequestResponse(ex.StackTrace.ToString());
            }
        }

        [HttpPut]
        [Route("YachtMerchantProductPricing")]
        public IActionResult UpdateYachtMerchantProductPricing(YachtMerchantProductPricingCreateOrUpdateModel model)
        {
            try
            {
                var res = _yachtMerchantProductInventoryService.UpdateYachtMerchantProductPricing(model);
                if (res.IsSucceed)
                    return this.OkResponse(res);
                return this.BadRequestResponse(res.Message);
            }
            catch (Exception ex)
            {
                return this.BadRequestResponse(ex.StackTrace.ToString());
            }
        }

        [HttpDelete]
        [Route("YachtMerchantProductPricing/{id}")]
        public IActionResult DeleteYachtMerchantProductPricing(int id)
        {
            try
            {
                var res = _yachtMerchantProductInventoryService.DeleteProductPricing(id);
                return this.OkResponse(res);
            }
            catch (Exception ex)
            {
                return this.BadRequestResponse(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("YachtMerchantProductPricing")]
        public IActionResult Search(int id)
        {

            try
            {
                var result = _yachtMerchantProductInventoryService.GetAllProductPricingByProductId(id);
                return this.OkResponse(result);
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorAsync(ex.Message).Result;
            }
        }
    }
}