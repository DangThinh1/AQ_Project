using Microsoft.AspNetCore.Mvc;
using Identity.Core.Models.Emails;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Conts;

namespace Identity.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("Emails/Send")]
        public IActionResult Send(SendMailModel sendModel)
        {
            var result = _emailService.Send(sendModel);
            return Ok(result);
        }
    }
}