using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Core.Models;
using YachtMerchant.Core.Models.YachtAttribute;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using AQBooking.Core.Helpers;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtAttributeService : ServiceBase,IYachtAttributeService
    {

        public YachtAttributeService(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
        }

        public async Task<BaseResponse<bool>> CreateAsync(YachtAttributeCreateModel modelCreate)
        {
            try
            {
                var createItem = new YachtAttributes();
                createItem.InjectFrom(modelCreate);
                createItem.Deleted = false;
                createItem.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                createItem.CreatedBy = GetUserGuidId();
                createItem.CreatedDate = DateTime.Now;
                createItem.LastModifiedBy = GetUserGuidId();
                createItem.LastModifiedDate = DateTime.Now;
                await _context.YachtAttributes.AddAsync(createItem);
                await _context.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }


        public async Task<BaseResponse<bool>> UpdateAsync(YachtAttributeUpdateModel model)
        {
            try
            {
                var entity =  _context.YachtAttributes.AsNoTracking().FirstOrDefault( k=>k.Deleted==false && k.Id==model.Id);

                if( entity!=null)
                {
                    entity.AttributeCategoryFid = model.AttributeCategoryFid;
                    entity.AttributeName = model.AttributeName;
                    entity.ResourceKey = model.ResourceKey;
                    entity.IconCssClass = model.IconCssClass;
                    entity.Remarks = model.Remarks;
                    entity.IsDefault = model.IsDefault;
                    entity.OrderBy = model.OrderBy;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;
                    int checkResult = await _context.SaveChangesAsync();
                    return BaseResponse<bool>.Success(true);
                }
                else
                    return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtAttributeViewModel>> SearchAsync(YachtAttributeSearchModel searchModel)
        {
            var query = (from c in _context.YachtAttributes.AsNoTracking().Where(k => k.Deleted ==false &&  k.AttributeCategoryFid== searchModel.AttributeCategoryFid)

                         select new YachtAttributeViewModel()
                         {
                             Id = c.Id,
                              AttributeCategoryFid= c.AttributeCategoryFid,
                               AttributeName= c.AttributeName,
                                CreatedBy= c.CreatedBy,
                                 CreatedDate= c.CreatedDate,
                                  IconCssClass= c.IconCssClass,
                                   IsDefault= c.IsDefault,
                                    LastModifiedBy= c.LastModifiedBy,
                                     LastModifiedDate= c.LastModifiedDate,
                                      OrderBy= c.OrderBy,
                                       Remarks= c.Remarks,
                                        ResourceKey= c.ResourceKey,
                                         UniqueId= c.UniqueId
                         });

            return BaseResponse<PagedList<YachtAttributeViewModel>>.Success( new PagedList<YachtAttributeViewModel>(query, 1, 10));

        }

        public async Task<BaseResponse<YachtAttributeViewModel>> FindByIdAsync(int id)
        {
            var entity = await _context.YachtAttributes.FindAsync(id);
            if (entity != null)
            {
                var viewModel = new YachtAttributeViewModel();
                viewModel.InjectFrom(entity);
                return BaseResponse<YachtAttributeViewModel>.Success(viewModel);
            }
            else
                return BaseResponse<YachtAttributeViewModel>.BadRequest();
        }

        public BaseResponse<YachtAttributeViewModel> FindByNameAsync(string attributeName)
        {
            var entity = _context.YachtAttributes.FirstOrDefault(p => p.AttributeName.Equals(attributeName));
            if (entity != null)
            {
                var viewModel = new YachtAttributeViewModel();
                viewModel.InjectFrom(entity);
                return BaseResponse < YachtAttributeViewModel > .Success(viewModel);
            }
            else
                return BaseResponse<YachtAttributeViewModel>.NoContent(new  YachtAttributeViewModel());
        }

        public BaseResponse<List<YachtAttributeViewModel>> SearchByCategoryIdAsync(int categoryId)
        {
            var result =  _context.YachtAttributes.AsNoTracking().Where(p => p.AttributeCategoryFid.Equals(categoryId)).Select(p=>new YachtAttributeViewModel() {
                Id = p.Id,
                 AttributeCategoryFid= p.AttributeCategoryFid,
                  AttributeName= p.AttributeName,
                   CreatedBy= p.CreatedBy,
                    CreatedDate= p.CreatedDate,
                     IconCssClass= p.IconCssClass,
                      IsDefault= p.IsDefault,
                       LastModifiedBy= p.LastModifiedBy,
                        LastModifiedDate= p.LastModifiedDate,
                         OrderBy= p.OrderBy,
                          Remarks= p.Remarks,
                           ResourceKey= p.ResourceKey,
                            UniqueId= p.UniqueId
            });

            if (result.Count() > 0)
                return BaseResponse < List < YachtAttributeViewModel >>.Success( result.ToList());
            return  BaseResponse<List<YachtAttributeViewModel>>.BadRequest();
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var res = _context.YachtAttributes.FirstOrDefault(r => r.Deleted == false && r.Id == id);
                if (res == null)
                    return BaseResponse<bool>.NoContent();
                res.Deleted = true;
                res.LastModifiedBy = GetUserGuidId();
                res.LastModifiedDate = DateTime.Now;
                await  _context.SaveChangesAsync();
                return  BaseResponse<bool>.Success(true);

            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
            }
        }
    }
}