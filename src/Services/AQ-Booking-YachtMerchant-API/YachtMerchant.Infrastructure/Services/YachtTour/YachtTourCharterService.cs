using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Models.CommonValues;
using AQConfigurations.Core.Services.Interfaces;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtTourCharters;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using System.Linq.Dynamic.Core;
using YachtMerchant.Core.Models.YachtTourCharterDetails;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourCharterService:ServiceBase, IYachtTourCharterService
    {
        private readonly IMapper _mapper;
        private readonly ICommonValueRequestService _commonValueResquestService;
        private List<CommonValueViewModel> _yachtTourCharterSourceList;

        public YachtTourCharterService(IMapper mapper, ICommonValueRequestService commonValueResquestService, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
            _commonValueResquestService = commonValueResquestService;

            // Load common value
            LoadCharterSource();
        }


        #region Private method

        private void LoadCharterSource()
        {
            try
            {
                _yachtTourCharterSourceList = _commonValueResquestService.GetListCommonValueByGroup("YACHTCHARTERINGSOURCE").ResponseData;
            }
            catch (Exception ex)
            {
                _yachtTourCharterSourceList = new List<CommonValueViewModel>();
            }
        }
        #endregion

        /// <summary>
        /// Get infomation of Yacht Tour Charter By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<YachtTourCharterViewModel> GetInfomationYachtTourCharterById(long id)
        {
            try
            {
                var entity = _context.YachtTourCharters.Find(id);
                if (entity == null)
                    return BaseResponse<YachtTourCharterViewModel>.NotFound();
                else
                    return BaseResponse<YachtTourCharterViewModel>.Success(_mapper.Map<YachtTourCharters, YachtTourCharterViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourCharterViewModel>.InternalServerError(message: ex.Message);
            }
        }

        /// <summary>
        /// Create new Yacht Tour Charter from aqbooking source
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> CreateCharterFromOriginSource(YachtTourCharterCreateModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest();

                    // Insert to YachtChartering 
                    var userId = GetUserGuidId();
                    var entityReservation = new YachtTourCharters();
                    entityReservation.InjectFrom(model);
                    entityReservation.YachtTourFid = model.YachtTourFid;
                    entityReservation.YachtFid = model.YachtFid;
                    entityReservation.SourceFid = (int)YachtCharteringSourceEnum.AQBookings; // Default set =1 (from aqbooking),.... ; 5: other source
                    entityReservation.SourceResKey = "SOURCEAQBOOKINGS";
                    entityReservation.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    entityReservation.BookingDate = DateTime.Now;
                    entityReservation.Processed = false;

                    await _context.YachtTourCharters.AddAsync(entityReservation);
                    await _context.SaveChangesAsync();

                    // Insert to Payment Log
                    var entityPaymentLog = new YachtTourCharterPaymentLogs();

                    entityPaymentLog.TourCharterFid = (int)entityReservation.Id;
                    entityPaymentLog.PaymentDate = DateTime.Now;
                    entityPaymentLog.PaymentBy = model.CustomerName;
                    entityPaymentLog.PaymentAmount = model.PaymentValue;
                    entityPaymentLog.CurrencyCode = model.CurrencyCode;
                    entityPaymentLog.CultureCode = model.CultureCode;
                    entityPaymentLog.StatusFid = model.StatusFid;

                    await _context.YachtTourCharterPaymentLogs.AddAsync(entityPaymentLog);
                    await _context.SaveChangesAsync();

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



        /// <summary>
        /// Create new Yacht Tour Charter From Other Source ( outside AQBooking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> CreateCharterFromOtherSource(CreateTourCharterFromOtherSourceModel model)
        {
            if (model == null)
                return BaseResponse<bool>.BadRequest();

            if (model.source != YachtCharteringSourceEnum.AQBookings)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Insert to YachtTourCharter
                        var userId = GetUserGuidId();
                        var entityReservation = new YachtTourCharters();
                        entityReservation.InjectFrom(model);
                        entityReservation.YachtFid = model.YachtFid;
                        if (model.source == YachtCharteringSourceEnum.Others)
                            entityReservation.SourceResKey = "SOURCEOTHERS";
                        else if (model.source == YachtCharteringSourceEnum.Walkin)
                            entityReservation.SourceResKey = "SOURCEWALKINS";
                        else if (model.source == YachtCharteringSourceEnum.ExternalAgency)
                            entityReservation.SourceResKey = "SOURCEAGENCY";
                        else if (model.source == YachtCharteringSourceEnum.Referrer)
                            entityReservation.SourceResKey = "SOURCEREFERRER";
                        else
                            entityReservation.SourceResKey = string.Empty;

                        entityReservation.SourceFid = (int)model.source; // Default set =1 (from aqbooking),.... ; 5: other source
                        entityReservation.IsExistingCustomer = false;
                        entityReservation.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                        entityReservation.BookingDate = DateTime.Now;
                        entityReservation.Processed = true;
                        entityReservation.GotSpecialRequest = false;
                        // Set Price  default 
                        entityReservation.OriginalValue = 0.0;
                        entityReservation.DiscountedValue = 0.0;
                        entityReservation.GrandTotalValue = 0.0;
                        entityReservation.PrepaidRate = 0.0;
                        entityReservation.PrepaidValue = 0.0;
                        entityReservation.OriginalValue = 0.0;
                        entityReservation.PaymentExchangeRate = 0.0;
                        entityReservation.PaymentExchangeDate = DateTime.Now;
                        entityReservation.PaymentValue = 0.0;

                        entityReservation.ProcessedBy = GetUserGuidId();
                        entityReservation.ProcessedDate = DateTime.Now;

                        // Set status confirm
                        entityReservation.StatusFid = (int)ReservationStatusEnum.Accepted;
                        entityReservation.StatusResKey = "ACCEPTED";

                        await _context.YachtTourCharters.AddAsync(entityReservation);
                        await _context.SaveChangesAsync();

                        // Insert to Payment Log
                        var entityPaymentLog = new YachtTourCharterPaymentLogs();

                        entityPaymentLog.TourCharterFid = (int)entityReservation.Id;
                        entityPaymentLog.PaymentDate = DateTime.Now;
                        entityPaymentLog.PaymentBy = model.CustomerName;
                        entityPaymentLog.PaymentAmount = 0.0;
                        entityPaymentLog.StatusFid = (int)ReservationStatusEnum.Accepted;

                        await _context.YachtTourCharterPaymentLogs.AddAsync(entityPaymentLog);
                        await _context.SaveChangesAsync();

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
            else
                return BaseResponse<bool>.BadRequest();


        }

        /// <summary>
        /// Update Status Process Yacht Tour Charter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> UpdateStatusAsync(YachtTourCharterConfirmStatusModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model != null)
                    {
                        var entity = _context.YachtTourCharters.Find(Convert.ToInt64(model.Id ?? "0"));
                        // Get Paymentlogs
                        var entityPaymentLog = _context.YachtTourCharterPaymentLogs.Where(p => p.TourCharterFid == Convert.ToInt64(model.Id ?? "0")).AsNoTracking()?.FirstOrDefault();
                        if (entity != null && entityPaymentLog != null)
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

                            entity.ProcessedDate = DateTime.Now;

                            _context.YachtTourCharters.Update(entity);
                            await _context.SaveChangesAsync();

                            // Update status reservastion entity ReservationPaymentLogs
                            entityPaymentLog.StatusFid = model?.Status.ToInt32() ?? entityPaymentLog.StatusFid;
                            _context.YachtTourCharterPaymentLogs.Update(entityPaymentLog);
                            await _context.SaveChangesAsync();

                            //Transaction commit all
                            transaction.Commit();
                            return BaseResponse<bool>.Success(true);

                        }
                        else
                            return BaseResponse<bool>.NotFound();
                    }
                    else
                        return BaseResponse<bool>.BadRequest();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BaseResponse<bool>.InternalServerError(message: ex.Message);
                }

            }


        }

        /// <summary>
        /// Delete Yacht Tour Charter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var entity = _context.YachtTourCharters.Find(id);
                if (entity != null)
                {
                    entity.StatusFid = (int)ReservationStatusEnum.Rejected;// Check Common Value Rule Status FID, 1: Delete; 2: Cancelled
                    entity.ProcessedBy = GetUserGuidId();
                    entity.ProcessedDate = DateTime.Now;

                    _context.YachtTourCharters.Update(entity);
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

        /// <summary>
        /// Search all Yacht Chartering of all Yacht with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtTourCharterViewModel>> SearchAllCharterPaging(YachtTourCharterSearchPagingModel searchModel)
        {

            var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "BookingDate DESC";
            int type = searchModel.Type; // // Type status need view
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();
            // Case view all status
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _context.YachtTourCharters.AsNoTracking().Where(k => k.StatusFid != 0 &&
                                 (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
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
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid= c.YachtTourFid,
                                 TourName=_context.YachtTours.AsNoTracking().FirstOrDefault( t=>t.Id== c.YachtTourFid).TourName,
                                 CultureCode = c.CultureCode,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);
                if (query.Count() > 0)
                    return BaseResponse<PagedList<YachtTourCharterViewModel>>.Success(new PagedList<YachtTourCharterViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtTourCharterViewModel>>.NoContent();
            }
            else
            {
                // Type view reservation
                int ViewReservationStatusMode = 1;// set default status view mode
                // check is processed field is : true of false in case : true (Accepted,Rejected,Canceled,Completed )
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

                var query = (from c in _context.YachtTourCharters.AsNoTracking().Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                        (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
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
                    return BaseResponse<PagedList<YachtTourCharterViewModel>>.Success(new PagedList<YachtTourCharterViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtTourCharterViewModel>>.NotFound();
            }

        }


        /// <summary>
        /// Search all Charter of [All yacht of Merchant with Paging]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtTourCharterViewModel>> SearchAllCharterOfMerchantPaging(YachtTourCharterOfMerchantSearchPagingModel searchModel)
        {

            var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "BookingDate DESC";
            int type = searchModel.Type; // // Type status need view
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();
            // Case view all status
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantId)
                             join t in _context.YachtTourCharters.AsNoTracking() on y.Id equals t.YachtFid into Temp
                             from c in Temp.Where(k => k.StatusFid != 0 &&
                                (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                    (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                    (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
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
                    return BaseResponse<PagedList<YachtTourCharterViewModel>>.Success(new PagedList<YachtTourCharterViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtTourCharterViewModel>>.NoContent();
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
                var query = (from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantId)
                             join t in _context.YachtTourCharters.AsNoTracking() on y.Id equals t.YachtFid into Temp
                             from c in Temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                 (k.Processed == checkProcessed) &&
                                     (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                         (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                             (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                 (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                     (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                         (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
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
                    return BaseResponse<PagedList<YachtTourCharterViewModel>>.Success(new PagedList<YachtTourCharterViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtTourCharterViewModel>>.NoContent();
            }

        }


        /// <summary>
        /// Search all Yacht Tour Charter of Tour with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtTourCharterViewModel>>SearchAllCharterOfTourPaging(YachtTourCharterOfTourSearchPagingModel searchModel)
        {
            var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                :"BookingDate DESC";
            int type = searchModel.Type; // // Type status need view
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();
            // Case view all status
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _context.YachtTourCharters.AsNoTracking().Where(k => k.StatusFid != 0 && k.YachtFid == searchModel.YachtTourFid &&
                                 (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
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
                    return BaseResponse<PagedList<YachtTourCharterViewModel>>.Success(new PagedList<YachtTourCharterViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtTourCharterViewModel>>.NoContent();
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
                var query = (from c in _context.YachtTourCharters.AsNoTracking().Where(k => k.YachtFid == searchModel.YachtTourFid && (k.StatusFid == ViewReservationStatusMode) &&
                               (k.Processed == checkProcessed) &&
                                (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                 (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                  (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                   (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
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
                    return BaseResponse<PagedList<YachtTourCharterViewModel>>.Success(new PagedList<YachtTourCharterViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtTourCharterViewModel>>.NoContent();
            }

        }

        /// <summary>
        /// Get all Yacht Tour Charter , of all Yacht no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<List<YachtTourCharterViewModel>> GetAllCharterByTypeNoPaging(YachtTourCharterSearchModel searchModel)
        {

            var sortString = "BookingDate DESC";
            int type = searchModel.Type; // Type status need view
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();

            // Case view all status reservation
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _context.YachtTourCharters.AsNoTracking().Where(k => k.StatusFid != 0 &&
                                 (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
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
                    return BaseResponse<List<YachtTourCharterViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtTourCharterViewModel>>.NoContent();
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
                var query = (from c in _context.YachtTourCharters.AsNoTracking().Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                               (k.Processed == checkProcessed) &&
                                (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                 (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                  (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                   (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
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
                    return BaseResponse<List<YachtTourCharterViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtTourCharterViewModel>>.NoContent();
            }

        }


        /// <summary>
        /// Get all Yacht Tour Charter of [Merchant] by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<List<YachtTourCharterViewModel>> GetAllCharterOfMerchantByTypeNoPaging(YachtTourCharterOfMerchantSearchModel searchModel)
        {

            var sortString = "BookingDate DESC";
            int type = searchModel.Type; // Type: 0: All reservation; 1: Completed; 2: Cancelled
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();
            // Case view all status reservation
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantId)
                             join t in _context.YachtTourCharters.AsNoTracking() on y.Id equals t.YachtFid into Temp
                             from c in Temp.Where(k => k.StatusFid != 0 &&
                               (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                 (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                   (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                     (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                       (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                         (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 YachtName = y.Name,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
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
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
                                 PrepaidRate= c.PrepaidRate,
                                 PrepaidValue=c.PrepaidValue,
                                 StatusResKey=c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtTourCharterViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtTourCharterViewModel>>.NoContent();
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
                var query = (from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantId)
                             join t in _context.YachtTourCharters.AsNoTracking() on y.Id equals t.YachtFid into Temp
                             from c in Temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                        (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 YachtName = y.Name,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
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
                                 IsExistingCustomer= c.IsExistingCustomer,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtTourCharterViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtTourCharterViewModel>>.NoContent();
            }

        }


        /// <summary>
        /// Get all Yacht Tour Charter of [Tour] by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<List<YachtTourCharterViewModel>> GetAllCharterOfTourByTypeNoPaging(YachtTourCharterOfTourSearchModel searchModel)
        {

            var sortString = "BookingDate DESC";
            int type = searchModel.Type; // Type: 0: All reservation; 1: Completed; 2: Cancelled
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();
            // Case view all status reservation
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false)
                             join t in _context.YachtTourCharters.AsNoTracking().Where(u => u.YachtTourFid == searchModel.YachtTourFid) on y.Id equals t.YachtFid into temp
                             from c in temp.Where(k => k.StatusFid != 0 &&
                                 (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 YachtName = y.Name,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceFid = c.SourceFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
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
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtTourCharterViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtTourCharterViewModel>>.NoContent();
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
                var query = (from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false)
                             join t in _context.YachtTourCharters.AsNoTracking().Where(u => u.YachtFid == searchModel.YachtTourFid) on y.Id equals t.YachtFid into temp
                             from c in temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (charterDate == null || k.DateFrom.Date == charterDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                        (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtTourCharterViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 YachtName = y.Name,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtTourCharterSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 CountItem = _context.YachtTourCharterDetails.AsNoTracking().Where(d => d.TourCharterFid == c.Id).Count(),
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
                                 SourceFid = c.SourceFid,
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtTourFid = c.YachtTourFid,
                                 TourName = _context.YachtTours.AsNoTracking().FirstOrDefault(t => t.Id == c.YachtTourFid).TourName,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtTourCharterViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtTourCharterViewModel>>.NoContent();
            }

        }


        /// <summary>
        /// Get Yacht Tour Charter Detail No Paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<YachtTourCharterDetailsModel> GetCharterDetail(long id)
        {
            var result = (from d in _context.YachtTourCharterDetails.AsNoTracking().Where(r => r.Id == id)
                          join c in _context.YachtTourCharters.AsNoTracking() on d.TourCharterFid equals c.Id
                          join y in _context.Yachts.AsNoTracking().Where(t => t.Deleted == false) on c.YachtFid equals y.Id
                          select new YachtTourCharterDetailsModel()
                          {
                              Id = d.Id,
                              YachtFid = c.YachtFid,
                              YachtUniqueId = y.UniqueId,
                              TourCharterFid = d.TourCharterFid,
                              YachtName = y.Name,
                              ItemTypeFid = d.ItemTypeFid,
                              RefFid = d.RefFid,
                              ItemName = d.ItemName,
                              CultureCode = d.CultureCode,
                              CurrencyCode = d.CurrencyCode,
                              OriginalValue = d.OriginalValue,
                              DiscountedValue = d.DiscountedValue,
                              FinalValue = d.FinalValue,
                              OrderAmount = d.OrderAmount,
                              GrandTotalValue = d.GrandTotalValue,
                              Remark = d.Remark
                          }).FirstOrDefault();

            if (result != null)
                return BaseResponse<YachtTourCharterDetailsModel>.Success(result);
            else
                return BaseResponse<YachtTourCharterDetailsModel>.NoContent();

        }

        /// <summary>
        /// Get Yacht Tour Charter Details with Paging ==>  [is Running]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtTourCharterDetailsModel>> GetCharterDetailPaging(YachtTourCharterDetailSearchPagingModel model)
        {
            if (model.ID != 0)
            {
                var sortString = "TourCharterFid DESC";
                var query = (from d in _context.YachtTourCharterDetails.AsNoTracking().Where(r => r.Id == model.ID)
                             join c in _context.YachtTourCharters.AsNoTracking() on d.TourCharterFid equals c.Id
                             join y in _context.Yachts.AsNoTracking().Where(t => t.Deleted == false) on c.YachtFid equals y.Id
                             select new YachtTourCharterDetailsModel()
                             {
                                 Id = d.Id,
                                 YachtFid = c.YachtFid,
                                 YachtUniqueId = y.UniqueId,
                                 TourCharterFid = d.TourCharterFid,
                                 YachtName = y.Name,
                                 ItemTypeFid = d.ItemTypeFid,
                                 RefFid = d.RefFid,
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
                    return BaseResponse<PagedList<YachtTourCharterDetailsModel>>.Success(new PagedList<YachtTourCharterDetailsModel>(query, model.PageIndex, model.PageSize));
                else
                    return BaseResponse<PagedList<YachtTourCharterDetailsModel>>.NoContent();
            }
            else
                return BaseResponse<PagedList<YachtTourCharterDetailsModel>>.BadRequest();
        }


        public BaseResponse<int> CalculateTotalReservationOfMerchantByMerchantId(GetTotalReservationOfMerchantModel model)
        {
            if (model.MerchantId != 0 && !String.IsNullOrEmpty(model.EffectiveStartDate))
            {
                DateTime? effectiveStartDate = null;
                if (model.EffectiveStartDate.HaveValue())
                    effectiveStartDate = model.EffectiveStartDate.ToNullDateTime();
                var query = (from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == model.MerchantId)
                             join d in _context.YachtTourCharters.AsNoTracking().Where(r => r.Processed == true
                                 && (r.StatusFid == (int)ReservationStatusEnum.Accepted || r.StatusFid == (int)ReservationStatusEnum.Completed)
                                 && (effectiveStartDate == null || r.BookingDate.Value.Date >= effectiveStartDate.Value.Date))
                             on y.Id equals d.YachtFid
                             select new
                             {
                                 y.Name
                             }).ToList();
                if (query.Count > 0)
                    return BaseResponse<int>.Success(query.Count);
                else
                    return BaseResponse<int>.Success(0);
            }
            else
                return BaseResponse<int>.BadRequest();
        }


        public BaseResponse<double> CalculateTotalAmountReservationOfMerchantByMerchantId(GetTotalAmountReservationOfMerchantWithItemTypeModel model)
        {
            if (model.MerchantId != 0 && model.ReservationItemType != 0 && !String.IsNullOrEmpty(model.EffectiveStartDate))
            {
                DateTime? effectiveStartDate = null;
                if (model.EffectiveStartDate.HaveValue())
                    effectiveStartDate = model.EffectiveStartDate.ToNullDateTime();
                var result = (from c in _context.YachtTourCharters.AsNoTracking().Where(r => r.Processed == true
                             && (r.StatusFid == (int)ReservationStatusEnum.Accepted || r.StatusFid == (int)ReservationStatusEnum.Completed)
                             && (effectiveStartDate == null || r.BookingDate.Value.Date >= effectiveStartDate.Value.Date))
                              join cd in _context.YachtCharteringDetails.AsNoTracking().Where(d => d.ItemTypeFid == model.ReservationItemType)
                              on c.Id equals cd.CharteringFid
                              join p in _context.Yachts.AsNoTracking().Where(t => t.Deleted == false && t.MerchantFid == model.MerchantId)
                              on c.YachtFid equals p.Id
                              select new
                              {
                                  cd.GrandTotalValue
                              }).Sum(x => x.GrandTotalValue);
                if (result >= 0)
                    return BaseResponse<double>.Success(result);
                else
                    return BaseResponse<double>.Success(0);
            }
            else
                return BaseResponse<double>.BadRequest();

        }



        public BaseResponse<List<YachtTourCharterDetailModel>> ShowDashboardReservationInfo(int yachtId)
        {
            DateTime thisWeek = DateTime.Now.AddDays(6);
            var query = from r in _context.YachtTourCharters.AsNoTracking()
                        where r.YachtFid == yachtId && r.StatusFid == (int)ReservationStatusEnum.Accepted
                        && r.DateFrom.Date >= DateTime.Now.Date && r.DateFrom <= thisWeek
                        orderby r.DateFrom descending
                        select _mapper.Map<YachtTourCharters, YachtTourCharterDetailModel>(r);

            if (query.Count() > 0)
                return BaseResponse<List<YachtTourCharterDetailModel>>.Success(query.ToList());
            else
                return BaseResponse<List<YachtTourCharterDetailModel>>.NoContent();

        }


        public BaseResponse<List<YachtTourCharterDetailModel>> ShowDashboardRequestProcessReservations(int yachtId)
        {
            var query = from r in _context.YachtTourCharters.AsNoTracking()
                        where r.YachtFid == yachtId && (r.StatusFid == (int)ReservationStatusEnum.Paid || r.StatusFid == (int)ReservationStatusEnum.Pending)
                        orderby r.DateFrom descending
                        select _mapper.Map<YachtTourCharters, YachtTourCharterDetailModel>(r);
            if (query.Count() > 0)
                return BaseResponse<List<YachtTourCharterDetailModel>>.Success(query.ToList());
            else
                return BaseResponse<List<YachtTourCharterDetailModel>>.NoContent();
        }


        public BaseResponse<List<YachtTourCharterDetailModel>> ShowDashboardRecentPaymentReservations(int yachtId)
        {
            var query = from r in _context.YachtTourCharters.AsNoTracking()
                        where r.YachtFid == yachtId && r.StatusFid == (int)ReservationStatusEnum.Paid
                        && r.PaymentExchangeDate.Value.Date >= DateTime.Now.Date.AddDays(-6) && r.PaymentExchangeDate <= DateTime.Now.Date
                        orderby r.PaymentExchangeDate descending
                        select new YachtTourCharterDetailModel()
                        {
                            Id = r.Id,
                            CustomerFid = r.CustomerFid,
                            CustomerName = r.CustomerName,
                            ReservationEmail = r.ReservationEmail,
                            ContactNo = r.ContactNo,
                            YachtPortFid = r.YachtPortFid,
                            YachtFid = r.YachtFid,
                            BookingDate = r.BookingDate,
                            DateFrom = r.DateFrom,
                            DateTo = r.DateTo,
                            CultureCode = r.CultureCode,
                            CurrencyCode = r.CurrencyCode,
                            Passengers = r.Passengers,
                            CustomerNote = r.CustomerNote,
                            DiscountedValue = r.DiscountedValue,
                            OriginalValue = r.OriginalValue,
                            PaymentValue = r.PaymentValue,
                            PrepaidRate = r.PrepaidRate,
                            PrepaidValue = r.PrepaidValue,
                            Processed = r.Processed,
                            ProcessedBy = r.ProcessedBy,
                            ProcessedDate = r.ProcessedDate,
                            ProcessedRemark = r.ProcessedRemark,
                            YachtName = r.YachtPortName,
                            StatusFid = r.StatusFid,
                            GotSpecialRequest = r.GotSpecialRequest,
                            SourceFid = r.SourceFid,
                            SpecialRequestDescriptions = r.SpecialRequestDescriptions,
                            UniqueID = r.UniqueId,
                            CancelReason = r.CancelReason,
                            SourceResKey = r.SourceResKey,
                            YachtPortName = r.YachtPortName,
                            CountItem = _context.YachtTourCharterDetails.Where(d=> d.TourCharterFid == r.Id).Count()
                        };

            if (query.Count() > 0)
                return BaseResponse<List<YachtTourCharterDetailModel>>.Success(query.ToList());
            else
                return BaseResponse<List<YachtTourCharterDetailModel>>.NoContent();
        }

    }
}
