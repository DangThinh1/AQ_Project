using APIHelpers.Response;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtTourCharterProcessingFees;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourCharterProcessingFeesService:ServiceBase, IYachtTourCharterProcessingFeesService
    {
        private readonly IMapper _mapper;
        public YachtTourCharterProcessingFeesService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }


        public async Task<BaseResponse<bool>> CreateYachtTourCharterProcessingFees(YachtTourCharterProcessingFeesCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var checkExistEntity = _context.YachtTourCharterProcessingFees.Find(model.TourCharterFid);
                if (checkExistEntity == null)
                {
                    var entity = new YachtTourCharterProcessingFees();
                    entity.InjectFrom(model);
                    entity.GeneratedDate = DateTime.Now;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    await _context.YachtTourCharterProcessingFees.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                else
                    return BaseResponse<bool>.NotFound(message: "Duplicated");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }

        }


        public async Task<BaseResponse<bool>> CreateYachtTourCharterProcessingFeesAndChangeStatusReservationTransaction(YachtTourCharterProcessingFeeWithChangeStatusReservationCreateModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest();
                    var userId = GetUserGuidId();

                    var entityYachtTourCharter = _context.YachtTourCharters.Find(Convert.ToInt64(model.Id ?? "0"));

                    var entityPaymentLog = _context.YachtTourCharterPaymentLogs.Where(p => p.TourCharterFid == Convert.ToInt64(model.Id ?? "0")).AsNoTracking()?.FirstOrDefault();

                    var checkExistEntityYachtTourCharterProcessingFees = _context.YachtTourCharterProcessingFees.Find(model.TourCharterFid);

                    if (entityYachtTourCharter != null && entityPaymentLog != null && checkExistEntityYachtTourCharterProcessingFees == null)
                    {
                        // Update entity YachtTourCharters 
                        entityYachtTourCharter.Processed = true;
                        entityYachtTourCharter.ProcessedRemark = model.ProcessRemark;
                        entityYachtTourCharter.CancelReason = model.CancelRemark;
                        entityYachtTourCharter.ProcessedBy = userId;
                        entityYachtTourCharter.StatusFid = model?.Status.ToInt32() ?? entityYachtTourCharter.StatusFid;
                        entityYachtTourCharter.ProcessedDate = DateTime.Now;
                        // Update resource key status base on Status reservation
                        if (model.Status == ((int)ReservationStatusEnum.WaitingPayment).ToString())
                            entityYachtTourCharter.StatusResKey = "WAITINGPAYMENT";
                        else if (model.Status == ((int)ReservationStatusEnum.Paid).ToString())
                            entityYachtTourCharter.StatusResKey = "PAID";
                        else if (model.Status == ((int)ReservationStatusEnum.Pending).ToString())
                            entityYachtTourCharter.StatusResKey = "PENDING";
                        else if (model.Status == ((int)ReservationStatusEnum.Accepted).ToString())
                            entityYachtTourCharter.StatusResKey = "ACCEPTED";
                        else if (model.Status == ((int)ReservationStatusEnum.Rejected).ToString())
                            entityYachtTourCharter.StatusResKey = "REJECTED";
                        else if (model.Status == ((int)ReservationStatusEnum.Cancelled).ToString())
                            entityYachtTourCharter.StatusResKey = "CANCELLED";
                        else if (model.Status == ((int)ReservationStatusEnum.Completed).ToString())
                            entityYachtTourCharter.StatusResKey = "COMPLETED";
                        else
                            entityYachtTourCharter.StatusResKey = "WAITINGPAYMENT";

                        _context.YachtTourCharters.Update(entityYachtTourCharter);
                        await _context.SaveChangesAsync();

                        // Insert entity YachtTourCharterProcessingFees
                        var entityYachtTourCharterProcessingFee = new YachtTourCharterProcessingFees();
                        entityYachtTourCharterProcessingFee.InjectFrom(model);
                        entityYachtTourCharterProcessingFee.GeneratedDate = DateTime.Now;
                        entityYachtTourCharterProcessingFee.LastModifiedBy = userId;
                        entityYachtTourCharterProcessingFee.LastModifiedDate = DateTime.Now;

                        await _context.YachtTourCharterProcessingFees.AddAsync(entityYachtTourCharterProcessingFee);
                        await _context.SaveChangesAsync();

                        // Update status reservastion entity PaymentLogs
                        entityPaymentLog.StatusFid = model?.Status.ToInt32() ?? entityPaymentLog.StatusFid;
                        _context.YachtTourCharterPaymentLogs.Update(entityPaymentLog);
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

        public async Task<BaseResponse<bool>> UpdateYachtTourCharterProcessingFees(YachtTourCharterProcessingFeesUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _context.YachtTourCharterProcessingFees.Find(model.TourCharterFid);
                if (entity != null)
                {
                    entity.InjectFrom(model);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtTourCharterProcessingFees.Update(entity);
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

        public BaseResponse<YachtTourCharterProcessingFeesViewModel> GetYachtTourCharterProcessingFeesByCharterId (long id)
        {
            try
            {
                var entity = _context.YachtTourCharterProcessingFees.Find(id);
                if (entity == null)
                    return BaseResponse<YachtTourCharterProcessingFeesViewModel>.NotFound();
                else
                    return BaseResponse<YachtTourCharterProcessingFeesViewModel>.Success(_mapper.Map<YachtTourCharterProcessingFees, YachtTourCharterProcessingFeesViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourCharterProcessingFeesViewModel>.InternalServerError(message: ex.Message);
            }
        }
    }
}
