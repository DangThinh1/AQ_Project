using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Services.Interfaces;
using AQEncrypts;
using AutoMapper;
using Identity.Core.Helpers;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using YachtMerchant.Core.Common;
using YachtMerchant.Core.Models.YachtExternalRefLinks;
using YachtMerchant.Core.Models.YachtTourExternalRefLinks;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourExternalRefLinkServices : IYachtTourExternalRefLinkServices
    {
        private readonly IMapper _mapper;
        private YachtOperatorDbContext _db;
        private readonly ICommonValueRequestService _commonValueRequestService;

        public YachtTourExternalRefLinkServices(YachtOperatorDbContext db,
            IMapper mapper, ICommonValueRequestService commonValueRequestService)
        {
            _db = db;
            _mapper = mapper;
            _commonValueRequestService = commonValueRequestService;
        }

        private bool IsUrlValid(string url)
        {
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }

        public BaseResponse<bool> Create(YachtTourExternalRefLinkModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                bool checkLink = IsUrlValid(model.UrlLink);
                var pricingTypeResponse = _commonValueRequestService
                    .GetCommonValueByGroupInt(CommonValueGroupConstant.ExternalRefLinkType, model.LinkTypeFid);

                var entity = new YachtTourExternalRefLinks();
                entity.InjectFrom(model);
                entity.UrlLink = checkLink == true ? model.UrlLink : null;
                entity.YachtTourFid = int.Parse(Terminator.Decrypt(model.YachtTourFid));
                entity.LinkTypeResKey = pricingTypeResponse.IsSuccessStatusCode ? pricingTypeResponse.ResponseData.ResourceKey : string.Empty;
                entity.Deleted = false;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = UserContextHelper.UserId;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = UserContextHelper.UserId;
                _db.YachtTourExternalRefLinks.Add(entity);
                _db.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(long id)
        {
            var entity = _db.YachtTourExternalRefLinks.Find(id);
            if (entity != null && !entity.Deleted)
            {
                entity.Deleted = true;
                entity.LastModifiedBy = UserContextHelper.UserId;
                entity.LastModifiedDate = DateTime.Now;
                _db.YachtTourExternalRefLinks.Update(entity);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            return BaseResponse<bool>.InternalServerError();
        }

        public BaseResponse<List<YachtTourExternalRefLinkModel>> GetAll()
        {
            try
            {
                var query = _db.YachtTourExternalRefLinks.Where(x => !x.Deleted)
                    .Select(x => _mapper.Map<YachtTourExternalRefLinks, YachtTourExternalRefLinkModel>(x))
                    .OrderBy(x => x.EffectiveDate)
                    .ThenBy(x => x.EffectiveDate)
                    .ToList();

                return BaseResponse<List<YachtTourExternalRefLinkModel>>.Success(query);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourExternalRefLinkModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(YachtTourExternalRefLinkModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _db.YachtTourExternalRefLinks.FirstOrDefault(x => x.Id.Equals(model.Id));
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();

                bool checkLink = IsUrlValid(model.UrlLink);
                entity.InjectFrom(model);
                entity.UrlLink = checkLink == true ? model.UrlLink : null;
                entity.LastModifiedBy = UserContextHelper.UserId;
                entity.LastModifiedDate = DateTime.Now;
                entity.YachtTourFid = int.Parse(Terminator.Decrypt(model.YachtTourFid));

                var pricingTypeResponse = _commonValueRequestService
                .GetCommonValueByGroupInt(CommonValueGroupConstant.ExternalRefLinkType, model.LinkTypeFid).ResponseData;
                entity.LinkTypeResKey = pricingTypeResponse != null ? pricingTypeResponse.ResourceKey : string.Empty;

                _db.YachtTourExternalRefLinks.Update(entity);
                _db.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtTourExternalRefLinkModel> FindById(long id)
        {
            try
            {
                var result = _db.YachtTourExternalRefLinks.Find(id);
                if (result != null && !result.Deleted)
                {
                    var model = _mapper.Map<YachtTourExternalRefLinks, YachtTourExternalRefLinkModel>(result);
                    return BaseResponse<YachtTourExternalRefLinkModel>.Success(model);
                }
                return BaseResponse<YachtTourExternalRefLinkModel>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourExternalRefLinkModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtTourExternalRefLinkModel>> GetByYachtTourId(int id, YachtTourExternalRefLinkSearchModel searchModel)
        {
            try
            {
                var query = _db.YachtTourExternalRefLinks.Where(x => !x.Deleted && x.YachtTourFid == id);
                if (query.Count() > 0)
                {
                    var data = query.Select(k => _mapper.Map<YachtTourExternalRefLinks, YachtTourExternalRefLinkModel>(k));
                    var result = new PagedList<YachtTourExternalRefLinkModel>(data, searchModel.PageIndex, searchModel.PageSize);
                    if (result != null)
                        return BaseResponse<PagedList<YachtTourExternalRefLinkModel>>.Success(result);
                    return BaseResponse<PagedList<YachtTourExternalRefLinkModel>>.BadRequest();
                }
                return BaseResponse<PagedList<YachtTourExternalRefLinkModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtTourExternalRefLinkModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}