using System;
using System.Collections.Generic;

namespace AQBooking.Admin.Infrastructure.Databases.CMSEntities
{
    public partial class Subscribers
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string SourceUrl { get; set; }
        public string ModuleName { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
