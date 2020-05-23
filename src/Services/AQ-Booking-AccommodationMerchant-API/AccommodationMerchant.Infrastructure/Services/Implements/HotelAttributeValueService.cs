using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Linq.Dynamic.Core;
using System;
using Microsoft.EntityFrameworkCore;
using APIHelpers.Response;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AutoMapper;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Core.Models.HotelAttributeValues;
using AccommodationMerchant.Infrastructure.Databases.Entities;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelAttributeValueService : ServiceBase, IHotelAttributeValueService
    {
        public HotelAttributeValueService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {

        }

        public BaseResponse<List<HotelAttributeValueViewModel>> GetAllAttributeValueByHotelId(int hotelId)
        {
            var query = from b in _db.HotelAttributeValues.AsNoTracking().Where(k => k.HotelFid == hotelId )
                               join c in _db.HotelAttributes.AsNoTracking() on b.AttributeFid equals c.Id
                               select new HotelAttributeValueViewModel
                               {
                                   AttributeFid = c.Id,
                                   AttributeCategoryFid = c.AttributeCategoryFid,
                                   AttributeName = c.AttributeName,
                                   BasedAffective = b.BasedAffective,
                                   EffectiveDate = b.EffectiveDate,
                                   IconCssClass = c.IconCssClass,
                                   Id = (int)b.Id,
                                   ResourceKey = c.ResourceKey,
                                   AttributeValue = b.AttributeValue
                               };
            if( query.Count() > 0)
                return BaseResponse<List<HotelAttributeValueViewModel>>.Success(query.OrderBy(k=> k.AttributeFid).ToList());
            return BaseResponse<List<HotelAttributeValueViewModel>>.NoContent(new List<HotelAttributeValueViewModel>());
        }

        public BaseResponse<List<HotelAttributeValueViewModel>> GetAllAttributeValueByHotelIdAndCategoryId(int hotelId, int attributeCategoryId)
        {
            var query = from b in _db.HotelAttributeValues.AsNoTracking().Where(k => k.HotelFid == hotelId && k.AttributeCategoryFid == attributeCategoryId)
                                join c in _db.HotelAttributes.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId) on b.AttributeFid equals c.Id
                                select new HotelAttributeValueViewModel
                                {
                                    Id = (int)b.Id,
                                    AttributeFid = c.Id,
                                    AttributeCategoryFid = c.AttributeCategoryFid,
                                    AttributeName = c.AttributeName,
                                    BasedAffective = b.BasedAffective,
                                    EffectiveDate = b.EffectiveDate,
                                    IconCssClass = c.IconCssClass,
                                    ResourceKey = c.ResourceKey,
                                    AttributeValue = b.AttributeValue
                                };
            if(query.Count()>0)
                return BaseResponse<List<HotelAttributeValueViewModel>>.Success(query.OrderBy(k => k.AttributeFid).ToList());
            return BaseResponse<List<HotelAttributeValueViewModel>>.NoContent(new List<HotelAttributeValueViewModel>());
        }

        public async Task<BaseResponse<bool>> UpdateAttributeValueRangeAsync(HotelAttributeValueUpdateRangeModel updateModel)
        {
            try
            {
                if (updateModel == null)
                    return BaseResponse<bool>.BadRequest();
                var userId = GetUserGuidId();
                if (updateModel == null || updateModel.ListAttributeId == null || updateModel.ListAttributeValue == null)
                    return BaseResponse<bool>.BadRequest();
                if (_db.HotelAttributeValues.AsNoTracking().Any(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.HotelFid == updateModel.HotelFid))
                {
                    if (updateModel.ListAttributeId.Count() == 0 && updateModel.ListAttributeValue.Count()==0)
                    {
                        var data = _db.HotelAttributeValues.Where(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.HotelFid == updateModel.HotelFid);
                        _db.RemoveRange(data);
                        _db.SaveChanges();
                        return BaseResponse<bool>.Success(true);
                    }
                    else
                    {
                        // step 1 : remove all current attribute of Hotel in db
                        var data = _db.HotelAttributeValues.Where(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.HotelFid == updateModel.HotelFid);
                        _db.RemoveRange(data);
                        _db.SaveChanges();

                        // step 2: add new all attribute to db
                        if (updateModel.ListAttributeId.Count() > 0 && updateModel.ListAttributeValue.Count() > 0 && updateModel.ListAttributeId.Count() == updateModel.ListAttributeValue.Count())
                        {
                            var listAttributeId = updateModel.ListAttributeId;
                            var listAtrributeValue = updateModel.ListAttributeValue;
                            var lstattribute = new List<HotelAttributeValues>();
                            for (int i = 0; i < listAttributeId.Count; i++)
                            {
                                var attributeValue = new HotelAttributeValues()
                                {
                                    HotelFid = updateModel.HotelFid,
                                    AttributeCategoryFid = updateModel.AttributeCategoryFid,
                                    AttributeFid = listAttributeId[i],
                                    AttributeValue = listAtrributeValue[i],
                                    EffectiveDate = DateTime.Now,
                                    LastModifiedBy = userId,
                                    LastModifiedDate = DateTime.UtcNow
                                };
                                // add to list
                                lstattribute.Add(attributeValue);
                            }
                            if (lstattribute.Count > 0)
                            {
                                await _db.HotelAttributeValues.AddRangeAsync(lstattribute);
                                await _db.SaveChangesAsync();
                                return BaseResponse<bool>.Success(true);
                            }
                        }

                        return BaseResponse<bool>.NoContent(false);
                    }
                }
                else
                {
                    if (updateModel.ListAttributeId.Count()> 0 && updateModel.ListAttributeValue.Count() > 0 && updateModel.ListAttributeId.Count() == updateModel.ListAttributeValue.Count())
                    {
                        var listAttributeId = updateModel.ListAttributeId;
                        var listAtrributeValue = updateModel.ListAttributeValue;

                        var lstattribute = new List<HotelAttributeValues>();
                        for (int i = 0; i < listAttributeId.Count; i++)
                        {
                            var attributeValue = new HotelAttributeValues()
                            {
                                HotelFid = updateModel.HotelFid,
                                AttributeCategoryFid = updateModel.AttributeCategoryFid,
                                AttributeFid = listAttributeId[i],
                                AttributeValue = listAtrributeValue[i],
                                EffectiveDate = DateTime.Now,
                                LastModifiedBy = userId,
                                LastModifiedDate = DateTime.UtcNow
                            };
                            // add to list
                            lstattribute.Add(attributeValue);
                        }
                        if (lstattribute.Count > 0)
                        {
                            await _db.HotelAttributeValues.AddRangeAsync(lstattribute);
                            await _db.SaveChangesAsync();
                            return BaseResponse<bool>.Success(true);
                        }
                    }

                    return BaseResponse<bool>.NoContent(false);
                }
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> CreateAsync(HotelAttributeValueCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var createItem = new HotelAttributeValues();
                createItem.InjectFrom(model);
                createItem.BasedAffective = false;
                createItem.LastModifiedBy = GetUserGuidId();
                createItem.LastModifiedDate = DateTime.UtcNow;
                await _db.HotelAttributeValues.AddAsync(createItem);
                await _db.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> CreateRangeAsync(List<HotelAttributeValueCreateModels> model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var createItems = new List<HotelAttributeValues>();
                foreach (var item in model)
                {
                    var createItem = new HotelAttributeValues();
                    createItem.InjectFrom(item);
                    createItem.BasedAffective = false;
                    createItem.LastModifiedBy = GetUserGuidId();
                    createItem.LastModifiedDate = DateTime.UtcNow;
                    createItems.Add(createItem);
                }
                if (createItems.Count() > 0)
                {
                    await _db.HotelAttributeValues.AddRangeAsync(createItems);
                    await _db.SaveChangesAsync();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<HotelAttributeValueMgtUpdateModel>> GetListUpdateAttributeValue(int hotelId, int attributeCategoryId)
        {
            var query = from c in _db.HotelAttributes.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId)
                        join d in _db.HotelAttributeValues.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId && k.HotelFid == hotelId) on c.Id equals d.AttributeFid 
                        into ps
                        from e in ps.DefaultIfEmpty()
                        select new HotelAttributeValueMgtUpdateModel()
                        {
                            Id = c.Id,
                            AttributeName = c.AttributeName,
                            AttributeValue = e.AttributeValue,
                            UniqueId = c.UniqueId,
                            IconCssClass = c.IconCssClass,                           
                            ResourceKey = c.ResourceKey,
                            Check = e.HotelFid > 0 ? true : false,
                            AttributeFid = c.Id
                        };
            if(query.Count()>0)
                return BaseResponse<List<HotelAttributeValueMgtUpdateModel>>.Success(query.OrderBy(k => k.AttributeFid).ToList());
            return BaseResponse<List<HotelAttributeValueMgtUpdateModel>>.NoContent(new List<HotelAttributeValueMgtUpdateModel>());
        }
    }
}