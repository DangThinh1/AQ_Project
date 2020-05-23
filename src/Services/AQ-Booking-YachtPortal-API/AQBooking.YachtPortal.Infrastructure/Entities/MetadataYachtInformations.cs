using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtInformations
    {
        public virtual Yachts Yacht { get; set; }

        public IEnumerable<YachtInformationDetails> InformationDetails { get; set; }

        public YachtInformations()
        {
            InformationDetails = new HashSet<YachtInformationDetails>();
        }
    }
}
