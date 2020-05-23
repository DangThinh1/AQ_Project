using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtTourAttribute;
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
    public class YachtTourAttributeService : ServiceBase, IYachtTourAttributeService
    {
        
        public YachtTourAttributeService(AQYachtContext aQYacht, IWorkContext workContext, IMapper mapper) : base(aQYacht, workContext, mapper)
        {
            
        }

        public async Task<BasicResponse> CreateOrUpdateYachtTourAttribute(YachtTourAttributeCreateModel model)
        {
            try
            {
                var entity = _dbYachtContext.YachtTourAttributes.FirstOrDefault(x => x.Id == model.Id);
                if (entity != null)
                {
                    entity = _mapper.Map<YachtTourAttributeCreateModel, YachtTourAttributes>(model, entity);
                    entity.IsDefault = model.IsDefault;
                    entity.LastModifiedBy = GetCurrentUserId();
                    entity.LastModifiedDate = DateTime.Now;
                    _dbYachtContext.YachtTourAttributes.Update(entity);
                }
                else
                {
                    var newEntity = new YachtTourAttributes();
                    newEntity = _mapper.Map<YachtTourAttributeCreateModel, YachtTourAttributes>(model, newEntity);
                    newEntity.UniqueId = UniqueIDHelper.GenerateRandomString(12, false);
                    newEntity.IsDefault = model.IsDefault;
                    newEntity.CreatedBy = GetCurrentUserId();
                    newEntity.CreatedDate = DateTime.Now;
                    newEntity.LastModifiedBy = GetCurrentUserId();
                    newEntity.LastModifiedDate = DateTime.Now;
                    _dbYachtContext.YachtTourAttributes.Add(newEntity);
                }
                await _dbYachtContext.SaveChangesAsync();
                return BasicResponse.Succeed("Success");
            }
            catch
            {
                throw;
            }
        }

        public async Task<BasicResponse> DeleteYachtTourAttribute(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtTourAttributes.FirstOrDefault(x => x.Id == id);
                if (entity != null)
                {
                    entity.Deleted = true;
                    _dbYachtContext.YachtTourAttributes.Update(entity);
                    await _dbYachtContext.SaveChangesAsync();
                    return BasicResponse.Succeed("Success");
                }
                else
                   return BasicResponse.Failed("Not Found Yacht Tour");
            }
            catch
            {
                throw;
            }
        }

        public YachtTourAttributeCreateModel GetYachtTourAttributeById(int id)
        {
            var entity = _dbYachtContext.YachtTourAttributes.FirstOrDefault(x => x.Id == id);
            var model = _mapper.Map<YachtTourAttributes,YachtTourAttributeCreateModel>(entity);
            return model;
        }

        public IPagedList<YachtTourAttributeViewModel> SearchYachtTourAttributes(YachtTourAttributeSearchModel searchModel)
        {
            var query = _dbYachtContext.YachtTourAttributes.Where(x => !x.Deleted
            && (string.IsNullOrEmpty(searchModel.AttributeName) || x.AttributeName.Contains(searchModel.AttributeName))
            && (searchModel.AttributeCategoryID == null || x.AttributeCategoryFid == searchModel.AttributeCategoryID)).OrderByDescending(x=>x.OrderBy)
            .Select(x=> new YachtTourAttributeViewModel
            {
                Id = x.Id,
                AttributeCategoryFid =x.AttributeCategoryFid,
                AttributeName = x.AttributeName,
                IconCssClass = x.IconCssClass,
                IsDefault = x.IsDefault,
                Remarks = x.Remarks,
                ResourceKey = x.ResourceKey,
                UniqueId = x.UniqueId,
                OrderBy = x.OrderBy
            });
            return new PagedList<YachtTourAttributeViewModel>(query, searchModel.PageIndex, searchModel.PageSize);
        }
    }
}
