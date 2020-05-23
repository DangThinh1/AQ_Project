using AQBooking.YachtPortal.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure
{
    public  class YachtSearchItem: YachtItem
    {
        public YachtSearchItem():base()
        {
            
        }      
        public int TotalRows { get; set; }

    

    }
}
