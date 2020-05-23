using APIHelpers.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtCharteringSchedules;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtCharteringSchedulesService:ServiceBase, IYachtCharteringSchedulesService
    {
        private readonly IMapper _mapper;
        public YachtCharteringSchedulesService(IMapper mapper,YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }

        private bool CheckDate(DateTime startDate, DateTime? endDate)
        {

            if (endDate.HasValue)
            {
                if (endDate.Value.Date < startDate.Date)
                    return true;
            }
            return false;
        }

        public BaseResponse<YachtCharteringSchedulesViewModel> GetYachtCharteringSchedulesById(long id)
        {
            try
            {
                var entity = _context.YachtCharteringSchedules.Find(id);
                if (entity == null)
                    return BaseResponse<YachtCharteringSchedulesViewModel>.NotFound();
                else
                    return BaseResponse<YachtCharteringSchedulesViewModel>.Success(_mapper.Map<YachtCharteringSchedules, YachtCharteringSchedulesViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringSchedulesViewModel>.InternalServerError(message: ex.Message);
            }
        }
        public BaseResponse<bool> CheckExistUserSetInSchedules(CheckDuplicateUserSchedulesModel model)
        {
            try
            {
                var entity = _context.YachtCharteringSchedules.Where(x => x.CharteringFid == model.CharteringId && x.UserFid == model.UserId && x.YachtFid == model.YachtId && x.Deleted == false).AsNoTracking().ToList();
                if (entity.Count() > 0)
                    return BaseResponse<bool>.Success(true);
                else
                    return BaseResponse<bool>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }
       

        public BaseResponse<bool> CheckExistRoleSetInSchedules(CheckDuplicateRoleSchedulesModel model)
        {
            try
            {
                var entity = _context.YachtCharteringSchedules.Where(x => x.CharteringFid == model.CharteringId && x.RoleFid == model.RoleId && x.YachtFid == model.YachtId && x.Deleted == false).AsNoTracking().ToList();
                if (entity.Count() > 0)
                    return BaseResponse<bool>.Success(true);
                else
                    return BaseResponse<bool>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }

        public BaseResponse<bool> CheckExistUserRoleInSchedules(CheckDuplicateSchedulesModel model)
        {
            try
            {
                var entity = _context.YachtCharteringSchedules.Where(x => x.CharteringFid == model.CharteringId && x.UserFid== model.UserId && x.RoleFid == model.RoleId && x.YachtFid == model.YachtId && x.Deleted == false).AsNoTracking().ToList();
                if (entity.Count() > 0)
                    return BaseResponse<bool>.Success(true);
                else
                    return BaseResponse<bool>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }


        public BaseResponse<List<YachtCharteringSchedulesViewModel>> GetAllScheduleSetOnCharteringSchedulesByCharteringId(long id)
        {
            try
            {
                var query = _context.YachtCharteringSchedules.Where(x => x.CharteringFid == id && x.Deleted == false).AsNoTracking().Select(s => _mapper.Map<YachtCharteringSchedules, YachtCharteringSchedulesViewModel>(s));

                if (query.Count() > 0)
                    return BaseResponse<List<YachtCharteringSchedulesViewModel>>.Success(query.ToList());
                else
                    return BaseResponse<List<YachtCharteringSchedulesViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtCharteringSchedulesViewModel>>.InternalServerError(message: ex.Message);
            }
        }

        public BaseResponse<List<YachtCharteringSchedulesViewModel>> GetYachtCharteringSchedulesByCharteringId(long id)
        {
            try
            {
                var query = (from ys in _context.YachtCharteringSchedules.Where(x => x.CharteringFid == id && x.Deleted == false).AsNoTracking()
                             join y in _context.Yachts.Where(c => c.Deleted == false).AsNoTracking()
                              on ys.YachtFid equals y.Id
                              join mu in _context.YachtMerchantUsers.Where(u => u.Deleted == false).AsNoTracking()
                              on ys.UserFid equals mu.UserFid
                              select new YachtCharteringSchedulesViewModel()
                              {
                                  Id = ys.Id,
                                  UserFid = ys.UserFid,
                                  CharteringFid = ys.CharteringFid,
                                  YachtFid = ys.YachtFid,
                                  RoleFid = ys.RoleFid,
                                  UserName = mu.UserName,
                                  YachtName = y.Name,
                                  EffectiveStartDate = ys.EffectiveStartDate,
                                  EffectiveEndDate = ys.EffectiveEndDate,
                                  Remark = ys.Remark,
                                  RoleResKey = ys.RoleResKey,
                                  AssignedBy = ys.AssignedBy,
                                  AssignedDate = ys.AssignedDate,
                                  Deleted = ys.Deleted,
                                  CreatedBy = ys.CreatedBy,
                                  CreatedDate = ys.CreatedDate,
                                  LastModifiedBy = ys.LastModifiedBy,
                                  LastModifiedDate = ys.LastModifiedDate
                              });
                             
                if (query.Count() > 0)
                    return BaseResponse<List<YachtCharteringSchedulesViewModel>>.Success(query.ToList());
                else
                    return BaseResponse<List<YachtCharteringSchedulesViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtCharteringSchedulesViewModel>>.InternalServerError(message: ex.Message);
            }
        }

        public async Task<BaseResponse<bool>> CreateYachtCharteringSchedules(YachtCharteringSchedulesCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var check = CheckDate(model.EffectiveStartDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();
                var entity = new YachtCharteringSchedules();
                entity.InjectFrom(model);
                entity.AssignedBy = GetUserGuidId();
                entity.AssignedDate = DateTime.Now;
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy= GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;

                // Set Role resourse
                if (model.RoleFid == (int)MerchantUserRoleEnum.MasterAccount)
                    entity.RoleResKey = "ROLEMASTER";
                else if (model.RoleFid == (int)MerchantUserRoleEnum.Manager)
                    entity.RoleResKey = "ROLEMANAGER";
                else if (model.RoleFid == (int)MerchantUserRoleEnum.Executive)
                    entity.RoleResKey = "ROLEEXECUTIVE";
                else if (model.RoleFid == (int)MerchantUserRoleEnum.ServiceExecutive)
                    entity.RoleResKey = "ROLESERVICEEXECUTIVE";
                else if (model.RoleFid == (int)MerchantUserRoleEnum.Captain)
                    entity.RoleResKey = "ROLECAPTAIN";
                else if (model.RoleFid == (int)MerchantUserRoleEnum.Chef)
                    entity.RoleResKey = "ROLECHEF";
                else if (model.RoleFid == (int)MerchantUserRoleEnum.CrewMember)
                    entity.RoleResKey = "ROLEMEMBER";
                else
                    entity.RoleResKey = "UNKNOWN";

                await _context.YachtCharteringSchedules.AddAsync(entity);
                await _context.SaveChangesAsync();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }


        public async Task<BaseResponse<bool>> UpdateYachtCharteringSchedules(YachtCharteringSchedulesUpdateModel model)
        {
            try
            {
                var check = CheckDate(model.EffectiveStartDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();
                var entity = _context.YachtCharteringSchedules.Find(model.Id);
                if (entity != null)
                {
                    entity.InjectFrom(model);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    // Set Role resourse
                    if (model.RoleFid == (int)MerchantUserRoleEnum.MasterAccount)
                        entity.RoleResKey = "ROLEMASTER";
                    else if (model.RoleFid == (int)MerchantUserRoleEnum.Manager)
                        entity.RoleResKey = "ROLEMANAGER";
                    else if (model.RoleFid == (int)MerchantUserRoleEnum.Executive)
                        entity.RoleResKey = "ROLEEXECUTIVE";
                    else if (model.RoleFid == (int)MerchantUserRoleEnum.ServiceExecutive)
                        entity.RoleResKey = "ROLESERVICEEXECUTIVE";
                    else if (model.RoleFid == (int)MerchantUserRoleEnum.Captain)
                        entity.RoleResKey = "ROLECAPTAIN";
                    else if (model.RoleFid == (int)MerchantUserRoleEnum.Chef)
                        entity.RoleResKey = "ROLECHEF";
                    else if (model.RoleFid == (int)MerchantUserRoleEnum.CrewMember)
                        entity.RoleResKey = "ROLEMEMBER";
                    else
                        entity.RoleResKey = "UNKNOWN";

                    _context.YachtCharteringSchedules.Update(entity);
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

        public async Task<BaseResponse<bool>> DeleteYachtCharteringSchedules(long id)
        {
            try
            {
                var entity = _context.YachtCharteringSchedules.Find(id);
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtCharteringSchedules.Update(entity);
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
