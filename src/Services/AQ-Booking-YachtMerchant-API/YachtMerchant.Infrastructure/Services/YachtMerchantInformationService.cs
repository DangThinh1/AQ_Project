using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Models.CommonLanguages;
using AQConfigurations.Core.Models.PortalLanguages;
using AQConfigurations.Core.Services.Interfaces;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtMerchantInformation;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtMerchantInformationService : ServiceBase, IYachtMerchantInformationService
    {
        private readonly ICommonLanguagesRequestServices _commonLanguageService;
        private readonly IPortalLanguageRequestService _portalLanguageRequestService;
        private List<CommonLanguagesViewModel> _languages;
        public YachtMerchantInformationService(YachtOperatorDbContext context,
            ICommonLanguagesRequestServices commonLanguages, IPortalLanguageRequestService portalLanguage) : base(context)
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

        public BaseResponse<PagedList<YachtMerchantInformationViewModel>> Search(YachtMerchantInformationSearchModel searchModel)
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

                var query = (from i in _context.YachtMerchantInformations
                             .Where(k => !k.Deleted && k.MerchantFid == searchModel.MerchantFid
                             && (string.IsNullOrEmpty(searchModel.Title) || k.DefaultTitle.Contains(searchModel.Title))
                             && (searchModel.IsActivated == null || k.IsActivated == isactive)
                             && (activatedDate == null || k.ActivatedDate.Value.Date == activatedDate.Value.Date))
                             select new YachtMerchantInformationViewModel()
                             {
                                 Id = i.Id,
                                 DefaultTitle = i.DefaultTitle,
                                 IsActivated = i.IsActivated,
                                 ActivatedDate = i.ActivatedDate,
                                 LanguagesSupported = (from d in _context.YachtMerchantInformationDetails
                                                       join l in _languages on d.LanguageFid equals l.Id
                                                       where !d.Deleted && d.InformationFid == i.Id
                                                       select l.ResourceKey).ToList()
                             }).OrderBy(sortString);
                return BaseResponse < PagedList <YachtMerchantInformationViewModel>>.Success( new PagedList<YachtMerchantInformationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
            }
            catch(Exception ex )
            {
                return BaseResponse<PagedList<YachtMerchantInformationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Create(YachtMerchantInformationAddOrUpdateModel createModel)
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

                    var isExistedInfo = _context.YachtMerchantInformations
                        .Any(r => r.Deleted == false
                        && r.MerchantFid == createModel.MerchantFid
                        && r.ActivatedDate == activetdDate);
                    if (isExistedInfo)
                        return BaseResponse<bool>.BadRequest();

                    var userId = GetUserGuidId();
                    // create YachtMerchantInformations
                    var info = new YachtMerchantInformations();
                    info.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    info.DefaultTitle = createModel.Title;
                    info.MerchantFid = createModel.MerchantFid;
                    info.ActivatedBy = userId;
                    info.ActivatedDate = activetdDate;
                    info.LastModifiedBy = userId;
                    //info.Remark = createModel.Remark;
                    info.LastModifiedDate = DateTime.Now;
                    info.IsActivated = true;
                    info.Deleted = false;
                    _context.YachtMerchantInformations.Add(info);
                    _context.SaveChanges();

                    // create YachtMerchantInformationDetails
                    var detail = new YachtMerchantInformationDetails();
                    detail.Title = createModel.Title;
                    detail.FileStreamFid = createModel.FileStreamFID;
                    detail.FileTypeFid = createModel.FileTypeFID;
                    detail.LanguageFid = createModel.LanguageFid;
                    detail.ShortDescriptions = createModel.ShortDescriptions;
                    detail.FullDescriptions = createModel.FullDescriptions;
                    detail.ActivatedDate = activetdDate;
                    detail.InformationFid = info.Id;
                    detail.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    detail.ActivatedBy = userId;
                    detail.LastModifiedBy = userId;
                    detail.LastModifiedDate = DateTime.Now;
                    detail.IsActivated = true;
                    detail.Deleted = false;
                    _context.YachtMerchantInformationDetails.Add(detail);
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

        public BaseResponse<YachtMerchantInformationAddOrUpdateModel> FindInfoDetailById(int id)
        {
            try
            {

                var entity = _context.YachtMerchantInformationDetails.AsNoTracking()
                    .FirstOrDefault(e => e.Deleted == false && e.Id == id);
                var viewModel = new YachtMerchantInformationAddOrUpdateModel();
                viewModel.InjectFrom(entity);
                viewModel.FileStreamFID = entity.FileStreamFid;
                 viewModel.ResourceKey = _languages.Count > 0 ?
                                    _languages.FirstOrDefault(r=>r.Id == entity.LanguageFid).ResourceKey: null;
                if (viewModel.ActivatedDate.HasValue)
                    viewModel.ActivatedDate = viewModel.ActivatedDate.Value.Date;
                return BaseResponse<YachtMerchantInformationAddOrUpdateModel>.Success(viewModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtMerchantInformationAddOrUpdateModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> IsActivated(int id, bool value)
        {
            try
            {
                var entity = _context.YachtMerchantInformations.FirstOrDefault(x => x.Id == id && x.Deleted == false);
                if (entity == null)
                {
                    return BaseResponse<bool>.NotFound(false);
                }

                var isSingleInfo = !_context.YachtMerchantInformations.
                   Any(r => r.Deleted == false && r.IsActivated == true
                   && r.MerchantFid == entity.MerchantFid && r.Id != entity.Id);

                if (isSingleInfo)
                {
                    return BaseResponse<bool>.BadRequest(false);
                }

                entity.IsActivated = value;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                _context.SaveChanges();
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
                var res = _context.YachtMerchantInformations.FirstOrDefault(r => r.Deleted == false && r.Id == id);
                if (res == null)
                {
                    return BaseResponse<bool>.NotFound(false);
                }

                res.Deleted = true;
                res.LastModifiedBy = GetUserGuidId();
                res.LastModifiedDate = DateTime.Now;
                var activeResult = _context.SaveChanges();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError( message: ex.Message, fullMsg:ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtMerchantInformationSupportModel>> GetInfoDetailSupportedByInfoId(int infoId)
        {
            try
            {
                var domainPortalFid = (int)AQPortalEnum.YachtMerchantPortal;
                var portalLanguageResponse = _portalLanguageRequestService.GetLanguagesByDomainId(domainPortalFid);
                var portalLanguage = new List<PortalLanguageViewModel>();
                if (portalLanguageResponse.IsSuccessStatusCode)
                    portalLanguage = portalLanguageResponse.ResponseData.Where(k => k.DomainPortalFid == domainPortalFid).ToList();
              
                var informationDetails = _context.YachtMerchantInformationDetails.Where(k => k.InformationFid == infoId).ToList();

                var languageSupportedList = (from c in portalLanguage
                                             join d in _languages on c.LanguageFid equals d.Id
                                             join e in informationDetails on c.LanguageFid equals e.LanguageFid into g
                                             from f in g.DefaultIfEmpty()
                                             select new YachtMerchantInformationSupportModel()
                                             {
                                                 Id = (f != null && f.Id > 0) ? f.Id : 0,
                                                 InfoFid = infoId,
                                                 LanguageFid = c.LanguageFid,
                                                 FileStreamFid = (f != null && f.Id > 0) ? f.FileStreamFid : 0,
                                                 LanguageName = c.LanguageName,
                                                 ResourceKey = d.ResourceKey,
                                                 Supported = (f != null && f.Id > 0) ? true : false
                                             }).OrderBy(k => k.LanguageFid).ToList();

                return BaseResponse<List<YachtMerchantInformationSupportModel>>.Success(languageSupportedList);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantInformationSupportModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> CreateDetail(YachtMerchantInformationAddOrUpdateModel createModel)
        {
            try
            {
                DateTime? activetdDate = null;
                if (createModel.ActivatedDate.HasValue)
                    activetdDate = createModel.ActivatedDate.Value.Date;
                var userId = GetUserGuidId();
                var entity = new YachtMerchantInformationDetails();
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

                _context.YachtMerchantInformationDetails.Add(entity);
                _context.SaveChangesAsync();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateDetail(YachtMerchantInformationAddOrUpdateModel updateModel)
        {
            try
            {
                if (updateModel == null)
                    return BaseResponse<bool>.BadRequest(false, "Update model can not be null");

                var entity = _context.YachtMerchantInformationDetails.FirstOrDefault(e => e.Deleted == false && e.Id == updateModel.Id);

                var entityInfo = _context.YachtMerchantInformations.FirstOrDefault(x => x.Id == updateModel.InformationFid);
                if (updateModel.LanguageFid == 1)
                    entityInfo.DefaultTitle = updateModel.Title;
                if (updateModel.ActivatedDate.HasValue)
                {
                    updateModel.ActivatedDate = updateModel.ActivatedDate.Value.Date;
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

                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> CheckInforDetail(int id, int inforId)
        {
            var result = _context.YachtMerchantInformationDetails.FirstOrDefault(x => x.Id == id && x.InformationFid == inforId);
            if (result == null)
                return BaseResponse<bool>.NotFound(false);
            return BaseResponse<bool>.Success(true);
        }
    }
}