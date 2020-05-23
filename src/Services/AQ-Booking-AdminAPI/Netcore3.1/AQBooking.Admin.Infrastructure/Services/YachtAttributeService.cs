using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtAttribute;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.DiningEntities;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class YachtAttributeService : ServiceBase, IYachtAttributeService
    {
        public YachtAttributeService(AQYachtContext aQYacht, IWorkContext workContext, IMapper mapper) : base(aQYacht, workContext, mapper)
        {
        }

        public async Task<BasicResponse> CreateOrUpdateYachtAttribute(YachtAttributeCreateModel model)
        {
            try
            {
                var entity = _dbYachtContext.YachtAttributes.FirstOrDefault(x => x.Id == model.Id);
                if (entity != null)
                {
                    entity = _mapper.Map<YachtAttributeCreateModel, YachtAttributes>(model, entity);
                    entity.IsDefault = model.IsDefault;
                    entity.LastModifiedBy = GetCurrentUserId();
                    entity.LastModifiedDate = DateTime.Now;
                    _dbYachtContext.YachtAttributes.Update(entity);
                }
                else
                {
                    var newEntity = new YachtAttributes();
                    newEntity = _mapper.Map<YachtAttributeCreateModel, YachtAttributes>(model, newEntity);
                    newEntity.UniqueId = UniqueIDHelper.GenerateRandomString(12, false);
                    newEntity.IsDefault = model.IsDefault;
                    newEntity.CreatedBy = GetCurrentUserId();
                    newEntity.CreatedDate = DateTime.Now;
                    newEntity.LastModifiedBy = GetCurrentUserId();
                    newEntity.LastModifiedDate = DateTime.Now;
                    _dbYachtContext.YachtAttributes.Add(newEntity);
                }
                await _dbYachtContext.SaveChangesAsync();
                return BasicResponse.Succeed("Success");
            }
            catch
            {
                throw;
            }
        }

        public async Task<BasicResponse> DeleteYachtAttribute(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtAttributes.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    entity.Deleted = true;
                    _dbYachtContext.YachtAttributes.Update(entity);
                    await _dbYachtContext.SaveChangesAsync();
                    return BasicResponse.Succeed("Success");
                }
                else
                    return BasicResponse.Failed("Not Found This Yacht Attribute");
            }
            catch
            {
                throw;
            }
        }

        public YachtAttributeCreateModel GetYachtAttributeById(int id)
        {
            var entity = _dbYachtContext.YachtAttributes.FirstOrDefault(x => x.Id == id);
            var model = _mapper.Map<YachtAttributes, YachtAttributeCreateModel>(entity);
            return model; throw new NotImplementedException();
        }

        public IPagedList<YachtAttributeViewModel> SearchYachtAttributes(YachtAttributeSearchModel searchModel)
        {
            var query = _dbYachtContext.YachtAttributes.Where(x => (x.Deleted == null || !x.Deleted.Value) 
            && (string.IsNullOrEmpty(searchModel.AttributeName) || x.AttributeName.Contains(searchModel.AttributeName))
            && (searchModel.AttributeCategoryID == null || x.AttributeCategoryFid == searchModel.AttributeCategoryID)).OrderBy(x=>x.AttributeName)
            .Select(x => new YachtAttributeViewModel
            {
                Id = x.Id,
                AttributeCategoryFid = x.AttributeCategoryFid,
                AttributeName = x.AttributeName,
                IconCssClass = x.IconCssClass,
                IsDefault = x.IsDefault,
                Remarks = x.Remarks,
                ResourceKey = x.ResourceKey,
                UniqueId = x.UniqueId,
                OrderBy = x.OrderBy
            });
            return new PagedList<YachtAttributeViewModel>(query, searchModel.PageIndex, searchModel.PageSize);
        }
    }
}
