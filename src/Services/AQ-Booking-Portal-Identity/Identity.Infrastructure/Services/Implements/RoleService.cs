using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using Identity.Core.Models.Roles;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Database.Entities;
using Identity.Infrastructure.Services.Interfaces;
namespace Identity.Infrastructure.Services.Implements
{
    public class RoleService : IRoleService
    {
        private readonly IdentityDbContext _db;
        private readonly IMapper _mapper;

        public RoleService(IMapper mapper,IdentityDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        public BaseResponse<List<RoleViewModel>> GetRolesByDomain(string domainFid)
        {
            try
            {
                if(string.IsNullOrEmpty(domainFid))
                    return BaseResponse<List<RoleViewModel>>.BadRequest();
                var result = _db.Roles
                    .AsNoTracking()
                    .Where(k=>k.DomainFid == domainFid)
                    .Select(k=> _mapper.Map<Roles, RoleViewModel>(k))
                    .ToList();
                if(result != null)
                    return BaseResponse<List<RoleViewModel>>.Success(result);
                return BaseResponse<List<RoleViewModel>>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RoleViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<RoleViewModel>> GetAllRoles()
        {
            try
            {
                var result = _db.Roles
                    .AsNoTracking()
                    .Select(k => _mapper.Map<Roles, RoleViewModel>(k))
                    .ToList();
                if (result != null)
                    return BaseResponse<List<RoleViewModel>>.Success(result);
                return BaseResponse<List<RoleViewModel>>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RoleViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<RoleViewModel>> GetSubordinateRole(int id)
        {
            try
            {
                var role = _db.Roles.Find(id);
                var subordinateRoles = _db.RoleControls
                    .AsNoTracking()
                    .Include(k => k.SubordinateRole)
                    .Where(k => k.SuperiorFid == id)
                    .Select(k => _mapper.Map<Roles, RoleViewModel>(k.SubordinateRole))
                    .ToList();
                if(subordinateRoles != null)
                    return BaseResponse<List<RoleViewModel>>.Success(subordinateRoles);
                return BaseResponse<List<RoleViewModel>>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RoleViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
