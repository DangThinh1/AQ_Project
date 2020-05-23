using AQBooking.Admin.Core.Models.CommonLanguague;
using AQBooking.Admin.Core.Models.CommonValue;
using AQBooking.Admin.Core.Models.PortalLocation;
using AQBooking.Admin.Core.Models.RestaurantMerchantAccount;
using AQBooking.Admin.Core.Models.RestaurantMerchantAccountMgt;
using AQBooking.Admin.Core.Models.YachtMerchant;
using AQBooking.Admin.Core.Models.YachtMerchantAccount;
using AQBooking.Admin.Core.Models.YachtMerchantAccountMgt;
using AQBooking.Admin.Core.Models.RestaurantMerchant;
using AQBooking.Admin.Core.Models.YachtMerchantCharterFee;

using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Databases.DiningEntities;
using AQBooking.Admin.Core.Models.YachtMerchantFileStream;
using AQBooking.Admin.Core.Models.MembershipPrivileges;
using AQBooking.Admin.Core.Models.YachtTourAttribute;
using AQBooking.Admin.Core.Models.YachtAttribute;
using AQBooking.Admin.Core.Models.EVisaMerchant;
using AQBooking.Admin.Infrastructure.Databases.EvisaEntities;
using AQBooking.Admin.Core.Models.EVisaMerchantAccount;
using AQBooking.Admin.Core.Models.HotelMerchant;
using AQBooking.Admin.Infrastructure.Databases.HotelEntities;
using AQBooking.Admin.Core.Models.HotelMerchantUser;
using AQBooking.Admin.Core.Models.CommonResource;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;
using AQBooking.Admin.Core.Models.Subscriber;
using AutoMapper;
using AQBooking.Admin.Core.Models.PostCategories;

namespace AQBooking.Admin.Infrastructure
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region RestaurantMerchant
            CreateMap<RestaurantMerchants, RestaurantMerchantViewModel>();
            CreateMap<RestaurantMerchantViewModel, RestaurantMerchants>();
            CreateMap<RestaurantMerchants, RestaurantMerchantCreateModel>();
            CreateMap<RestaurantMerchantCreateModel, RestaurantMerchants>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.UniqueId, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region RestaurantMerchantUser
            CreateMap<RestaurantMerchantAccViewModel, RestaurantMerchantUsers>();
            CreateMap<RestaurantMerchantUsers, RestaurantMerchantAccViewModel>();
            CreateMap<RestaurantMerchantUsers, RestaurantMerchantAccCreateModel>();
            CreateMap<RestaurantMerchantAccCreateModel, RestaurantMerchantUsers>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region RestaurantMerchantAqmgts
            CreateMap<RestaurantMerchantAccMgtViewModel, RestaurantMerchantAqmgts>();
            CreateMap<RestaurantMerchantAqmgts, RestaurantMerchantAccMgtViewModel>();
            CreateMap<RestaurantMerchantAqmgts, RestaurantMerchantAccMgtCreateModel>();
            CreateMap<RestaurantMerchantAccMgtCreateModel, RestaurantMerchantAqmgts>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region YachtMerchant
            CreateMap<YachtMerchants, YachtMerchantViewModel>();
            CreateMap<YachtMerchantViewModel, YachtMerchants>();
            CreateMap<YachtMerchants, YachtMerchantCreateModel>();
            CreateMap<YachtMerchantCreateModel, YachtMerchants>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.UniqueId, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region YachtMerchantUser
            CreateMap<YachtMerchantAccViewModel, YachtMerchantUsers>();
            CreateMap<YachtMerchantUsers, YachtMerchantAccViewModel>();
            CreateMap<YachtMerchantUsers, YachtMerchantAccCreateModel>();
            CreateMap<YachtMerchantAccCreateModel, YachtMerchantUsers>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region YachtMerchantAqmgts
            CreateMap<YachtMerchantAccMgtViewModel, YachtMerchantAqmgts>();
            CreateMap<YachtMerchantAqmgts, YachtMerchantAccMgtViewModel>();
            CreateMap<YachtMerchantAqmgts, YachtMerchantAccMgtCreateModel>();
            CreateMap<YachtMerchantAccMgtCreateModel, YachtMerchantAqmgts>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region YachtMerchantCharterFee
            CreateMap<YachtMerchantCharterFeeOptions, YachtMerchantCharterFeeViewModel>();
            CreateMap<YachtMerchantCharterFeeUpdateModel, YachtMerchantCharterFeeOptions>();
            CreateMap<YahctMerchantCharterFeeCreateModel, YachtMerchantCharterFeeOptions>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region YachtMerchantFileStream
            CreateMap<YachtMerchantFileStreams, YachtMerchantFileStreamViewModel>();
            CreateMap<YachtMerchantFileStreamUpdateModel, YachtMerchantFileStreams>();
            CreateMap<YachtMerchantFileStreamCreateModel, YachtMerchantFileStreams>()
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region PortalLocation
            CreateMap<PortalLocationViewModel, PortalLocationControls>();
            CreateMap<PortalLocationControls, PortalLocationViewModel>();
            CreateMap<PortalLocationControls, PortalLocationCreateModel>();
            CreateMap<PortalLocationCreateModel, PortalLocationControls>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            #endregion

            #region CommonValue
            CreateMap<CommonValueCreateModel, CommonValues>();
            CreateMap<CommonValues, CommonValueCreateModel>();

            CreateMap<CommonValueViewModel, CommonValues>();
            CreateMap<CommonValues, CommonValueViewModel>();

            CreateMap<CommonValueUpdateModel, CommonValues>();
            CreateMap<CommonValues, CommonValueUpdateModel>();
            #endregion

            #region CommonLanguage
            CreateMap<CommonLanguaguesCreateModel, CommonLanguages>();
            CreateMap<CommonLanguages, CommonLanguaguesCreateModel>();

            CreateMap<CommonLanguaguesViewModel, CommonLanguages>();
            CreateMap<CommonLanguages, CommonLanguaguesViewModel>();

            CreateMap<CommonLanguaguesUpdateModel, CommonLanguages>();
            CreateMap<CommonLanguages, CommonLanguaguesUpdateModel>();
            #endregion

            #region Common Resource
            CreateMap<CommonResources, CommonResourcesViewModel>();
            CreateMap<CommonResourcesViewModel, CommonResources>();
            CreateMap<CommonResourcesCreateModel, CommonResources>();
            CreateMap<CommonResources, CommonResourcesCreateModel>();
            #endregion

            #region MembershipPrivilege
            CreateMap<MembershipPrivilegesCreateModel, AqmembershipDiscountPrivileges>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore());
            CreateMap<AqmembershipDiscountPrivileges, MembershipPrivilegesCreateModel>();

            CreateMap<MembershipPrivilegesDetailCreateModel, AqmembershipDiscountPrivilegeDetails>()
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore()); ;
            CreateMap<AqmembershipDiscountPrivilegeDetails, MembershipPrivilegesDetailCreateModel>();

            #endregion

            #region Yacht Attribute
            CreateMap<YachtTourAttributeCreateModel, YachtTourAttributes>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore());
            CreateMap<YachtAttributeCreateModel, YachtAttributes>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore());

            CreateMap<YachtTourAttributes, YachtTourAttributeCreateModel>();
            CreateMap<YachtAttributes, YachtAttributeCreateModel>();

            #endregion

            #region EvisaMerchant
            CreateMap<EVisaMerchantCreateUpdateModel, VisaMerchants>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore());
            CreateMap<VisaMerchants, EVisaMerchantViewModel>();
            #endregion

            #region EvisaMerchantUser
            CreateMap<EVisaMerchantAccCreateUpdateModel, VisaMerchantUsers>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore());
            CreateMap<VisaMerchantUsers, EVisaMerchantAccViewModel>();
            #endregion

            #region EVisaMerchantManagement
            #endregion

            #region HotelMerchant
            CreateMap<HotelMerchantCreateUpdateModel, HotelMerchants>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore());
            CreateMap<HotelMerchants, HotelMerchantViewModel>();
            #endregion

            #region HotelMerchantUser
            CreateMap<HotelMerchantUserCreateUpdateModel, HotelMerchantUsers>()
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedDate, x => x.Ignore());
            CreateMap<HotelMerchantUsers, HotelMerchantUserViewModel>();
            #endregion

            #region Post, PostDetail, PostFileStream

            CreateMap<Posts, PostCreateModel>();
            CreateMap<PostCreateModel, Posts>();

            CreateMap<Posts, PostViewModel>();
            CreateMap<PostViewModel, Posts>();

            CreateMap<PostDetails, PostDetailCreateModel>();
            CreateMap<PostDetailCreateModel, PostDetails>();

            CreateMap<PostDetails, PostDetailViewModel>();
            CreateMap<PostDetailViewModel, PostDetails>();

            CreateMap<PostFileStreams, PostFileStreamCreateModel>();
            CreateMap<PostFileStreamCreateModel, PostFileStreams>();

            CreateMap<PostFileStreams, PostFileStreamViewModel>();
            CreateMap<PostFileStreamViewModel, PostFileStreams>();

            #endregion

            #region Post category , category detail
            CreateMap<PostCategories, PostCategoriesViewModel>();
            CreateMap<PostCategoriesViewModel, PostCategories>();
            CreateMap<PostCategoriesCreateModel, PostCategories>();
            CreateMap<PostCategories, PostCategoriesCreateModel>();

            CreateMap<PostCategoryDetails, PostCategoryDetailViewModel>();
            CreateMap<PostCategoryDetailViewModel, PostCategoryDetails>();

            CreateMap<PostCategoryDetailCreateModel, PostCategoryDetails>();
            CreateMap<PostCategoryDetails, PostCategoryDetailCreateModel>();
            #endregion

            #region Subscriber            
            CreateMap<Subscribers, SubscriberCreateModel>();
            CreateMap<SubscriberCreateModel, Subscribers>();

            CreateMap<Subscribers, SubscriberViewModel>();
            CreateMap<SubscriberViewModel, Subscribers>();
            #endregion
        }
    }
}