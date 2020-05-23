using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Controllers
{
    public class BaseController : Controller
    {
        #region Fields
        private  IWorkContext _workContext =null;
        public IWorkContext WorkContext
        {
            get
            {
                if (_workContext == null)
                    _workContext = DependencyInjectionHelper.GetService<IWorkContext>();
                return _workContext;
            }
        }
        #endregion

        #region Ctor

        #endregion

        #region Methods

        #endregion
    }
}
