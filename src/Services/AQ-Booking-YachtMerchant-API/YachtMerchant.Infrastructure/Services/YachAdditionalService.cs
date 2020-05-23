using APIHelpers.Response;
using AQBooking.Core.Helpers;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Models.YachAdditionalServices;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachAdditionalService : ServiceBase, IYachAdditionalService
    {
        public YachAdditionalService(YachtOperatorDbContext context) : base(context)
        {
        }

        #region AdditionalService

       

        public BaseResponse<bool> MapServicesToYacht(List<YachtAdditionalServiceControlCreateModel> creatModels)
        {
            try
            {
                if (creatModels == null)
                    return BaseResponse<bool>.BadRequest();
                var serviceControls = new List<YachtAdditionalServiceControls>();
                creatModels.ForEach((item) =>
                {
                    var now = DateTime.Now.Date;
                    var entity = DefaultYachtAdditionalServiceControls();
                    entity.InjectFrom(item);
                    serviceControls.Add(entity);
                });
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> MapDetailsToService(List<YachtAdditionalServiceDetailCreateModel> creatModels)
        {
            try
            {
                if (creatModels == null)
                    return BaseResponse<bool>.BadRequest();
                var serviceDetails = new List<YachtAdditionalServiceDetails>();
                creatModels.ForEach((item) =>
                {
                    var entity = DefaultYachtAdditionalServiceDetails();
                    entity.InjectFrom(item);
                    serviceDetails.Add(entity);
                });
                _context.YachtAdditionalServiceDetails.AddRange(serviceDetails);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        private YachtAdditionalServices DefaultYachtAdditionalServices() => new YachtAdditionalServices()
        {
            UniqueId = UniqueIDHelper.GenarateRandomString(12),
            Deleted = false,
            IsActive = true,
            CreatedBy = GetUserGuidId(),
            LastModifiedBy = GetUserGuidId(),
            CreatedDate = DateTime.Now.Date,
            LastModifiedDate = DateTime.Now.Date
        };

        private YachtAdditionalServiceControls DefaultYachtAdditionalServiceControls() => new YachtAdditionalServiceControls()
        {
            Deleted = false,
            CreatedBy = GetUserGuidId(),
            LastModifiedBy = GetUserGuidId(),
            CreatedDate = DateTime.Now.Date,
            LastModifiedDate = DateTime.Now.Date
        };

        private YachtAdditionalServiceDetails DefaultYachtAdditionalServiceDetails() => new YachtAdditionalServiceDetails()
        {
            Deleted = false,
            CreatedBy = GetUserGuidId(),
            LastModifiedBy = GetUserGuidId(),
            CreatedDate = DateTime.Now.Date,
            LastModifiedDate = DateTime.Now.Date
        };

        public BaseResponse<PagedList<YachAdditionalServiceViewModel>> SearchYachtAdditionalService(YachAdditionalServiceSearchModel model)
        {
            try
            {
                var query = (from i in _context.YachtAdditionalServices
                             .Where(k => !k.Deleted && k.MerchantFid == model.MerchantFid
                              && (string.IsNullOrEmpty(model.Name) || k.Name.Contains(model.Name)))
                             select new YachAdditionalServiceViewModel()
                             {
                                 Id = i.Id,
                                 Name = i.Name,
                                 AdditonalServiceTypeResKey = i.AdditonalServiceTypeResKey,
                                 Remark = i.Remark,
                                 IsActive = i.IsActive,
                                 ActiveFrom = i.ActiveFrom,
                                 ActiveTo = i.ActiveTo,
                                 CountServiceDetail = _context.YachtAdditionalServiceDetails.Where(x=>x.AdditionalServiceFid == i.Id).Count(),
                                 CountServiceControl = _context.YachtAdditionalServiceControls.Where(x => x.AdditionalServiceFid == i.Id).Count(),
                             }).OrderByDescending(x=>x.Id);

                return BaseResponse<PagedList<YachAdditionalServiceViewModel >>.Success(new PagedList<YachAdditionalServiceViewModel>(query, model.PageIndex, model.PageSize));
            }
            catch
            {
                return BaseResponse<PagedList<YachAdditionalServiceViewModel>>.Success(new PagedList<YachAdditionalServiceViewModel>());
            }
        }
        public BaseResponse<bool> Create(YachAdditionalServiceUpdateModel createModel)
        {
            try
            {
                if (createModel == null)
                    return BaseResponse<bool>.BadRequest();
                var check = CheckDate(createModel.ActiveFrom, createModel.ActiveTo);
                if(check)
                    return BaseResponse<bool>.BadRequest();

                var entity = DefaultYachtAdditionalServices();
                entity.InjectFrom(createModel);
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                _context.YachtAdditionalServices.Add(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(YachAdditionalServiceUpdateModel createModel)
        {
            try
            {
                if (createModel == null)
                    return BaseResponse<bool>.BadRequest();
                var entity = _context.YachtAdditionalServices.FirstOrDefault(x => x.Id == createModel.id);
                if (entity == null)
                    return BaseResponse<bool>.NotFound();
                var check = CheckDate(createModel.ActiveFrom, createModel.ActiveTo);
                if (check)
                    return BaseResponse<bool>.BadRequest();

                entity.AdditonalServiceTypeFid = createModel.AdditonalServiceTypeFid;
                entity.AdditonalServiceTypeResKey = createModel.AdditonalServiceTypeResKey;
                entity.Name = createModel.Name;
                entity.Remark = createModel.Remark;
                entity.ActiveFrom = createModel.ActiveFrom;
                entity.ActiveTo = createModel.ActiveTo;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;

                _context.YachtAdditionalServices.Update(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = _context.YachtAdditionalServices.Find(id);
                    if (result == null)
                        return BaseResponse<bool>.NotFound();
                    var isSingleInfo = !_context.YachtAdditionalServices.
                          Any(r => r.Deleted == false && r.IsActive == true
                          && r.Id != result.Id && r.MerchantFid == result.MerchantFid);

                    if (isSingleInfo)
                    {
                        return BaseResponse<bool>.BadRequest();
                    }
                    result.Deleted = true;
                    result.LastModifiedDate = DateTime.Now;
                    result.LastModifiedBy = GetUserGuidId();

                    var details = _context.YachtAdditionalServiceDetails.Where(x => x.AdditionalServiceFid == id && !x.Deleted).ToList();
                    foreach (var item in details)
                    {
                        item.Deleted = true;
                        item.LastModifiedBy = GetUserGuidId();
                        item.LastModifiedDate = DateTime.Now;
                    }

                    var controls = _context.YachtAdditionalServiceControls.Where(x => x.AdditionalServiceFid == id && !x.Deleted).ToList();
                    foreach (var item in controls)
                    {
                        item.Deleted = true;
                        item.LastModifiedBy = GetUserGuidId();
                        item.LastModifiedDate = DateTime.Now;
                    }
                    _context.SaveChangesAsync().Wait();
                    tran.Commit();
                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return BaseResponse<bool>.BadRequest();
                }
            }
        }

        public BaseResponse<YachAdditionalServiceViewModel> GetAdditionalServiceById(int id)
        {
            var result = _context.YachtAdditionalServices.Where(x => x.Id == id && !x.Deleted)
                        .Select(x => new YachAdditionalServiceViewModel
                        {
                            Id = x.Id,
                            AdditonalServiceTypeResKey = x.AdditonalServiceTypeResKey,
                            AdditonalServiceTypeFid = x.AdditonalServiceTypeFid,
                            Name = x.Name,
                            ActiveFrom = x.ActiveFrom,
                            ActiveTo = x.ActiveTo,
                            Remark = x.Remark
                        }).FirstOrDefault();
            return BaseResponse<YachAdditionalServiceViewModel>.Success(result);
        }

        public BaseResponse<bool> IsActivated(int id, bool value)
        {
            try
            {
                var entity = _context.YachtAdditionalServices.FirstOrDefault(x => x.Id == id && x.Deleted == false);
                if (entity == null)
                {
                    return BaseResponse<bool>.NotFound();
                }
                var isSingleInfo = !_context.YachtAdditionalServices.
                   Any(r => r.Deleted == false && r.IsActive == true
                   && r.Id != entity.Id && r.MerchantFid == entity.MerchantFid);

                if (isSingleInfo)
                {
                    return BaseResponse<bool>.BadRequest();
                }

                entity.IsActive = value;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                _context.SaveChanges();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.BadRequest();
            }
        }

        public BaseResponse<YachtAdditionalServiceDetailViewModel> GetAllAdditionalServiceDetailByAdditionalId(int id)
        {
            var result = _context.YachtAdditionalServiceDetails.Where(x => x.AdditionalServiceFid == id && !x.Deleted)
                           .Select(x => new YachtAdditionalServiceDetailViewModel
                           {
                               AdditionalServiceName = _context.YachtAdditionalServices.FirstOrDefault(m => m.Id == id).Name,
                               AdditionalServiceFid = x.AdditionalServiceFid,
                               ListYachtAdditionalServiceDetail = _context.YachtAdditionalServiceDetails.
                                                     Where(m => m.AdditionalServiceFid == id && !m.Deleted)
                                                    .Select(m => new YachtAdditionalServiceDetailModel
                                                    {
                                                        ProductFid = m.ProductFid,
                                                        ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(v => v.Id == m.ProductFid).ProductName,
                                                        EffectiveDate = m.EffectiveDate,
                                                        EffectiveEndDate = m.EffectiveEndDate,
                                                        Remark = m.Remark
                                                    }).ToList(),
                           }).FirstOrDefault();
            return BaseResponse< YachtAdditionalServiceDetailViewModel>.Success(result);
        }

        #endregion AdditionalService

        private bool CheckDate(DateTime startDate, DateTime? endDate)
        {
            
            if (endDate.HasValue)
            {
                if (endDate.Value.Date < startDate.Date)
                    return  true;
            }    
            return false;
        }
        #region ServiceDetail

        public BaseResponse<bool> CreateServiceDetail(YachtAdditionalServiceDetailViewModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var check = CheckDate(model.EffectiveDate, model.EffectiveEndDate);
                if(check)
                    return BaseResponse<bool>.BadRequest();
                var entity = new YachtAdditionalServiceDetails();
                entity.AdditionalServiceFid = model.AdditionalServiceFid;
                entity.ProductFid = model.ProductFid;
                entity.Remark = model.Remark;
                entity.EffectiveDate = model.EffectiveDate;
                entity.EffectiveEndDate = model.EffectiveEndDate;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                _context.YachtAdditionalServiceDetails.Add(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateServiceDetail(YachtAdditionalServiceDetailViewModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var entity = _context.YachtAdditionalServiceDetails.FirstOrDefault(x => x.AdditionalServiceFid == model.AdditionalServiceFid && x.ProductFid == model.ProductFid);
                if (entity == null)
                    return BaseResponse<bool>.NotFound();
                var check = CheckDate(model.EffectiveDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();

                entity.Remark = model.Remark;
                entity.EffectiveEndDate = model.EffectiveEndDate;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;

                _context.YachtAdditionalServiceDetails.Update(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtAdditionalServiceDetailViewModel> GetServiceDetailByServiceId(int serviceId)
        {
            try
            {
                var entity = _context.YachtAdditionalServiceDetails.Where(x => x.AdditionalServiceFid == serviceId && !x.Deleted)
                            .Select(x => new YachtAdditionalServiceDetailViewModel
                            {
                                AdditionalServiceFid = _context.YachtAdditionalServices.FirstOrDefault(z => z.Id == serviceId && !x.Deleted).AdditonalServiceTypeFid,
                                AdditionalServiceName = _context.YachtAdditionalServices.FirstOrDefault(z => z.Id == serviceId && !x.Deleted).Name,

                                ListYachtAdditionalServiceDetail = _context.YachtAdditionalServiceDetails
                                                         .Where(m => m.Deleted == false && m.AdditionalServiceFid == serviceId)
                                                         .Select(m => new YachtAdditionalServiceDetailModel
                                                         {
                                                             ProductFid = m.ProductFid,
                                                             ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(v => v.Id == m.ProductFid && !x.Deleted).ProductName,
                                                             EffectiveDate = m.EffectiveDate,
                                                             EffectiveEndDate = m.EffectiveEndDate,
                                                             Remark = m.Remark
                                                         }).OrderBy(m => m.EffectiveDate).ThenBy(m => m.EffectiveDate).ToList()
                            }).FirstOrDefault();
                return BaseResponse<YachtAdditionalServiceDetailViewModel >.Success(entity);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtAdditionalServiceDetailViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtAdditionalServiceDetailViewModel> GetDetailServiceDetail(int additionalServiceFid, int productId)
        {
            var result = _context.YachtAdditionalServiceDetails.Where(x => x.AdditionalServiceFid == additionalServiceFid &&
                                                                        x.ProductFid == productId && !x.Deleted)
                     .Select(x => new YachtAdditionalServiceDetailViewModel
                     {
                         AdditionalServiceFid = x.AdditionalServiceFid,
                         AdditionalServiceName = _context.YachtAdditionalServices.FirstOrDefault(z => z.Id == additionalServiceFid && !x.Deleted).Name,

                         ProductFid = x.ProductFid,
                         ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(z => z.Id == productId && !x.Deleted).ProductName,

                         EffectiveDate = x.EffectiveDate,
                         EffectiveEndDate = x.EffectiveEndDate,
                         Remark = x.Remark
                     }).FirstOrDefault();
            return BaseResponse<YachtAdditionalServiceDetailViewModel>.Success( result);
        }
        public BaseResponse<bool> DeleteServiceDetail(int AdditionalServiceId, int ProductId)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = _context.YachtAdditionalServiceDetails.FirstOrDefault(x => x.AdditionalServiceFid == AdditionalServiceId && x.ProductFid == ProductId && !x.Deleted);
                    if (entity == null)
                        return BaseResponse<bool>.BadRequest();

                    entity.Deleted = true;
                    entity.LastModifiedDate = DateTime.Now;
                    entity.LastModifiedBy = GetUserGuidId();
                    _context.YachtAdditionalServiceDetails.Update(entity);

                    _context.SaveChangesAsync().Wait();
                    tran.Commit();
                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return BaseResponse<bool>.BadRequest();
                }
            }
        }

        #endregion ServiceDetail

        #region ServiceControl

        public BaseResponse<bool> CreateServiceControl(YachtAdditionalServiceControlViewModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var check = CheckDate(model.EffectiveDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();

                var entity = new YachtAdditionalServiceControls();
                entity.AdditionalServiceFid = model.AdditionalServiceFid;
                entity.YachtFid = model.YachtFid;
                entity.EffectiveDate = model.EffectiveDate;
                entity.EffectiveEndDate = model.EffectiveEndDate;
                entity.Remark = model.Remark;
                entity.Deleted = false;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                _context.YachtAdditionalServiceControls.Add(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateServiceControl(YachtAdditionalServiceControlViewModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _context.YachtAdditionalServiceControls.FirstOrDefault(x => x.AdditionalServiceFid == model.AdditionalServiceFid && x.YachtFid == model.YachtFid);

                if (entity == null)
                    return BaseResponse<bool>.NotFound();

                var check = CheckDate(model.EffectiveDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();

                entity.Remark = model.Remark;
                entity.EffectiveEndDate = model.EffectiveEndDate;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;

                _context.YachtAdditionalServiceControls.Update(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtAdditionalServiceControlViewModel> GetServiceControlByServiceId(int serviceId)
        {
            try
            {
                var entity = _context.YachtAdditionalServiceControls.Where(x => x.AdditionalServiceFid == serviceId && !x.Deleted)
                            .Select(x => new YachtAdditionalServiceControlViewModel
                            {
                                AdditionalServiceFid = x.AdditionalServiceFid,
                                AdditionalServiceName = _context.YachtAdditionalServices.FirstOrDefault(z => z.Id == serviceId && !x.Deleted).Name,

                                ListServiceControl = _context.YachtAdditionalServiceControls
                                                         .Where(m => m.Deleted == false && m.AdditionalServiceFid == serviceId)
                                                         .Select(m => new YachtAdditionalServiceControlModel
                                                         {
                                                             YachtFid = m.YachtFid,
                                                             YachtName = _context.Yachts.FirstOrDefault(v => v.Id == m.YachtFid && !x.Deleted).Name,
                                                             EffectiveDate = m.EffectiveDate,
                                                             EffectiveEndDate = m.EffectiveEndDate,
                                                             Remark = m.Remark
                                                         }).OrderBy(m => m.EffectiveDate).ThenBy(m => m.EffectiveDate).ToList()
                            }).FirstOrDefault();
                return BaseResponse<YachtAdditionalServiceControlViewModel >.Success(entity);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtAdditionalServiceControlViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtAdditionalServiceControlViewModel> GetDetailServiceControl(int additionalServiceFid, int yachtId)
        {
            var result = _context.YachtAdditionalServiceControls.Where(x => x.AdditionalServiceFid == additionalServiceFid &&
                                                                         x.YachtFid == yachtId && !x.Deleted)
                      .Select(x => new YachtAdditionalServiceControlViewModel
                      {
                          AdditionalServiceFid = x.AdditionalServiceFid,
                          AdditionalServiceName = _context.YachtAdditionalServices.FirstOrDefault(z => z.Id == additionalServiceFid && !x.Deleted).Name,

                          YachtFid = x.YachtFid,
                          YachtName = _context.Yachts.FirstOrDefault(z => z.Id == yachtId && !x.Deleted).Name,

                          EffectiveDate = x.EffectiveDate,
                          EffectiveEndDate = x.EffectiveEndDate,
                          Remark = x.Remark
                      }).FirstOrDefault();
            return BaseResponse< YachtAdditionalServiceControlViewModel>.Success(result);
        }

        public BaseResponse<bool> DeleteServiceControl(int AdditionalServiceId, int YachtId)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = _context.YachtAdditionalServiceControls.FirstOrDefault(x => x.AdditionalServiceFid == AdditionalServiceId && x.YachtFid == YachtId && !x.Deleted);
                    if (entity == null)
                        return BaseResponse<bool>.BadRequest();

                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;
                    _context.YachtAdditionalServiceControls.Update(entity);

                    _context.SaveChangesAsync().Wait();
                    tran.Commit();
                    return BaseResponse<bool>.Success();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return BaseResponse<bool>.BadRequest();
                }
            }
        }

        public BaseResponse<PagedList<YachtAdditionalServiceDetailModel>> AdditionalServiceDetails(int additionalId, YachtAdditionalServiceDetailSearchModel model)
        {
            try
            {
                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var query = (from v in _context.YachtAdditionalServices
                             join s in _context.YachtAdditionalServiceDetails on v.Id equals s.AdditionalServiceFid
                             where s.Deleted == false && v.Deleted == false && s.AdditionalServiceFid == additionalId
                             select new YachtAdditionalServiceDetailModel()
                             {
                                 ProductFid = s.ProductFid,
                                 AdditionalServiceFid = s.AdditionalServiceFid,
                                 ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(x => x.Id == s.ProductFid) != null ?
                                    _context.YachtMerchantProductInventories.FirstOrDefault(x => x.Id == s.ProductFid).ProductName : string.Empty,
                                 EffectiveDate = s.EffectiveDate,
                                 EffectiveEndDate = s.EffectiveEndDate,
                                 Remark = s.Remark
                             }).OrderBy(m => m.EffectiveDate).ThenBy(m => m.EffectiveDate);
                var result = new PagedList<YachtAdditionalServiceDetailModel>(query, pageIndex, pageSize);

                return BaseResponse<PagedList<YachtAdditionalServiceDetailModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtAdditionalServiceDetailModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> CreateAdditionalServiceDetail(YachtAdditionalServiceDetailCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var check = CheckDate(model.EffectiveDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();
                var entity = new YachtAdditionalServiceDetails();
                entity.InjectFrom(model);
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                _context.YachtAdditionalServiceDetails.Add(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        

        public BaseResponse<PagedList<YachtAdditionalServiceControlModel>> AdditionalServiceControls(int additionalId, YachtAdditionalServiceControlSearchModel model)
        {
            try
            {
                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var query = (from v in _context.YachtAdditionalServices
                             join s in _context.YachtAdditionalServiceControls on v.Id equals s.AdditionalServiceFid
                             where s.Deleted == false && v.Deleted == false && s.AdditionalServiceFid == additionalId
                             select new YachtAdditionalServiceControlModel()
                             {
                                 YachtFid = s.YachtFid,
                                 AdditionalServiceFid = s.AdditionalServiceFid,
                                 YachtName = _context.Yachts.FirstOrDefault(x => x.Id == s.YachtFid) != null ?
                                    _context.Yachts.FirstOrDefault(x => x.Id == s.YachtFid).Name : string.Empty,
                                 EffectiveDate = s.EffectiveDate,
                                 EffectiveEndDate = s.EffectiveEndDate,
                                 Remark = s.Remark
                             }).OrderBy(m => m.EffectiveDate).ThenBy(m => m.EffectiveDate);
                var result = new PagedList<YachtAdditionalServiceControlModel>(query, pageIndex, pageSize);

                return BaseResponse<PagedList<YachtAdditionalServiceControlModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtAdditionalServiceControlModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> CreateAdditionalServiceControl(YachtAdditionalServiceControlCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var check = CheckDate(model.EffectiveDate, model.EffectiveEndDate);
                if (check)
                    return BaseResponse<bool>.BadRequest();
                var entity = new YachtAdditionalServiceControls();
                entity.InjectFrom(model);
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                _context.YachtAdditionalServiceControls.Add(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        
        
        #endregion ServiceControl
    }
}