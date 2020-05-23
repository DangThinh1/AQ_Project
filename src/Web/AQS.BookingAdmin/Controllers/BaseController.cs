using AQS.BookingAdmin.Infrastructure.Helpers;
using AQS.BookingAdmin.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQS.BookingAdmin.Controllers
{
    [Authorize]
    public class BaseController:Controller
    {
        private IWorkContext _workContext;
        protected IWorkContext WorkContext => _workContext ?? (_workContext = DependencyInjectionHelper.GetService<IWorkContext>());

        protected IActionResult Success(object id,string msg=null,string data=null)
        {
            return Ok(new { Id = id,msg=msg,data=data });
        }
        protected IActionResult Error(string message)
        {
            return Ok(new { Id = 0,Msg=message });
        }
    }
}
