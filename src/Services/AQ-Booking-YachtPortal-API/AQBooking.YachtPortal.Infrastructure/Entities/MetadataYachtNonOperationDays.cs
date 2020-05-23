using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtNonOperationDays
    {
        public virtual Yachts Yacht { get; set; }
    }
}
