using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtCharterings
    {
        public virtual Yachts Yacht { get; set; }

        public virtual List<YachtCharteringDetails> CharteringDetails { get; set; }
    }
}
