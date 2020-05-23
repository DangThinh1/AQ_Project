using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtAttributeValues;
using Omu.ValueInjecter;
using System.Linq.Dynamic.Core;
using System;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Infrastructure.Services;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using APIHelpers.Response;

namespace DiningMerchant.Infrastructure.Services
{
    public class YachtAttributeValueService : ServiceBase, IYachtAttributeValueService
    {
        public YachtAttributeValueService(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {

        }

        public BaseResponse<List<YachtAttributeValuesViewModel>> GetAllAttributeValueByYachtIdAsync(int yachtId)
        {
            var query = from b in _context.YachtAttributeValues.AsNoTracking().Where(k => k.YachtFid == yachtId )
                               join c in _context.YachtAttributes.AsNoTracking() on b.AttributeFid equals c.Id
                               select new YachtAttributeValuesViewModel
                               {
                                   AttributeFid = c.Id,
                                   AttributeCategoryFid = c.AttributeCategoryFid,
                                   AttributeName = c.AttributeName,
                                   BasedAffective = b.BasedAffective,
                                   EffectiveDate = b.EffectiveDate,
                                   IconCssClass = c.IconCssClass,
                                   Id = (int)b.Id,
                                   ResourceKey = c.ResourceKey,
                                   YachFid = b.YachtFid
                               };
            return BaseResponse<List<YachtAttributeValuesViewModel >>.Success(query.ToList());
        }


        public BaseResponse<List<YachtAttributeValuesViewModel>> GetAllAttributeValueByYachtIdAndCategoryIdAsync(int yachtId, int attributeCategoryId)
        {
            var query = from b in _context.YachtAttributeValues.AsNoTracking().Where(k => k.YachtFid == yachtId && k.AttributeCategoryFid == attributeCategoryId)
                                join c in _context.YachtAttributes.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId) on b.AttributeFid equals c.Id
                                select new YachtAttributeValuesViewModel
                                {
                                    Id = (int)b.Id,
                                    AttributeFid = c.Id,
                                    AttributeCategoryFid = c.AttributeCategoryFid,
                                    AttributeName = c.AttributeName,
                                    BasedAffective = b.BasedAffective,
                                    EffectiveDate = b.EffectiveDate,
                                    IconCssClass = c.IconCssClass,
                                    ResourceKey = c.ResourceKey,
                                    YachFid = b.YachtFid
                                };
            return BaseResponse<List<YachtAttributeValuesViewModel>>.Success(query.ToList());
        }

        public async Task<BaseResponse<bool>> UpdateAttributeValueRangeAsync(YachtAttributeValuesUpdateRangeModel updateModel)
        {
            try
            {
                var userId = GetUserGuidId();
                if (updateModel == null || updateModel.ListAttributeId == null || updateModel.ListAttributeValue == null)
                    return BaseResponse<bool>.BadRequest();
                if (_context.YachtAttributeValues.AsNoTracking().Any(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.YachtFid == updateModel.YachtFid))
                {
                    if (updateModel.ListAttributeId.Count() == 0 && updateModel.ListAttributeValue.Count()==0)
                    {
                        var data = _context.YachtAttributeValues.Where(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.YachtFid == updateModel.YachtFid);
                        _context.RemoveRange(data);
                        _context.SaveChanges();
                        return BaseResponse<bool>.Success(true);
                    }
                    else
                    {
                        // step 1 : remove all current attribute of Yacht in db
                        var data = _context.YachtAttributeValues.Where(k => k.AttributeCategoryFid == updateModel.AttributeCategoryFid && k.YachtFid == updateModel.YachtFid);
                        _context.RemoveRange(data);
                        _context.SaveChanges();

                        // step 2: add new all attribute to db
                        if (updateModel.ListAttributeId.Count() > 0 && updateModel.ListAttributeValue.Count() > 0 && updateModel.ListAttributeId.Count() == updateModel.ListAttributeValue.Count())
                        {
                            var listAttributeId = updateModel.ListAttributeId;
                            var listAtrributeValue = updateModel.ListAttributeValue;
                            var lstattribute = new List<YachtAttributeValues>();
                            for (int i = 0; i < listAttributeId.Count; i++)
                            {
                                var attributeValue = new YachtAttributeValues()
                                {
                                    YachtFid = updateModel.YachtFid,
                                    AttributeCategoryFid = updateModel.AttributeCategoryFid,
                                    AttributeFid = listAttributeId[i],
                                    AttributeValue = listAtrributeValue[i].ToString(),
                                    EffectiveDate = DateTime.Now,
                                    LastModifiedBy = userId,
                                    LastModifiedDate = DateTime.Now
                                };
                                // add to list
                                lstattribute.Add(attributeValue);
                            }
                            if (lstattribute.Count > 0)
                            {
                                await _context.YachtAttributeValues.AddRangeAsync(lstattribute);
                                await _context.SaveChangesAsync();
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

                        var lstattribute = new List<YachtAttributeValues>();
                        for (int i = 0; i < listAttributeId.Count; i++)
                        {
                            var attributeValue = new YachtAttributeValues()
                            {
                                YachtFid = updateModel.YachtFid,
                                AttributeCategoryFid = updateModel.AttributeCategoryFid,
                                AttributeFid = listAttributeId[i],
                                AttributeValue = listAtrributeValue[i].ToString(),
                                EffectiveDate = DateTime.Now,
                                LastModifiedBy = userId,
                                LastModifiedDate = DateTime.Now
                            };
                            // add to list
                            lstattribute.Add(attributeValue);
                        }
                        if (lstattribute.Count > 0)
                        {
                            await _context.YachtAttributeValues.AddRangeAsync(lstattribute);
                            await _context.SaveChangesAsync();
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

        public async Task<BaseResponse<bool>> CreateAsync(YachtAttributeValuesCreateModels modelCreate)
        {
            try
            {
                var createItem = new YachtAttributeValues();
                createItem.InjectFrom(modelCreate);
                createItem.BasedAffective = false;
                createItem.LastModifiedBy = GetUserGuidId();
                createItem.LastModifiedDate = DateTime.Now;
                await _context.YachtAttributeValues.AddAsync(createItem);
                await _context.SaveChangesAsync();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> CreateRangeAsync(List<YachtAttributeValuesCreateModel> modelCreate)
        {
            try
            {
                var createItems = new List<YachtAttributeValues>();
                foreach (var item in modelCreate)
                {
                    var createItem = new YachtAttributeValues();
                    createItem.InjectFrom(item);
                    createItem.BasedAffective = false;
                    createItem.LastModifiedBy = GetUserGuidId();
                    createItem.LastModifiedDate = DateTime.Now;
                    createItems.Add(createItem);
                }
                if (createItems.Count() > 0)
                {
                    await _context.YachtAttributeValues.AddRangeAsync(createItems);
                    await _context.SaveChangesAsync();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtAttributeValueMgtUpdateModel>> GetListUpdateAttributeValueAsync(int yachtId, int attributeCategoryId)
        {
            var query = from c in _context.YachtAttributes.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId)
                        join d in _context.YachtAttributeValues.AsNoTracking().Where(k => k.AttributeCategoryFid == attributeCategoryId && k.YachtFid == yachtId) on c.Id equals d.AttributeFid 
                        into ps
                        from e in ps.DefaultIfEmpty()
                        select new YachtAttributeValueMgtUpdateModel()
                        {
                            Id = c.Id,
                            AttributeName = c.AttributeName,
                            AttributeValue = e.AttributeValue,
                            UniqueId = c.UniqueId,
                            IconCssClass = c.IconCssClass,                           
                            ResourceKey = c.ResourceKey,
                            Check = e.YachtFid > 0 ? true : false
                        };
            return BaseResponse<List<YachtAttributeValueMgtUpdateModel>>.Success(query.ToList());
        }
    }
}
