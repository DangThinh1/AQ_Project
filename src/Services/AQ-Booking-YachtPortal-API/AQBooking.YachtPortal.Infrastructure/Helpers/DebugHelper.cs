using AQBooking.YachtPortal.Core.Helpers;
using AQBooking.YachtPortal.Infrastructure.Entities;
using System;

namespace AQBooking.YachtPortal.Infrastructure.Helpers
{
    public class DebugHelper
    {
        private static readonly AQYachtContext _db = DependencyInjectionHelper.GetService<AQYachtContext>();
        public static int LogBug(string section,string logMessage)
        {
            try
            {
                var bug = new Debugs()
                {
                    Section = section,
                    LogMessage = logMessage,
                    LogDate = DateTime.Now
                };
                _db.Debugs.Add(bug);
                var result = _db.SaveChanges();
                return bug.Id;
            }
            catch
            {
                return 0;
            }
        }
    }
}
