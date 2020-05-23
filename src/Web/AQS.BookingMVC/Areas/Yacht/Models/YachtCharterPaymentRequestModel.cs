using AQS.BookingMVC.Models.Payment;
using System.Collections.Generic;

namespace AQS.BookingMVC.Areas.Yacht.Models
{
    public class YachtCartStorageViewModel
    {
        public string YachtId { get; set; }
        public List<MerchantProductInventoriesModel> ProductPackage { get; set; }
    }
    public class YachtPackageServiceModel : YachtCartStorageViewModel
    {
        public int Passenger { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }
    public class YachtSavePackageServiceModel
    {
        public BookingInfo BookingRequestModel { get; set; }
        public YachtPackageServiceModel YachtBooking { get; set; }
    }

    public class YachtPaymentInfo : PaymentStripInfo
    {
        public YachtSavePackageServiceModel data { get; set; }
    }

    public class MerchantProductInventoriesModel
    {
        public string productInventoryFId { get; set; }
        public string productName { get; set; }
        public string categroryFId { get; set; }
        public int quantity { get; set; }
    }
    public class MerchantPaymentEachPackageViewModel
    {
        public string UniqueId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string DiscountDisplay { get; set; }
        public double Total { get; set; }
        public int Quantity { get; set; }
        public string PricedDisplay { get; set; }
        public string TotalDisplay { get; set; }
        public double PrepaidValue { get; set; }
        public string PrepaidValueDisplay { get; set; }
    }
    public class SaveCharterPaymentResponseViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ResuldCode { get; set; }
        public string Describes { get; set; }
        public string Id { get; set; }
        public string UniqueId { get; set; }
        public List<SaveCharterPaymentDetailViewModel> Detail { get; set; }
    }
    public class SaveCharterPaymentDetailViewModel
    {
        public string Value { get; set; }
        public string Describes { get; set; }
    }
    public class MerchantPaymentPackageViewModel
    {
        public string Id { get; set; }
        public double Total { get; set; }
        public double YachtTotal { get; set; }
        public double DiscountTotal { get; set; }
        public string DiscountTotalDisplay { get; set; }
        public int Passenger { get; set; }
        public string YachtTotalDisplay { get; set; }
        public double PackageTotal { get; set; }
        public string PackageTotalDisplay { get; set; }
        public string TotalDisplay { get; set; }
        public double PrePaidRate { get; set; }
        public double PrepaidValue { get; set; }
        public string PrepaidValueDisplay { get; set; }
        public List<MerchantPaymentEachPackageViewModel> lstPaymentPackage { get; set; }
    }
    public class CharteringUpdateStatusModel
    {
        public string UniqueId { get; set; }
        public int StatusFId { get; set; }
    }
    public class RedisStorage
    {
        public string Domain { get; set; }
        public List<YachtPackageServiceModel> PackageStorage { get; set; }
    }
    public class RedisStorageModelView
    {
        public string Domain { get; set; }
        public List<YachtPackageViewModel> PackageStorage { get; set; }
    }
    public class YachtPackageViewModel
    {
        public double YachtFee { get; set; }
        public string DisplayYachtFee { get; set; }
        public double PackageFee { get; set; }
        public string DisplayPackageFee { get; set; }
        public double TotalFee { get; set; }
        public string DisplayTotalFee { get; set; }
        public double Prepaid { get; set; }
        public string DisplayPrepaid { get; set; }
        public string YachtName { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int FileStreamFid { get; set; }
        public string YachtId { get; set; }
        public int Passenger { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string ErrorCode { get; set; }

        public List<MerchantProductInventoriesViewModel> ProductPackage { get; set; }
    }

    public class MerchantProductInventoriesViewModel
    {
        public double PackageFee { get; set; }
        public string DisplayPackageFee { get; set; }
        public double TotalPackageFee { get; set; }
        public string DisplayTotalPackageFee { get; set; }
        public string productInventoryFId { get; set; }
        public string productName { get; set; }
        public string categroryFId { get; set; }
        public int quantity { get; set; }
    }

    public class YachtCartInfo
    {
        public string Name { get; set; }
        public int FileStreamId { get; set; }
    }
}
