using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtCharterBookingViewModel
    {
        public string YachtName { get; set; }
        public string UniqueID { get; set; }
        public string YachtFId { get; set; }
        public string CharteringID { get; set; }
        public string ContactNo { get; set; }
        public DateTime CharterDateFrom { get; set; }
        public DateTime CharterDateTo { get; set; }
        public double PrepaidValue { get; set; }
        public double GrandTotalValue { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int Cabins { get; set; }
        public double LengthMeters { get; set; }
        public int MaxPassenger { get; set; }
        public string EngineGenerators { get; set; }
        public string StatusResourceKey { get; set; }
        public int StatusID { get; set; }
        public string PortName { get; set; }
        public int PortId { get; set; }
        public string CustomerName {get;set;}
        public string CustomerEmail { get; set; }
        public string CustomerNote { get; set; }
        public int Passengers { get; set; }
        public int streamFileId { get; set; }
        public DateTime? BookingDate { get; set; }
        public List<YachtCharteringDetailViewModel> CharteringDetails { get; set; }
        public YachtBookingViewModel Yacht { get; set; }

    }
    public class YachtBookingViewModel
    {
        public string Id { get; set; }
        public string  MerchantFid { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
       // public  YachtCharterBookingViewModel Chartering { get; set; }

    }
    
    public class  BookingYachtPort {
        public string PortName { get; set; }
        public int PortId { get; set; }
    }
}
