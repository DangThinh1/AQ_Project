using Microsoft.AspNetCore.Mvc;

namespace AQS.BookingMVC.Controllers
{
    public class ErrorController : BaseController
    {
        [Route("not-found",Name ="NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}