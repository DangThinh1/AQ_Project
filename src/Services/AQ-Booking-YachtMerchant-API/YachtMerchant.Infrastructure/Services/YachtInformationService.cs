using System;
using System.Linq;
using ExtendedUtility;
using Omu.ValueInjecter;
using AQBooking.Core.Models;
using AQBooking.Core.Helpers;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Core.Models.YachtInformation;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Core.Models.YachtInformationDetail;
using AQConfigurations.Core.Services.Interfaces;
using YachtMerchant.Core.Enum;
using AQConfigurations.Core.Models.PortalLanguages;
using AQConfigurations.Core.Models.CommonLanguages;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtInformationService : ServiceBase, IYachtInformationService
    {
        private readonly ICommonLanguagesRequestServices _commonLanguageService;
        private readonly IPortalLanguageRequestService _portalLanguageRequestService;
        private List<CommonLanguagesViewModel> _languages;
        public YachtInformationService(YachtOperatorDbContext dbcontext,
            ICommonLanguagesRequestServices commonLanguages, IPortalLanguageRequestService portalLanguage) : base(dbcontext)
        {
            _portalLanguageRequestService = portalLanguage;
            _commonLanguageService = commonLanguages;
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
        public BaseResponse<bool> Create(YachtInformationCreateModel createModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (createModel == null)
                        return BaseResponse<bool>.BadRequest(false);

                    DateTime? activetdDate = null;
                    if (createModel.ActivatedDate.HasValue)
                        activetdDate = createModel.ActivatedDate.Value.Date;

                    var isExistedInfo = _context.YachtInformations
                        .Any(r => r.Deleted == false
                        && r.YachtFid == createModel.YachtFid
                        && r.ActivatedDate == activetdDate);
                    if (isExistedInfo)
                        return BaseResponse<bool>.BadRequest(false);

                    var userId = GetUserGuidId();
                    // create information
                    var info = new YachtInformations();
                    info.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    info.DefaultTitle = createModel.Title;
                    info.YachtFid = createModel.YachtFid;
                    info.ActivatedBy = userId;
                    info.ActivatedDate = activetdDate;
                    info.LastModifiedBy = userId;
                    info.LastModifiedDate = DateTime.Now;
                    info.IsActivated = true;
                    info.Deleted = false;
                    _context.YachtInformations.Add(info);
                    _context.SaveChanges();

                    // create information detail
                    var detail = new YachtInformationDetails();
                    //detail.InjectFrom(createModel);
                    detail.Title = createModel.Title;
                    detail.FileStreamFid = createModel.FileStreamFID;
                    detail.FileTypeFid = createModel.FileTypeFID;
                    detail.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    detail.InformationFid = info.Id;
                    detail.LanguageFid = createModel.LanguageFid;
                    detail.ActivatedBy = userId;
                    detail.ShortDescriptions = createModel.ShortDescriptions;
                    detail.FullDescriptions = createModel.FullDescriptions;
                    detail.ActivatedDate = activetdDate;
                    detail.LastModifiedBy = userId;
                    detail.LastModifiedDate = DateTime.Now;
                    detail.IsActivated = true;
                    detail.Deleted = false;
                    _context.YachtInformationDetails.Add(detail);
                    _context.SaveChanges();

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

        public BaseResponse<bool> CreateDetail(YachtInformationCreateModel createModel)
        {
            try
            {
                DateTime? activetdDate = null;
                if (createModel.ActivatedDate.HasValue)
                    activetdDate = createModel.ActivatedDate.Value.Date;
                var userId = GetUserGuidId();
                var entity = new YachtInformationDetails();
                entity.InformationFid = createModel.InformationFid;
                entity.FileStreamFid = createModel.FileStreamFID;
                entity.FileTypeFid = createModel.FileTypeFID;
                entity.LanguageFid = createModel.LanguageFid;
                entity.Title = createModel.Title;
                entity.ShortDescriptions = createModel.ShortDescriptions;
                entity.FullDescriptions = createModel.FullDescriptions;
                entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                entity.ActivatedBy = userId; ;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now;
                entity.ActivatedDate = activetdDate;
                entity.IsActivated = true;
                entity.Deleted = false;

                _context.YachtInformationDetails.Add(entity);
                _context.SaveChangesAsync();

                return BaseResponse<bool>.Success(true);
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
                var res = _context.YachtInformations.FirstOrDefault(r => r.Deleted == false && r.Id == id);
                if (res == null)
                {
                    return BaseResponse<bool>.NoContent(false);
                }

                res.Deleted = true;
                res.LastModifiedBy = GetUserGuidId();
                res.LastModifiedDate = DateTime.Now;
                var activeResult = _context.SaveChanges();
                if (activeResult == 1)
                    return BaseResponse<bool>.Success(true);

                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtInformationViewModel> FindById(int id)
        {
            try
            {
                var result = _context.YachtInformations.Find(id);
                if (result != null && result.Deleted == false)
                {
                    var model = new YachtInformationViewModel();
                    model.InjectFrom(result);
                    return BaseResponse< YachtInformationViewModel>.Success(model);
                }
                return BaseResponse<YachtInformationViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtInformationViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtInformationCreateModel> FindInfoDetailById(int id)
        {
            try
            {
                var entity = _context.YachtInformationDetails.AsNoTracking()
                    .FirstOrDefault(e => e.Deleted == false && e.Id == id);
                var viewModel = new YachtInformationCreateModel();
                viewModel.InjectFrom(entity);
                viewModel.FileStreamFID = entity.FileStreamFid;
                viewModel.ResourceKey = _languages.Count >0 ?
                                   _languages.FirstOrDefault(r => r.Id == entity.LanguageFid).ResourceKey : null;
                if (viewModel.ActivatedDate.HasValue)
                    viewModel.ActivatedDate = viewModel.ActivatedDate.Value.Date;
                return BaseResponse < YachtInformationCreateModel >.Success( viewModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtInformationCreateModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
         
        public BaseResponse<List<YachtInformationDetailSupportModel>> GetInfoDetailSupportedByInfoId(int infoId)
        {
            try
            {
                var domainPortalFid = (int)AQPortalEnum.YachtMerchantPortal;
                var portalLanguageResponse = _portalLanguageRequestService.GetLanguagesByDomainId(domainPortalFid);

                var portalLanguage = new List<PortalLanguageViewModel>();
                if (portalLanguageResponse.IsSuccessStatusCode)
                    portalLanguage = portalLanguageResponse.ResponseData.Where(k => k.DomainPortalFid == domainPortalFid).ToList();
              
                var informationDetails = _context.YachtInformationDetails.Where(k => k.InformationFid == infoId).ToList();

                var languageSupportedList = (from c in portalLanguage
                                             join d in _languages on c.LanguageFid equals d.Id
                                             join e in informationDetails on c.LanguageFid equals e.LanguageFid into g
                                             from f in g.DefaultIfEmpty()
                                             select new YachtInformationDetailSupportModel()
                                             {
                                                 Id = (f != null && f.Id > 0) ? f.Id : 0,
                                                 InfoFid = infoId,
                                                 LanguageFid = c.LanguageFid,
                                                 FileStreamFid = (f != null && f.Id > 0) ? f.FileStreamFid : 0,
                                                 LanguageName = c.LanguageName,
                                                 ResourceKey = d.ResourceKey,
                                                 Supported = (f != null && f.Id > 0) ? true : false
                                             }).OrderBy(k => k.LanguageFid).ToList();


                return BaseResponse<List<YachtInformationDetailSupportModel >>.Success( languageSupportedList);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtInformationDetailSupportModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> IsActivated(int id, bool value)
        {
            try
            {
                var entity = _context.YachtInformations.FirstOrDefault(x => x.Id == id && x.Deleted == false && x.IsActivated != value);
                if (entity == null)
                {
                    return BaseResponse<bool>.NotFound(false);
                }

                var isSingleInfo = !_context.YachtInformations.
                   Any(r => r.Deleted == false && r.IsActivated == true
                   && r.YachtFid == entity.YachtFid && r.Id != entity.Id);

                if (isSingleInfo)
                {
                    return BaseResponse<bool>.BadRequest();
                }

                entity.IsActivated = value;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                _context.SaveChanges();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtInformationViewModel>> Search(YachtInformationSearchModel searchModel)
        {
            try
            {

                var sortString = !string.IsNullOrEmpty(searchModel.SortString)
               ? searchModel.SortString
               : "ActivatedDate DESC";
                bool? isactive = searchModel.IsActivated.ToNullBoolean();
                DateTime? activatedDate = null;
                if (!string.IsNullOrEmpty(searchModel.ActivatedDate))
                    activatedDate = searchModel.ActivatedDate.ToNullDateTime();

                var query = (from i in _context.YachtInformations
                             .Where(k => !k.Deleted && k.YachtFid == searchModel.YachtFid
                             && (string.IsNullOrEmpty(searchModel.Title) || k.DefaultTitle.Contains(searchModel.Title))
                             && (searchModel.IsActivated == null || k.IsActivated == isactive)
                             && (activatedDate == null || k.ActivatedDate.Value.Date == activatedDate.Value.Date))
                             select new YachtInformationViewModel()
                             {
                                 Title = i.DefaultTitle,
                                 IsActivated = i.IsActivated,
                                 ActivatedBy = i.ActivatedBy,
                                 ActivatedDate = i.ActivatedDate,
                                 Id = i.Id,
                                 LanguagesSupported = (from d in _context.YachtInformationDetails
                                                       join l in _languages on d.LanguageFid equals l.Id
                                                       where !d.Deleted && d.InformationFid == i.Id
                                                       select l.ResourceKey).ToList()
                             }).OrderBy(sortString);
                return BaseResponse<PagedList<YachtInformationViewModel>>.Success(new PagedList<YachtInformationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<YachtInformationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateDetail(YachtInformationCreateModel updateModel)
        {
            try
            {
                if (updateModel == null)
                    return BaseResponse<bool>.BadRequest(false);

                var entity = _context.YachtInformationDetails.FirstOrDefault(e => e.Deleted == false && e.Id == updateModel.Id);

                var entityInfo = _context.YachtInformations.FirstOrDefault(x => x.Id == updateModel.InformationFid);
                if (updateModel.LanguageFid == 1) {
                    entityInfo.DefaultTitle = updateModel.Title;
                    entityInfo.ActivatedDate = updateModel.ActivatedDate.Value.Date;
                }
                   
                if (updateModel.ActivatedDate.HasValue)
                {
                    entity.ActivatedDate = updateModel.ActivatedDate.Value.Date;
                }

                entity.Title = updateModel.Title;
                entity.ShortDescriptions = updateModel.ShortDescriptions;
                entity.FullDescriptions = updateModel.FullDescriptions;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                if (updateModel.FileStreamFID > 0)
                    entity.FileStreamFid = updateModel.FileStreamFID;
                if (updateModel.FileTypeFID > 0)
                    entity.FileTypeFid = updateModel.FileTypeFID;
                _context.YachtInformationDetails.Update(entity);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}