using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.Subscriber;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        #region Fields
        private readonly ISubscriberService _subscriberService;
        #endregion

        #region Ctor

        public SubscriberController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }
        #endregion
        #region Methods
        [Route("Subscriber")]
        [HttpPost]
        public IActionResult CreateNewSubscriber([FromBody]SubscriberCreateModel model)
        {
            var res = _subscriberService.CreateNewSubscriber(model);
            if (!res.Succeeded)
                return NotFound();
            return Ok(res);
        }

        [Route("SubscriberSearch")]
        [HttpGet]
        public IActionResult SearchPost([FromQuery]SubscriberSearchModel model)
        {
            try
            {
                var res = _subscriberService.SearchSubscriber(model);
                if (res != null)
                    return Ok(res);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        #endregion
    }
}