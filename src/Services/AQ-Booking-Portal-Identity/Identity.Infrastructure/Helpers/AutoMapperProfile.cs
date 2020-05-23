using AutoMapper;
using Identity.Core.Models.Roles;
using Identity.Core.Models.Users;
using Identity.Core.Models.Authentications;
using Identity.Infrastructure.Database.Entities;
using Identity.Core.Portal.Models.SigninControls;

namespace Identity.Infrastructure.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Roles, RoleViewModel>();

            CreateMap<Users, UserProfileViewModel>()
                .ForMember(model=> model.RoleId, entity=> entity.MapFrom(k=> k.Roles != null ? k.Roles.Id : 0))
                .ForMember(model => model.RoleName, entity => entity.MapFrom(k => k.Roles != null ? k.Roles.Name : string.Empty))
                .ForMember(model => model.DomainFid, entity => entity.MapFrom(k => k.Roles != null ? k.Roles.DomainFid : string.Empty));

            CreateMap<Users, UserProfileUpdateModel>();

            CreateMap<SigninControls, SigninControlViewModel>();
            CreateMap<SigninControlCreateModel, SigninControls>();

            CreateMap<Users, AuthenticateViewModel>()
                .ForMember(model => model.UserId, entity => entity.MapFrom(k => k.UserId.ToString()))
                .ForMember(model => model.RoleId, entity => entity.MapFrom(k => k.Roles.Id))
                .ForMember(model => model.UserRole, entity => entity.MapFrom(k => k.Roles.Name))
                .ForMember(model => model.Name, entity => entity.MapFrom(k => $"{k.FirstName} {k.LastName}"));
        }
    }
}
