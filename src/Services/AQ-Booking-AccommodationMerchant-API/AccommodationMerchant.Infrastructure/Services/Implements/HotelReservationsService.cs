using AccommodationMerchant.Core.Enum;
using AccommodationMerchant.Core.Models.HotelReservationDetails;
using AccommodationMerchant.Core.Models.HotelReservations;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelReservationsService : ServiceBase, IHotelReservationsService
    {
        public HotelReservationsService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }


        public BaseResponse<HotelReservationViewModel> GetInfomationReservationById(long id)
        {
            try
            {
                var entity = _db.HotelReservations.Find(id);
                if (entity == null)
                    return BaseResponse<HotelReservationViewModel>.NotFound(new HotelReservationViewModel());
                return BaseResponse<HotelReservationViewModel>.Success(_mapper.Map<HotelReservations, HotelReservationViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<HotelReservationViewModel>.InternalServerError(message: ex.Message);
            }
        }

        public async Task<BaseResponse<bool>> CreateReservationFromOriginSource(HotelReservationCreateModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest();
                    // Add Reservations
                    var userId = GetUserGuidId();
                    string uniqueId = UniqueIDHelper.GenarateRandomString(12);
                    var entityReservation = new HotelReservations();
                    entityReservation.InjectFrom(model);
                    entityReservation.HotelFid = model.HotelFid;
                    entityReservation.UniqueId = uniqueId;
                    entityReservation.ReservationDate = DateTime.UtcNow;
                    entityReservation.PaymentExchangeDate = DateTime.UtcNow;
                    entityReservation.StatusFid = model.StatusFid;
                    entityReservation.Processed = false; // default = false

                    await _db.HotelReservations.AddAsync(entityReservation);
                    await _db.SaveChangesAsync();

                    // Insert to Payment Log
                    var entityPaymentLog = new HotelReservationPaymentLogs();

                    entityPaymentLog.ReservationsFid = entityReservation.Id;
                    entityPaymentLog.PaymentDate = DateTime.Now;
                    entityPaymentLog.PaymentBy = model.CustomerName;
                    entityPaymentLog.PaymentAmount = model.PaymentValue;
                    entityPaymentLog.CurrencyCode = model.CurrencyCode;
                    entityPaymentLog.CultureCode = model.CultureCode;
                    entityPaymentLog.StatusFid = model.StatusFid;

                    await _db.HotelReservationPaymentLogs.AddAsync(entityPaymentLog);
                    await _db.SaveChangesAsync();

                    //Commit all
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


        public async Task<BaseResponse<bool>> CreateReservationFromOtherSource(HotelCreateReservationFromOtherSourceModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest();
                    // Add Reservations
                    var userId = GetUserGuidId();
                    string uniqueId = UniqueIDHelper.GenarateRandomString(12);
                    var entityReservation = new HotelReservations();
                    entityReservation.InjectFrom(model);
                    entityReservation.HotelFid = model.HotelFid;
                    entityReservation.UniqueId = uniqueId;
                    entityReservation.ReservationDate = DateTime.UtcNow;
                    entityReservation.PaymentExchangeDate = DateTime.UtcNow;
                    entityReservation.StatusFid = model.StatusFid;
                    entityReservation.Processed = false;// default = false

                    await _db.HotelReservations.AddAsync(entityReservation);
                    await _db.SaveChangesAsync();

                    // Insert to Payment Log
                    var entityPaymentLog = new HotelReservationPaymentLogs();

                    entityPaymentLog.ReservationsFid = entityReservation.Id;
                    entityPaymentLog.PaymentDate = DateTime.Now;
                    entityPaymentLog.PaymentBy = model.CustomerName;
                    entityPaymentLog.PaymentAmount = 0.0;
                    entityPaymentLog.StatusFid = (int)ReservationStatusEnum.Accepted;

                    await _db.HotelReservationPaymentLogs.AddAsync(entityPaymentLog);
                    await _db.SaveChangesAsync();

                    //Commit all
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

        public async Task<BaseResponse<bool>> UpdateStatusAsync(HotelReservationConfirmStatusModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!String.IsNullOrEmpty(model.Id))
                    {
                        //Find reservation
                        var entity = _db.HotelReservations.Find(Convert.ToInt64(model.Id));
                        // Get Paymentlogs
                        var entityPaymentLog = _db.HotelReservationPaymentLogs.Where(p => p.ReservationsFid == Convert.ToInt64(model.Id ?? "0")).AsNoTracking()?.FirstOrDefault();

                        if (entity != null && entityPaymentLog !=null)
                        {
                            entity.Processed = !String.IsNullOrEmpty(model.Stage) ? Convert.ToBoolean(model.Stage) : false;
                            entity.ProcessedRemark = model.ProcessRemark;
                            entity.CancelReason = model.CancelRemark;
                            entity.ProcessedBy = GetUserGuidId();
                            entity.StatusFid = Convert.ToInt32(model.Status);
                            // Update resource key status base on status
                            if (model.Status == ((int)ReservationStatusEnum.WaitingPayment).ToString())
                                entity.StatusResKey = "WAITINGPAYMENT";
                            else if (model.Status == ((int)ReservationStatusEnum.Paid).ToString())
                                entity.StatusResKey = "PAID";
                            else if (model.Status == ((int)ReservationStatusEnum.Pending).ToString())
                                entity.StatusResKey = "PENDING";
                            else if (model.Status == ((int)ReservationStatusEnum.Accepted).ToString())
                                entity.StatusResKey = "ACCEPTED";
                            else if (model.Status == ((int)ReservationStatusEnum.Rejected).ToString())
                                entity.StatusResKey = "REJECTED";
                            else if (model.Status == ((int)ReservationStatusEnum.Cancelled).ToString())
                                entity.StatusResKey = "CANCELLED";
                            else if (model.Status == ((int)ReservationStatusEnum.Completed).ToString())
                                entity.StatusResKey = "COMPLETED";
                            else
                                entity.StatusResKey = "WAITINGPAYMENT";

                            entity.ProcessedDate = DateTime.UtcNow;
                            _db.HotelReservations.Update(entity);
                            await _db.SaveChangesAsync();


                            // Update status reservastion entity ReservationPaymentLogs
                            entityPaymentLog.StatusFid = model?.Status.ToInt32() ?? entityPaymentLog.StatusFid;
                            _db.HotelReservationPaymentLogs.Update(entityPaymentLog);
                            await _db.SaveChangesAsync();

                            //Transaction commit all
                            transaction.Commit();
                            return BaseResponse<bool>.Success(true);

                        }
                        else
                            return BaseResponse<bool>.NotFound(false);
                    }
                    else
                        return BaseResponse<bool>.BadRequest(false);


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BaseResponse<bool>.InternalServerError(message: ex.Message);
                }
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(long id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var item = _db.HotelReservations.Find(id);
                    if (item != null)
                    {
                        item.StatusFid = (int)ReservationStatusEnum.Rejected;// Check Common Value Rule Status FID, 1: Delete; 2: Cancelled
                        item.ProcessedBy = GetUserGuidId();
                        item.ProcessedDate = DateTime.UtcNow;
                        _db.HotelReservations.Update(item);
                        await _db.SaveChangesAsync();

                        // Commit transaction
                        transaction.Commit();
                        return BaseResponse<bool>.Success(true);
                    }
                    return BaseResponse<bool>.NotFound(false);
                }
                catch (Exception ex)
                {
                    // Roll back
                    transaction.Rollback();
                    return BaseResponse<bool>.InternalServerError(message:ex.Message);
                }
            }
            
        }


        public BaseResponse<PagedList<HotelReservationViewModel>> SearchAllReservationPaging(ReservationSearchPagingModel searchModel)
        {
            if (searchModel == null)
                return BaseResponse<PagedList<HotelReservationViewModel>>.BadRequest();
            var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "ReservationDate DESC";
            int type = searchModel.Type; // Type: 0: All reservation; 1: Completed; 2: Cancelled
            DateTime? diningDate = null;
            if (searchModel.DiningDate.HaveValue())
                diningDate = searchModel.DiningDate.ToNullDateTime();
            //Case view all status reservation
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k =>( k.StatusFid !=0) &&
                                (diningDate == null ||  k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                    (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
            else
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted, Rejected,Canceled, Completed )
                bool checkProcessed = false;
                switch (type)
                {
                    case 1:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                    case 2:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Paid;
                        checkProcessed = false;
                        break;
                    case 3:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Pending;
                        checkProcessed = false;
                        break;
                    case 4:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Accepted;
                        checkProcessed = true;
                        break;
                    case 5:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Rejected;
                        checkProcessed = true;
                        break;
                    case 6:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Cancelled;
                        checkProcessed = true;
                        break;
                    case 7:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Completed;
                        checkProcessed = true;
                        break;
                    default:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                }
                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                               (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                (k.Processed == checkProcessed) &&
                                    (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
        }


        public BaseResponse<PagedList<HotelReservationViewModel>> SearchAllReservationOfMerchantPaging(ReservationForMerchantSearchPagingModel searchModel)
        {
            if (searchModel == null)
                return BaseResponse<PagedList<HotelReservationViewModel>>.BadRequest();
            var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "ReservationDate DESC";
            int type = searchModel.Type; // Type: 0: All reservation; 1: Completed; 2: Cancelled
            DateTime? diningDate = null;
            if (searchModel.DiningDate.HaveValue())
                diningDate = searchModel.DiningDate.ToNullDateTime();
            //Case view all status reservation
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from y in _db.Hotels.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantFid)
                             join t in _db.HotelReservations.AsNoTracking() on y.Id equals t.HotelFid into Temp
                             from c in Temp.Where(k => (k.StatusFid != 0) &&
                                (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                    (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate=c.PrepaidRate,
                                 PrepaidValue=c.PrepaidValue,
                                 StatusResKey=c.StatusResKey
                             }).OrderBy(sortString);

                if(query.Count()>0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
            else 
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted, Rejected,Canceled, Completed )
                bool checkProcessed = false;
                switch (type)
                {
                    case 1:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                    case 2:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Paid;
                        checkProcessed = false;
                        break;
                    case 3:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Pending;
                        checkProcessed = false;
                        break;
                    case 4:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Accepted;
                        checkProcessed = true;
                        break;
                    case 5:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Rejected;
                        checkProcessed = true;
                        break;
                    case 6:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Cancelled;
                        checkProcessed = true;
                        break;
                    case 7:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Completed;
                        checkProcessed = true;
                        break;
                    default:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                }
                var query = (from y in _db.Hotels.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantFid)
                             join t in _db.HotelReservations.AsNoTracking() on y.Id equals t.HotelFid into Temp
                             from c in Temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                 (k.Processed == checkProcessed) &&
                                    (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if(query.Count()>0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
        }

        public BaseResponse<PagedList<HotelReservationViewModel>> SearchAllReservationOfHotelPaging(ReservationOfHotelSearchPagingModel searchModel)
        {
            if (searchModel == null)
                return BaseResponse<PagedList<HotelReservationViewModel>>.BadRequest();
            var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "ReservationDate DESC";
            int type = searchModel.Type; // Type status need view
            DateTime? diningDate = null;
            if (searchModel.DiningDate.HaveValue())
                diningDate = searchModel.DiningDate.ToNullDateTime();

            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => k.StatusFid != 0 && k.HotelFid == searchModel.HotelFid &&
                               (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                    (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
            else
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted, Rejected,Canceled, Completed )
                bool checkProcessed = false;
                switch (type)
                {
                    case 1:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                    case 2:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Paid;
                        checkProcessed = false;
                        break;
                    case 3:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Pending;
                        checkProcessed = false;
                        break;
                    case 4:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Accepted;
                        checkProcessed = true;
                        break;
                    case 5:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Rejected;
                        checkProcessed = true;
                        break;
                    case 6:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Cancelled;
                        checkProcessed = true;
                        break;
                    case 7:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Completed;
                        checkProcessed = true;
                        break;
                    default:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                }

                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => k.HotelFid == searchModel.HotelFid && (k.StatusFid == ViewReservationStatusMode) &&
                               (k.Processed == checkProcessed) &&
                                    (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
            
        }

        public BaseResponse<PagedList<HotelReservationViewModel>> GetAllReservationByTypePaging(long reservationType = 1)
        {
            var sortString = "ReservationDate DESC";
            long type = reservationType; // Type status need view

            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => k.StatusFid != 0)

                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, 1, 15));
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
            else 
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted, Rejected,Canceled, Completed )
                bool checkProcessed = false;
                switch (type)
                {
                    case 1:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                    case 2:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Paid;
                        checkProcessed = false;
                        break;
                    case 3:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Pending;
                        checkProcessed = false;
                        break;
                    case 4:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Accepted;
                        checkProcessed = true;
                        break;
                    case 5:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Rejected;
                        checkProcessed = true;
                        break;
                    case 6:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Cancelled;
                        checkProcessed = true;
                        break;
                    case 7:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Completed;
                        checkProcessed = true;
                        break;
                    default:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                }

                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => (k.StatusFid == ViewReservationStatusMode &&
                               (k.Processed == checkProcessed)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<PagedList<HotelReservationViewModel>>.Success(new PagedList<HotelReservationViewModel>(query, 1, 15) );
                return BaseResponse<PagedList<HotelReservationViewModel>>.NotFound(new PagedList<HotelReservationViewModel>());
            }
            
        }


        public BaseResponse<List<HotelReservationViewModel>> GetAllReservationByTypeNoPaging(long reservationType = 1)
        {
            var sortString = "ReservationDate DESC";
            long type = reservationType; // Type status need view

            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => k.StatusFid != 0)

                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<List<HotelReservationViewModel>>.Success(query.ToList());
                return BaseResponse<List<HotelReservationViewModel>>.NotFound(new List<HotelReservationViewModel>());
            }
            else
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted, Rejected,Canceled, Completed )
                bool checkProcessed = false;
                switch (type)
                {
                    case 1:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                    case 2:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Paid;
                        checkProcessed = false;
                        break;
                    case 3:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Pending;
                        checkProcessed = false;
                        break;
                    case 4:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Accepted;
                        checkProcessed = true;
                        break;
                    case 5:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Rejected;
                        checkProcessed = true;
                        break;
                    case 6:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Cancelled;
                        checkProcessed = true;
                        break;
                    case 7:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Completed;
                        checkProcessed = true;
                        break;
                    default:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                }

                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => (k.StatusFid == ViewReservationStatusMode &&
                               (k.Processed == checkProcessed)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<List<HotelReservationViewModel>>.Success(query.ToList());
                return BaseResponse<List<HotelReservationViewModel>>.NotFound(new List<HotelReservationViewModel>());
            }

        }

        public BaseResponse<List<HotelReservationViewModel>> GetAllReservationOfMerchantByTypeNoPaging(ReservationOfMerchantSearchModel searchModel)
        {
            if (searchModel == null)
                return BaseResponse<List<HotelReservationViewModel>>.BadRequest();
            var sortString = "ReservationDate DESC";
            int type = searchModel.Type; // Type: status need view
            DateTime? diningDate = null;
            if (searchModel.DiningDate.HaveValue())
                diningDate = searchModel.DiningDate.ToNullDateTime();

            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from y in _db.Hotels.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantFid)
                             join t in _db.HotelReservations.AsNoTracking() on y.Id equals t.HotelFid into Temp
                             from c in Temp.Where(k => k.StatusFid != 0 &&
                                (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                    (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<HotelReservationViewModel>>.Success(query.ToList());
                return BaseResponse<List<HotelReservationViewModel>>.NotFound(new List<HotelReservationViewModel>());
            }
            else 
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted, Rejected,Canceled, Completed )
                bool checkProcessed = false;
                switch (type)
                {
                    case 1:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                    case 2:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Paid;
                        checkProcessed = false;
                        break;
                    case 3:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Pending;
                        checkProcessed = false;
                        break;
                    case 4:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Accepted;
                        checkProcessed = true;
                        break;
                    case 5:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Rejected;
                        checkProcessed = true;
                        break;
                    case 6:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Cancelled;
                        checkProcessed = true;
                        break;
                    case 7:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Completed;
                        break;
                    default:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                }

                var query = (from y in _db.Hotels.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantFid)
                             join t in _db.HotelReservations.AsNoTracking() on y.Id equals t.HotelFid into Temp
                             from c in Temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                             (k.Processed == checkProcessed) &&
                                (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                    (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                        select new HotelReservationViewModel()
                        {
                            Id = c.Id,
                            HotelFid = c.HotelFid,
                            UniqueId = c.UniqueId,
                            CustomerName = c.CustomerName,
                            ReservationEmail = c.ReservationEmail,
                            ContactNo = c.ContactNo,
                            CustomerFid = c.CustomerFid,
                            Adult = c.Adult,
                            Child = c.Child,
                            CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                            CustomerNote = c.CustomerNote,
                            DiningDate = c.DiningDate,
                            ReservationDate = c.ReservationDate,
                            CultureCode = c.CultureCode,
                            CurrencyCode = c.CurrencyCode,
                            OriginalValue = c.OriginalValue,
                            DiscountedValue = c.DiscountedValue,
                            GrandTotalValue = c.GrandTotalValue,
                            PaymentCurrency = c.PaymentCurrency,
                            PaymentExchangeRate = c.PaymentExchangeRate,
                            PaymentExchangeDate = c.PaymentExchangeDate,
                            PaymentValue = c.PaymentValue,
                            GotSpecialRequest = c.GotSpecialRequest,
                            SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                            CancelReason = c.CancelReason,
                            StatusFid = c.StatusFid,
                            Processed = c.Processed,
                            ProcessedBy = c.ProcessedBy,
                            ProcessedDate = c.ProcessedDate,
                            ProcessedRemark = c.ProcessedRemark,
                            PrepaidRate = c.PrepaidRate,
                            PrepaidValue = c.PrepaidValue,
                            StatusResKey = c.StatusResKey
                        }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<HotelReservationViewModel>>.Success(query.ToList());
                return BaseResponse<List<HotelReservationViewModel>>.NoContent(new List<HotelReservationViewModel>());
            }
        }

        public BaseResponse<List<HotelReservationViewModel>> GetAllReservationOfHotelByTypeNoPaging(ReservationOfHotelSearchModel searchModel)
        {
            if (searchModel == null)
                return BaseResponse<List<HotelReservationViewModel>>.BadRequest();
            var sortString = "ReservationDate DESC";
            int type = searchModel.Type; // Type status need view
            DateTime? diningDate = null;
            if (searchModel.DiningDate.HaveValue())
                diningDate = searchModel.DiningDate.ToNullDateTime();

            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => k.StatusFid != 0 && k.HotelFid == searchModel.HotelFid &&
                               (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                    (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<List<HotelReservationViewModel>>.Success(query.ToList());
                return BaseResponse<List<HotelReservationViewModel>>.NoContent();
            }
            else
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted, Rejected,Canceled, Completed )
                bool checkProcessed = false;
                switch (type)
                {
                    case 1:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                    case 2:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Paid;
                        checkProcessed = false;
                        break;
                    case 3:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Pending;
                        checkProcessed = false;
                        break;
                    case 4:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Accepted;
                        checkProcessed = true;
                        break;
                    case 5:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Rejected;
                        checkProcessed = true;
                        break;
                    case 6:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Cancelled;
                        checkProcessed = true;
                        break;
                    case 7:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.Completed;
                        break;
                    default:
                        ViewReservationStatusMode = (int)ReservationStatusEnum.WaitingPayment;
                        checkProcessed = false;
                        break;
                }

                var query = (from c in _db.HotelReservations.AsNoTracking().Where(k => k.HotelFid == searchModel.HotelFid && (k.StatusFid ==  ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (diningDate == null || k.DiningDate.Value.Date == diningDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationCode) || k.UniqueId.StartsWith(searchModel.ReservationCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)))
                             select new HotelReservationViewModel()
                             {
                                 Id = c.Id,
                                 HotelFid = c.HotelFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 Adult = c.Adult,
                                 Child = c.Child,
                                 CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                                 CustomerNote = c.CustomerNote,
                                 DiningDate = c.DiningDate,
                                 ReservationDate = c.ReservationDate,
                                 CultureCode = c.CultureCode,
                                 CurrencyCode = c.CurrencyCode,
                                 OriginalValue = c.OriginalValue,
                                 DiscountedValue = c.DiscountedValue,
                                 GrandTotalValue = c.GrandTotalValue,
                                 PaymentCurrency = c.PaymentCurrency,
                                 PaymentExchangeRate = c.PaymentExchangeRate,
                                 PaymentExchangeDate = c.PaymentExchangeDate,
                                 PaymentValue = c.PaymentValue,
                                 GotSpecialRequest = c.GotSpecialRequest,
                                 SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                                 CancelReason = c.CancelReason,
                                 StatusFid = c.StatusFid,
                                 Processed = c.Processed,
                                 ProcessedBy = c.ProcessedBy,
                                 ProcessedDate = c.ProcessedDate,
                                 ProcessedRemark = c.ProcessedRemark,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<HotelReservationViewModel>>.Success(query.ToList());
                return BaseResponse<List<HotelReservationViewModel>>.NoContent(new List<HotelReservationViewModel>());
            }
        }

        /// <summary>
        /// Get all detail of reservation with ReservationId input
        /// </summary>
        /// <param name="model">ReservationId</param>
        /// <returns>PagedList<ReservationDetailModel></returns>
        public BaseResponse<PagedList<HotelReservationDetailModel>> GetReservationDetail(ReservationDetailSearchPagingModel model)
        {
            if (model == null)
                return BaseResponse<PagedList<HotelReservationDetailModel>>.BadRequest();
            if (model.ReservationFid != 0)
            {
                var sortString = "ReservationsFid DESC";
                var query = (from d in _db.HotelReservationDetails.AsNoTracking().Where(r => r.ReservationsFid == model.ReservationFid)
                             join c in _db.Hotels.AsNoTracking().Where(r=>r.Deleted==false) on d.HotelFid equals c.Id
                             join i in _db.HotelInventories.Where( i=>i.Deleted==false).AsNoTracking() on d.InventoryFid  equals i.Id
                             join r in _db.HotelInventoryRates.Where( t=>t.Deleted==false).AsNoTracking() on d.InvetoryRateFid equals r.Id
                             select new HotelReservationDetailModel()
                             {
                                 Id = d.Id,
                                 HotelFid = d.HotelFid,
                                 HotelUniqueId = c.UniqueId,
                                 ReservationsFid = d.ReservationsFid,
                                 HotelName = c.HotelName,
                                 InventoryFid = d.InventoryFid,
                                 InvetoryRateFid=d.InvetoryRateFid,
                                 RoomName= i.RoomName,
                                 RoomTypeFid= i.RoomTypeFid,
                                 RoomTypeResKey=i.RoomTypeResKey,
                                 HotelRoomCode=i.HotelRoomCode,
                                 Quantity=i.Quantity,
                                 RoomSizeSqm= i.RoomSizeSqm,
                                 RoomSizeSqft=i.RoomSizeSqft,
                                 MaxOccupiedPerson=i.MaxOccupiedPerson,
                                 BedQuantity=i.BedQuantity,
                                 HasPrice = r.HasPrice,
                                 Discounted=r.Discounted,
                                 WithBreakfast=r.WithBreakfast,
                                 ItemName = d.ItemName,
                                 CultureCode = d.CultureCode,
                                 CurrencyCode = d.CurrencyCode,
                                 OriginalValue = d.OriginalValue,
                                 DiscountedValue = d.DiscountedValue,
                                 FinalValue = d.FinalValue,
                                 OrderAmount = d.OrderAmount,
                                 GrandTotalValue = d.GrandTotalValue,
                                 Remark = d.Remark
                                  
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<HotelReservationDetailModel>>.Success(new PagedList<HotelReservationDetailModel>(query, model.PageIndex, model.PageSize));
                return BaseResponse<PagedList<HotelReservationDetailModel>>.NoContent();
            }
            return BaseResponse<PagedList<HotelReservationDetailModel>>.BadRequest(new PagedList<HotelReservationDetailModel>());
        }

        public BaseResponse<List<HotelReservationShowDashBoardModel>> ShowDashboardReservationInfo(int hotelId)
        {
            DateTime thisWeek = DateTime.Now.AddDays(6);
            var query = from r in _db.HotelReservations.AsNoTracking()
                        where r.HotelFid == hotelId && r.StatusFid == (int)ReservationStatusEnum.Accepted
                        && r.ReservationDate.Value.Date >= DateTime.Now.Date && r.ReservationDate.Value.Date <= thisWeek
                        orderby r.ReservationDate descending
                        select _mapper.Map<HotelReservations, HotelReservationShowDashBoardModel>(r);

            if (query.Count() > 0)
                return BaseResponse<List<HotelReservationShowDashBoardModel>>.Success(query.ToList());
            return BaseResponse<List<HotelReservationShowDashBoardModel>>.NoContent(new List<HotelReservationShowDashBoardModel>());

        }


        public BaseResponse<List<HotelReservationShowDashBoardModel>> ShowDashboardRequestProcessReservations(int hotelId)
        {
            var query = from r in _db.HotelReservations.AsNoTracking()
                        where r.HotelFid == hotelId && (r.StatusFid == (int)ReservationStatusEnum.Paid || r.StatusFid == (int)ReservationStatusEnum.Pending)
                        orderby r.ReservationDate descending
                        select _mapper.Map<HotelReservations, HotelReservationShowDashBoardModel>(r);
            if (query.Count() > 0)
                return BaseResponse<List<HotelReservationShowDashBoardModel>>.Success(query.ToList());
            return BaseResponse<List<HotelReservationShowDashBoardModel>>.NoContent(new List<HotelReservationShowDashBoardModel>());
        }


        public BaseResponse<List<HotelReservationShowDashBoardModel>> ShowDashboardRecentPaymentReservations(int hotelId)
        {
            var query = from c in _db.HotelReservations.AsNoTracking()
                        where c.HotelFid == hotelId && c.StatusFid == (int)ReservationStatusEnum.Paid
                        && c.ReservationDate.Value.Date >= DateTime.Now.Date.AddDays(-6) && c.ReservationDate <= DateTime.Now.Date
                        orderby c.ReservationDate descending
                        select new HotelReservationShowDashBoardModel()
                        {
                            Id = c.Id,
                            HotelFid = c.HotelFid,
                            UniqueId = c.UniqueId,
                            CustomerName = c.CustomerName,
                            ReservationEmail = c.ReservationEmail,
                            ContactNo = c.ContactNo,
                            CustomerFid = c.CustomerFid,
                            Adult = c.Adult,
                            Child = c.Child,
                            CountItem = _db.HotelReservationDetails.AsNoTracking().Where(d => d.ReservationsFid == c.Id).Count(),
                            CustomerNote = c.CustomerNote,
                            DiningDate = c.DiningDate,
                            ReservationDate = c.ReservationDate,
                            CultureCode = c.CultureCode,
                            CurrencyCode = c.CurrencyCode,
                            OriginalValue = c.OriginalValue,
                            DiscountedValue = c.DiscountedValue,
                            GrandTotalValue = c.GrandTotalValue,
                            PaymentCurrency = c.PaymentCurrency,
                            PaymentExchangeRate = c.PaymentExchangeRate,
                            PaymentExchangeDate = c.PaymentExchangeDate,
                            PaymentValue = c.PaymentValue,
                            GotSpecialRequest = c.GotSpecialRequest,
                            SpecialRequestDescriptions = c.SpecialRequestDescriptions,
                            CancelReason = c.CancelReason,
                            StatusFid = c.StatusFid,
                            Processed = c.Processed,
                            ProcessedBy = c.ProcessedBy,
                            ProcessedDate = c.ProcessedDate,
                            ProcessedRemark = c.ProcessedRemark,
                            PrepaidRate = c.PrepaidRate,
                            PrepaidValue = c.PrepaidValue,
                            StatusResKey = c.StatusResKey
                        };

            if (query.Count() > 0)
                return BaseResponse<List<HotelReservationShowDashBoardModel>>.Success(query.ToList());
            return BaseResponse<List<HotelReservationShowDashBoardModel>>.NoContent(new List<HotelReservationShowDashBoardModel>());
        }


    }
}