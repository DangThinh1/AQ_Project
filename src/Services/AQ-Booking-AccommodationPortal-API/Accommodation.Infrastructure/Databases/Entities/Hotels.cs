﻿using System;
using System.Collections.Generic;

namespace Accommodation.Infrastructure.Databases.Entities
{
    public partial class Hotels
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int SourceFid { get; set; }
        public string SourceResKey { get; set; }
        public int MerchantFid { get; set; }
        public string HotelCode { get; set; }
        public string ChainCode { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string HotelName { get; set; }
        public string HotelShortName { get; set; }
        public string WhenBuild { get; set; }
        public string StartOperation { get; set; }
        public int StatusFid { get; set; }
        public string StatusResKey { get; set; }
        public int HotelCategoryFid { get; set; }
        public string HotelCategoryResKey { get; set; }
        public int HotelTypeFid { get; set; }
        public string HotelTypeResKey { get; set; }
        public int TotalRooms { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int MapCapturedFileStreamFid { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public int CityFid { get; set; }
        public string Country { get; set; }
        public int CountryFid { get; set; }
        public string FullAddress { get; set; }
        public bool ActiveForOperation { get; set; }
        public bool Deleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}