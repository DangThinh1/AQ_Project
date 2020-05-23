using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.API.Helpers;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    [LogHelper]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtMerchantProductInventoryController : ControllerBase
    {
        private readonly IYachtMerchantProductInventoryService _yachtMerchantProductInventorService;

        public YachtMerchantProductInventoryController(IYachtMerchantProductInventoryService yachtMerchantProductInventorService)
        {
            _yachtMerchantProductInventorService = yachtMerchantProductInventorService;
        }

        [HttpGet("Yachts/YachtMerchantProductInventories/MerchantProductInventories/additionalFId/{additionalFId}")]
        public IActionResult GetMerchantProductInventoriesByadditionalFId(int additionalFId)
        {
            var result = _yachtMerchantProductInventorService.GetProductInventorByAdditionalFId(additionalFId);
            return Ok(result);
        }
        [HttpGet("Yachts/YachtMerchantProductInventories/MerchantProductInventoriesWithPricing/additionalFId/{additionalFId}")]
        public IActionResult GetProductInventoryWithPricingByAdditionalFId(string additionalFId)
        {
            var result = _yachtMerchantProductInventorService.GetProductInventoryPricingByAdditionalFId(additionalFId);
            return Ok(result);
        }
        [HttpGet("Yachts/YachtMerchantProductInventories/MerchantProductInventoriesWithPricing/productId/{productId}")]
        public IActionResult GetPriceOfProductInventoryByArrayOfProductId([FromQuery]List<string> productId)
        {
            var result = _yachtMerchantProductInventorService.GetPriceOfProductInventoryByArrayOfProductId(productId);
            return Ok(result);
        }
    }
}