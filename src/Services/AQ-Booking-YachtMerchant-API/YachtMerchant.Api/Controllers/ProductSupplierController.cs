using AQBooking.Core.Helpers;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using YachtMerchant.Core.Models.YachtMerchantProductSuppliers;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class ProductSupplierController : ControllerBase
    {
        private readonly IYachtMerchantProductSupplierServices _yachtMerchantProductSupplier;

        public ProductSupplierController(IYachtMerchantProductSupplierServices yachtMerchantProductSupplier)
        {
            _yachtMerchantProductSupplier = yachtMerchantProductSupplier;
        }

        [HttpPost]
        [Route("ProductSupplier/Vendor/{vendorId}")]
        public IActionResult ProductSupplierByVendorId(int vendorId, [FromBody]ProductSupplierSearchModel model)
        {
            
            var result = _yachtMerchantProductSupplier.GetSupplierByVendorId(vendorId, model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("ProductSupplier/Product/{productId}")]
        public IActionResult ProductSupplierByProductId(int productId, [FromBody]ProductSupplierSearchModel model)
        {
            var result = _yachtMerchantProductSupplier.GetSupplierByProductId(productId, model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("ProductSupplier")]
        public IActionResult Create(ProductSupplierAddorUpdateModel model)
        {
            var result = _yachtMerchantProductSupplier.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("ProductSupplier")]
        public IActionResult Update(ProductSupplierAddorUpdateModel model)
        {
            var result = _yachtMerchantProductSupplier.Update(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("ProductSupplier/Vendor/{VendorId}/Product/{ProductId}")]
        public IActionResult Delete(int vendorId, int productId)
        {
            var result = _yachtMerchantProductSupplier.Delete(vendorId, productId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }
    }
}