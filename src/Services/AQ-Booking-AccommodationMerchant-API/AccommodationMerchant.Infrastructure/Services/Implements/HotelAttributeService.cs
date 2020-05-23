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
    public class HotelAttributeService : ServiceBase, IHotelAttributeService
    {

        public HotelAttributeService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<BaseResponse<bool>> CreateAsync(HotelAttributeCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var createItem = new HotelAttributes();
                createItem.InjectFrom(model);
                createItem.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                createItem.Deleted = false;
                createItem.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                createItem.CreatedBy = GetUserGuidId();
                createItem.CreatedDate = DateTime.UtcNow;
                createItem.LastModifiedBy = GetUserGuidId();
                createItem.LastModifiedDate = DateTime.UtcNow;
                await _db.HotelAttributes.AddAsync(createItem);
                await _db.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }


        public async Task<BaseResponse<bool>> UpdateAsync(HotelAttributeUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var entity =  _db.HotelAttributes.AsNoTracking().FirstOrDefault( k=>k.Deleted==false && k.Id==model.Id);

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

        public BaseResponse<PagedList<HotelAttributeViewModel>> Search(HotelAttributeSearchModel searchModel)
        {
            if (searchModel == null)
                return BaseResponse<PagedList<HotelAttributeViewModel>>.BadRequest();
            var entity = _db.HotelAttributes.AsNoTracking().Where(k => k.Deleted == false && k.AttributeCategoryFid == searchModel.AttributeCategoryFid).Select(s => _mapper.Map<HotelAttributes, HotelAttributeViewModel>(s));
            if(entity.Count()>0)
                return BaseResponse<PagedList<HotelAttributeViewModel>>.Success( new PagedList<HotelAttributeViewModel>(entity, 1, 10));
            return  BaseResponse<PagedList<HotelAttributeViewModel>>.NoContent(new PagedList<HotelAttributeViewModel>());
        }

        public async Task<BaseResponse<HotelAttributeViewModel>> FindByIdAsync(int id)
        {
            var entity = await _db.HotelAttributes.FindAsync(id);
            if (entity != null)
            {
                var viewModel = new HotelAttributeViewModel();
                viewModel.InjectFrom(entity);
                return BaseResponse<HotelAttributeViewModel>.Success(viewModel);
            }
            return BaseResponse<HotelAttributeViewModel>.NoContent(new HotelAttributeViewModel());
        }

        public async Task<BaseResponse<HotelAttributeViewModel>> FindByNameAsync(string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName))
                return BaseResponse<HotelAttributeViewModel>.BadRequest();
            var entity = await _db.HotelAttributes.FirstOrDefaultAsync(p => p.AttributeName.Equals(attributeName));
            if (entity != null)
            {
                var viewModel = new HotelAttributeViewModel();
                viewModel.InjectFrom(entity);
                return BaseResponse<HotelAttributeViewModel>.Success(viewModel);
            }
            return BaseResponse<HotelAttributeViewModel>.NoContent(new HotelAttributeViewModel());
        }

        public BaseResponse<List<HotelAttributeViewModel>> SearchByCategoryId(int categoryId)
        {
            var result = _db.HotelAttributes.AsNoTracking().Where(p => p.AttributeCategoryFid.Equals(categoryId)).Select(s => _mapper.Map<HotelAttributes, HotelAttributeViewModel>(s));
            if (result.Count() > 0)
                return BaseResponse<List<HotelAttributeViewModel>>.Success(result.ToList());
            return  BaseResponse<List<HotelAttributeViewModel>>.NoContent(new List<HotelAttributeViewModel>());
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var res = _db.HotelAttributes.FirstOrDefault(r => r.Deleted == false && r.Id == id);
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