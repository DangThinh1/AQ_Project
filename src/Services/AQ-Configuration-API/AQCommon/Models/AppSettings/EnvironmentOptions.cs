namespace AQConfigurations.Core.Models.AppSettings
{
    #region Models

    #region AQ Base Domain
    public class AQBaseDomainPortal : StringOption
    {
        public AQBaseDomainPortal()
        {
            LOCAL = EnvironmentOptionValues.AQBaseDomain_LOCAL;
            VN = EnvironmentOptionValues.AQBaseDomain_VN;
            BETA = EnvironmentOptionValues.AQBaseDomain_BETA;
            LIVE = EnvironmentOptionValues.AQBaseDomain_LIVE;
        }
    }
    #endregion

    #region Yacht
    public class YachtPortal : StringOption {
        public YachtPortal()
        {
            LOCAL = EnvironmentOptionValues.YACHT_PORTAL_LOCAL;
            VN = EnvironmentOptionValues.YACHT_PORTAL_VN;
            BETA = EnvironmentOptionValues.YACHT_PORTAL_BETA;
            LIVE = EnvironmentOptionValues.YACHT_PORTAL_LIVE;
        }
    }
    public class YachtApi : StringOption {
        public YachtApi()
        {
            LOCAL = EnvironmentOptionValues.YACHT_API_LOCAL;
            VN = EnvironmentOptionValues.YACHT_API_VN;
            BETA = EnvironmentOptionValues.YACHT_API_BETA;
            LIVE = EnvironmentOptionValues.YACHT_API_LIVE;
        }
    }
    public class YachtMerchantPortal : StringOption {
        public YachtMerchantPortal()
        {
            LOCAL = EnvironmentOptionValues.YACHT_MERCHANT_PORTAL_LOCAL;
            VN = EnvironmentOptionValues.YACHT_MERCHANT_PORTAL_VN;
            BETA = EnvironmentOptionValues.YACHT_MERCHANT_PORTAL_BETA;
            LIVE = EnvironmentOptionValues.YACHT_MERCHANT_PORTAL_LIVE;
        }
    }
    public class YachtMerchantApi : StringOption {
        public YachtMerchantApi()
        {
            LOCAL = EnvironmentOptionValues.YACHT_MERCHANT_API_LOCAL;
            VN = EnvironmentOptionValues.YACHT_MERCHANT_API_VN;
            BETA = EnvironmentOptionValues.YACHT_MERCHANT_API_BETA;
            LIVE = EnvironmentOptionValues.YACHT_MERCHANT_API_LIVE;
        }
    }
    #endregion Yacht

    #region Dining
    public class DiningPortal : StringOption {
        public DiningPortal()
        {
            LOCAL = EnvironmentOptionValues.DINING_PORTAL_LOCAL;
            VN = EnvironmentOptionValues.DINING_PORTAL_VN;
            BETA = EnvironmentOptionValues.DINING_PORTAL_BETA;
            LIVE = EnvironmentOptionValues.DINING_PORTAL_LIVE;
        }
    }
    public class DiningApi : StringOption {
        public DiningApi()
        {
            LOCAL = EnvironmentOptionValues.DINING_API_LOCAL;
            VN = EnvironmentOptionValues.DINING_API_VN;
            BETA = EnvironmentOptionValues.DINING_API_BETA;
            LIVE = EnvironmentOptionValues.DINING_API_LIVE;
        }
    }
    public class DiningMerchantPortal : StringOption {
        public DiningMerchantPortal()
        {
            LOCAL = EnvironmentOptionValues.DINING_MERCHANT_PORTAL_LOCAL;
            VN = EnvironmentOptionValues.DINING_MERCHANT_PORTAL_VN;
            BETA = EnvironmentOptionValues.DINING_MERCHANT_PORTAL_BETA;
            LIVE = EnvironmentOptionValues.DINING_MERCHANT_PORTAL_LIVE;
        }
    }
    public class DiningMerchantApi : StringOption {
        public DiningMerchantApi()
        {
            LOCAL = EnvironmentOptionValues.DINING_MERCHANT_API_LOCAL;
            VN = EnvironmentOptionValues.DINING_MERCHANT_API_VN;
            BETA = EnvironmentOptionValues.DINING_MERCHANT_API_BETA;
            LIVE = EnvironmentOptionValues.DINING_MERCHANT_API_LIVE;
        }
    }
    #endregion Dining

    #region Accommodation
    public class AccommodationPortal : StringOption {
        public AccommodationPortal()
        {
            LOCAL = EnvironmentOptionValues.ACCOMMODATION_PORTAL_LOCAL;
            VN = EnvironmentOptionValues.ACCOMMODATION_PORTAL_VN;
            BETA = EnvironmentOptionValues.ACCOMMODATION_PORTAL_BETA;
            LIVE = EnvironmentOptionValues.ACCOMMODATION_PORTAL_LIVE;
        }
    }
    public class AccommodationApi : StringOption {
        public AccommodationApi()
        {
            LOCAL = EnvironmentOptionValues.ACCOMMODATION_API_LOCAL;
            VN = EnvironmentOptionValues.ACCOMMODATION_API_VN;
            BETA = EnvironmentOptionValues.ACCOMMODATION_API_BETA;
            LIVE = EnvironmentOptionValues.ACCOMMODATION_API_LIVE;
        }
    }
    public class AccommodationMerchantPortal : StringOption {
        public AccommodationMerchantPortal()
        {
            LOCAL = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_PORTAL_LOCAL;
            VN = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_PORTAL_VN;
            BETA = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_PORTAL_BETA;
            LIVE = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_PORTAL_LIVE;
        }
    }
    public class AccommodationMerchantApi : StringOption {
        public AccommodationMerchantApi()
        {
            LOCAL = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_API_LOCAL;
            VN = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_API_VN;
            BETA = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_API_BETA;
            LIVE = EnvironmentOptionValues.ACCOMMODATION_MERCHANT_API_LIVE;
        }
    }
    #endregion Accommodation

    #region Evisa
    public class EvisaPortal : StringOption { }
    public class EvisaApi : StringOption { }
    public class EvisaMerchantPortal : StringOption { }
    public class EvisaMerchantApi : StringOption { }
    #endregion Evisa

    #region HolidayPackage
    public class HolidayPackagePortal : StringOption { }
    public class HolidayPackageApi : StringOption { }
    public class HolidayPackageMerchantPortal : StringOption { }
    public class HolidayPackageMerchantApi : StringOption { }
    #endregion HolidayPackage

    #region Admins

    public class AdminPortal : StringOption {
        public AdminPortal()
        {
            LOCAL = EnvironmentOptionValues.ADMIN_LOCAL;
            VN = EnvironmentOptionValues.ADMIN_VN;
            BETA = EnvironmentOptionValues.ADMIN_BETA;
            LIVE = EnvironmentOptionValues.ADMIN_LIVE;
        }
    }
    public class AdminApi : StringOption {
        public AdminApi()
        {
            LOCAL = EnvironmentOptionValues.ADMIN_API_LOCAL;
            VN = EnvironmentOptionValues.ADMIN_API_VN;
            BETA = EnvironmentOptionValues.ADMIN_API_BETA;
            LIVE = EnvironmentOptionValues.ADMIN_API_LIVE;
        }
    }

    #endregion Admins

    #region Identity
    public class SSOPortal : StringOption {
        public SSOPortal()
        {
            LOCAL = EnvironmentOptionValues.SSO_IDENTITY_LOCAL;
            VN = EnvironmentOptionValues.SSO_IDENTITY_VN;
            BETA = EnvironmentOptionValues.SSO_IDENTITY_BETA;
            LIVE = EnvironmentOptionValues.SSO_IDENTITY_LIVE;
        }
    }
    public class UserIdentityApi : StringOption {
        public UserIdentityApi()
        {
            LOCAL = EnvironmentOptionValues.USER_IDENTITY_LOCAL;
            VN = EnvironmentOptionValues.USER_IDENTITY_VN;
            BETA = EnvironmentOptionValues.USER_IDENTITY_BETA;
            LIVE = EnvironmentOptionValues.USER_IDENTITY_LIVE;
        }
    }
    public class IdentityApi : StringOption {
        public IdentityApi()
        {
            LOCAL = EnvironmentOptionValues.IDENTITY_LOCAL;
            VN = EnvironmentOptionValues.IDENTITY_VN;
            BETA = EnvironmentOptionValues.IDENTITY_BETA;
            LIVE = EnvironmentOptionValues.IDENTITY_LIVE;
        }
    }
    #endregion Identity

    #region Configuration
    public class ConfigurationsApi : StringOption {
        public ConfigurationsApi()
        {
            LOCAL = EnvironmentOptionValues.CONFIGURATION_LOCAL;
            VN = EnvironmentOptionValues.CONFIGURATION_VN;
            BETA = EnvironmentOptionValues.CONFIGURATION_BETA;
            LIVE = EnvironmentOptionValues.CONFIGURATION_LIVE;
        }
    }
    #endregion Configuration

    #region FileStream
    public class FileStreamsApi : StringOption {
        public FileStreamsApi()
        {
            LOCAL = EnvironmentOptionValues.FILESTREAM_LOCAL;
            VN = EnvironmentOptionValues.FILESTREAM_VN;
            BETA = EnvironmentOptionValues.FILESTREAM_BETA;
            LIVE = EnvironmentOptionValues.FILESTREAM_LIVE;
        }
    }
    #endregion FileStream

    #region ConnectionString
    public class ConnectionString : StringOption {
        private const string DEFAULT_USER = "sa";
        private const string DEFAULT_PASSWORD = "tester@2019";
        private const string DEFAULT_DATABASE = "AQ_DATABASE";

        public ConnectionString()
        {
            var user = DEFAULT_USER;
            var password = DEFAULT_PASSWORD;
            var database = DEFAULT_DATABASE;

            LOCAL = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_LOCAL, database, user, password);
            VN = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_VN, database, user, password);
            BETA = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_BETA, database, user, password);
            LIVE = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_LIVE, database, user, password);
        }

        public ConnectionString(string database, string user = DEFAULT_USER, string password = DEFAULT_PASSWORD)
        {
            user = !string.IsNullOrEmpty(user) ? user : DEFAULT_USER;
            password = !string.IsNullOrEmpty(password) ? password : DEFAULT_PASSWORD;
            database = !string.IsNullOrEmpty(database) ? database : DEFAULT_DATABASE;

            LOCAL = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_LOCAL, database, user, password);
            VN = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_VN, database, user, password);
            BETA = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_BETA, database, user, password);
            LIVE = string.Format(EnvironmentOptionValues.CONNECTIONSTRING_LIVE, database, user, password);
        }
    }
    #endregion ConnectionString

    #region RedisCache

    public class RedisCacheHost : RedisCacheOption {
        public RedisCacheHost()
        {
            LOCAL = new RedisCacheModel()
            {
                Host = EnvironmentOptionValues.REDISCACHE_HOST_LOCAL,
                Port = EnvironmentOptionValues.REDISCACHE_PORT_LOCAL,
                InstanceName = EnvironmentOptionValues.REDISCACHE_NAME_LOCAL,
                Password = EnvironmentOptionValues.REDISCACHE_PASSWORD_LOCAL
            };
            VN = new RedisCacheModel()
            {
                Host = EnvironmentOptionValues.REDISCACHE_HOST_VN,
                Port = EnvironmentOptionValues.REDISCACHE_PORT_VN,
                InstanceName = EnvironmentOptionValues.REDISCACHE_NAME_VN,
                Password = EnvironmentOptionValues.REDISCACHE_PASSWORD_VN
            };
            BETA = new RedisCacheModel()
            {
                Host = EnvironmentOptionValues.REDISCACHE_HOST_BETA,
                Port = EnvironmentOptionValues.REDISCACHE_PORT_BETA,
                InstanceName = EnvironmentOptionValues.REDISCACHE_NAME_BETA,
                Password = EnvironmentOptionValues.REDISCACHE_PASSWORD_BETA
            };
            LIVE = new RedisCacheModel()
            {
                Host = EnvironmentOptionValues.REDISCACHE_HOST_LIVE,
                Port = EnvironmentOptionValues.REDISCACHE_PORT_LIVE,
                InstanceName = EnvironmentOptionValues.REDISCACHE_NAME_LIVE,
                Password = EnvironmentOptionValues.REDISCACHE_PASSWORD_LIVE
            };
        }
    }

    #endregion RedisCache

    #endregion Models
}