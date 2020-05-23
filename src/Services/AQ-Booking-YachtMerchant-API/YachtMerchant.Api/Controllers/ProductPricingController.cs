using AQBooking.Core.Helpers;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using YachtMerchant.Core.Models.YachtMerchantProductPricings;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class ProductPricingController : ControllerBase
    {
        private readonly IProductPricingServices _pricingServices;

        public ProductPricingController(IProductPricingServices pricingServices)
        {
            _pricingServices = pricingServices;
        }

        [HttpPost]
        [Route("ProductPricing/Product/{productId}")]
        public IActionResult ProductSupplierByProductId(int productId, [FromBody]ProductPricingSearchModel model)
        {
            var result = _pricingServices.GetAllProductPricingByProductId(productId, model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
           
        }

        [HttpPost]
        [Route("ProductPricing")]
        public IActionResult Create(ProductPricingCreateModel model)
        {
            var result = _pricingServices.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }
    }
}