using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelInformationDetails;
using AccommodationMerchant.Core.Models.HotelInformations;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Models.CommonLanguages;
using AQConfigurations.Core.Models.PortalLanguages;
using AQConfigurations.Core.Services.Interfaces;
using AutoMapper;
using ExtendedUtility;
using Identity.Core.Helpers;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using HotelInformationDetailViewModel = AccommodationMerchant.Core.Models.HotelInformations.HotelInformationDetailViewModel;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelInformationService : ServiceBase, IHotelInformationService
    {
        private readonly IHotelInformationDetailService _hotelInformationDetailService;
        private readonly ICommonLanguagesRequestServices _commonLanguageService;
        private readonly IPortalLanguageRequestService _portalLanguageRequestService;
        private List<CommonLanguagesViewModel> _languages;

        public HotelInformationService(AccommodationContext db, IMapper mapper,
            IHotelInformationDetailService hotelInformationDetailService, ICommonLanguagesRequestServices commonLanguageService,
            IPortalLanguageRequestService portalLanguageRequestService) : base(db, mapper)
        {
            _hotelInformationDetailService = hotelInformationDetailService;
            _commonLanguageService = commonLanguageService;
            _portalLanguageRequestService = portalLanguageRequestService;
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

        public BaseResponse<bool> Create(HotelInformationCreateModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (model != null)
                    {
                        //Create Info
                        var entity = _mapper.Map<HotelInformationCreateModel, HotelInformations>(model);
                        entity.DefaultTitle = model.Title;
                        entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                        entity.Deleted = false;
                        entity.IsActivated = false;
                        entity.ActivatedBy = UserContextHelper.UserId;
                        entity.LastModifiedDate = DateTime.Now.Date;
                        entity.LastModifiedBy = UserContextHelper.UserId;
                        _db.HotelInformations.Add(entity);
                        _db.SaveChanges();

                        //Create Info Details
                        var detailsCreateModel = new HotelInformationDetailCreateModel();
                        detailsCreateModel.InjectFrom(model);
                        detailsCreateModel.ActivatedDate = model.ActivatedDate.ToDateTime();
                        detailsCreateModel.InformationFid = entity.Id;
                        _hotelInformationDetailService.Create(detailsCreateModel);
                        transaction.Commit();
                        return BaseResponse<bool>.Success(true);
                    }
                    return BaseResponse<bool>.BadRequest();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BaseResponse<bool>.InternalServerError(ex);
                }
            }
        }

        public BaseResponse<bool> Update(HotelInformationUpdateModel model)
        {
            try
            {
                if (model != null)
                {
                    var entity = _mapper.Map<HotelInformationUpdateModel, HotelInformations>(model);
                    entity.ActivatedDate = DateTimeHelper.ToDateTimeNullable(model.ActivatedDate);
                    entity.Deleted = false;
                    entity.IsActivated = false;
                    entity.LastModifiedDate = DateTime.Now.Date;
                    entity.LastModifiedBy = UserContextHelper.UserId;
                    _db.HotelInformations.Add(entity);
                    _db.SaveChanges();
                    return BaseResponse<bool>.Success();
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> IsActivated(int id, bool value)
        {
            try
            {
                var entity = _db.HotelInformations.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (entity != null)
                {
                    entity.IsActivated = value;
                    entity.ActivatedBy = UserContextHelper.UserId;
                    entity.LastModifiedBy = UserContextHelper.UserId;
                    entity.LastModifiedDate = DateTime.Now.Date;
                    _db.HotelInformations.Update(entity);
                    _db.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var entity = _db.HotelInformations.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.LastModifiedBy = UserContextHelper.UserId;
                    entity.LastModifiedDate = DateTime.Now.Date;
                    _db.HotelInformations.Update(entity);
                    _db.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<HotelInformationDetailViewModel> Find(int id)
        {
            try
            {
                var entity = _db.HotelInformationDetails.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (entity != null)
                {
                    var model = _mapper.Map<HotelInformationDetails, HotelInformationDetailViewModel>(entity);
                    return BaseResponse<HotelInformationDetailViewModel>.Success(model);
                }
                return BaseResponse<HotelInformationDetailViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<HotelInformationDetailViewModel>.InternalServerError(ex);
            }
        }

        public BaseResponse<PagedList<HotelInformationViewModel>> Search(HotelInformationSearchModel model)
        {
            try
            {
                var hotelFid = Decrypt.DecryptToInt32(model.HotelFid);
                if (model != null)
                {
                    var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                    var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                    var sortColumn = !string.IsNullOrEmpty(model.SortColumn) ? model.SortColumn : "Id";
                    var sortType = !string.IsNullOrEmpty(model.SortType) ? model.SortType : "ASC";
                    var sortString = $"{sortColumn} {sortType}";
                    bool? isactive = model.IsActivated.ToNullBoolean();
                    DateTime? activatedDate = null;
                    if (!string.IsNullOrEmpty(model.ActivatedDate))
                        activatedDate = model.ActivatedDate.ToNullDateTime();

                    var query = (from i in _db.HotelInformations
                                 .Where(k => !k.Deleted && k.HotelFid == hotelFid
                                 && (string.IsNullOrEmpty(model.Title) || k.DefaultTitle.Contains(model.Title))
                                 && (model.IsActivated == null || k.IsActivated == isactive)
                                 && (activatedDate == null || k.ActivatedDate.Value.Date == activatedDate.Value.Date))
                                 select new HotelInformationViewModel()
                                 {
                                     Id = i.Id,
                                     DefaultTitle = i.DefaultTitle,
                                     IsActivated = i.IsActivated,
                                     ActivatedDate = i.ActivatedDate,
                                     LanguagesSupported = (from d in _db.HotelInformationDetails
                                                           orderby d.LanguageFid
                                                           join l in _languages on d.LanguageFid equals l.Id
                                                           where !d.Deleted && d.InformationFid == i.Id
                                                           select l.ResourceKey).ToList()
                                 }).OrderByDescending(x=>x.ActivatedDate);

                    var result = new PagedList<HotelInformationViewModel>(query, pageIndex, pageSize);
                    return BaseResponse<PagedList<HotelInformationViewModel>>.Success(result);
                }
                return BaseResponse<PagedList<HotelInformationViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<HotelInformationViewModel>>.InternalServerError(ex);
            }
        }

        public BaseResponse<List<HotelInformationSupportModel>> GetInfoDetailSupportedByInfoId(int infoId)
        {
            try
            {
                //AccommodationMerchantPortal = 4
                var domainPortalFid = 3;
                var portalLanguageResponse = _portalLanguageRequestService.GetLanguagesByDomainId(domainPortalFid);
                var portalLanguage = new List<PortalLanguageViewModel>();
                if (portalLanguageResponse.IsSuccessStatusCode)
                    portalLanguage = portalLanguageResponse.ResponseData.Where(k => k.DomainPortalFid == domainPortalFid).ToList();

                var informationDetails = _db.HotelInformationDetails.Where(k => k.InformationFid == infoId).ToList();

                var languageSupportedList = (from c in portalLanguage
                                             join d in _languages on c.LanguageFid equals d.Id
                                             join e in informationDetails on c.LanguageFid equals e.LanguageFid into g
                                             from f in g.DefaultIfEmpty()
                                             select new HotelInformationSupportModel()
                                             {
                                                 Id = (f != null && f.Id > 0) ? f.Id : 0,
                                                 LanguageFid = c.LanguageFid,
                                                 FileStreamFid = (f != null && f.Id > 0) ? f.FileStreamFid : 0,
                                                 LanguageName = c.LanguageName,
                                                 ResourceKey = d.ResourceKey,
                                                 Supported = (f != null && f.Id > 0) ? true : false,
                                                 InfoFid = infoId
                                             }).OrderBy(k => k.LanguageFid).ToList();

                return BaseResponse<List<HotelInformationSupportModel>>.Success(languageSupportedList);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<HotelInformationSupportModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}