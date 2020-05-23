namespace AQDiningPortal.Core.Models.RestaurantReservationFees
{
    public class RestaurantReservationFeeViewModel
    {
        public int FeeTypeFid { get; set; }
        public double FlatFee { get; set; }
        public double AdultFee { get; set; }
        public double ChildFee { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public string Remark { get; set; }
    }
}
