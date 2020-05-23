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
    public class HotelInventoryAttributeValueService : ServiceBase, IHotelInventoryAttributeValueService
    {
        public HotelInventoryAttributeValueService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {

        }

        public BaseResponse<List<HotelInventoryAttributeValueViewModel>> GetAllAttributeValueByInventoryId(int inventoryId)
        {
            var query = from b in _db.HotelInventoryAttributeValues.AsNoTracking().Where(k => k.InventoryFid == inventoryId)
                               join c in _db.HotelInventoryAttributes.AsNoTracking() on b.AttributeFid equals c.Id
                               select new HotelInventoryAttributeValueViewModel
                               {
                                   AttributeFid = c.Id,
                                   AttributeCategoryFid = c.AttributeCategoryFid,
                                   AttributeName = c.AttributeName,
                                   BasedAffective = b.BasedAffective,
                                   EffectiveDate = b.EffectiveDate,
                                   IconCssClass = c.IconCssClass,
                                   Id = (int)b.Id,
                                   ResourceKey = c.ResourceKey,
                                   InventoryFid = b.InventoryFid
                               };
            if( query.Count() >0)
                return BaseResponse<List<HotelInventoryAttributeValueViewModel>>.Success(query.ToList());
            return BaseResponse<List<HotelInventoryAttributeValueViewModel>>.NoContent(new List<HotelInventoryAttributeValueViewModel>());
        }


        public BaseResponse<List<HotelInventoryAttributeValueViewModel>> GetAllAttributeValueByInventoryIdAndCategoryId(int inventoryId, int attributeCategoryId)
        {
            var query = from b in _db.HotelInventoryAttributeValues.AsNoTracking().Where(k => k.InventoryFid == inventoryId && k.AttributeCategoryFid == attributeCategoryId)
                                join c in _db.HotelInventoryAttributes.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId) on b.AttributeFid equals c.Id
                                select new HotelInventoryAttributeValueViewModel
                                {
                                    Id = (int)b.Id,
                                    AttributeFid = c.Id,
                                    AttributeCategoryFid = c.AttributeCategoryFid,
                                    AttributeName = c.AttributeName,
                                    BasedAffective = b.BasedAffective,
                                    EffectiveDate = b.EffectiveDate,
                                    IconCssClass = c.IconCssClass,
                                    ResourceKey = c.ResourceKey,
                                    InventoryFid = b.InventoryFid
                                };
            if(query.Count()>0)
                return BaseResponse<List<HotelInventoryAttributeValueViewModel>>.Success(query.ToList());
            return BaseResponse<List<HotelInventoryAttributeValueViewModel>>.NoContent(new List<HotelInventoryAttributeValueViewModel>());
        }

        public async Task<BaseResponse<bool>> UpdateAttributeValueRangeAsync(HotelInventoryAttributeValueUpdateRangeModel updateModel)
        {
            try
            {
                if (updateModel == null)
                    return BaseResponse<bool>.BadRequest();
                var userId = GetUserGuidId();
                if (updateModel == null || updateModel.ListAttributeId == null || updateModel.ListAttributeValue == null)
                    return BaseResponse<bool>.BadRequest();
                if (_db.HotelInventoryAttributeValues.AsNoTracking().Any(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.InventoryFid == updateModel.InventoryFid))
                {
                    if (updateModel.ListAttributeId.Count() == 0 && updateModel.ListAttributeValue.Count()==0)
                    {
                        var data = _db.HotelInventoryAttributeValues.Where(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.InventoryFid == updateModel.InventoryFid);
                        _db.RemoveRange(data);
                        _db.SaveChanges();
                        return BaseResponse<bool>.Success(true);
                    }
                    else
                    {
                        // step 1 : remove all current attribute of Inventory in db
                        var data = _db.HotelInventoryAttributeValues.Where(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.InventoryFid == updateModel.InventoryFid);
                        _db.RemoveRange(data);
                        _db.SaveChanges();

                        // step 2: add new all attribute to db
                        if (updateModel.ListAttributeId.Count() > 0 && updateModel.ListAttributeValue.Count() > 0 && updateModel.ListAttributeId.Count() == updateModel.ListAttributeValue.Count())
                        {
                            var listAttributeId = updateModel.ListAttributeId;
                            var listAtrributeValue = updateModel.ListAttributeValue;
                            var lstattribute = new List<HotelInventoryAttributeValues>();
                            for (int i = 0; i < listAttributeId.Count; i++)
                            {
                                var attributeValue = new HotelInventoryAttributeValues()
                                {
                                    InventoryFid = updateModel.InventoryFid,
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
                                await _db.HotelInventoryAttributeValues.AddRangeAsync(lstattribute);
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

                        var lstattribute = new List<HotelInventoryAttributeValues>();
                        for (int i = 0; i < listAttributeId.Count; i++)
                        {
                            var attributeValue = new HotelInventoryAttributeValues()
                            {
                                InventoryFid= updateModel.InventoryFid,
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
                            await _db.HotelInventoryAttributeValues.AddRangeAsync(lstattribute);
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

        public async Task<BaseResponse<bool>> CreateAsync(HotelInventoryAttributeValueCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var createItem = new HotelInventoryAttributeValues();
                createItem.InjectFrom(model);
                createItem.BasedAffective = false;
                createItem.LastModifiedBy = GetUserGuidId();
                createItem.LastModifiedDate = DateTime.UtcNow;
                await _db.HotelInventoryAttributeValues.AddAsync(createItem);
                await _db.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> CreateRangeAsync(List<HotelInventoryAttributeValueCreateModel> model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var createItems = new List<HotelInventoryAttributeValues>();
                foreach (var item in model)
                {
                    var createItem = new HotelInventoryAttributeValues();
                    createItem.InjectFrom(item);
                    createItem.BasedAffective = false;
                    createItem.LastModifiedBy = GetUserGuidId();
                    createItem.LastModifiedDate = DateTime.UtcNow;
                    createItems.Add(createItem);
                }
                if (createItems.Count() > 0)
                {
                    await _db.HotelInventoryAttributeValues.AddRangeAsync(createItems);
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

        public BaseResponse<List<HotelInventoryAttributeValueMgtUpdateModel>> GetListUpdateAttributeValue(int inventoryId, int attributeCategoryId)
        {
            var query = from c in _db.HotelInventoryAttributes.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId)
                        join d in _db.HotelInventoryAttributeValues.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId && k.InventoryFid == inventoryId) on c.Id equals d.AttributeFid 
                        into ps
                        from e in ps.DefaultIfEmpty()
                        select new HotelInventoryAttributeValueMgtUpdateModel()
                        {
                            Id = c.Id,
                            AttributeName = c.AttributeName,
                            AttributeValue = e.AttributeValue,
                            UniqueId = c.UniqueId,
                            IconCssClass = c.IconCssClass,                           
                            ResourceKey = c.ResourceKey,
                            Check = e.InventoryFid > 0 ? true : false
                        };
            if(query.Count()>0)
                return BaseResponse<List<HotelInventoryAttributeValueMgtUpdateModel>>.Success(query.ToList());
            return BaseResponse<List<HotelInventoryAttributeValueMgtUpdateModel>>.Success(new List<HotelInventoryAttributeValueMgtUpdateModel>());
        }
    }
}
