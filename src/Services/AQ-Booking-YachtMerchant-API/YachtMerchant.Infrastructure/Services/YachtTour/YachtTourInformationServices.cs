using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AutoMapper;
using ExtendedUtility;
using Identity.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Core.Models.YachtTourInformations;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using Identity.Core.Helpers;
using AQConfigurations.Core.Services.Interfaces;
using YachtMerchant.Core.Enum;
using AQConfigurations.Core.Models.PortalLanguages;
using AQConfigurations.Core.Models.CommonLanguages;
using YachtMerchant.Core.Common;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourInformationServices : IYachtTourInformationServices
    {
        private readonly IMapper _mapper;
        private readonly Guid _userId = new Guid();
        private YachtOperatorDbContext _db;
        private readonly ICommonLanguagesRequestServices _commonLanguageService;
        private readonly IPortalLanguageRequestService _portalLanguageRequestService;
        private readonly ICommonValueRequestService _commonValueService;
        private List<CommonLanguagesViewModel> _languages;
        public YachtTourInformationServices(YachtOperatorDbContext db, IMapper mapper,
            ICommonLanguagesRequestServices commonLanguageService,
            IPortalLanguageRequestService portalLanguageRequestService,
            ICommonValueRequestService commonValueService)
        {
            _db = db;
            _mapper = mapper;
            _userId = UserContextHelper.UserId;
            _commonLanguageService = commonLanguageService;
            _portalLanguageRequestService = portalLanguageRequestService;
            _commonValueService = commonValueService;
            LoadLanguages();
        }
        private void LoadLanguages()
        {
            try
            {
                _languages = _commonLanguageService.GetAllCommonValue().ResponseData;
            }
            catch
            {
                _languages = new List<CommonLanguagesViewModel>();
            }
        }
        public BaseResponse<bool> Create(YachtTourInformationCreateModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (model == null)
                        return BaseResponse<bool>.BadRequest(false);
                    var rs = _commonValueService.GetListCommonValueByGroup(CommonValueGroupConstant.TourInfoType);
                  
                    DateTime? activetdDate = null;
                    if (model.ActivatedDate.HasValue)
                        activetdDate = model.ActivatedDate.Value.Date;
                    var date = DateTime.Now.Date;
                  
                    var info = new YachtTourInformations();
                    info.InjectFrom(model);
                    info.TourInformationTypeResKey = rs.IsSuccessStatusCode ?
                        rs.ResponseData.FirstOrDefault(x => x.ValueInt == model.TourInformationTypeFid).ResourceKey : null;
                    info.DefaultTitle = model.Title;
                    info.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    info.LastModifiedBy = _userId;
                    info.LastModifiedDate = date;
                    info.IsActivated = true;
                    info.ActivatedBy = _userId;
                    info.Deleted = false;
                    _db.YachtTourInformations.Add(info);
                    _db.SaveChanges();

                    var detail = new YachtTourInformationDetails();
                    detail.InjectFrom(model);
                    detail.InformationFid = info.Id;
                    detail.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    detail.ActivatedBy = _userId;
                    detail.LastModifiedBy = _userId;
                    detail.LastModifiedDate = date;
                    detail.IsActivated = true;
                    detail.Deleted = false;
                    _db.YachtTourInformationDetails.Add(detail);
                    _db.SaveChanges();

                    transaction.Commit();
                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
                }
            }
        }

        public BaseResponse<bool> IsActivated(int id, bool value)
        {
            try
            {
                var check = _db.YachtTourInformations.Any(x => x.Id == id && !x.Deleted && x.IsActivated == value);
                if (check)
                    return BaseResponse<bool>.BadRequest();
                var entity = _db.YachtTourInformations.FirstOrDefault(x => x.Id == id && x.Deleted == false);
                if (entity == null)
                    return BaseResponse<bool>.NotFound();

                entity.IsActivated = value;
                entity.LastModifiedBy = _userId;
                entity.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                DateTime date = DateTime.Now;
                var res = _db.YachtTourInformations.FirstOrDefault(r => r.Deleted == false && r.Id == id);
                if (res == null)
                    return BaseResponse<bool>.NotFound();

                res.Deleted = true;
                res.LastModifiedBy = _userId;
                res.LastModifiedDate = date;

                var details = _db.YachtTourInformationDetails.Where(x => x.InformationFid == id).ToList();
                if(details.Count > 0)
                {
                    foreach(var item in details)
                    {
                        item.Deleted = true;
                        item.LastModifiedBy = _userId;
                        item.LastModifiedDate = date;
                    }
                } _db.SaveChanges();

                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtTourInformationViewModel>> Search(YachtTourInformationSearchModel model)
        {
            try
            {
               
                var sortString = !string.IsNullOrEmpty(model.SortString)
               ? model.SortString
               : "ActivatedDate DESC";
                bool? isactive = model.IsActivated.ToNullBoolean();
                DateTime? activatedDate = null;
                if (!string.IsNullOrEmpty(model.ActivatedDate))
                    activatedDate = model.ActivatedDate.ToNullDateTime();
                var query = (from i in _db.YachtTourInformations
                             .Where(k => !k.Deleted && k.TourFid == model.TourFid
                             && (string.IsNullOrEmpty(model.Title) || k.DefaultTitle.Contains(model.Title))
                             && (model.TourInformationTypeFid == 0 || k.TourInformationTypeFid.Equals(model.TourInformationTypeFid))
                             && (model.IsActivated == null || k.IsActivated == isactive)
                             && (activatedDate == null || k.ActivatedDate.Value.Date == activatedDate.Value.Date))
                             select new YachtTourInformationViewModel()
                             {
                                 Id = i.Id,
                                 DefaultTitle = i.DefaultTitle,
                                 TourInformationTypeResKey = i.TourInformationTypeResKey,
                                 IsActivated = i.IsActivated,
                                 ActivatedDate = i.ActivatedDate,
                                 LanguagesSupported = (from d in _db.YachtTourInformationDetails 
                                                       join l in _languages on d.LanguageFid equals l.Id
                                                       where !d.Deleted && d.InformationFid == i.Id
                                                       select l.ResourceKey).ToList()
                             }).OrderBy(sortString);
                var result = new PagedList<YachtTourInformationViewModel>(query, model.PageIndex, model.PageSize);
                if(result != null)
                    return BaseResponse<PagedList<YachtTourInformationViewModel>>.Success(result);
                return BaseResponse<PagedList<YachtTourInformationViewModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<YachtTourInformationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateDetail(YachtTourInformationUpdateDetailModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest(false);

                var entity = _db.YachtTourInformationDetails.FirstOrDefault(e => e.Deleted == false && e.Id == model.Id);

                var entityInfo = _db.YachtTourInformations.FirstOrDefault(x => x.Id == model.InformationFid);
                if (model.LanguageFid == 1)
                {
                    entityInfo.DefaultTitle = model.Title;
                    entityInfo.ActivatedDate = model.ActivatedDate.Value.Date;
                }
                  
                if (model.ActivatedDate.HasValue)
                {
                    entity.ActivatedDate = model.ActivatedDate.Value.Date;
                }

                entity.Title = model.Title;
                entity.ShortDescriptions = model.ShortDescriptions;
                entity.FullDescriptions = model.FullDescriptions;
                entity.LastModifiedBy = _userId;
                entity.LastModifiedDate = DateTime.Now;
                if (model.FileStreamFid > 0)
                    entity.FileStreamFid = model.FileStreamFid;
                if (model.FileTypeFid > 0)
                    entity.FileTypeFid = model.FileTypeFid;
                _db.YachtTourInformationDetails.Update(entity);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> CreateDetail(YachtTourInformationUpdateDetailModel model)
        {
            try
            {
                DateTime? activetdDate = null;
                if (model.ActivatedDate.HasValue)
                    activetdDate = model.ActivatedDate.Value.Date;

                if (model == null)
                    return BaseResponse<bool>.BadRequest(false);

                var entity = new YachtTourInformationDetails();
                entity.InjectFrom(model);
                entity.InformationFid = model.InformationFid;
                entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                entity.ActivatedBy = _userId;
                entity.LastModifiedBy = _userId;
                entity.LastModifiedDate = DateTime.Now;
                entity.ActivatedDate = activetdDate;
                entity.IsActivated = true;
                entity.Deleted = false;

                _db.YachtTourInformationDetails.Add(entity);
                _db.SaveChangesAsync();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtTourInformationUpdateDetailModel> FindInfoDetailById(long id)
        {
            try
            {
                var entity = _db.YachtTourInformationDetails.AsNoTracking()
                    .FirstOrDefault(e => e.Deleted == false && e.Id == id);
                if (entity == null)
                    return BaseResponse<YachtTourInformationUpdateDetailModel>.BadRequest();
                var model = _mapper.Map<YachtTourInformationDetails, YachtTourInformationUpdateDetailModel>(entity);
                return BaseResponse<YachtTourInformationUpdateDetailModel>.Success(model);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourInformationUpdateDetailModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourInformationSupportModel>> GetInfoDetailSupportedByInfoId(int infoId)
        {
            try
            {
                var domainPortalFid = (int)AQPortalEnum.YachtMerchantPortal;
                var portalLanguageResponse = _portalLanguageRequestService.GetLanguagesByDomainId(domainPortalFid);
                var portalLanguage = new List<PortalLanguageViewModel>();
                if (portalLanguageResponse.IsSuccessStatusCode)
                    portalLanguage = portalLanguageResponse.ResponseData.Where(k => k.DomainPortalFid == domainPortalFid).ToList();
               
                var informationDetails = _db.YachtTourInformationDetails.Where(k => k.InformationFid == infoId).ToList();

                var languageSupportedList = (from c in portalLanguage
                                             join d in _languages on c.LanguageFid equals d.Id
                                             join e in informationDetails on c.LanguageFid equals e.LanguageFid into g
                                             from f in g.DefaultIfEmpty()
                                             select new YachtTourInformationSupportModel()
                                             {
                                                 Id = (f != null && f.Id > 0) ? f.Id : 0,
                                                 InfoFid = infoId,
                                                 LanguageFid = c.LanguageFid,
                                                 FileStreamFid = (f != null && f.Id > 0) ? f.FileStreamFid : 0,
                                                 LanguageName = c.LanguageName,
                                                 ResourceKey = d.ResourceKey,
                                                 Supported = (f != null && f.Id > 0) ? true : false
                                             }).OrderBy(k => k.LanguageFid).ToList();
                if (languageSupportedList.Count > 0)
                    return BaseResponse<List<YachtTourInformationSupportModel>>.Success(languageSupportedList);
                return BaseResponse<List<YachtTourInformationSupportModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourInformationSupportModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}