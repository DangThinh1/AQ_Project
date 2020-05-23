using APIHelpers.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtTourCharterProcessingFees;
using YachtMerchant.Core.Models.YachtTourCharterSchedules;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourCharterSchedulesService:ServiceBase, IYachtTourCharterSchedulesService
    {
        private readonly IMapper _mapper;
        public YachtTourCharterSchedulesService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
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

        public BaseResponse<YachtTourCharterSchedulesViewModel> GetYachtTourCharterSchedulesById(long id)
        {
            try
            {
                var entity = _context.YachtTourCharterSchedules.Find(id);
                if (entity == null)
                    return BaseResponse<YachtTourCharterSchedulesViewModel>.NotFound();
                else
                    return BaseResponse<YachtTourCharterSchedulesViewModel>.Success(_mapper.Map<YachtTourCharterSchedules, YachtTourCharterSchedulesViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourCharterSchedulesViewModel>.InternalServerError(message: ex.Message);
            }
        }

        public BaseResponse<bool> CheckExistUserSetInSchedules(CheckDuplicateUserSchedulesModel model)
        {
            try
            {
                var entity = _context.YachtTourCharterSchedules.Where(x => x.TourCharterFid == model.TourCharterFid && x.UserFid == model.UserId && x.YachtFid == model.YachtId && x.Deleted == false).AsNoTracking().ToList();
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
                var entity = _context.YachtTourCharterSchedules.Where(x => x.TourCharterFid == model.TourCharterFid && x.RoleFid == model.RoleId && x.YachtFid == model.YachtId && x.Deleted == false).AsNoTracking().ToList();
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
                var entity = _context.YachtTourCharterSchedules.Where(x => x.TourCharterFid == model.TourCharterFid && x.UserFid == model.UserId && x.RoleFid == model.RoleId && x.YachtFid == model.YachtId && x.Deleted == false).AsNoTracking().ToList();
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


        public BaseResponse<List<YachtTourCharterSchedulesViewModel>> GetAllScheduleSetOnCharterSchedulesByCharterId(long id)
        {
            try
            {
                var query = _context.YachtTourCharterSchedules.Where(x => x.TourCharterFid == id && x.Deleted == false).AsNoTracking().Select(s => _mapper.Map<YachtTourCharterSchedules, YachtTourCharterSchedulesViewModel>(s));

                if (query.Count() > 0)
                    return BaseResponse<List<YachtTourCharterSchedulesViewModel>>.Success(query.ToList());
                else
                    return BaseResponse<List<YachtTourCharterSchedulesViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourCharterSchedulesViewModel>>.InternalServerError(message: ex.Message);
            }
        }

        public BaseResponse<List<YachtTourCharterSchedulesViewModel>> GetYachtTourCharterSchedulesByCharterId(long id)
        {
            try
            {
                var query = (from ys in _context.YachtTourCharterSchedules.Where(x => x.TourCharterFid == id && x.Deleted == false).AsNoTracking()
                             join y in _context.Yachts.Where(c => c.Deleted == false).AsNoTracking()
                              on ys.YachtFid equals y.Id
                             join mu in _context.YachtMerchantUsers.Where(u => u.Deleted == false).AsNoTracking()
                             on ys.UserFid equals mu.UserFid
                             select new YachtTourCharterSchedulesViewModel()
                             {
                                 Id = ys.Id,
                                 UserFid = ys.UserFid,
                                 TourCharterFid = ys.TourCharterFid,
                                 YachtFid = ys.YachtFid,
                                 RoleFid = ys.RoleFid,
                                 UserName = mu.UserName,
                                 YachtName = y.Name,
                                 EffectiveStartDate = ys.EffectiveStartDate,
                                 EffectiveEndDate = ys.EffectiveEndDate.GetValueOrDefault(),
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
                    return BaseResponse<List<YachtTourCharterSchedulesViewModel>>.Success(query.ToList());
                else
                    return BaseResponse<List<YachtTourCharterSchedulesViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourCharterSchedulesViewModel>>.InternalServerError(message: ex.Message);
            }
        }

        public async Task<BaseResponse<bool>> CreateYachtTourCharterSchedules(YachtTourCharterSchedulesCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var check = CheckDate(model.EffectiveStartDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();
                var entity = new YachtTourCharterSchedules();
                entity.InjectFrom(model);
                entity.AssignedBy = GetUserGuidId();
                entity.AssignedDate = DateTime.Now;
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
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

                await _context.YachtTourCharterSchedules.AddAsync(entity);
                await _context.SaveChangesAsync();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }


        public async Task<BaseResponse<bool>> UpdateYachtTourCharterSchedules(YachtTourCharterSchedulesUpdateModel model)
        {
            try
            {
                var check = CheckDate(model.EffectiveStartDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();
                var entity = _context.YachtTourCharterSchedules.Find(model.Id);
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

                    _context.YachtTourCharterSchedules.Update(entity);
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

        public async Task<BaseResponse<bool>> DeleteYachtTourCharterSchedules(long id)
        {
            try
            {
                var entity = _context.YachtTourCharterSchedules.Find(id);
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtTourCharterSchedules.Update(entity);
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
