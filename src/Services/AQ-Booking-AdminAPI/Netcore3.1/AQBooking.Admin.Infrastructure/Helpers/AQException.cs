using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Helpers
{
    public class AQException:Exception
    {
        public AQException(Exception ex):base(ex.Message,ex.InnerException)
        {

        }
        public AQException(string message) : base(message)
        {

        }
        public static void ThrowException(Exception ex)
        {
            Debug.Write(ex.InnerException);
        }
    }
}
