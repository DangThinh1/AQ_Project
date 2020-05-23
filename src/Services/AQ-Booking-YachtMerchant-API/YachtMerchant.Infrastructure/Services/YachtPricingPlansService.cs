using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.Helpers;
using AQBooking.Core.Models;
using AutoMapper;
using YachtMerchant.Core.Models.YachtPricingPlan;
using YachtMerchant.Infrastructure.Interfaces;
using AQBooking.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using Identity.Core.Helpers;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtPricingPlansService : ServiceBase, IYachtPricingPlansService
    {
        private readonly IWorkContext _workContext;
        private readonly IMapper _mapper;
        public YachtPricingPlansService(IWorkContext workContext,
            IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _workContext = workContext;
            _mapper = mapper;
        }
        public async Task<BaseResponse<bool>> CreateYachtPricingPlans(YachtPricingPlanCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest(false);

                YachtPricingPlans yachtPricing = new YachtPricingPlans();
                yachtPricing = _mapper.Map<YachtPricingPlans>(model);
                yachtPricing.Deleted = model.Deleted;
                yachtPricing.PricingCategoryFid = model.PricingCategoryFid;
                yachtPricing.PricingCategoryResKey = model.PricingCategoryResKey;
                yachtPricing.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                yachtPricing.CreatedBy = new Guid("B7A7A1E2-7F34-43E2-BE21-746F2271ECEC");
                yachtPricing.CreatedDate = DateTime.Now;
                yachtPricing.LastModifiedBy = new Guid("B7A7A1E2-7F34-43E2-BE21-746F2271ECEC");
                yachtPricing.LastModifiedDate = DateTime.Now;
                yachtPricing.IsActivated = model.IsActivated;
                await _context.YachtPricingPlans.AddAsync(yachtPricing);
                await _context.SaveChangesAsync();

                foreach (var item in model.lstPricingPlanDetail)
                {
                    YachtPricingPlanDetails yachtPricingDetail = new YachtPricingPlanDetails();
                    yachtPricingDetail = _mapper.Map<YachtPricingPlanDetails>(item);
                    yachtPricingDetail.PricingPlanFid = yachtPricing.Id;
                    yachtPricingDetail.Deleted = false;
                    //yachtPricingDetail.CreatedBy = GetUserGuidId();
                    yachtPricingDetail.CreatedDate = DateTime.Now;
                    //yachtPricingDetail.LastModifiedBy = GetUserGuidId();
                    yachtPricingDetail.LastModifiedDate = DateTime.Now;
                    yachtPricingDetail.CultureCode = model.CultureCode;
                    yachtPricingDetail.CurrencyCode = model.CurrencyCode;
                    await _context.YachtPricingPlanDetails.AddAsync(yachtPricingDetail);
                    await _context.SaveChangesAsync();
                }
                YachtPricingPlanInformations yachtPricingInfo = new YachtPricingPlanInformations();
                yachtPricingInfo = _mapper.Map<YachtPricingPlanInformations>(model);
                yachtPricingInfo.PricingPlanFid = yachtPricing.Id;
                yachtPricingInfo.Deleted = false;
                yachtPricingInfo.LastModifiedBy = new Guid("B7A7A1E2-7F34-43E2-BE21-746F2271ECEC");
                yachtPricingInfo.LastModifiedDate = DateTime.Now;
                await _context.YachtPricingPlanInformations.AddAsync(yachtPricingInfo);
                await _context.SaveChangesAsync();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteYachtPricingPlans(int Id)
        {
            try
            {
                if (Id == 0 && Id < 0)
                    return BaseResponse<bool>.BadRequest(false);

                YachtPricingPlanViewModel yachtPricingView = new YachtPricingPlanViewModel();
                YachtPricingPlans yachtPricingPlans = _context.YachtPricingPlans.AsNoTracking().FirstOrDefault(x => x.Id == Id);
                yachtPricingPlans.Deleted = true;
                _context.Update(yachtPricingPlans);
                _context.SaveChanges();
                List<YachtPricingPlanDetails> yachtPricingPlanDetails = _context.YachtPricingPlanDetails.AsNoTracking().Where(x => x.PricingPlanFid == yachtPricingPlans.Id).ToList();
                foreach (var item in yachtPricingPlanDetails)
                {
                    item.Deleted = true;
                    _context.Update(item);
                    _context.SaveChanges();
                }
                YachtPricingPlanInformations yachtPricingPlanInfo = _context.YachtPricingPlanInformations.AsNoTracking().FirstOrDefault(x => x.PricingPlanFid == yachtPricingPlans.Id);
                yachtPricingPlanInfo.Deleted = true;
                _context.Update(yachtPricingPlanInfo);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtPricingPlanViewModel> GetById(int Id)
        {
            try
            {
                if (Id > 0)
                {
                    var lstDetail = _context.YachtPricingPlanDetails.AsNoTracking().Where(x => x.Deleted == false && x.PricingPlanFid == Id).Select(x => _mapper.Map<YachtPricingPlanDetailCreateModel>(x)).ToList();
                    var query = (from pricing in _context.YachtPricingPlans.AsNoTracking().Where(x => x.Deleted == false && x.Id == Id)
                                 join detail in _context.YachtPricingPlanDetails.AsNoTracking().Where(x => x.Deleted == false && x.PricingPlanFid == Id) on pricing.Id equals detail.PricingPlanFid into details
                                 join info in _context.YachtPricingPlanInformations.AsNoTracking().Where(x => x.Deleted == false && x.PricingPlanFid == Id) on pricing.Id equals info.PricingPlanFid
                                 select new YachtPricingPlanViewModel
                                 {
                                     Id = pricing.Id,
                                     YachtFid = pricing.YachtFid,
                                     PricingCategoryFid = pricing.PricingCategoryFid,
                                     PricingCategoryResKey = pricing.PricingCategoryResKey,
                                     EffectiveDate = pricing.EffectiveDate,
                                     EffectiveEndDate = pricing.EffectiveEndDate.HasValue ? pricing.EffectiveEndDate.Value : DateTime.Now,
                                     IsRecurring = pricing.IsRecurring,
                                     PlanName = pricing.PlanName,
                                     BasedPortLocation = pricing.BasedPortLocation,
                                     YachtPortFid = int.Parse(pricing.YachtPortFid.ToString()),
                                     YachtPortName = pricing.YachtPortName,
                                     Remark = pricing.Remark,
                                     Deleted = pricing.Deleted,
                                     IsActivated = pricing.IsActivated,
                                     PricingTypeFid = details.FirstOrDefault().PricingTypeFid,
                                     PricingTypeResKey = details.FirstOrDefault().PricingTypeResKey,
                                     ContactOwner = details.FirstOrDefault().ContactOwner,
                                     Price = details.FirstOrDefault().Price,
                                     CurrencyCode = details.FirstOrDefault().CurrencyCode,
                                     CultureCode = details.FirstOrDefault().CultureCode,
                                     LanguageFid = info.LanguageFid,
                                     yachtPricingDetail = lstDetail,
                                     PackageInfo = info.PackageInfo
                                 }).FirstOrDefault();

                    return BaseResponse<YachtPricingPlanViewModel>.Success(query);
                }
                return BaseResponse<YachtPricingPlanViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtPricingPlanViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtPricingPlanViewModel>> SearchYachtPricingPlan(YachtPricingPlanSearchModel searchModel)
        {
            try
            {

                DateTime validValue;
                var sortString = !string.IsNullOrEmpty(searchModel.SortString) ? searchModel.SortString : "EffectiveDate DESC";
                searchModel.YachtFID = searchModel.YachtFID != 0 ? searchModel.YachtFID : 1;
                var result = (from pricing in _context.YachtPricingPlans.AsNoTracking().Where(x => x.Deleted == false)
                              where (pricing.PlanName.Contains(searchModel.SearchString) || string.IsNullOrEmpty(searchModel.SearchString))
                              && (!string.IsNullOrEmpty(searchModel.EffectiveDateFrom.ToString()) || pricing.EffectiveDate >= searchModel.EffectiveDateFrom)
                              && (!string.IsNullOrEmpty(searchModel.EffectiveDateTo.ToString()) || pricing.EffectiveDate <= searchModel.EffectiveDateTo)
                              && (!searchModel.EffectiveEndDateFrom.HasValue || pricing.EffectiveEndDate >= searchModel.EffectiveEndDateFrom)
                              && (!searchModel.EffectiveEndDateTo.HasValue || pricing.EffectiveEndDate <= searchModel.EffectiveEndDateTo)
                              && (pricing.YachtFid == searchModel.YachtFID)
                              select new YachtPricingPlanViewModel
                              {
                                  Id = pricing.Id,
                                  YachtFid = pricing.YachtFid,
                                  PricingCategoryFid = pricing.PricingCategoryFid,
                                  PricingCategoryResKey = pricing.PricingCategoryResKey,
                                  EffectiveDate = pricing.EffectiveDate,
                                  EffectiveEndDate = pricing.EffectiveEndDate.Value,
                                  IsRecurring = pricing.IsRecurring,
                                  PlanName = pricing.PlanName,
                                  BasedPortLocation = pricing.BasedPortLocation,
                                  YachtPortFid = int.Parse(pricing.YachtPortFid.ToString()),
                                  YachtPortName = pricing.YachtPortName,
                                  Remark = pricing.Remark,
                                  Deleted = pricing.Deleted,
                                  IsActivated = pricing.IsActivated,
                                  yachtPricingInfo = _context.YachtPricingPlanInformations.AsNoTracking().Where(x => x.PricingPlanFid == pricing.Id).Select(x => _mapper.Map<YachtPricingPlanInformationCreateModel>(x)).ToList(),
                                  yachtPricingDetail = _context.YachtPricingPlanDetails.AsNoTracking().Where(x => x.PricingPlanFid == pricing.Id).Select(x => _mapper.Map<YachtPricingPlanDetails, YachtPricingPlanDetailCreateModel>(x)).ToList(),
                              }).OrderBy(sortString);
                if (result != null)
                    return BaseResponse<PagedList<YachtPricingPlanViewModel>>.Success(new PagedList<YachtPricingPlanViewModel>(result, searchModel.PageIndex, searchModel.PageSize));
                return BaseResponse<PagedList<YachtPricingPlanViewModel>>.Success(new PagedList<YachtPricingPlanViewModel>());
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtPricingPlanViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateYachtPricingPlans(YachtPricingPlanCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                int updatedInfo = 0;
                int resultUpdatedDetail = 0;
                YachtPricingPlans yachtPricing = _context.YachtPricingPlans.AsNoTracking().FirstOrDefault(x => x.Id == model.Id && x.YachtFid == model.YachtFid);
                yachtPricing.YachtFid = model.YachtFid;
                yachtPricing.PricingCategoryFid = model.PricingCategoryFid;
                yachtPricing.PricingCategoryResKey = model.PricingCategoryResKey;
                yachtPricing.EffectiveDate = model.EffectiveDate;
                yachtPricing.EffectiveEndDate = model.EffectiveEndDate;
                yachtPricing.IsRecurring = model.IsRecurring;
                yachtPricing.PlanName = model.PlanName;
                yachtPricing.BasedPortLocation = model.BasedPortLocation;
                yachtPricing.YachtPortFid = model.YachtPortFid;
                yachtPricing.YachtPortName = model.YachtPortName;
                yachtPricing.Remark = model.Remark;
                yachtPricing.Deleted = model.Deleted;
                yachtPricing.IsActivated = model.IsActivated;
                _context.Update(yachtPricing);
                int resultUpdated = _context.SaveChanges();

                if (resultUpdated != 0)
                {
                    YachtPricingPlanInformations yachtPricingInfo = _context.YachtPricingPlanInformations.AsNoTracking().FirstOrDefault(x => x.PricingPlanFid == model.Id);
                    yachtPricingInfo.LanguageFid = model.LanguageFid;
                    yachtPricingInfo.PackageInfo = model.PackageInfo;
                    yachtPricingInfo.Remark = model.Remark;
                    yachtPricingInfo.Deleted = model.Deleted;
                    yachtPricingInfo.IsActivated = model.IsActivated;
                    _context.YachtPricingPlanInformations.Update(yachtPricingInfo);
                    updatedInfo = _context.SaveChanges();
                    if (updatedInfo != 0)
                    {
                        List<YachtPricingPlanDetails> yachtPricingDetails = _context.YachtPricingPlanDetails.AsNoTracking().Where(x => x.PricingPlanFid == model.Id).ToList();
                        for (int i = 0; i < yachtPricingDetails.Count(); i++)
                        {
                            yachtPricingDetails[i].PricingTypeFid = model.lstPricingPlanDetail[i].PricingTypeFid;
                            yachtPricingDetails[i].PricingTypeResKey = model.lstPricingPlanDetail[i].PricingTypeResKey;
                            yachtPricingDetails[i].ContactOwner = model.lstPricingPlanDetail[i].ContactOwner;
                            yachtPricingDetails[i].Price = model.lstPricingPlanDetail[i].Price;
                            yachtPricingDetails[i].CurrencyCode = model.CurrencyCode;
                            yachtPricingDetails[i].CultureCode = model.CultureCode;
                            yachtPricingDetails[i].Deleted = model.lstPricingPlanDetail[i].Deleted;
                            _context.YachtPricingPlanDetails.Update(yachtPricingDetails[i]);
                        }
                        resultUpdatedDetail = _context.SaveChanges();
                    }
                }
                if (resultUpdatedDetail != 0)
                    return BaseResponse<bool>.Success(true, "Success", "MESSAGE_UPDATESUCCESS");
                return BaseResponse<bool>.NoContent(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
