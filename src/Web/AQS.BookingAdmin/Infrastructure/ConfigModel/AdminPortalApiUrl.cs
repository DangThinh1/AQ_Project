using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Infrastructure.ConfigModel
{
    public class AdminPortalApiUrl
    {
        public IdentityAdminAPI IdentityAdminAPI { get; set; }
        public IdentityUserAPI IdentityUserAPI { get; set; }
        public PermissionAPI PermissionAPI { get; set; }
        public RestaurantAPI RestaurantAPI { get; set; }
        public YachtAPI YachtAPI { get; set; }
        public EVisaAPI EVisaAPI { get; set; }
        public HotelAPI HotelAPI { get; set; }
        public SelectListAPI SelectListAPI { get; set; }
        public ConfigurationAPI ConfigurationAPI { get; set; }
        public PortalLocation PortalLocation { get; set; }
        public CommonLanguageAPI CommonLanguageAPI { get; set; }
        public CommonResourceAPI CommonResourceAPI { get; set; }
        public CommonValueAPI CommonValueAPI { get; set; }
        public MembershipPrivilegeAPI MembershipPrivilegeAPI { get; set; }
        public FileStreamAPI FileStreamAPI { get; set; }
        public PostAPI PostAPI { get; set; }
        public PostCategoriesAPI PostCategoriesAPI { get; set; }
        public PostCategoryDetailAPI PostCategoryDetailAPI { get; set; }
    }

    public class CommonLanguageAPI
    {
        public string Index { get; set; }
        public string SearchCommonLanguage { get; set; }
        public string GetLangList { get; set; }
        public string GetLanguageByPortalUniqueId { get; set; }
    }

    public class CommonResourceAPI
    {
        public string Index { get; set; }
        public string PostListResource { get; set; }
        public string SearchCommonResource { get; set; }
        public string PostResourceStringByLangId { get; set; }
        public string ResourceLstByLangId { get; set; }
        public string CommonResourceById { get; set; }
    }

    public class CommonValueAPI
    {
        public string Index { get; set; }
        public string CommonValues { get; set; }
        public string SearchCommonValue { get; set; }
        public string GetValueGroupLst { get; set; }
        public string GetByGroupName { get; set; }
    }

    public class PermissionAPI
    {
        public string GetModuleByRole { get; set; }
        public string GetModuleByControllerName { get; set; }
        public string GetModuleByid { get; set; }
        public string GetPageByModuleId { get; set; }
        public string GetModulePageFunction { get; set; }
        public string GetPageFunction { get; set; }
        public string UpdatePageFunction { get; set; }
        public string GetlistFunction { get; set; }
        public string GetRolePageFunction { get; set; }
        public string GetRoleFunctionByRoleId { get; set; }
        public string UpdateRoleFunction { get; set; }
        public string CheckRolePageFunction { get; set; }
    }

    public class RestaurantAPI
    {
        public string RestaurantMerchants { get; set; }
        public string RestaurantMerchantAccounts { get; set; }
        public string GetRestaurantMerchantAccountByMerchantId { get; set; }
        public string RestaurantMerchantAccountMgts { get; set; }
        public string GetRestaurantMerchantAccountMgtByMerchantId { get; set; }
    }

    public class YachtAPI
    {
        public string YachtMerchants { get; set; }
        public string YachtMerchantAccounts { get; set; }
        public string GetYachtMerchantAccountByMerchantId { get; set; }
        public string YachtMerchantAccountMgts { get; set; }
        public string GetYachtMerchantAccountMgtByMerchantId { get; set; }
        public string YachtMerchantCharterFee { get; set; }
        public string YachtMerchantCharterFeeByMerchantId { get; set; }
    }

    public class SelectListAPI
    {
        public string GetRestaurantMerchantWithoutManagerSelectList { get; set; }
        public string GetRestaurantMerchantWithoutAccountSelectList { get; set; }
        public string GetAllRestaurantMerchantSelectList { get; set; }
        public string GetYachtMerchantWithoutManagerSelectList { get; set; }
        public string GetYachtMerchantWithoutAccountSelectList { get; set; }
        public string GetAllYachtMerchantSelectList { get; set; }
        public string GetListYachtAccMgtHasMerchant { get; set; }
        public string GetListYachtAccHasMerchant { get; set; }
        public string GetListResAccMgtHasMerchant { get; set; }
        public string GetListResAccHasMerchant { get; set; }
        public string GetEVisaMerchantNoUserSll { get; set; }
        public string GetAllEVisaMerchantSll { get; set; }
        public string GetLanguageByDomainIdSll { get; set; }
    }

    public class IdentityAdminAPI
    {
        public string Auth { get; set; }
        public string Accounts { get; set; }
        public string GetAccount { get; set; }
        public string GetAccountById { get; set; }
        public string SearchAccounts { get; set; }
        public string GetAllAccounts { get; set; }
        public string ChangeUserStatus { get; set; }
        public string UpdateLanguage { get; set; }
        public string UpdateAvatar { get; set; }
        public string GetRoleByRoleId { get; set; }
    }

    public class IdentityUserAPI
    {
        public string Accounts { get; set; }
        public string GetAccountById { get; set; }
        public string GetAccount { get; set; }
        public string SearchAccounts { get; set; }
        public string GetRoleByDomainId { get; set; }
        public string GetRoleByRoleId { get; set; }
    }

    public class ConfigurationAPI
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CityByCountryName { get; set; }
        public string StateByCityName { get; set; }
        public string AllLanguages { get; set; }
    }

    public class PortalLocation
    {
        public string Index { get; set; }
    }
    public class MembershipPrivilegeAPI
    {
        public string MembershipPrivileges { get; set; }
        public string InsertOrUpdateMembershipPrivilege { get; set; }
    }

    public class EVisaAPI
    {
        public string EVisaMerchant { get; set; }
        public string EVisaMerchantAcc { get; set; }
    }

    public class HotelAPI
    {
        public string HotelMerchant { get; set; }
        public string HotelMerchantUser { get; set; }
        public string HotelMerchantMgt { get; set; }
    }

    public class FileStreamAPI
    {
        public string File { get; set; }
        public string FileData { get; set; }
        public string FileInfo { get; set; }
        public string DeleteFile { get; set; }
    }
    public class PostAPI
    {
        public string Post { get; set; }
        public string RestorePost { get; set; }
        public string ChangeStatus { get; set; }
        public string Search { get; set; }
        public string PostDetail { get; set; }
        public string FileStreamPostDetail { get; set; }
        public string PostDetailLanguageIds { get; set; }
        public string SearchEmailSubcriber { get; set; }
        public string GetListSubToExport { get; set; }
    }

    public class PostCategoriesAPI
    {
        public string PostCategories { get; set; }
        public string Search { get; set; }
        public string PostCategoryParentLst { get; set; }
    }
    public class PostCategoryDetailAPI
    {
        public string PostCategoryDetail { get; set; }
        public string Search { get; set; }
        public string GetPostCateDetailByPostCateId { get; set; }
        public string CheckPostCateDuplicate { get; set; }
    }

}
