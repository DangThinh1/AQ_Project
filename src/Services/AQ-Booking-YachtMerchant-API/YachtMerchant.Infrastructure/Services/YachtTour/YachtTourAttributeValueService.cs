using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using System.Collections.Generic;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Core.Models.YachtTourAttributeValues;
using Identity.Core.Helpers;
using Omu.ValueInjecter;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using Microsoft.EntityFrameworkCore;
using AQEncrypts;
using ExtendedUtility;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourAttributeValueService : IYachtTourAttributeValueService
    {
        private readonly IMapper _mapper;
        private readonly YachtOperatorDbContext _db;
        public YachtTourAttributeValueService(YachtOperatorDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public BaseResponse<bool> Create(YachtTourAttributeValueCreateModel model)
        {
            try
            {
                var entity = GenerateForCreate(model);
                _db.YachtTourAttributeValues.Add(entity);
                var resul = _db.SaveChanges();
                return resul > 0
                        ? BaseResponse<bool>.Success(true)
                        : BaseResponse<bool>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Create(List<YachtTourAttributeValueCreateModel> models, bool disableSaveChange = false)
        {
            try
            {
                if (models.Count == 0)
                    return BaseResponse<bool>.Success(true);
                var entities = models
                    .Select(k=> GenerateForCreate(k))
                    .ToList();
                _db.YachtTourAttributeValues.AddRange(entities);

                //return without save change
                if (disableSaveChange)
                    return BaseResponse<bool>.Success(true);

                var resul = _db.SaveChanges();
                return resul > 0
                        ? BaseResponse<bool>.Success(true)
                        : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(List<YachtTourAttributeValueUpdateModel> models, bool disableSaveChange = false)
        {
            try
            {
                if(models.Count == 0)
                    return BaseResponse<bool>.Success(true);
                foreach (var item in models)
                {
                    var tourId = item.YachtTourFid.Decrypt().ToInt32();
                    var entity = _db.YachtTourAttributeValues.FirstOrDefault(k=> k.YachtTourFid == tourId && k.AttributeFid == item.AttributeFid);
                    if (entity != null)
                    {
                        entity = GenerateForUpdate(entity);
                        entity.AttributeValue = item.AttributeValue;
                        _db.YachtTourAttributeValues.Update(entity);
                    }
                }
                //return without save change
                if (disableSaveChange)
                    return BaseResponse<bool>.Success(true);

                var result = _db.SaveChanges();
                return result > 0
                    ? BaseResponse<bool>.Success(true)
                    : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //Update list Attribute values if existed or create new if not existed
        public BaseResponse<bool> Synchronize(List<YachtTourAttributeValueUpdateModel> models)
        {
            try
            {
                var listForCreate = new List<YachtTourAttributeValueCreateModel>();
                var listForUpdate = new List<YachtTourAttributeValueUpdateModel>();
                foreach (var item in models)
                {
                    var tourId = item.YachtTourFid.Decrypt().ToInt32();
                    var entity = _db.YachtTourAttributeValues.AsNoTracking().FirstOrDefault(k => k.YachtTourFid == tourId && k.AttributeFid == item.AttributeFid);
                    if(entity != null)
                    {
                        listForUpdate.Add(item);
                    }
                    else
                    {
                        var createModel = new YachtTourAttributeValueCreateModel();
                        createModel.InjectFrom(item);
                        listForCreate.Add(createModel);
                    }
                }
                var createResult = Create(listForCreate);
                var updateResult = Update(listForUpdate);
                if(createResult.IsSuccessStatusCode && updateResult.IsSuccessStatusCode)
                {
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourAttributeValueViewModel>> GetByTourId(int id)
        {
            try
            {
                var result = _db.YachtTourAttributeValues
                    .AsNoTracking()
                    .Where(k => k.YachtTourFid == id)
                    .Select(k=> _mapper.Map<YachtTourAttributeValues, YachtTourAttributeValueViewModel>(k))
                    .ToList();
                if (result != null)
                    return BaseResponse<List<YachtTourAttributeValueViewModel>>.Success(result);

                return BaseResponse<List<YachtTourAttributeValueViewModel>>.BadRequest();

            }
            catch(Exception ex)
            {
                return BaseResponse<List<YachtTourAttributeValueViewModel>>.InternalServerError(message:ex.Message);
            }
        }

        public BaseResponse<List<YachtTourAttributeValueMgtModel>> GetListUpdateAttributeValue(int tourId)
        {
            try
            {
                var listAttValues = _db.YachtTourAttributeValues.AsNoTracking().Where(k => k.YachtTourFid == tourId).ToList();
                var retult = _db.YachtTourAttributes
                    .AsNoTracking()
                    .Where(k => !k.Deleted)
                    .Select(k => new YachtTourAttributeValueMgtModel() {
                        Id = k.Id,
                        ResourceKey = k.ResourceKey,
                        IconCssClass = k.IconCssClass,
                        AttributeValue = (listAttValues.FirstOrDefault(g => g.AttributeFid == k.Id)) != null 
                                       ? listAttValues.FirstOrDefault(g => g.AttributeFid == k.Id).AttributeValue 
                                       : string.Empty
                    })
                    .ToList();

                var query = from c in _db.YachtTourAttributes.AsNoTracking().Where(k=> !k.Deleted)
                            join d in _db.YachtTourAttributeValues.AsNoTracking().Where(k => k.YachtTourFid == tourId) on c.Id equals d.AttributeFid
                            select new YachtTourAttributeValueMgtModel()
                            {
                                Id = c.Id,
                                AttributeName = c.AttributeName,
                                AttributeValue = d.AttributeValue,
                                UniqueId = c.UniqueId,
                                IconCssClass = c.IconCssClass,
                                ResourceKey = c.ResourceKey,
                                Check = d.YachtTourFid > 0 ? true : false
                            };
                var data = retult.ToList();
                if(data != null)
                    return BaseResponse<List<YachtTourAttributeValueMgtModel>>.Success(data);
                return BaseResponse<List<YachtTourAttributeValueMgtModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<List<YachtTourAttributeValueMgtModel>>.InternalServerError(message:ex.Message);
            }
        }

        #region Private Methods

        private YachtTourAttributeValues GenerateForCreate(YachtTourAttributeValueCreateModel model)
        {
            var entity = _mapper.Map<YachtTourAttributeValueCreateModel, YachtTourAttributeValues>(model);
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            entity.LastModifiedBy = userId;
            entity.LastModifiedDate = now;
            return entity;
        }

        private YachtTourAttributeValues GenerateForUpdate(YachtTourAttributeValues entity)
        {
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            entity.LastModifiedBy = userId;
            entity.LastModifiedDate = now;
            return entity;
        }

        #endregion Private Methods
    }
}
