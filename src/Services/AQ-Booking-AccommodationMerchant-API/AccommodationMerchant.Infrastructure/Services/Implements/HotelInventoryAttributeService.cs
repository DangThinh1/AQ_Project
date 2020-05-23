using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using AQBooking.Core.Helpers;
using APIHelpers.Response;
using AccommodationMerchant.Infrastructure.Databases;
using AutoMapper;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AccommodationMerchant.Core.Models.HotelAttributes;
using AccommodationMerchant.Infrastructure.Databases.Entities;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelInventoryAttributeService : ServiceBase, IHotelInventoryAttributeService
    {

        public HotelInventoryAttributeService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<BaseResponse<bool>> CreateAsync(HotelInventoryAttributeCreateModel model)
        {
            try
            {
                if(model ==null)
                    return BaseResponse<bool>.BadRequest();
                var createItem = new HotelInventoryAttributes();
                createItem.InjectFrom(model);
                createItem.Deleted = false;
                createItem.CreatedBy = GetUserGuidId();
                createItem.CreatedDate = DateTime.UtcNow;
                createItem.LastModifiedBy = GetUserGuidId();
                createItem.LastModifiedDate = DateTime.UtcNow;
                await _db.HotelInventoryAttributes.AddAsync(createItem);
                await _db.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }


        public async Task<BaseResponse<bool>> UpdateAsync(HotelInventoryAttributeUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var entity =  _db.HotelInventoryAttributes.AsNoTracking().FirstOrDefault( k=>k.Deleted==false && k.Id==model.Id);
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
                    entity.LastModifiedDate = DateTime.UtcNow;
                    int checkResult = await _db.SaveChangesAsync();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<HotelInventoryAttributeViewModel>> Search(HotelInventoryAttributeSearchModel searchModel)
        {
            try
            {
                if (searchModel == null)
                    return BaseResponse<PagedList<HotelInventoryAttributeViewModel>>.BadRequest();
                var entity = _db.HotelInventoryAttributes.AsNoTracking().Where(k => k.Deleted == false && k.AttributeCategoryFid == searchModel.AttributeCategoryFid).Select(s => _mapper.Map<HotelInventoryAttributes, HotelInventoryAttributeViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<PagedList<HotelInventoryAttributeViewModel>>.Success(new PagedList<HotelInventoryAttributeViewModel>(entity, 1, 10));
                return BaseResponse<PagedList<HotelInventoryAttributeViewModel>>.NoContent(new PagedList<HotelInventoryAttributeViewModel>());
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<HotelInventoryAttributeViewModel>>.InternalServerError(ex);
            }
            
        }

        public async Task<BaseResponse<HotelInventoryAttributeViewModel>> FindByIdAsync(int id)
        {
            try
            {
                var entity = await _db.HotelInventoryAttributes.FindAsync(id);
                if (entity != null)
                {
                    var viewModel = new HotelInventoryAttributeViewModel();
                    viewModel.InjectFrom(entity);
                    return BaseResponse<HotelInventoryAttributeViewModel>.Success(viewModel);
                }
                return BaseResponse<HotelInventoryAttributeViewModel>.NoContent(new HotelInventoryAttributeViewModel());
            }
            catch(Exception ex)
            {
                return BaseResponse<HotelInventoryAttributeViewModel>.InternalServerError(ex);
            }
            
        }

        public async Task<BaseResponse<HotelInventoryAttributeViewModel>> FindByNameAsync(string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName))
                return BaseResponse<HotelInventoryAttributeViewModel>.BadRequest();
            var entity = await _db.HotelInventoryAttributes.FirstOrDefaultAsync(p => p.AttributeName.Equals(attributeName));
            if (entity != null)
            {
                var viewModel = new HotelInventoryAttributeViewModel();
                viewModel.InjectFrom(entity);
                return BaseResponse<HotelInventoryAttributeViewModel>.Success(viewModel);
            }
            return BaseResponse<HotelInventoryAttributeViewModel>.NoContent(new HotelInventoryAttributeViewModel());
        }

        public BaseResponse<List<HotelInventoryAttributeViewModel>> SearchByCategoryId(int categoryId)
        {
            var result = _db.HotelInventoryAttributes.AsNoTracking().Where(p => p.AttributeCategoryFid.Equals(categoryId)).Select(s => _mapper.Map<HotelInventoryAttributes, HotelInventoryAttributeViewModel>(s));
            if (result.Count() > 0)
                return BaseResponse<List<HotelInventoryAttributeViewModel>>.Success(result.ToList());
            return  BaseResponse<List<HotelInventoryAttributeViewModel>>.NoContent(new List<HotelInventoryAttributeViewModel>());
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var res = _db.HotelInventoryAttributes.FirstOrDefault(r => r.Deleted == false && r.Id == id);
                if (res == null)
                {
                    return BaseResponse<bool>.NoContent();
                }
                res.Deleted = true;
                res.LastModifiedBy = GetUserGuidId();
                res.LastModifiedDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return  BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
            }
        }
    }
}