using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Models.Config
{
   
        public class YachtPortalApiUrl
        {
            public RestaurantApi Restaurant { get; set; }
            public RestaurantAttributeApi RestaurantAttribute { get; set; }
            public CommonValuesApi CommonValues { get; set; }
            public PortalLocationApi PortalLocations { get; set; }
            public PortalLanguageApi PortalLanguages { get; set; }
            public PortLocationApi PortLocation { get; set; }
            public YachtApi Yatchs { get; set; }
            public CommonResource CommonResource { get; set; }
            public CurrenciesApi Currencies { get; set; }
            public RestaurantMerchantApi RestaurantMerchant { get; set; }
            public YachtMerchantApi YachtMerchants { get; set; }
            public YachtAttributevaluedApi YachtAttributevalues { get; set; }
            public YachtFileStreamApi YachtFileStreams { get; set; }
            public YachtAdditionalApi YachtAdditionals { get; set; }
            public YachtMerchantProductInventoryApi YachtMerchantProductInventories { get; set; }
            public YachtPricingPlanInfomationsApi YachtPricingPlanInfomations { get; set; }
            public YachtInformationDetailsApi YachtInformationDetails { get; set; }
            public YachtPricingPlanDetailsApi YachtPricingPlanDetails { get; set; }
            public YachtCharteringsApi YachtCharterings { get; set; }
            public StripePaymentApi StripePayment { get; set; }
            public YachtCharteringPaymentLogAPI YachtCharteringPaymentLog { get; set; }
            public YachtTourApi YachtTourApi { get; set; }
            public RedisCacheAPI RedisCache { get; set; }
        }
        public class RedisCacheAPI
        {
            public string KeyAddModel { get; set; }
            public string SimpleKeyAdd { get; set; }
            public string KeyGet { get; set; }
            public string KeyRemove { get; set; }

            public string HaskeyGetAll { get; set; }

            public string YachtAddModel { get; set; }
            public string YachtGet { get; set; }
            public string YachtRemove { get; set; }
            public string YachtCopyLocalStorage { get; set; }
        }
        public class StripePaymentApi
        {
            public string GetTokenCardStripe { get; set; }
            public string RequestPaymentStripe { get; set; }
            public string ProccessPaymentStrip { get; set; }
            public string AuthenticationToken { get; set; }
            public string UserName { get; set; }
            public string PassWord { get; set; }
            public string Url { get; set; }
            public string GetTransaction { get; set; }
            public string ExecutePaymentPayPal { get; set; }
        }

        public class RestaurantApi
        {
            public string Search { get; set; }
            public string Detail { get; set; }
            public string ComboBinding { get; set; }
            public string RestaurantPartners { get; set; }
            public string RestaurantByMerchantId { get; set; }
            public string RestaurantVenueLst { get; set; }
        }
        public class RestaurantMerchantApi
        {
            public string ResMerchantsByDisplayNumber { get; set; }
            public string ResMerchantsById { get; set; }
        }
        public class RestaurantAttributeApi
        {
            public string GetAllByCategoryFid { get; set; }
            public string GetAllByListCategoryFids { get; set; }
            public string ForSearch { get; set; }
        }
     
        public class PortalLocationApi
        {
            public string GetLocationsByPortalUniqueId { get; set; }
            public string GetLocationsByPortalUniqueIdAndCountryCode { get; set; }
            public string GetLocationsByPortalUniqueIdAndCountryName { get; set; }
        }

        public class PortalLanguageApi
        {
            public string GetLanguageByPortalUniqueId { get; set; }
        }
        public class PortLocationApi
        {
            public string PortLocationByCity { get; set; }
            public string PortLocationByCountry { get; set; }
    }
        public class YachtApi
        {
            public string Search { get; set; }
            public string SearchSimilar { get; set; }
            public string Detail { get; set; }
            public string ListYachName { get; set; }
            public string FindingById { get; set; }
            public string ListYachts { get; set; }
            public string GetAllMerchantYacht { get; set; }
            public string YachtsByMerchantId { get; set; }
            public string CharterPrivatePayment { get; set; }
            public string YachtAnyExclusive { get; set; }
            public string YachtAnyNew { get; set; }
            public string YachtAnyPromotion { get; set; }
            public string BookingList { get; set; }
            public string BookingDetail { get; set; }
            public string BookingUniqueId { get; set; }
            public string CartDetail { get; set; }
            public string CartStorage { get; set; }
            public string CartDelete { get; set; }
            public string CartAdd { get; set; }
            public string CartTotalFee { get; set; }
        }
        public class YachtCharteringPaymentLogAPI
        {
            public string CharteringPaymentLogBycharteringFId { get; set; }
            public string CharteringPaymentLogByCharteringUniqueId { get; set; }
            public string Update { get; set; }
            public string UpdateBycharteringUniqueId { get; set; }
        }
        public class YachtMerchantApi
        {
            public string YachtMerchantsByDisplayNumber { get; set; }
            public string YachtMerchantsById { get; set; }
            public string YachtMerchantFileStream { get; set; }
        }
        public class YachtAttributevaluedApi
        {
            public string CharterPrivate { get; set; }
            public string CharterGeneral { get; set; }
        }
        public class YachtFileStreamApi
        {
            public string FileStream { get; set; }
            public string FileStreamPaging { get; set; }
        }
        public class YachtAdditionalApi
        {
            public string AddictionalPackageByYachtId { get; set; }
        }
        public class YachtMerchantProductInventoryApi
        {
            public string ProductInventoriesByadditionalFId { get; set; }
            public string ProductInventoriesWithPricingByadditionalFId { get; set; }
            public string PriceOfProductInventoryByArrayOfProductId { get; set; }
        }
        public class YachtPricingPlanInfomationsApi
        {
            public string PricingPlanInfomationByYachtFId { get; set; }
        }
        public class YachtPricingPlanDetailsApi
        {
            public string PricingPlanDetailYachtFId { get; set; }
            public string PricingPlanDetailYachtFIdAndPricingTypeFId { get; set; }
            public string PricingPlanDetailYachtFIdPricingTypeFId { get; set; }
        }
        public class YachtInformationDetailsApi
        {
            public string YachtInformationDetailByYachtFId { get; set; }
        }
        public class YachtCharteringsApi
        {
            public string CharteringDetail { get; set; }
            public string Chartering { get; set; }
            public string CharteringUniqueId { get; set; }
            public string UpdateStatus { get; set; }
            public string CharteringById { get; set; }
        }

        public class CommonResource
        {
            public string GetAll { get; set; }
        }
        public class CurrenciesApi
        {
            public string CurrencyByCountryName { get; set; }
        }

        public class YachtTourApi
        {
            public string AllTour { get; set; }
            public string TourDetail { get; set; }
            public string TourFileStream { get; set; }
            public string TourAttribute { get; set; }
            public string TourCategory { get; set; }
            public string YachtOfTour { get; set; }
            public string YachtTourPrice { get; set; }
            public string YachtTourCharter { get; set; }
            public string YachtTourCharterByUid { get; set; }
            public string YachtTourPaymentLog { get; set; }
        }
    
}
