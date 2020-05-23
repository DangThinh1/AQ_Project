namespace AQConfigurations.Core.Models.AppSettings
{
    //Alias class of AppSettingOptionsCollection
    public class AQ_ASC : AppSettingCollection { }
    public class AppSettingCollection
    {
        #region AQ Base Domain
        public static AQBaseDomainPortal AQBaseDomainPortal => new AQBaseDomainPortal();
        #endregion
        #region Yachts
        public static YachtPortal YachtPortal => new YachtPortal();
        public static YachtApi YachtApi => new YachtApi();
        public static YachtMerchantPortal YachtMerchantPortal => new YachtMerchantPortal();
        public static YachtMerchantApi YachtMerchantApi => new YachtMerchantApi();
        #endregion Yachts

        #region Dinings
        public static DiningPortal DiningPortal => new DiningPortal();
        public static DiningApi DiningApi => new DiningApi();
        public static DiningMerchantPortal DiningMerchantPortal => new DiningMerchantPortal();
        public static DiningMerchantApi DiningMerchantApi => new DiningMerchantApi();
        #endregion Dinings

        #region Accommodations
        public static AccommodationPortal AccommodationPortal => new AccommodationPortal();
        public static AccommodationApi AccommodationApi => new AccommodationApi();
        public static AccommodationMerchantPortal AccommodationMerchantPortal => new AccommodationMerchantPortal();
        public static AccommodationMerchantApi AccommodationMerchantApi => new AccommodationMerchantApi();
        #endregion Accommodations

        #region Evisas
        public static EvisaPortal EvisaPortal => new EvisaPortal();
        public static EvisaApi EvisaApi => new EvisaApi();
        public static EvisaMerchantPortal EvisaMerchantPortal => new EvisaMerchantPortal();
        public static EvisaMerchantApi EvisaMerchantApi => new EvisaMerchantApi();
        #endregion Evisas

        #region HolidayPackages
        public static HolidayPackagePortal HolidayPackagePortal => new HolidayPackagePortal();
        public static HolidayPackageApi HolidayPackageApi => new HolidayPackageApi();
        public static HolidayPackageMerchantPortal HolidayPackageMerchantPortal => new HolidayPackageMerchantPortal();
        public static HolidayPackageMerchantApi HolidayPackageMerchantApi => new HolidayPackageMerchantApi();
        #endregion HolidayPackages

        #region Admins
        public static AdminPortal AdminPortal => new AdminPortal();
        public static AdminApi AdminApi => new AdminApi();
        #endregion Admins

        #region Identities
        public static SSOPortal AQSSOPortal => new SSOPortal();
        public static UserIdentityApi UserIdentityApi => new UserIdentityApi();
        public static IdentityApi IdentityApi => new IdentityApi();
        #endregion Identities

        #region Configurations

        public static ConfigurationsApi ConfigurationsApi => new ConfigurationsApi();

        #endregion Configurations

        #region FileStreamsApi

        public static FileStreamsApi FileStreamsApi => new FileStreamsApi();

        #endregion FileStreamsApis


        #region RedisCache
        public static RedisCacheHost RedisCacheHost => new RedisCacheHost();

        #endregion RedisCache

        #region ConnectionString
        public static ConnectionString ConnectionString = new ConnectionString();
        #endregion ConnectionString
    }
}