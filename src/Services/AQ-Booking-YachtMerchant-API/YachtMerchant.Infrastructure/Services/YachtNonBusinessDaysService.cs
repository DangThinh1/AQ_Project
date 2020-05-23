using APIHelpers.Response;
using AQBooking.Core.Models;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtNonBusinessDay;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtNonBusinessDaysService : ServiceBase, IYachtNonBusinessDaysService
    {
        public YachtNonBusinessDaysService(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
        }

        private bool CheckExists(DateTime startDate, DateTime endDate, DateTime dateFrom, DateTime dateTo)
        {
            if ((startDate.Date >= dateFrom.Date && startDate.Date <= dateTo.Date) || (endDate.Date >= dateFrom.Date && endDate.Date <= dateTo.Date))
                return false;
            if ((startDate.Date <= dateFrom.Date && endDate.Date >= dateFrom.Date) || (dateTo.Date <= endDate.Date && dateTo.Date >= startDate.Date))
                return false;
            return true;
        }
        public async Task<BaseResponse<bool>> CreateAsync(YachtNonBusinessDayCreateModel model)
        {
            try
            {
                var result = _context.YachtNonOperationDays.Where(x => x.YachtFid == model.YachtFid && !x.Deleted && x.Recurring == false)
                    .OrderByDescending(x => x.EndDate.Date)
                    .FirstOrDefault(x=> x.StartDate.Date <= model.EndDate.Date);
           
                if(result != null){
                    bool check = CheckExists(model.StartDate.Date, model.EndDate.Date, result.StartDate.Date, result.EndDate.Date);
                    if (check == false)
                        return BaseResponse<bool>.NotFound(false);
                }
            
                var entity = new YachtNonOperationDays();
                entity.InjectFrom(model);
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                await _context.YachtNonOperationDays.AddAsync(entity);
                await _context.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> CreateRangeAsync(YachtNonBusinessDayCreateRangeModel createModel)
        {
            try
            {
                var listNonBusinessDay = createModel.NonBusinessDay.Split(",");
                var listCreate = new List<YachtNonOperationDays>();
                foreach (var nonBusinessDay in listNonBusinessDay)
                {
                    var date = nonBusinessDay.Replace(".", "-");
                    var dateTime = (date + "-" + DateTime.Now.Year).ToNullDateTime();
                    if (dateTime != null)
                    {
                        var model = new YachtNonOperationDays()
                        {
                            YachtFid = createModel.YachtFid,
                            Recurring = createModel.Recurring,
                            Remark = createModel.Remark,
                            StartDate = dateTime.Value,
                            EndDate = dateTime.Value,
                            CreatedBy = GetUserGuidId(),
                            CreatedDate = DateTime.Now,
                            Deleted = false,
                            LastModifiedBy = GetUserGuidId(),
                            LastModifiedDate = DateTime.Now
                        };
                        listCreate.Add(model);
                    }
                }
                await _context.YachtNonOperationDays.AddRangeAsync(listCreate);
                await _context.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> DeleteNonBusinessDayByIdAsync(int id)
        {
            try
            {
                var item = _context.YachtNonOperationDays.FirstOrDefault(e => e.Deleted == false && e.Id == id);
                if (item == null)
                    return BaseResponse<bool>.NotFound(false);
                var userId = GetUserGuidId();
                item.Deleted = true;
                item.LastModifiedBy = userId;
                item.LastModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtNonBusinessDayViewModel>> GetNonBusinessDaysByYachtID(int yachtId)
        {
            var result = _context.YachtNonOperationDays.AsNoTracking()
                .Where(p => p.YachtFid == yachtId && p.Deleted == false)
                .Select(k => new YachtNonBusinessDayViewModel()
                {
                    Id = k.Id,
                    EndDate = k.EndDate,
                    Recurring = k.Recurring,
                    StartDate = k.StartDate,
                    Remark = k.Remark,
                    LastModifiedBy = k.LastModifiedBy.ToString(),
                    LastModifiedDate = k.LastModifiedDate
                });

            if (result.Count() > 0)
                return BaseResponse<List<YachtNonBusinessDayViewModel>>.Success(result.ToList());
            return BaseResponse<List<YachtNonBusinessDayViewModel>>.Success( new List<YachtNonBusinessDayViewModel>());
        }

        public BaseResponse<YachtNonBusinessDayViewModel> GetYactNonBusinessDayById(int id)
        {
            try
            {
                var result = _context.YachtNonOperationDays.AsNoTracking().FirstOrDefault(p => p.Id == id && p.Deleted == false);
                if (result != null)
                {
                    var model = new YachtNonBusinessDayViewModel() {
                        Id = result.Id,
                        YachtFid = result.YachtFid,
                        EndDate = result.EndDate.Date,
                        Recurring = result.Recurring,
                        StartDate = result.StartDate.Date,
                        Remark = result.Remark,
                    };
                    return BaseResponse<YachtNonBusinessDayViewModel>.Success(model);
                }
                else
                {
                    return BaseResponse<YachtNonBusinessDayViewModel>.BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtNonBusinessDayViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(YachtNonBusinessDayUpdateModel model)
        {
            try
            {
                var entity = _context.YachtNonOperationDays.FirstOrDefault(x => x.Id == model.Id);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest(false);
                entity.InjectFrom(model);
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                _context.YachtNonOperationDays.Update(entity);
                _context.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}