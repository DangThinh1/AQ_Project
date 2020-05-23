using AQBooking.YachtPortal.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtSearchModel: PagableModel
    {
        public string Id { get; set; }
      
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Country { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string City { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string District { get; set; }
        public int CharterCategoryFID { get; set; }
        public int CharterTypeFID { get; set; }
        public int YachtTypeFID {get;set;}
        public int HullTypeFID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string YachtName { get; set; }
        public int PortFID { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public int CrewMembers { get; set; }
        public int ScoreRating { get; set; }
        public int Passengers { get; set; }
        public int Cabins { get; set; }       
        public float LengthMin { get; set; } = 0;
        public float LengthMax { get; set; } = 300;     
        public int Lang { get; set; } = 1;
    }
}
