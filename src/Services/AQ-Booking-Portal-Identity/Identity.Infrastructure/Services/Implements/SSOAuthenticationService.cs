using APIHelpers.Response;
using Identity.Core.Portal.Models.SSOAuthentication;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Database.Entities;
using Identity.Infrastructure.Helpers;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Infrastructure.Services.Implements
{
    public class SSOAuthenticationService : ISSOAuthenticationService
    {
        private readonly IdentityDbContext _db;
        public SSOAuthenticationService(IdentityDbContext db)
        {
            _db = db;
        }

        public BaseResponse<string> Create(List<SSOAuthenticationCreateModel> createModel)
        {
            try
            {
                if(createModel == null)
                    return BaseResponse<string>.BadRequest();

                var authId = UniqueIDHelper.GenarateRandomString(12);
                var entitiesToCreate = createModel.Select(k => new SSOAuthentication()
                {
                    AuthId = authId,
                    DomainId = k.DomainId,
                    Token = k.Token,
                    RedirectUrl =k.RedirectUrl,
                    Type = k.Type,
                    UserUid = k.UserUid
                });

                _db.SSOAuthentication.AddRange(entitiesToCreate);

                if(_db.SaveChanges() > 0)
                    return BaseResponse<string>.Success(authId);

                return BaseResponse<string>.NotFound();
            }
            catch(Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<SSOAuthenticationViewModel> FindByDomainId(string id, string domainId)
        {
            try
            {
                if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(domainId))
                    return BaseResponse<SSOAuthenticationViewModel>.BadRequest();

                var findedEntity = _db.SSOAuthentication.AsNoTracking().FirstOrDefault(k=> k.AuthId == id && k.DomainId == domainId);
                if(findedEntity != null)
                    return BaseResponse<SSOAuthenticationViewModel>.Success(new SSOAuthenticationViewModel() {
                        AuthId = findedEntity.AuthId,
                        Id = findedEntity.Id,
                        DomainId = findedEntity.DomainId,
                        RedirectUrl = findedEntity.RedirectUrl,
                        Token = findedEntity.Token,
                        Type = findedEntity.Type,
                    });

                return BaseResponse<SSOAuthenticationViewModel>.NotFound();
            }
            catch(Exception ex)
            {
                return BaseResponse<SSOAuthenticationViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<SSOAuthenticationViewModel>> FindById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BaseResponse<List<SSOAuthenticationViewModel>>.BadRequest();

                var result = _db.SSOAuthentication
                    .AsNoTracking()
                    .Where(k => k.AuthId == id).
                    Select(k => new SSOAuthenticationViewModel()
                    {
                        Id = k.Id,
                        DomainId = k.DomainId,
                        RedirectUrl = k.RedirectUrl,
                        Token = k.Token,
                        Type = k.Type,
                    })
                    .ToList();

                if (result != null)
                    return BaseResponse<List<SSOAuthenticationViewModel>>.Success(result);

                return BaseResponse<List<SSOAuthenticationViewModel>>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SSOAuthenticationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteById(string id)
        {
            try
            {
                var entitiesToDelete = _db.SSOAuthentication.Where(k => k.AuthId == id).ToList();
                _db.SSOAuthentication.RemoveRange(entitiesToDelete);
                if(_db.SaveChanges() > 0)
                    return BaseResponse<bool>.Success(true);
                return BaseResponse<bool>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteByUserUid(string uid)
        {
            try
            {
                var entitiesToDelete = _db.SSOAuthentication.Where(k => k.UserUid == uid).ToList();
                _db.SSOAuthentication.RemoveRange(entitiesToDelete);
                if (_db.SaveChanges() > 0)
                    return BaseResponse<bool>.Success(true);
                return BaseResponse<bool>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}