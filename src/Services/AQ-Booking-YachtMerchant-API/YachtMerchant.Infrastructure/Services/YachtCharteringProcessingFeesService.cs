using System;
using System.Linq;
using System.Threading.Tasks;
using APIHelpers.Response;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtCharteringProcessingFees;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtCharteringProcessingFeesService : ServiceBase, IYachtCharteringProcessingFeesService
    {
        private readonly IMapper _mapper;
        public YachtCharteringProcessingFeesService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> CreateYachtCharteringProcessingFees(YachtCharteringProcessingFeesCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var checkExistEntity = _context.YachtCharteringProcessingFees.Find(model.CharteringId);
                if (checkExistEntity == null)
                {
                    var entity = new YachtCharteringProcessingFees();
                    entity.InjectFrom(model);
                    entity.GeneratedDate = DateTime.Now;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    await _context.YachtCharteringProcessingFees.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                else
                    return BaseResponse<bool>.NotFound(message:"Duplicated");
            }
            catch (Exception ex)
            {
               return BaseResponse<bool>.InternalServerError(message:ex.Message);
            }

        }


        public async Task<BaseResponse<bool>> CreateYachtCharteringProcessingFeesAndChangeStatusReservationTransaction(YachtCharteringProcessingFeeWithChangeStatusReservationCreateModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest();
                    var userId = GetUserGuidId();
                   
                    var entityYachtChartring = _context.YachtCharterings.Find(Convert.ToInt64(model.Id ?? "0"));

                    var entityPaymentLog = _context.YachtCharteringPaymentLogs.Where(p => p.CharteringFid == Convert.ToInt64(model.Id ?? "0")).AsNoTracking()?.FirstOrDefault();

                    var checkExistEntityYachtCharteringProcessingFees = _context.YachtCharteringProcessingFees.Find(model.CharteringId);

                    if(checkExistEntityYachtCharteringProcessingFees != null)
                    {
                        //Transaction commit all
                        transaction.Commit();
                        return BaseResponse<bool>.NotFound(message: "EXIST");
                    }
                    if (entityYachtChartring != null && entityPaymentLog != null && checkExistEntityYachtCharteringProcessingFees == null)
                    {
                        // Update entity YachtChartering 
                        entityYachtChartring.Processed = true;
                        entityYachtChartring.ProcessedRemark = model.ProcessRemark;
                        entityYachtChartring.CancelReason = model.CancelRemark;
                        entityYachtChartring.ProcessedBy = userId;
                        entityYachtChartring.StatusFid = model?.Status.ToInt32()??entityYachtChartring.StatusFid;
                        entityYachtChartring.ProcessedDate = DateTime.Now;
                        // Update resource key status base on Status reservation
                        if (model.Status == ((int)ReservationStatusEnum.WaitingPayment).ToString())
                            entityYachtChartring.StatusResKey = "WAITINGPAYMENT";
                        else if (model.Status == ((int)ReservationStatusEnum.Paid).ToString())
                            entityYachtChartring.StatusResKey = "PAID";
                        else if (model.Status == ((int)ReservationStatusEnum.Pending).ToString())
                            entityYachtChartring.StatusResKey = "PENDING";
                        else if (model.Status == ((int)ReservationStatusEnum.Accepted).ToString())
                            entityYachtChartring.StatusResKey = "ACCEPTED";
                        else if (model.Status == ((int)ReservationStatusEnum.Rejected).ToString())
                            entityYachtChartring.StatusResKey = "REJECTED";
                        else if (model.Status == ((int)ReservationStatusEnum.Cancelled).ToString())
                            entityYachtChartring.StatusResKey = "CANCELLED";
                        else if (model.Status == ((int)ReservationStatusEnum.Completed).ToString())
                            entityYachtChartring.StatusResKey = "COMPLETED";
                        else
                            entityYachtChartring.StatusResKey = "WAITINGPAYMENT";

                        _context.YachtCharterings.Update(entityYachtChartring);
                        await _context.SaveChangesAsync();

                        // Insert entity YachtCharteringProcessingFees
                        var entityYachtCharteringProcessingFee = new YachtCharteringProcessingFees();
                        entityYachtCharteringProcessingFee.InjectFrom(model);
                        entityYachtCharteringProcessingFee.GeneratedDate = DateTime.Now;
                        entityYachtCharteringProcessingFee.LastModifiedBy = userId;
                        entityYachtCharteringProcessingFee.LastModifiedDate = DateTime.Now;

                        await _context.YachtCharteringProcessingFees.AddAsync(entityYachtCharteringProcessingFee);
                        await _context.SaveChangesAsync();

                        // Update status reservastion entity PaymentLogs
                        entityPaymentLog.StatusFid = model?.Status.ToInt32() ?? entityPaymentLog.StatusFid;
                        _context.YachtCharteringPaymentLogs.Update(entityPaymentLog);
                        await _context.SaveChangesAsync();

                    }

                    //Transaction commit all
                    transaction.Commit();
                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BaseResponse<bool>.InternalServerError(message: ex.Message);
                }
            }
        }

        public async Task<BaseResponse<bool>> UpdateYachtCharteringProcessingFees(YachtCharteringProcessingFeesUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _context.YachtCharteringProcessingFees.Find(model.CharteringId);
                if (entity != null)
                {
                    entity.InjectFrom(model);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtCharteringProcessingFees.Update(entity);
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

        public BaseResponse<YachtCharteringProcessingFeesViewModel> GetYachtCharteringProcessingFeesByCharteringId(long id)
        {
            try
            {
                var entity = _context.YachtCharteringProcessingFees.Find(id);
                if (entity == null)
                    return BaseResponse<YachtCharteringProcessingFeesViewModel>.NotFound();
                else
                    return BaseResponse<YachtCharteringProcessingFeesViewModel>.Success(_mapper.Map<YachtCharteringProcessingFees, YachtCharteringProcessingFeesViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringProcessingFeesViewModel>.InternalServerError(message: ex.Message);
            }
        }

        
    }
}
