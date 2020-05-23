using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using ExtendedUtility;
using Omu.ValueInjecter;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using YachtMerchant.Core.Models.YachtOtherInformation;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.CommonLanguages;
using System.Collections.Generic;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtOtherInformationService : ServiceBase, IYachtOtherInformationService
    {
        private readonly ICommonLanguagesRequestServices _languagesServices;
        private List<CommonLanguagesViewModel> _languages;
        public YachtOtherInformationService(YachtOperatorDbContext dbcontext,
            ICommonLanguagesRequestServices languagesServices) : base(dbcontext)
        {
            _languagesServices = languagesServices;
            LoadLanguages();
        }

        private void LoadLanguages()
        {
            try
            {
                _languages = _languagesServices.GetAllCommonValue().ResponseData;
            }
            catch
            {
                _languages = new List<CommonLanguagesViewModel>();
            }
        }

        public BaseResponse<bool> Create(YachtOtherInformationAddOrUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                DateTime? activatedDate = null;
                if (!string.IsNullOrEmpty(model.ActivatedDate))
                    activatedDate = model.ActivatedDate.ToNullDateTime();

                var isExistedInfo = _context
                   .YachtOtherInformations
                   .Any(r => r.Deleted == false
                   && r.YachtFid == model.YachtFid
                   && r.ActivatedDate.GetValueOrDefault().Date == activatedDate.GetValueOrDefault().Date);
                if (isExistedInfo)
                    return BaseResponse<bool>.NotFound(false);
                var userId = GetUserGuidId();
                var entity = new YachtOtherInformations();
                entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                entity.Title = model.Title;
                entity.Descriptions = model.Descriptions;
                entity.InfoTypeFid = model.InfoTypeFid;
                entity.LanguageFid = model.LanguageFid;
                entity.FileTypeFid = model.FileTypeFid;
                entity.FileStreamFid = model.FileStreamFid;
                entity.IsActivated = true;
                entity.ActivatedBy = userId;
                entity.ActivatedDate = DateTime.Now;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now;
                entity.CreatedBy = userId;
                entity.CreatedDate = DateTime.Now;
                _context.YachtOtherInformations.Add(entity);
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
                var res = _context.YachtOtherInformations.FirstOrDefault(r => r.Deleted == false && r.Id == id);
                if (res == null)
                {
                    return BaseResponse<bool>.BadRequest();
                }

                var userId = GetUserGuidId();
                res.Deleted = true;
                res.LastModifiedBy = userId;
                res.LastModifiedDate = DateTime.Now;
                 _context.SaveChanges();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YacthOtherInformationViewModel> FindById(int id)
        {
            try
            {
                var result = _context.YachtOtherInformations.Find(id);
                if (result != null && result.Deleted == false)
                {
                    var model = new YacthOtherInformationViewModel();
                    model.InjectFrom(result);
                    return BaseResponse<YacthOtherInformationViewModel>.Success(model);
                }
                return null;
            }
            catch (Exception ex)
            {
                return BaseResponse<YacthOtherInformationViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> IsActivated(int id, bool value)
        {
            try
            {
                var userId = GetUserGuidId();
                var entity = _context.YachtOtherInformations.FirstOrDefault(x => x.Id == id && x.Deleted == false);
                if (entity == null)
                {
                    return BaseResponse<bool>.NotFound(false);
                }

                var isSingleInfo = !_context.YachtOtherInformations.
                   Any(r => r.Deleted == false && r.IsActivated == true
                   && r.YachtFid == entity.YachtFid && r.Id != entity.Id);

                if (isSingleInfo)
                {
                    return BaseResponse<bool>.NotFound(false);
                }

                entity.IsActivated = value;
                entity.ActivatedBy = userId;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now;
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return  BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YacthOtherInformationViewModel>> Search(YachtOtherInformatioSearchModel searchModel)
        {
            try
            {
                var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "ActivatedDate DESC";

                searchModel.PageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                searchModel.PageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;

                bool? isactive = searchModel.IsActivated.ToNullBoolean();
                DateTime? activatedDate = null;
                if (!string.IsNullOrEmpty(searchModel.ActivatedDate))
                    activatedDate = searchModel.ActivatedDate.ToNullDateTime();

                var query = (from i in _context.YachtOtherInformations
                            .Where(k => !k.Deleted && k.YachtFid == searchModel.YachtFid
                             && (string.IsNullOrEmpty(searchModel.Title) || k.Title.Contains(searchModel.Title))
                             && (searchModel.IsActivated == null || k.IsActivated == isactive)
                             && (activatedDate == null || k.ActivatedDate.Value.Date == activatedDate.Value.Date)
                             && (searchModel.LanguageId == 0 || k.LanguageFid == searchModel.LanguageId))
                             select new YacthOtherInformationViewModel()
                             {
                                 InfoTypeFid = i.InfoTypeFid,
                                 FileTypeFid = i.FileStreamFid,
                                 FileStreamFid = i.FileStreamFid,
                                 LanguageFid = i.LanguageFid,
                                 YachtFid = i.YachtFid,
                                 Title = i.Title,
                                 Descriptions = i.Descriptions,
                                 IsActivated = i.IsActivated,
                                 ActivatedDate = i.ActivatedDate,
                                 Id = i.Id,
                                 UniqueId = i.UniqueId,
                                 ResourceKey = (from d in _context.YachtOtherInformations
                                                join l in _languages on d.LanguageFid equals l.Id
                                                where d.Id == i.Id
                                                select l.ResourceKey).FirstOrDefault()
                             }).OrderBy(sortString);

                return BaseResponse<PagedList<YacthOtherInformationViewModel>>.Success(new PagedList<YacthOtherInformationViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YacthOtherInformationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(YachtOtherInformationAddOrUpdateModel model)
        {
            try
            {
                var entity = _context.YachtOtherInformations.Find(model.Id);
                if (entity != null)
                {
                    entity.Title = model.Title;
                    entity.Descriptions = model.Descriptions;
                    entity.InfoTypeFid = model.InfoTypeFid;
                    entity.LanguageFid = model.LanguageFid;
                    entity.FileStreamFid = model.FileStreamFid > 0 ? model.FileStreamFid : entity.FileStreamFid;
                    entity.FileTypeFid = model.FileTypeFid > 0 ? model.FileTypeFid : entity.FileTypeFid;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}