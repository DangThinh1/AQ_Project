using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.YachtPortal.Core.Enum;
using AQBooking.YachtPortal.Core.Helpers;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQBooking.YachtPortal.Core.Models.YachtMerchantProductInventories;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtCharteringService : IYachtCharteringService
    {
        private readonly AQYachtContext _aqYachtContext;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IYachtPricingPlanDetailService _yachtPricingPlanDetailService;
        private readonly IYachtMerchantProductInventoryService _yachtMerchantProductInventoryService;
        public YachtCharteringService(AQYachtContext searchContext
            , IMapper mapper
            , IDistributedCache distributedCache
            , IYachtPricingPlanDetailService yachtPricingPlanDetailService
            , IYachtMerchantProductInventoryService yachtMerchantProductInventoryService
            )
        {
            _aqYachtContext = searchContext;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _yachtPricingPlanDetailService = yachtPricingPlanDetailService;
            _yachtMerchantProductInventoryService = yachtMerchantProductInventoryService;
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtCharteringDetailViewModel>> GetCharteringDetailByCharteringFId(string charteringFId)
        {
            List<YachtCharteringDetailViewModel> responseCharteringDl = new List<YachtCharteringDetailViewModel>();
            try
            {
                var resCharteringFId = Terminator.Decrypt(charteringFId).ToDouble();

                responseCharteringDl = (_aqYachtContext.YachtCharteringDetails
                    .Where(k => k.CharteringFid == resCharteringFId)
                    .Select(i => _mapper.Map<YachtCharteringDetails, YachtCharteringDetailViewModel>(i))).ToList();
                return BaseResponse<List<YachtCharteringDetailViewModel>>.Success(responseCharteringDl);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtCharteringDetailViewModel>>.InternalServerError(responseCharteringDl, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharteringViewModel> GetCharteringByCharteringFId(string charteringFId)
        {
            YachtCharteringViewModel chartering = new YachtCharteringViewModel();
            try
            {
                var resCharteringFId = Terminator.Decrypt(charteringFId).ToDouble();

                chartering = _aqYachtContext.YachtCharterings
                    .Where(k => k.Id == resCharteringFId
                            && k.StatusFid == Convert.ToInt32(YachtCharterStatusEnum.Waiting)
                            && k.Processed == false
                    )
                    .Select(i => _mapper.Map<YachtCharterings, YachtCharteringViewModel>(i)).FirstOrDefault();

                return BaseResponse<YachtCharteringViewModel>.Success(chartering);

            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharteringViewModel> GetChartering(YachtCharteringRequestModel RequestModel)
        {
            YachtCharteringViewModel chartering = new YachtCharteringViewModel();
            try
            {
                DateTime checkinDate = DateTime.Now;
                DateTime checkoutDate = DateTime.Now;
                if (RequestModel.CheckIn != "" && RequestModel.CheckOut != "")
                {
                    checkinDate = DateTime.Parse(RequestModel.CheckIn);
                    checkoutDate = DateTime.Parse(RequestModel.CheckOut);
                }

                var resYachtFId = Terminator.Decrypt(RequestModel.YachtFId).ToInt32();

                chartering = _aqYachtContext.YachtCharterings
                    .Where(k => k.YachtFid == resYachtFId
                            && (RequestModel.StatusId == null || RequestModel.StatusId.Count > 0 || (RequestModel.StatusId.Contains(k.StatusFid)))
                            && ((RequestModel.CheckIn == "" && RequestModel.CheckOut == "")
                                              || (checkinDate >= k.CharterDateFrom && checkinDate <= k.CharterDateTo) || (checkoutDate >= k.CharterDateFrom && checkoutDate <= k.CharterDateTo)

                                              || (checkinDate <= k.CharterDateFrom && checkoutDate >= k.CharterDateTo)
                               )
                    )
                    .Select(i => _mapper.Map<YachtCharterings, YachtCharteringViewModel>(i)).FirstOrDefault();

                return BaseResponse<YachtCharteringViewModel>.Success(chartering);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharteringViewModel> GetCharteringByUniqueId(string uniqueId)
        {
            YachtCharteringViewModel chartering = new YachtCharteringViewModel();
            try
            {
                chartering = _aqYachtContext.YachtCharterings
                    .Where(k => k.UniqueId == uniqueId && k.StatusFid == Convert.ToInt32(YachtCharterStatusEnum.Waiting) && k.Processed == false)
                    .Select(i => _mapper.Map<YachtCharterings, YachtCharteringViewModel>(i)).FirstOrDefault();

                return BaseResponse<YachtCharteringViewModel>.Success(chartering);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<SaveCharterPaymentResponseViewModel> UpdateStatusCharterPrivatePayment(CharteringUpdateStatusModel charteringModel)
        {
            #region initalize logging
            string errCode = "0";
            SaveCharterPaymentResponseViewModel errGlobal = new SaveCharterPaymentResponseViewModel();
            #endregion
            try
            {
                #region logging Subject 
                string dataSubjectLogging = "";
                errGlobal.Value = dataSubjectLogging;
                errGlobal.ResuldCode = errCode;
                errGlobal.UniqueId = charteringModel.UniqueId;
                #endregion
                var responsecharter = _aqYachtContext.YachtCharterings.FirstOrDefault(x => x.UniqueId.Trim().ToLower() == charteringModel.UniqueId.Trim().ToLower());
                if (responsecharter != null)
                {
                    if (charteringModel.StatusFId == Convert.ToInt32(YachtCharterStatusEnum.Waiting))
                    {
                        responsecharter.StatusResKey = "WAITINGPAYMENT";
                    }
                    if (charteringModel.StatusFId == Convert.ToInt32(YachtCharterStatusEnum.Paid))
                    {
                        responsecharter.StatusResKey = "PAID";
                    }
                    if (charteringModel.StatusFId == Convert.ToInt32(YachtCharterStatusEnum.Pending))
                    {
                        responsecharter.StatusResKey = "PENDING";
                    }
                    if (charteringModel.StatusFId == Convert.ToInt32(YachtCharterStatusEnum.Accepted))
                    {
                        responsecharter.StatusResKey = "ACCEPTED";
                    }
                    if (charteringModel.StatusFId == Convert.ToInt32(YachtCharterStatusEnum.Rejected))
                    {
                        responsecharter.StatusResKey = "REJECTED";
                    }
                    if (charteringModel.StatusFId == Convert.ToInt32(YachtCharterStatusEnum.Cancelled))
                    {
                        responsecharter.StatusResKey = "CANCELLED";
                    }
                    if (charteringModel.StatusFId == Convert.ToInt32(YachtCharterStatusEnum.Completed))
                    {
                        responsecharter.StatusResKey = "COMPLETED";
                    }

                    responsecharter.StatusFid = charteringModel.StatusFId;
                    _aqYachtContext.SaveChanges();
                    errGlobal.UniqueId = responsecharter.UniqueId;
                    errGlobal.Id = Terminator.Encrypt(responsecharter.Id.ToString());
                }

                #region logging 
                errCode = "1";
                errGlobal.ResuldCode = errCode;
                #endregion

                return BaseResponse<SaveCharterPaymentResponseViewModel>.Success(errGlobal);
            }
            catch (Exception ex)
            {

                errCode = "-1";
                errGlobal.ResuldCode = errCode;
                errGlobal.Describes = ex.Message.ToString();
                return BaseResponse<SaveCharterPaymentResponseViewModel>.InternalServerError(errGlobal, ex.Message);
            }
        }

        public BaseResponse<YachtCharteringViewModel> GetCharterByReservationEmail(string email)
        {
            var data = _aqYachtContext.YachtCharterings.Where(x => x.ReservationEmail.Contains(email) && x.StatusFid == YachtCharterStatusEnum.Waiting.ToInt32())
                .Select(x => _mapper.Map<YachtCharterings, YachtCharteringViewModel>(x)).FirstOrDefault();
            if (data != null)
                return BaseResponse<YachtCharteringViewModel>.Success(data);
            return BaseResponse<YachtCharteringViewModel>.NotFound();
        }

        #region PAYMENT
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //*****using Yacht/CartPayment
        public BaseResponse<SaveCharterPaymentResponseViewModel> SaveChartering(SaveBookingRequestModel requestModel, string PaymentMethod)
        {
            #region initalize logging
            string errCode = "0";
            SaveCharterPaymentResponseViewModel errGlobal = new SaveCharterPaymentResponseViewModel();
            #endregion

            #region logging Subject 
            string dataSubjectLogging = "";

            errGlobal.Name = "SavePaymentTrip";
            errGlobal.Value = dataSubjectLogging;
            errGlobal.ResuldCode = errCode;
            errGlobal.Id = "0";
            errGlobal.UniqueId = "";
            #endregion
            try
            {
                var redisCartRequestModel = requestModel.RedisCartRequestModel;
                var bookingRequestModel = requestModel.BookingRequestModel;
                var value = _distributedCache.GetString(redisCartRequestModel.Key);
                if (value != null)
                {
                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == redisCartRequestModel.HashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;
                        if (result != null)
                        {
                            List<YachtPackageServiceModel> lstYachtPackage = result.Where(x => redisCartRequestModel.itemList.Contains(x.YachtId)).Select(i => i).ToList();

                            if (lstYachtPackage != null)
                            {
                                ///Foreach Yacht choosed
                                foreach (YachtPackageServiceModel yachtItem in lstYachtPackage)
                                {
                                    #region CALCULATION

                                    MerchantPaymentPackageViewModel responsePackageModel = new MerchantPaymentPackageViewModel();
                                    List<MerchantPaymentEachPackageViewModel> lstProductInventories = new List<MerchantPaymentEachPackageViewModel>();
                                    #region logging Subject 
                                    if (dataSubjectLogging != "")
                                    {
                                        dataSubjectLogging += ",";
                                    }
                                    dataSubjectLogging += "{";
                                    dataSubjectLogging += $"YachtId:{yachtItem.YachtId},";
                                    dataSubjectLogging += $"Passenger:{yachtItem.Passenger },";
                                    dataSubjectLogging += $"CheckIn:{yachtItem.CheckIn },";
                                    dataSubjectLogging += $"CheckOut:{yachtItem.CheckOut },";
                                    string ErrorCode = "0";
                                    string ErrorDescription = "";
                                    string ErrorGlobalPackageDescription = "";
                                    #endregion
                                    try
                                    {
                                        #region YACHT               
                                        var yachtFIdde = Terminator.Decrypt(yachtItem.YachtId).ToInt32();

                                        double dbYachtFee = 0;
                                        string yachtCultureCode = "";
                                        string yachtCurrencyCode = "";

                                        #region NUMBER OF DAY OR WEEK
                                        int bookingDayNumber = GlobalMethod.BookingDayNumber(yachtItem.CheckIn, yachtItem.CheckOut);

                                        #region GET PRICE
                                        var responsePricingPlanDetail = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFId(yachtItem.YachtId);
                                        if (responsePricingPlanDetail != null && responsePricingPlanDetail.IsSuccessStatusCode && responsePricingPlanDetail.ResponseData != null)
                                        {
                                            if (responsePricingPlanDetail.ResponseData.Details != null)
                                            {
                                                var priceDetail = responsePricingPlanDetail.ResponseData.Details.OrderByDescending(x => x.PricingTypeFid).FirstOrDefault();
                                                if (priceDetail != null)
                                                {
                                                    yachtCultureCode = priceDetail.CultureCode;
                                                    yachtCurrencyCode = priceDetail.CurrencyCode;
                                                }
                                                GlobalMethod.GetPriceRecuse(responsePricingPlanDetail.ResponseData.Details, bookingDayNumber, ref dbYachtFee);
                                            }
                                        }

                                        #endregion

                                        responsePackageModel.Id = yachtItem.YachtId;
                                        responsePackageModel.Passenger = yachtItem.Passenger;
                                        responsePackageModel.YachtTotal = dbYachtFee;
                                        responsePackageModel.Total = dbYachtFee;
                                        responsePackageModel.PackageTotal = 0;
                                        responsePackageModel.DiscountTotal = 0;
                                        #endregion

                                        /**INSERT INTO CHARTERING**/

                                        #region YachtPort Infomation
                                        var responseYachtPort = (
                                                       from p in _aqYachtContext.YachtPorts
                                                        .Where(k => k.YachtFid == yachtFIdde
                                                        && k.Deleted == false
                                                        && k.EffectiveDate <= DateTime.Now.Date
                                                        && k.IsActivated == true
                                                        && k.EffectiveDate == _aqYachtContext.YachtPorts
                                                           .Where(o => o.YachtFid == yachtFIdde
                                                            && o.Deleted == false
                                                            && o.EffectiveDate <= DateTime.Now.Date
                                                            && o.IsActivated == true
                                                           )
                                                           .OrderByDescending(x => x.EffectiveDate)
                                                           .Select(i => i.EffectiveDate).FirstOrDefault()
                                                           ).DefaultIfEmpty().Take(1)

                                                       select p
                                             ).FirstOrDefault();
                                        #endregion

                                        #region YachtInfomation
                                        bool isCrewmember = false;
                                        Yachts yachtOjb = _aqYachtContext.Yachts.FirstOrDefault(x => x.Id == yachtFIdde);
                                        if (yachtOjb != null)
                                        {
                                            if (yachtOjb.CrewMembers > 0)
                                            {
                                                isCrewmember = true;
                                            }
                                        }
                                        #endregion

                                        #region INSERT INTO CHARTERING
                                        YachtCharterings charteringModel = new YachtCharterings();
                                        charteringModel.YachtFid = yachtFIdde;

                                        charteringModel.SourceFid = 1;
                                        charteringModel.SourceResKey = "SOURCEAQBOOKINGS";
                                        charteringModel.UniqueId = UniqueIDHelper.GenarateRandomString(12);

                                        //customer
                                        charteringModel.CustomerName = bookingRequestModel.NameOfUser;
                                        charteringModel.ReservationEmail = bookingRequestModel.EmailOfUser;
                                        bool isUerExisting = false;
                                        if (bookingRequestModel.IsEmailExist != 0)
                                        {
                                            isUerExisting = true;
                                        }
                                        charteringModel.IsExistingCustomer = isUerExisting;

                                        if (bookingRequestModel.IdOfUser.Trim() != "")
                                        {
                                            charteringModel.CustomerFid = new Guid(bookingRequestModel.IdOfUser.Trim());
                                        }
                                        charteringModel.ContactNo = bookingRequestModel.ContactNo;
                                        charteringModel.Passengers = yachtItem.Passenger;
                                        charteringModel.CharterDateFrom = DateTime.Now;
                                        charteringModel.CharterDateTo = DateTime.Now;
                                        charteringModel.BookingDate = DateTime.Now;

                                        //yacht port
                                        if (responseYachtPort != null)
                                        {
                                            charteringModel.YachtPortFid = responseYachtPort.PortFid;
                                            charteringModel.YachtPortName = responseYachtPort.PortName;
                                        }
                                        else
                                        {
                                            charteringModel.YachtPortFid = -1;
                                            charteringModel.YachtPortName = "";
                                        }


                                        charteringModel.HaveCrewsMember = isCrewmember;
                                        charteringModel.CultureCode = yachtCultureCode;
                                        charteringModel.CurrencyCode = yachtCurrencyCode;

                                        charteringModel.StatusFid = Convert.ToInt32(YachtCharterStatusEnum.Waiting);
                                        charteringModel.StatusResKey = "WAITINGPAYMENT";
                                        charteringModel.Processed = false;

                                        _aqYachtContext.YachtCharterings.Add(charteringModel);
                                        _aqYachtContext.SaveChanges();
                                        long charteringModelId = charteringModel.Id;
                                        #region logging
                                        errGlobal.Id = Terminator.Encrypt(charteringModelId.ToString());
                                        errGlobal.UniqueId = charteringModel.UniqueId;

                                        #endregion

                                        #endregion

                                        #endregion

                                        #region PACKAGE
                                        bool isPackageAddition = false;
                                        List<MerchantProductInventoriesModel> lstProductPackage = yachtItem.ProductPackage;
                                        double dbPackageFee = 0;
                                        double dbTotalFinalValue = 0;
                                        double dbTotalGrandTotalValue = 0;
                                        double dbTotalDiscountPackage = 0;

                                        if (lstProductPackage != null)
                                        {
                                            List<string> lstProductId = lstProductPackage.Select(x => x.productInventoryFId).ToList();
                                            var responsePriceOfProduct = _yachtMerchantProductInventoryService.GetPriceOfProductInventoryByArrayOfProductId(lstProductId);
                                            if (responsePriceOfProduct != null && responsePriceOfProduct.IsSuccessStatusCode && responsePriceOfProduct.ResponseData != null)
                                            {

                                                foreach (MerchantProductInventoriesModel proItem in lstProductPackage)
                                                {
                                                    #region logging Detail
                                                    //SaveCharterPaymentDetailViewModel errDetail = new SaveCharterPaymentDetailViewModel();
                                                    string dataSubLogging = "";
                                                    dataSubLogging += "{";
                                                    dataSubLogging += $"ProductInventoryFId:\"{proItem.productInventoryFId }\",";
                                                    dataSubLogging += $"Quantity:{ proItem.quantity}";
                                                    string ErrorDetailCode = "0";
                                                    string ErrorDetailPackageDescription = "";
                                                    #endregion
                                                    try
                                                    {
                                                        YachtMerchantProductInventoriesWithPriceViewModel reponseProduct = responsePriceOfProduct.ResponseData.FirstOrDefault(x => x.Id.Trim() == proItem.productInventoryFId.Trim());
                                                        if (reponseProduct != null)
                                                        {
                                                            MerchantPaymentEachPackageViewModel objPackageWithPrice = new MerchantPaymentEachPackageViewModel();

                                                            /**INSERT INTO  CHARTERINGDETAIL**/
                                                            #region INSERT INTO  CHARTERINGDETAIL
                                                            YachtCharteringDetails charteringDetailModel = new YachtCharteringDetails();
                                                            charteringDetailModel.CharteringFid = charteringModelId;
                                                            charteringDetailModel.YachtFid = yachtFIdde;
                                                            charteringDetailModel.ItemTypeFid = 1;
                                                            charteringDetailModel.ItemTypeResKey = "VENDORSERVICES";

                                                            charteringDetailModel.RefFid = Terminator.Decrypt(reponseProduct.Id).ToInt32();

                                                            charteringDetailModel.ItemName = reponseProduct.ProductName;
                                                            charteringDetailModel.CultureCode = reponseProduct.CultureCode;
                                                            charteringDetailModel.CurrencyCode = reponseProduct.CurrencyCode;
                                                            charteringDetailModel.OrderAmount = proItem.quantity;
                                                            charteringDetailModel.DiscountedValue = 0;
                                                            charteringDetailModel.OriginalValue = reponseProduct.Price;
                                                            charteringDetailModel.FinalValue = reponseProduct.Price - charteringDetailModel.DiscountedValue;
                                                            charteringDetailModel.GrandTotalValue = GlobalMethod.PackageTotal(charteringDetailModel.FinalValue, proItem.quantity);
                                                            // charteringDetailModel.FinalValue = charteringDetailModel.GrandTotalValue - charteringDetailModel.DiscountedValue;

                                                            _aqYachtContext.YachtCharteringDetails.Add(charteringDetailModel);

                                                            _aqYachtContext.SaveChanges();
                                                            #endregion
                                                            dbTotalFinalValue += charteringDetailModel.FinalValue;
                                                            dbTotalGrandTotalValue += charteringDetailModel.GrandTotalValue;
                                                            dbPackageFee += GlobalMethod.PackageTotal(charteringDetailModel.OriginalValue, proItem.quantity);
                                                            dbTotalDiscountPackage += GlobalMethod.PackageTotal(charteringDetailModel.DiscountedValue, proItem.quantity);
                                                            isPackageAddition = true;
                                                        }
                                                        ErrorDetailCode = "1";
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        #region logging add detail to errGlobal variable
                                                        ErrorDetailCode = "-1";
                                                        ErrorDetailPackageDescription = ex.Message.ToString();
                                                        #endregion
                                                    }
                                                    dataSubLogging += $"ErrorCode:{ ErrorDetailCode}";
                                                    dataSubLogging += $"ErrorDescription:{ ErrorDetailPackageDescription}";
                                                    dataSubLogging += "}";
                                                    #region ADD PACKAGE DETAIL ERROR TO THE YACHT ERROR.
                                                    if (ErrorGlobalPackageDescription != "")
                                                    {
                                                        ErrorGlobalPackageDescription += ",";
                                                    }
                                                    ErrorGlobalPackageDescription += dataSubLogging;
                                                    #endregion
                                                }

                                                responsePackageModel.PackageTotal = dbPackageFee;
                                                responsePackageModel.DiscountTotal = responsePackageModel.DiscountTotal + dbTotalDiscountPackage;

                                                responsePackageModel.Total = (responsePackageModel.PackageTotal + responsePackageModel.Total);
                                                responsePackageModel.lstPaymentPackage = lstProductInventories;
                                            }
                                        }
                                        #endregion

                                        responsePackageModel.PrePaidRate = 0.5;
                                        responsePackageModel.PrepaidValue = responsePackageModel.PrePaidRate * responsePackageModel.Total;

                                        //***UPDATE CHARTERING PRICING
                                        var newChartering = _aqYachtContext.YachtCharterings.FirstOrDefault(x => x.Id == charteringModelId);
                                        if (newChartering != null)
                                        {
                                            newChartering.HaveAdditionalServices = isPackageAddition;
                                            newChartering.PrepaidRate = responsePackageModel.PrePaidRate;
                                            newChartering.PrepaidValue = responsePackageModel.PrepaidValue;
                                            newChartering.GrandTotalValue = responsePackageModel.Total - responsePackageModel.DiscountTotal;
                                            newChartering.DiscountedValue = responsePackageModel.DiscountTotal;
                                            newChartering.OriginalValue = responsePackageModel.Total;

                                            _aqYachtContext.SaveChanges();
                                        }

                                        //****INSERT TO PAYMENTLOGS TABLE
                                        YachtCharteringPaymentLogs paymentLogs = new YachtCharteringPaymentLogs();
                                        paymentLogs.CharteringFid = charteringModelId;

                                        //call api payment from Mr Long
                                        paymentLogs.PaymentBy = "";
                                        paymentLogs.PaymentRef = "";
                                        paymentLogs.PaymentMethod = PaymentMethod;

                                        paymentLogs.PaymentDate = DateTime.Now;
                                        paymentLogs.PaymentAmount = responsePackageModel.PrepaidValue;
                                        paymentLogs.CultureCode = yachtCultureCode;
                                        paymentLogs.CurrencyCode = yachtCurrencyCode;
                                        paymentLogs.StatusFid = Convert.ToInt32(YachtCharterStatusEnum.Waiting);//wating for payment
                                        _aqYachtContext.YachtCharteringPaymentLogs.Add(paymentLogs);
                                        _aqYachtContext.SaveChanges();
                                        #endregion

                                        ErrorCode = "1";
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorCode = "-1";
                                        ErrorDescription = ex.Message.ToString();
                                    }
                                    dataSubjectLogging += $"ErrorCode:{ErrorCode },";
                                    dataSubjectLogging += $"ErrorDescription:{ErrorDescription }";
                                    dataSubjectLogging += $"ErrorDetail:{ErrorGlobalPackageDescription }";
                                    dataSubjectLogging += "}";
                                }
                                errGlobal.Value = dataSubjectLogging;
                            }
                        }
                    }
                    #endregion
                }

                #region logging  
                errCode = "1";
                errGlobal.ResuldCode = errCode;
                #endregion

                return BaseResponse<SaveCharterPaymentResponseViewModel>.Success(errGlobal);
            }
            catch (Exception ex)
            {
                errCode = "-1";
                errGlobal.ResuldCode = errCode;
                errGlobal.Describes = ex.Message.ToString();
                return BaseResponse<SaveCharterPaymentResponseViewModel>.InternalServerError(errGlobal, ex.Message);
            }
        }


        #endregion
    }
}
