using AQBooking.Core.Helpers;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using YachtMerchant.Core.Models.YachtMerchantProductInventories;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtMerchantProductInventoryController : ControllerBase
    {
        private readonly IYachtMerchantProductInventoryService _yachtMerchantProductInventoryService;

        public YachtMerchantProductInventoryController(IYachtMerchantProductInventoryService yachtMerchantProductInventoryService)
        {
            _yachtMerchantProductInventoryService = yachtMerchantProductInventoryService;
        }

        [HttpGet]
        [Route("YachtMerchantProductInventory")]
        public IActionResult Search([FromQuery]YachtMerchantProductInventorySearchModel model)
        {
            var result = _yachtMerchantProductInventoryService.GetAllYachtMerchantProductInventory(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductInventory/{id}")]
        public IActionResult FindYachtMerchantProductInventory(int id)
        {
            var res = _yachtMerchantProductInventoryService.GetYachtMerchantProductInventoryById(id);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest(res.Message);
        }

        [HttpPost]
        [Route("YachtMerchantProductInventory")]
        public IActionResult CreateYachtMerchantProductInventory(YachtMerchantProductInventoryCreateModel model)
        {
            var res = _yachtMerchantProductInventoryService.CreateYachtMerchantProductInventory(model);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtMerchantProductInventory")]
        public IActionResult UpdateYachtMerchantProductInventory(YachtMerchantProductInventoryUpdateModel model)
        {
            var res = _yachtMerchantProductInventoryService.UpdateYachtMerchantProductInventory(model);
            if (res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtMerchantProductInventory/{id}")]
        public IActionResult DeleteYachtMerchantProductInventory(int id)
        {
            var res = _yachtMerchantProductInventoryService.DeleteYachtMerchantProductInventory(id);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductInventory/YachtMerchantProductPricing/{id}")]
        public IActionResult GetYachtMerchantProduct(int id)
        {
            var res = _yachtMerchantProductInventoryService.GetAllProductPricingByProductId(id);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtMerchantProductInventory/YachtMerchantProductPricing")]
        public IActionResult CreateYachtMerchantProductPricing(ProductPricingCreateOrUpdateModel model)
        {
            var res = _yachtMerchantProductInventoryService.CreateProductPricing(model);
            if(res.IsSuccessStatusCode)
                return this.Ok(res);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtMerchantProductInventory/YachtMerchantProductPricing")]
        public IActionResult UpdateYachtMerchantProductPricing(ProductPricingCreateOrUpdateModel model)
        {
            var res = _yachtMerchantProductInventoryService.UpdateProductPricing(model);
            if (res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtMerchantProductInventory/YachtMerchantProductPricing/{id}")]
        public IActionResult DeleteYachtMerchantProductPricing(int id)
        {
            var res = _yachtMerchantProductInventoryService.DeleteProductPricing(id);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductInventory/ProductPricingById/{id}")]
        public IActionResult GetProductPricing(int id)
        {
            var result = _yachtMerchantProductInventoryService.GetPricingById(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductInventory/GetProductSupplierByInventoryId/{id}")]
        public IActionResult GetProductSuppliserByProductId(int id)
        {
            var result = _yachtMerchantProductInventoryService.GetProductSupplierByProductIdTest(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtMerchantProductInventory/ProductSupplier")]
        public IActionResult UpdateProductSupplier(YachtMerchantProductSupplierViewModel model)
        {
            if (model == null)
                return BadRequest();
            var result = _yachtMerchantProductInventoryService.UpdateProductSupplier(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductInventory/ProductInventorySelectList/{id}/{categoryId}")]
        public IActionResult GetAllProductInventory(int id, int categoryId)
        {
            var result = _yachtMerchantProductInventoryService.GetAllProductInventoryByMerchantId(id, categoryId);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtMerchantProductInventory/ProductSupplier")]
        public IActionResult CreateProductSupplierByVendor(YachtMerchantProductSupplierViewModel model)
        {
            var result = _yachtMerchantProductInventoryService.CreateProductSupplier(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}