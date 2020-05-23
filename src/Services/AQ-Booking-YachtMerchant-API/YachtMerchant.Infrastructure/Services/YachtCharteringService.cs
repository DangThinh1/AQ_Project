using AQBooking.Core.Helpers;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtCharterings;
using YachtMerchant.Infrastructure.Interfaces;
using System.Linq.Dynamic.Core;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.CommonValues;
using YachtMerchant.Core.Models.YachtCharteringDetails;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtCharteringService:ServiceBase, IYachtCharteringService
    {
        private readonly IMapper _mapper;
        private readonly ICommonValueRequestService _commonValueResquestService;
        private List<CommonValueViewModel> _yachtPricingTypeList;
        private List<CommonValueViewModel> _yachtCharteringSourceList;

        public YachtCharteringService(IMapper mapper,
            ICommonValueRequestService commonValueResquestService,
            YachtOperatorDbContext dbcontext) :base( dbcontext)
        {
            _mapper = mapper;
            _commonValueResquestService = commonValueResquestService;

            // Load common value
            LoadPricingType();
            LoadCharteringSource();
           
        }

        #region Private method
        private void LoadPricingType()
        {
            try
            {
                _yachtPricingTypeList = _commonValueResquestService.GetListCommonValueByGroup("YACHTPRICINGTYPE").ResponseData;
            }
            catch (Exception ex)
            {
                _yachtPricingTypeList = new List<CommonValueViewModel>();
            }
        }

        private void LoadCharteringSource()
        {
            try
            {
                _yachtCharteringSourceList = _commonValueResquestService.GetListCommonValueByGroup("YACHTCHARTERINGSOURCE").ResponseData;
            }
            catch (Exception ex)
            {
                _yachtCharteringSourceList = new List<CommonValueViewModel>();
            }
        }
        #endregion


        /// <summary>
        /// Get infomation of Yacht Chartering By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<YachtCharteringsViewModel> GetInfomationYachtCharteringById(long id)
        {
            try
            {
                var entity = _context.YachtCharterings.Find(id);
                if (entity == null)
                    return BaseResponse<YachtCharteringsViewModel>.NotFound();
                else
                    return BaseResponse<YachtCharteringsViewModel>.Success(_mapper.Map<YachtCharterings, YachtCharteringsViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringsViewModel>.InternalServerError(message: ex.Message);
            }
        }

        /// <summary>
        /// Create new Yacht Chartering from aqbooking source
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> CreateCharteringFromOriginSource(YachtCharteringsCreateModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest();
                    
                    // Insert to YachtChartering 
                    var userId = GetUserGuidId();
                    var entityReservation = new YachtCharterings();
                    entityReservation.InjectFrom(model);
                    entityReservation.YachtFid = model.YachtFid;
                    entityReservation.SourceFid = (int)YachtCharteringSourceEnum.AQBookings; // Default set =1 (from aqbooking),.... ; 5: other source
                    entityReservation.SourceResKey = "SOURCEAQBOOKINGS";
                    entityReservation.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    entityReservation.BookingDate = DateTime.Now;
                    entityReservation.Processed = false;

                    await _context.YachtCharterings.AddAsync(entityReservation);
                    await _context.SaveChangesAsync();

                    // Insert to Payment Log
                    var entityPaymentLog = new YachtCharteringPaymentLogs();

                    entityPaymentLog.CharteringFid = (int) entityReservation.Id;
                    entityPaymentLog.PaymentDate = DateTime.Now;
                    entityPaymentLog.PaymentBy = model.CustomerName;
                    entityPaymentLog.PaymentAmount = model.PaymentValue;
                    entityPaymentLog.CurrencyCode = model.CurrencyCode;
                    entityPaymentLog.CultureCode = model.CultureCode;
                    entityPaymentLog.StatusFid = model.StatusFid;

                    await _context.YachtCharteringPaymentLogs.AddAsync(entityPaymentLog);
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
        /// Create new Yacht Chartering From Other Source ( outside AQBooking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> CreateCharteringFromOtherSource(CreateCharteringFromOtherSourceModel model)
        {
            if (model == null)
                return BaseResponse<bool>.BadRequest();

            if ( model.source != YachtCharteringSourceEnum.AQBookings )
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Insert to YachtChartering 
                        var userId = GetUserGuidId();
                        var entityReservation = new YachtCharterings();
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
                        // Set Price chartering default 
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

                        await _context.YachtCharterings.AddAsync(entityReservation);
                        await _context.SaveChangesAsync();

                        // Insert to Payment Log
                        var entityPaymentLog = new YachtCharteringPaymentLogs();

                        entityPaymentLog.CharteringFid = (int)entityReservation.Id;
                        entityPaymentLog.PaymentDate = DateTime.Now;
                        entityPaymentLog.PaymentBy = model.CustomerName;
                        entityPaymentLog.PaymentAmount = 0.0;
                        entityPaymentLog.StatusFid = (int)ReservationStatusEnum.Accepted;

                        await _context.YachtCharteringPaymentLogs.AddAsync(entityPaymentLog);
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
        /// Update Status Process Yacht Chartering
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> UpdateStatusAsync(YachtCharteringsConfirmStatusModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model != null)
                    {
                        var entity = _context.YachtCharterings.Find(Convert.ToInt64(model.Id ?? "0"));
                        // Get Paymentlogs
                        var entityPaymentLog = _context.YachtCharteringPaymentLogs.Where(p => p.CharteringFid == Convert.ToInt64(model.Id ?? "0")).AsNoTracking()?.FirstOrDefault();
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

                            _context.YachtCharterings.Update(entity);
                            await _context.SaveChangesAsync();

                            // Update status reservastion entity ReservationPaymentLogs
                            entityPaymentLog.StatusFid = model?.Status.ToInt32() ?? entityPaymentLog.StatusFid;
                            _context.YachtCharteringPaymentLogs.Update(entityPaymentLog);
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
        /// Delete Yacht Chartering
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var entity = _context.YachtCharterings.Find(id);
                if (entity != null)
                {
                    entity.StatusFid = (int)ReservationStatusEnum.Rejected;// Check Common Value Rule Status FID, 1: Delete; 2: Cancelled
                    entity.ProcessedBy = GetUserGuidId();
                    entity.ProcessedDate = DateTime.Now;

                    _context.YachtCharterings.Update(entity);
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
        public BaseResponse<PagedList<YachtCharteringsViewModel>> SearchAllYachtCharteringPaging(YachtCharteringsSearchPagingModel searchModel)
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
                var query = (from c in _context.YachtCharterings.AsNoTracking().Where(k => k.StatusFid != 0 &&
                                 (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                                                    CharterDateFrom = c.CharterDateFrom,
                                                                        CharterDateTo = c.CharterDateTo,
                                                                            BookingDate = c.BookingDate,
                                                                                YachtPortName = c.YachtPortName,
                                                                                    YachtPortFid = c.YachtPortFid,
                                                                                        PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                                                                            PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                                                                               SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                                                                                  SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
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
                                                                                                                                                                    PricingTypeFid = c.PricingTypeFid,
                                                                                                                                                                            HaveAdditionalServices = c.HaveAdditionalServices,
                                                                                                                                                                                HaveChef = c.HaveChef,
                                                                                                                                                                                    HaveCrewsMember = c.HaveCrewsMember,
                                                                                                                                                                                        CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                                                                                                                                                                            IsExistingCustomer = c.IsExistingCustomer,
                                                                                                                                                                                                YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
                                                                                                                                                                                                    CultureCode = c.CultureCode,
                                                                                                                                                                                                        PrepaidRate = c.PrepaidRate,
                                                                                                                                                                                                            PrepaidValue = c.PrepaidValue,
                                                                                                                                                                                                                StatusResKey = c.StatusResKey,
                                                                                                                                                                                                                   SourceFid=c.SourceFid
                                                                                                                                                                                                       


                             }).OrderBy(sortString);
                if (query.Count()>0)
                    return BaseResponse<PagedList<YachtCharteringsViewModel>>.Success(new PagedList<YachtCharteringsViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtCharteringsViewModel>>.NoContent();
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

                var query = (from c in _context.YachtCharterings.AsNoTracking().Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                        (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 PricingTypeFid = c.PricingTypeFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef, 
                                 HaveCrewsMember= c.HaveCrewsMember,
                                 CountItem= _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault( y=> y.Id == c.YachtFid).Name,
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
                                 StatusResKey = c.StatusResKey,
                                 SourceFid=c.SourceFid

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<YachtCharteringsViewModel>>.Success(new PagedList<YachtCharteringsViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtCharteringsViewModel>>.NotFound();
            }
            
        }


        /// <summary>
        /// Search all Yacht Chartering of [All yacht of Merchant with Paging]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtCharteringsViewModel>> SearchAllYachtCharteringOfMerchantPaging(YachtCharteringsOfMerchantSearchPagingModel searchModel)
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
                var query =(from y in _context.Yachts.AsNoTracking().Where(y=>y.Deleted==false && y.MerchantFid==searchModel.MerchantId)
                            join t in _context.YachtCharterings.AsNoTracking() on y.Id  equals t.YachtFid into Temp
                            from c in Temp.Where( k => k.StatusFid != 0 && 
                                (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) && 
                                    (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                        (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                            (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                    (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingTypeFid = c.PricingTypeFid,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
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
                                 StatusResKey = c.StatusResKey,
                                 SourceFid=c.SourceFid

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<YachtCharteringsViewModel>>.Success(new PagedList<YachtCharteringsViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtCharteringsViewModel>>.NoContent();
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
                var query =(from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == searchModel.MerchantId)
                            join t in _context.YachtCharterings.AsNoTracking() on y.Id equals t.YachtFid into Temp
                            from c in Temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                        (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingTypeFid = c.PricingTypeFid,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
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
                                 StatusResKey = c.StatusResKey,
                                 SourceFid = c.SourceFid

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<YachtCharteringsViewModel>>.Success(new PagedList<YachtCharteringsViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtCharteringsViewModel>>.NoContent();
            }
            
        }


        /// <summary>
        /// Search all Yacht Chartering of Yacht with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtCharteringsViewModel>> SearchAllYachtCharteringOfYachtPaging(YachtCharteringsOfYachtSearchPagingModel searchModel)
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
                var query = (from c in _context.YachtCharterings.AsNoTracking().Where(k => k.StatusFid != 0 && k.YachtFid== searchModel.YachtId &&
                                 (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingTypeFid = c.PricingTypeFid,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
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
                                 PrepaidRate=c.PrepaidRate,
                                 PrepaidValue=c.PrepaidValue,
                                 StatusResKey=c.StatusResKey

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<YachtCharteringsViewModel>>.Success( new PagedList<YachtCharteringsViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtCharteringsViewModel>>.NoContent();
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
                var query = (from c in _context.YachtCharterings.AsNoTracking().Where(k => k.YachtFid== searchModel.YachtId && (k.StatusFid == ViewReservationStatusMode) &&
                               (k.Processed == checkProcessed) &&
                                (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                 (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                  (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                   (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingTypeFid = c.PricingTypeFid,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
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
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<PagedList<YachtCharteringsViewModel>>.Success( new PagedList<YachtCharteringsViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

                return BaseResponse<PagedList<YachtCharteringsViewModel>>.NoContent();
            }
            
        }

        /// <summary>
        /// Get all Yacht Chartering , of all Yacht no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<List<YachtCharteringsViewModel>> GetAllYachtCharteringByTypeNoPaging(YachtCharteringsSearchModel searchModel)
        {

            var sortString = "BookingDate DESC";
            int type = searchModel.Type; // Type status need view
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();

            // Case view all status reservation
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from c in _context.YachtCharterings.AsNoTracking().Where(k => k.StatusFid != 0  &&
                                 (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingTypeFid = c.PricingTypeFid,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
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
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey

                             }).OrderBy(sortString);
                if(query.Count()>0)
                    return BaseResponse<List<YachtCharteringsViewModel>>.Success( query.ToList());
                return BaseResponse<List<YachtCharteringsViewModel>>.NoContent();
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
                var query = (from c in _context.YachtCharterings.AsNoTracking().Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                               (k.Processed == checkProcessed) &&
                                (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                 (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                  (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                   (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
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
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingTypeFid = c.PricingTypeFid,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
                                 IsExistingCustomer = c.IsExistingCustomer,
                                 YachtName = _context.Yachts.AsNoTracking().FirstOrDefault(y => y.Id == c.YachtFid).Name,
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
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtCharteringsViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtCharteringsViewModel>>.NoContent();
            }
            
        }


        /// <summary>
        /// Get all Yacht Chartering of [Merchant] by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<List<YachtCharteringsViewModel>> GetAllYachtCharteringOfMerchantByTypeNoPaging(YachtCharteringsOfMerchantSearchModel searchModel)
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
                               join t in _context.YachtCharterings.AsNoTracking() on y.Id equals t.YachtFid into Temp
                                 from c in Temp.Where(k => k.StatusFid != 0 &&
                                   (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                       (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                         (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                           (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                             (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 YachtName= y.Name,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingTypeFid = c.PricingTypeFid,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
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
                                 SourceFid = c.SourceFid,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtCharteringsViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtCharteringsViewModel>>.NoContent();
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
                             join t in _context.YachtCharterings.AsNoTracking() on y.Id equals t.YachtFid into Temp
                             from c in Temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                        (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 CustomerFid = c.CustomerFid,
                                 YachtName= y.Name,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 PricingTypeFid = c.PricingTypeFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
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
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey,
                                 IsExistingCustomer=c.IsExistingCustomer
                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtCharteringsViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtCharteringsViewModel>>.NoContent();
            }
            
        }


        /// <summary>
        /// Get all Yacht Chartering of [Yacht] by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<List<YachtCharteringsViewModel>> GetAllYachtCharteringOfYachtByTypeNoPaging(YachtCharteringsOfYachtSearchModel searchModel)
        {

            var sortString = "BookingDate DESC";
            int type = searchModel.Type; // Type: 0: All reservation; 1: Completed; 2: Cancelled
            DateTime? charterDate = null;
            if (searchModel.CharterDate.HaveValue())
                charterDate = searchModel.CharterDate.ToNullDateTime();
            // Case view all status reservation
            if (type == (int)ReservationViewModeEnum.ViewAllStatusReservation)
            {
                var query = (from y in _context.Yachts.AsNoTracking().Where(y=>y.Deleted==false)
                             join t in _context.YachtCharterings.AsNoTracking().Where(u=>u.YachtFid== searchModel.YachtId) on y.Id equals t.YachtFid into temp
                             from c in temp.Where(k => k.StatusFid != 0 &&
                                 (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                     (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                         (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                             (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                 (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                     (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 YachtName= y.Name,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 PricingTypeFid = c.PricingTypeFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
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
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey,
                                 IsExistingCustomer=c.IsExistingCustomer

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtCharteringsViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtCharteringsViewModel>>.NoContent();
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
                             join t in _context.YachtCharterings.AsNoTracking().Where(u => u.YachtFid == searchModel.YachtId) on y.Id equals t.YachtFid into temp
                             from c in temp.Where(k => (k.StatusFid == ViewReservationStatusMode) &&
                                (k.Processed == checkProcessed) &&
                                    (charterDate == null || k.CharterDateFrom.Date == charterDate.Value.Date) &&
                                        (string.IsNullOrEmpty(searchModel.CharteringCode) || k.UniqueId.StartsWith(searchModel.CharteringCode)) &&
                                            (string.IsNullOrEmpty(searchModel.ReservationEmail) || k.ReservationEmail.StartsWith(searchModel.ReservationEmail)) &&
                                                (string.IsNullOrEmpty(searchModel.ContactNo) || k.ContactNo.StartsWith(searchModel.ContactNo)) &&
                                                    (string.IsNullOrEmpty(searchModel.CustomerName) || k.CustomerName.StartsWith(searchModel.CustomerName) || k.CustomerName.Contains(searchModel.CustomerName)) &&
                                                        (string.IsNullOrEmpty(searchModel.YachtPortName) || k.YachtPortName.StartsWith(searchModel.YachtPortName) || k.YachtPortName.Contains(searchModel.YachtPortName)))
                             select new YachtCharteringsViewModel()
                             {
                                 Id = c.Id,
                                 YachtFid = c.YachtFid,
                                 UniqueId = c.UniqueId,
                                 CustomerName = c.CustomerName,
                                 ReservationEmail = c.ReservationEmail,
                                 ContactNo = c.ContactNo,
                                 YachtName=y.Name,
                                 CustomerFid = c.CustomerFid,
                                 CustomerNote = c.CustomerNote,
                                 Passengers = c.Passengers,
                                 CharterDateFrom = c.CharterDateFrom,
                                 CharterDateTo = c.CharterDateTo,
                                 BookingDate = c.BookingDate,
                                 YachtPortName = c.YachtPortName,
                                 YachtPortFid = c.YachtPortFid,
                                 PricingTypeFid = c.PricingTypeFid,
                                 SourceName = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).Text,
                                 SourceResKey = _yachtCharteringSourceList.FirstOrDefault(x => x.ValueInt == c.SourceFid).ResourceKey,
                                 PricingType = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).Text,
                                 PricingTypeResKey = _yachtPricingTypeList.FirstOrDefault(x => x.ValueInt == c.PricingTypeFid).ResourceKey,
                                 HaveAdditionalServices = c.HaveAdditionalServices,
                                 HaveChef = c.HaveChef,
                                 HaveCrewsMember = c.HaveCrewsMember,
                                 CountItem = _context.YachtCharteringDetails.AsNoTracking().Where(d => d.CharteringFid == c.Id).Count(),
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
                                 IsExistingCustomer=c.IsExistingCustomer,
                                 PrepaidRate = c.PrepaidRate,
                                 PrepaidValue = c.PrepaidValue,
                                 StatusResKey = c.StatusResKey

                             }).OrderBy(sortString);

                if (query.Count() > 0)
                    return BaseResponse<List<YachtCharteringsViewModel>>.Success(query.ToList());
                return BaseResponse<List<YachtCharteringsViewModel>>.NoContent();
            }
            
        }


        /// <summary>
        /// Get Yacht Chartering Detail No Paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<YachtCharteringDetailsModel> GetYachtCharteringDetail(long id)
        {

            var result = (from d in _context.YachtCharteringDetails.AsNoTracking().Where(r => r.Id == id)
                         join y in _context.Yachts.AsNoTracking().Where(t => t.Deleted == false) on d.YachtFid equals y.Id
                         select new YachtCharteringDetailsModel()
                         {
                             Id = d.Id,
                             YachtFid = d.YachtFid,
                             YachtUniqueId = y.UniqueId,
                             CharteringFid = d.CharteringFid,
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
                             Remark = d.Remark,
                             ItemTypeResKey=d.ItemTypeResKey
                               

                         }).FirstOrDefault();

            if (result != null)
                return BaseResponse<YachtCharteringDetailsModel>.Success(result);
            else
                return BaseResponse<YachtCharteringDetailsModel>.NoContent();
            
        }

        /// <summary>
        /// Get Yacht Chartering Detail with Paging ===>  Version 1
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtCharteringsDetailModel>> GetYachtCharteringDetail(YachtCharteringsDetailSearchPagingModel model)
        {

            if (model.ID != 0)
            {
                var query = (from d in _context.YachtCharterings.AsNoTracking().Where(r => r.Id == model.ID)
                             join y in _context.Yachts.AsNoTracking().Where(t => t.Deleted == false) on d.YachtFid equals y.Id
                             select new YachtCharteringsDetailModel()
                             {
                                 Id = d.Id,
                                 YachtFid = d.YachtFid,
                                 UniqueID = d.UniqueId,
                                 CharteringFID = (int) d.Id,
                                 YachtName = y.Name??"",
                                 YachtPortName= d.YachtPortName??"",
                                 CustomerNote = d.CustomerNote??"",
                                 CultureCode = d.CultureCode,
                                 CurrencyCode = d.CurrencyCode,
                                 OriginalValue = d.OriginalValue,
                                 DiscountedValue = d.DiscountedValue,
                                 GrandTotalValue = d.GrandTotalValue,
                                 PaymentValue = d.PaymentValue,

                             });

                if (query.Count() > 0)
                    return BaseResponse<PagedList<YachtCharteringsDetailModel >>.Success( new PagedList<YachtCharteringsDetailModel>(query, model.PageIndex, model.PageSize));
                else
                    return BaseResponse< PagedList<YachtCharteringsDetailModel>>.NoContent();
            }
            else
                return BaseResponse<PagedList<YachtCharteringsDetailModel>>.BadRequest();
        }


        /// <summary>
        /// Get Yacht Chartering Details with Paging ==> Version 2 [is Running]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<PagedList<YachtCharteringDetailsModel>> GetYachtCharteringDetails(YachtCharteringsDetailSearchPagingModel model)
        {

            if (model.ID != 0)
            {
                var sortString = "CharteringFID DESC";
                var query = (from d in _context.YachtCharteringDetails.AsNoTracking().Where(r => r.CharteringFid == model.ID)
                             join y in _context.Yachts.AsNoTracking().Where(r => r.Deleted == false) on d.YachtFid equals y.Id
                             select new YachtCharteringDetailsModel()
                             {
                                 Id = d.Id,
                                 YachtFid = d.YachtFid,
                                 YachtUniqueId = y.UniqueId,
                                 CharteringFid = d.CharteringFid,
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
                    return BaseResponse<PagedList<YachtCharteringDetailsModel>>.Success(new PagedList<YachtCharteringDetailsModel>(query, model.PageIndex, model.PageSize));
                else
                    return BaseResponse<PagedList<YachtCharteringDetailsModel>>.NoContent();
            }
            else
                return BaseResponse<PagedList<YachtCharteringDetailsModel>>.BadRequest();
        }


        public BaseResponse<int> CalculateTotalReservationOfMerchantByMerchantId(GetTotalReservationOfMerchantModel model)
        {
            if (model.MerchantId != 0 && !String.IsNullOrEmpty(model.EffectiveStartDate) )
            {
                DateTime? effectiveStartDate = null;
                if (model.EffectiveStartDate.HaveValue())
                    effectiveStartDate = model.EffectiveStartDate.ToNullDateTime();
                var query =(from y in _context.Yachts.AsNoTracking().Where(y => y.Deleted == false && y.MerchantFid == model.MerchantId)
                            join d in _context.YachtCharterings.AsNoTracking().Where(r => r.Processed == true 
                                && ( r.StatusFid== (int)ReservationStatusEnum.Accepted || r.StatusFid== (int)ReservationStatusEnum.Completed)
                                && (effectiveStartDate == null ||  r.BookingDate.Value.Date >= effectiveStartDate.Value.Date))
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
            if (model.MerchantId != 0 && model.ReservationItemType!=0 && !String.IsNullOrEmpty(model.EffectiveStartDate))
            {
                DateTime? effectiveStartDate = null;
                if (model.EffectiveStartDate.HaveValue())
                    effectiveStartDate = model.EffectiveStartDate.ToNullDateTime();
                var result = (from c in _context.YachtCharterings.AsNoTracking().Where(r => r.Processed == true
                             && (r.StatusFid == (int)ReservationStatusEnum.Accepted || r.StatusFid == (int)ReservationStatusEnum.Completed)
                             && (effectiveStartDate == null || r.BookingDate.Value.Date >= effectiveStartDate.Value.Date))
                             join cd in _context.YachtCharteringDetails.AsNoTracking().Where(d=>d.ItemTypeFid==model.ReservationItemType)
                             on c.Id equals cd.CharteringFid
                             join p in _context.Yachts.AsNoTracking().Where(t => t.Deleted == false && t.MerchantFid == model.MerchantId) 
                             on c.YachtFid equals p.Id
                             select new
                             {
                                 cd.GrandTotalValue
                             }).Sum(x=>x.GrandTotalValue);
                if (result >= 0)
                    return BaseResponse<double>.Success(result);
                else
                    return BaseResponse<double>.Success(0);
            }
            else
                return BaseResponse<double>.BadRequest();

       }


       
        public BaseResponse<List<YachtCharteringsDetailModel>> ShowDashboardReservationInfo(int yachtId)
        {
            DateTime thisWeek = DateTime.Now.AddDays(6);
            var query = (from r in _context.YachtCharterings.AsNoTracking()
                         where r.YachtFid == yachtId && r.StatusFid == (int)ReservationStatusEnum.Accepted
                         && r.BookingDate.Value.Date >= DateTime.Now.Date && r.BookingDate.Value.Date <= thisWeek
                         orderby r.BookingDate descending
                         select _mapper.Map<YachtCharterings, YachtCharteringsDetailModel>(r)).Take(20);

            if (query.Count() > 0)
                return BaseResponse<List<YachtCharteringsDetailModel>>.Success(query.ToList());
            else
                return BaseResponse<List<YachtCharteringsDetailModel>>.NoContent();

        }

       
        public BaseResponse<List<YachtCharteringsDetailModel>> ShowDashboardRequestProcessReservations(int yachtId)
        {
            var query = (from r in _context.YachtCharterings.AsNoTracking()
                        where r.YachtFid == yachtId && ( r.StatusFid == (int)ReservationStatusEnum.Paid || r.StatusFid == (int)ReservationStatusEnum.Pending)
                        orderby r.BookingDate descending
                        select _mapper.Map<YachtCharterings, YachtCharteringsDetailModel>(r)).Take(20);
            if (query.Count() > 0)
                return BaseResponse<List<YachtCharteringsDetailModel>>.Success(query.ToList());
            else
                return BaseResponse<List<YachtCharteringsDetailModel>>.NoContent();
        }

       
        public BaseResponse<List<YachtCharteringsDetailModel>> ShowDashboardRecentPaymentReservations(int yachtId)
        {
            var query = (from r in _context.YachtCharterings.AsNoTracking()
                        where r.YachtFid == yachtId && r.StatusFid == (int)ReservationStatusEnum.Paid
                        && r.BookingDate.Value.Date >= DateTime.Now.Date.AddDays(-6) && r.BookingDate <= DateTime.Now.Date
                        orderby r.BookingDate descending
                        select new YachtCharteringsDetailModel()
                        {
                            Id = r.Id,
                            CustomerFid = r.CustomerFid,
                            CustomerName = r.CustomerName,
                            ReservationEmail = r.ReservationEmail,
                            ContactNo = r.ContactNo,
                            YachtPortFid = r.YachtPortFid,
                            YachtFid = r.YachtFid,
                            BookingDate = r.BookingDate,
                            CharterDateFrom = r.CharterDateFrom,
                            CharterDateTo = r.CharterDateTo,
                            CultureCode = r.CultureCode,
                            CurrencyCode = r.CurrencyCode,
                            Passengers = r.Passengers,
                            CustomerNote = r.CustomerNote,
                            DiscountedValue = r.DiscountedValue,
                            HaveChef = r.HaveChef,
                            HaveAdditionalServices = r.HaveCrewsMember,
                            HaveCrewsMember = r.HaveCrewsMember,
                            OriginalValue = r.OriginalValue,
                            PaymentValue = r.PaymentValue,
                            PrepaidRate = r.PrepaidRate,
                            PrepaidValue = r.PrepaidValue,
                            PricingTypeFid = r.PricingTypeFid,
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
                            CountItem = _context.YachtCharteringDetails.Where(d => d.YachtFid == yachtId && d.CharteringFid == r.Id).Count()
                        }).Take(20);

            if (query.Count() > 0)
                return BaseResponse<List<YachtCharteringsDetailModel>>.Success(query.ToList());
            else
                return BaseResponse<List<YachtCharteringsDetailModel>>.NoContent();
        }





    }
}
