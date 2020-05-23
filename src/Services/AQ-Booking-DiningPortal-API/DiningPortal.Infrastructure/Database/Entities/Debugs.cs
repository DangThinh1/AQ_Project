using System;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public class Debugs
    {
        public int Id { get; set; }
        public string Section { get; set; }
        public string LogMessage { get; set; }
        public DateTime LogDate { get; set; }
    }
}
