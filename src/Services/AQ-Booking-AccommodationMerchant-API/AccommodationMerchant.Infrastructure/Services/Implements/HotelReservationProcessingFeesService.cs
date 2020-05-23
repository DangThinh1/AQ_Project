using AccommodationMerchant.Core.Enum;
using AccommodationMerchant.Core.Models.HotelReservationProcessingFees;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelReservationProcessingFeesService:ServiceBase, IHotelReservationProcessingFeesService
    {
        public HotelReservationProcessingFeesService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<BaseResponse<bool>> CreateReservationProcessingFees(HotelReservationProcessingFeeCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var checkExistEntity = _db.HotelReservationProcessingFees.Find(model.ReservationsFid);
                if (checkExistEntity == null)
                {
                    var entity = new HotelReservationProcessingFees();
                    entity.InjectFrom(model);
                    entity.GeneratedDate = DateTime.Now;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    await _db.HotelReservationProcessingFees.AddAsync(entity);
                    await _db.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.NotFound(message: "Duplicated");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }

        }


        public async Task<BaseResponse<bool>> CreateReservationProcessingFeesAndChangeStatusReservationTransaction(HotelReservationProcessingFeeWithChangeStatusReservationCreateModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest();
                    var userId = GetUserGuidId();

                    var entityReservation = _db.HotelReservations.Find(Convert.ToInt64(model.Id ?? "0"));

                    var entityPaymentLog = _db.HotelReservationPaymentLogs.Where(p => p.ReservationsFid == Convert.ToInt64(model.Id ?? "0")).AsNoTracking()?.FirstOrDefault();

                    var checkExistEntityReservationProcessingFees = _db.HotelReservationProcessingFees.Find(model.ReservationsFid);

                    if (entityReservation != null && entityPaymentLog !=null && checkExistEntityReservationProcessingFees == null)
                    {
                        // Update entity HotelReservations 
                        entityReservation.Processed = true;
                        entityReservation.ProcessedRemark = model.ProcessRemark;
                        entityReservation.CancelReason = model.CancelRemark;
                        entityReservation.ProcessedBy = userId;
                        entityReservation.StatusFid = model?.Status.ToInt32() ?? entityReservation.StatusFid;
                        entityReservation.ProcessedDate = DateTime.Now;
                        // Update resource key status base on Status reservation
                        if (model.Status == ((int)ReservationStatusEnum.WaitingPayment).ToString())
                            entityReservation.StatusResKey = "WAITINGPAYMENT";
                        else if (model.Status == ((int)ReservationStatusEnum.Paid).ToString())
                            entityReservation.StatusResKey = "PAID";
                        else if (model.Status == ((int)ReservationStatusEnum.Pending).ToString())
                            entityReservation.StatusResKey = "PENDING";
                        else if (model.Status == ((int)ReservationStatusEnum.Accepted).ToString())
                            entityReservation.StatusResKey = "ACCEPTED";
                        else if (model.Status == ((int)ReservationStatusEnum.Rejected).ToString())
                            entityReservation.StatusResKey = "REJECTED";
                        else if (model.Status == ((int)ReservationStatusEnum.Cancelled).ToString())
                            entityReservation.StatusResKey = "CANCELLED";
                        else if (model.Status == ((int)ReservationStatusEnum.Completed).ToString())
                            entityReservation.StatusResKey = "COMPLETED";
                        else
                            entityReservation.StatusResKey = "WAITINGPAYMENT";

                        _db.HotelReservations.Update(entityReservation);
                        await _db.SaveChangesAsync();

                        // Insert entity HotelReservationProcessingFees
                        var entityReservationProcessingFee = new HotelReservationProcessingFees();
                        entityReservationProcessingFee.InjectFrom(model);
                        entityReservationProcessingFee.GeneratedDate = DateTime.Now;
                        entityReservationProcessingFee.LastModifiedBy = userId;
                        entityReservationProcessingFee.LastModifiedDate = DateTime.Now;

                        await _db.HotelReservationProcessingFees.AddAsync(entityReservationProcessingFee);
                        await _db.SaveChangesAsync();

                        // Update status reservastion entity HotelReservationPaymentLogs
                        entityPaymentLog.StatusFid = model?.Status.ToInt32() ??  entityPaymentLog.StatusFid;
                        _db.HotelReservationPaymentLogs.Update(entityPaymentLog);
                        await _db.SaveChangesAsync();

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

        public async Task<BaseResponse<bool>> UpdateReservationProcessingFees(HotelReservationProcessingFeeUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _db.HotelReservationProcessingFees.Find(model.ReservationsFid);
                if (entity != null)
                {
                    entity.InjectFrom(model);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _db.HotelReservationProcessingFees.Update(entity);
                    await _db.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }

        public BaseResponse<HotelReservationProcessingFeeViewModel> GetReservationProcessingFeesByReservationId(long id)
        {
            try
            {
                var entity = _db.HotelReservationProcessingFees.Find(id);
                if (entity == null)
                    return BaseResponse<HotelReservationProcessingFeeViewModel>.NotFound(new HotelReservationProcessingFeeViewModel());
                return BaseResponse<HotelReservationProcessingFeeViewModel>.Success(_mapper.Map<HotelReservationProcessingFees, HotelReservationProcessingFeeViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<HotelReservationProcessingFeeViewModel>.InternalServerError(message: ex.Message);
            }
        }
    }
}
