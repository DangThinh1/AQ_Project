using AQDiningPortal.Core.Models.PagingPortal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQDiningPortal.Core.Models.Restaurants
{
    public class RestaurantSearchModel : PagableModel
    {
        //Relationship properties
        public List<string> AttributeFid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RestaurantName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BusinessDay { get; set; } //21 May 2019
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ZoneDistrict { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Time { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Adults { get; set; }
        public bool ServingType { get; set; }
        

    }
}
