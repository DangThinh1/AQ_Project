using System;
using System.Collections.Generic;
using System.Text;
using YachtMerchant.Core.Models.YachtFileStreams;

namespace YachtMerchant.Core.Models.YachtTourPreview
{
    public class YachtTourYachtModel
    {
        public int YachtId { get; set; }
        public string YachtName { get; set; }
        public int YachtDefaultImage { get; set; }
        public string ShortDescription { get; set; }
        public string Size { get; set; }
        public int Cabins { get; set; }
        public string EngineGenerators { get; set; }
        public int MaxPassengers { get; set; }
        public int CrewMembers { get; set; }
        public List<YachtFileStreamViewModel> ListYachtFileStream { get; set; }

        public YachtTourYachtModel()
        {
            ListYachtFileStream = new List<YachtFileStreamViewModel>();
        }
    }
}
