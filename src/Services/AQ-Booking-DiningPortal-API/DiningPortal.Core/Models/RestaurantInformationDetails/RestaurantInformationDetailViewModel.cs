using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.RestaurantInformationDetails
{
    public class RestaurantInformationDetailViewModel
    {
        //public long Id { get; set; }
        //public string UniqueId { get; set; }
        //public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
    }
}
