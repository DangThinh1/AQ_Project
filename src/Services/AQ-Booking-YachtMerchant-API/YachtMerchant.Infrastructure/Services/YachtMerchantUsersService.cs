using APIHelpers.Response;
using AutoMapper;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtMerchantUsers;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtMerchantUsersService: ServiceBase, IYachtMerchantUsersService
    {

        private readonly IMapper _mapper;
        public YachtMerchantUsersService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public BaseResponse<YachtMerchantUsersViewModel> GetInfomationOfMerchantUserById(int id)
        {
            try
            {
                var entity = _context.YachtMerchantUsers.Find(id);
                if (entity == null)
                    return BaseResponse<YachtMerchantUsersViewModel>.NotFound();
                else
                    return BaseResponse<YachtMerchantUsersViewModel>.Success(_mapper.Map<YachtMerchantUsers, YachtMerchantUsersViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtMerchantUsersViewModel>.InternalServerError(message: ex.Message);
            }
        }


        public BaseResponse<List<YachtMerchantUsersViewModel>> GetAllUserOfMerchantByMerchantId(int merchantId)
        {
            try
            {
                var entity = _context.YachtMerchantUsers.AsNoTracking().Where(x => x.MerchantFid == merchantId && x.Deleted==false).Select(s => _mapper.Map<YachtMerchantUsers, YachtMerchantUsersViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<YachtMerchantUsersViewModel>>.Success(entity.ToList());
                else
                    return BaseResponse<List<YachtMerchantUsersViewModel>>.NoContent(new List<YachtMerchantUsersViewModel>());

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantUsersViewModel>>.InternalServerError(message: ex.Message);
            }
        }

        public BaseResponse<List<SelectListItem>> GetDropdownListUserOfMerchantByMerchantId(int merchantId)
        {
            try
            {
                var entity = _context.YachtMerchantUsers.Where(x => x.MerchantFid == merchantId && x.Deleted == false).OrderBy(x=>x.CreatedDate).AsNoTracking().Select( m=> new SelectListItem {  Text= m.UserName, Value= m.UserFid.ToString()});
                if (entity.Count() > 0)
                    return BaseResponse<List<SelectListItem>>.Success(entity.ToList());
                else
                    return BaseResponse<List<SelectListItem>>.NoContent(new List<SelectListItem>());
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SelectListItem>>.InternalServerError(message: ex.Message);
            }
        }


        public BaseResponse<List<YachtMerchantUsersViewModel>> GetAllUserOfMerchantByRole(YachtMerchantUsersRequestGetAllUserWithRolesOfMerchantModel model)
        {
            try
            {
                var entity = _context.YachtMerchantUsers.AsNoTracking().Where(x => x.MerchantFid == model.MerchantId && x.MerchantUserRoleFid== model.Role && x.Deleted == false).Select(s => _mapper.Map<YachtMerchantUsers, YachtMerchantUsersViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<YachtMerchantUsersViewModel>>.Success(entity.ToList());
                else
                    return BaseResponse<List<YachtMerchantUsersViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantUsersViewModel>>.InternalServerError(message: ex.Message);
            }
        }


        public async Task<BaseResponse<bool>> CreateYachtMerchantUser(YachtMerchantUsersCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = new YachtMerchantUsers();
                entity.InjectFrom(model);
                
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                await _context.YachtMerchantUsers.AddAsync(entity);
                await _context.SaveChangesAsync();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }


        public async Task<BaseResponse<bool>> UpdateYachtMerchantUsers(YachtMerchantUsersUpdateModel model)
        {
            try
            {
                var entity = _context.YachtMerchantUsers.Find(model.Id);
                if (entity != null)
                {
                    entity.InjectFrom(model);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtMerchantUsers.Update(entity);
                    await _context.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                else
                    return BaseResponse<bool>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }



        public async Task<BaseResponse<bool>> DeleteYachtMerchantUsers(int id)
        {
            try
            {
                var entity = _context.YachtMerchantUsers.Find(id);
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtMerchantUsers.Update(entity);
                    await _context.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                else
                    return BaseResponse<bool>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }

        
    }
}
