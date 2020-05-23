using System;
using System.Linq;
using ExtendedUtility;
using Omu.ValueInjecter;
using APIHelpers.Response;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Identity.Core.Models.Users;
using Identity.Core.Models.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Identity.Infrastructure.Database;
using System.ComponentModel.DataAnnotations;
using Identity.Infrastructure.Database.Entities;
using Identity.Infrastructure.Services.Interfaces;
using AutoMapper;

namespace Identity.Infrastructure.Services.Implements
{
    public class AccountService : IdentityServiceBase, IAccountService
    {
        private readonly IMapper _mapper;

        public AccountService(IdentityDbContext db, UserManager<Users> userManager, IMapper mapper) : base(db, userManager)
        {
            _mapper = mapper;
        }

        #region Method
        public BaseResponse<bool> VerifyEmailForSignIn(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || !new EmailAddressAttribute().IsValid(email))
                    return BaseResponse<bool>.BadRequest(message: "Email is invalid");

                if (Find(email) != null)
                    return BaseResponse<bool>.Success(message: "Email can be signin");

                return BaseResponse<bool>.BadRequest(message: "Couldn't find your email");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> VerifyEmailForCreate(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || !new EmailAddressAttribute().IsValid(email))
                    return BaseResponse<bool>.BadRequest(message: "Email is invalid");

                if(Find(email) == null)
                    return BaseResponse<bool>.Success(message: "Email can be use");
                else
                    return BaseResponse<bool>.BadRequest(message: "Email already exists");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> Resigster(UserCreateModel registerModel, string creatorId)
        {
            try
            {
                if (registerModel == null)
                    return BaseResponse<bool>.BadRequest();
                var userId = Guid.NewGuid();
                var createdById = !string.IsNullOrEmpty(creatorId) ? Guid.Parse(creatorId) : userId;
                var user = new Users(registerModel.Email, registerModel.FirstName ?? string.Empty, registerModel.LastName ?? string.Empty)
                {
                    CreatedBy = createdById,
                };
                var createUserResult = _userManager.CreateAsync(user, registerModel.Password).Result;
                if (createUserResult.Succeeded)
                {
                    var RoleId = registerModel.RoleId.ToInt32();
                    return (RoleId > 0) ? GrantRolesToUser(user.Email, RoleId) : BaseResponse<bool>.Success(true);
                }
                string msesage = string.Join(";", createUserResult.Errors.Select(k => k.Description).ToList());
                return BaseResponse<bool>.BadRequest(message: msesage);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> GrantRolesToUser(string email, int roleId)
        {
            try
            {
                var user = _userManager.FindByEmailAsync(email).Result;
                var role = _db.Roles.AsNoTracking().FirstOrDefault(k => k.Id == roleId) ?? new Roles() ;
                var roles = new List<Roles>() {
                    role
                };
                var actionResult = GrantRolesToUser(user, roles);
                return actionResult;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> GrantRolesToUser(Users user, List<Roles> roles)
        {
            try
            {
                if (user == null || roles == null || roles.Count == 0)
                    return BaseResponse<bool>.BadRequest();
                var userRoles = new List<UserRoles>();
                foreach (var r in roles)
                {
                    userRoles.Add(new UserRoles()
                    {
                        UserFid = user.Id,
                        RoleFid = r.Id
                    });
                }
                _db.UserRoles.AddRange(userRoles);
                var actionResult = _db.SaveChanges();
                if (actionResult == 1)
                {
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<UserProfileViewModel> GetUserProfile(string key)
        {
            try
            {
                Users user = Find(key);
                if (user == null)
                    return BaseResponse<UserProfileViewModel>.NotFound();
                LoadRelated(user);
                var model = _mapper.Map<Users, UserProfileViewModel>(user);
                return BaseResponse<UserProfileViewModel>.Success(model);
            }
            catch (Exception ex)
            {
                return BaseResponse<UserProfileViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<PagedList<UserProfileViewModel>> Search(UserSearchModel model)
         {
            try
            {
                if (model == null)
                    return BaseResponse<PagedList<UserProfileViewModel>>.BadRequest();
                var roles = model.RoleIds == null || model.RoleIds.Count == 0 ? null : model.RoleIds;
                var query = _db.Users
                    .Where(k => k.Deleted == false
                             && (roles == null || (k.UserRoles != null && k.UserRoles.FirstOrDefault() != null && roles.Contains(k.UserRoles.FirstOrDefault().RoleFid)))
                             && (string.IsNullOrEmpty(model.SearchString)
                                || k.FirstName.Contains(model.SearchString)
                                || k.LastName.Contains(model.SearchString)
                                || k.Email.Contains(model.SearchString)
                                || k.PhoneNumber.Contains(model.SearchString)
                                || k.Designation.Contains(model.SearchString)));

                var sortColumn = ObjectContainPropertyName(typeof(Users), model.SortColumn) ? model.SortColumn : "Email";
                var sortType = string.IsNullOrEmpty(model.SortType) || model.SortType.ToUpper() != "DESC" ? "ASC" : "DESC";
                var sortString = $"{sortColumn} {sortType}";

                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var pageSize = model.PageSize == -1 ? query.Count() : model.PageSize != 0 ? model.PageSize : 10;
                var skip = pageSize == -1 ? 0 : pageSize * (pageIndex - 1);
                var take = pageSize == -1 ? query.Count() : pageSize;

                var entities = query
                    .OrderBy(sortString)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                entities.ForEach((item) => LoadRelated(item));

                var data = entities
                    .Select(k => _mapper.Map<Users, UserProfileViewModel>(k))
                    .ToList();

                var pagedList = new PagedList<UserProfileViewModel>(data, pageIndex, pageSize, query.Count());
                return BaseResponse<PagedList<UserProfileViewModel>>.Success(pagedList);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<UserProfileViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> UpdateProperty(string key, string propertyName, object value)
        {
            try
            {
                var user = Find(key);
                if (user == null)
                    return BaseResponse<bool>.NotFound();
                 
                var propertyList = user.GetType().GetProperties();
                foreach(var p in propertyList)
                {
                    if(p.Name.ToUpper() == propertyName.ToUpper())
                    {
                        p.SetValue(user, value);
                    }
                }
                _db.Update(user);
                if (_db.SaveChanges() == 1)
                    return BaseResponse<bool>.Success(true);
                return BaseResponse<bool>.BadRequest(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> UpdateProfile(string key, UserProfileUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var user = Find(key);
                if (user == null)
                    return BaseResponse<bool>.NotFound();
                user.InjectFrom(model);
                _db.Update(user);
                var succced = _db.SaveChanges() == 1;
                if(succced)
                    return BaseResponse<bool>.Success();
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> UpdateUser(Users user)
        {
            try
            {
                if (user == null)
                    return BaseResponse<bool>.BadRequest();
                var userDb = Find(user.Email);
                userDb.InjectFrom(user);
                _db.Update(userDb);
                 var saveSucceed = _db.SaveChanges() == 1;
                if(saveSucceed)
                    return BaseResponse<bool>.Success(true);
                return BaseResponse<bool>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion

        #region Private Method
        private List<string> GetPropertyNames(Type type)
        {
            try
            {
                var listPropNames = type.GetProperties().Select(k => k.Name).ToList();
                return listPropNames;
            }
            catch
            {
                return null;
            }
        }
        private bool ObjectContainPropertyName(Type type, string propertyName)
        {
            try
            {
                return GetPropertyNames(type).Any(k => k.ToUpper() == propertyName.ToUpper());
            }
            catch
            {
                return false;
            }
        }
        public BaseResponse<bool> ChangePassword(string key, string password)
        {
            try
            {
                var user = Find(key);
                if (user == null)
                    return BaseResponse<bool>.NotFound();
                var updateSecurityStampResult = _userManager.UpdateSecurityStampAsync(user).Result;
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
                user.PasswordHash = newPasswordHash;
                var result = _userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                    return BaseResponse<bool>.Success(true);
                return BaseResponse<bool>.BadRequest(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion Private Method
    }
}
