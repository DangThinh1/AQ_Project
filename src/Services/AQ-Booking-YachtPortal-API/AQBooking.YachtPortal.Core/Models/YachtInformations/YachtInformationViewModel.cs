using AQBooking.YachtPortal.Core.Models.YachtInformationDetails;

namespace AQBooking.YachtPortal.Core.Models.YachtInformations
{
    public class YachtInformationViewModel
    {
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }

        //Detail Properties
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
    }
}
