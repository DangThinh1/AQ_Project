using AccommodationMerchant.Core.Models.HotelMerchantUsers;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelMerchantUserService: ServiceBase, IHotelMerchantUserService
    {

        public HotelMerchantUserService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public BaseResponse<HotelMerchantUserViewModel> GetInfomationOfMerchantUserById(int id)
        {
            try
            {
                var entity = _db.HotelMerchantUsers.Find(id);
                if (entity == null)
                    return BaseResponse<HotelMerchantUserViewModel>.NotFound(new HotelMerchantUserViewModel());
                return BaseResponse<HotelMerchantUserViewModel>.Success(_mapper.Map<HotelMerchantUsers, HotelMerchantUserViewModel>(entity));
            }
            catch (Exception ex)
            {
                return BaseResponse<HotelMerchantUserViewModel>.InternalServerError(message: ex.Message, fullMsg:ex.StackTrace);
            }
        }


        public BaseResponse<List<HotelMerchantUserViewModel>> GetAllUserOfMerchantByMerchantId(int merchantId)
        {
            try
            {
                var entity = _db.HotelMerchantUsers.AsNoTracking().Where(x => x.MerchantFid == merchantId && x.Deleted==false).Select(s => _mapper.Map<HotelMerchantUsers, HotelMerchantUserViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<HotelMerchantUserViewModel>>.Success(entity.ToList());
                return BaseResponse<List<HotelMerchantUserViewModel>>.NoContent(new List<HotelMerchantUserViewModel>());

            }
            catch (Exception ex)
            {
                return BaseResponse<List<HotelMerchantUserViewModel>>.InternalServerError(message: ex.Message, fullMsg:ex.StackTrace);
            }
        }

        public BaseResponse<List<SelectListItem>> GetDropdownListUserOfMerchantByMerchantId(int merchantId)
        {
            try
            {
                var entity = _db.HotelMerchantUsers.Where(x => x.MerchantFid == merchantId && x.Deleted == false).OrderBy(x=>x.CreatedDate).AsNoTracking().Select( m=> new SelectListItem {  Text= m.UserName, Value= m.UserFid.ToString()});
                if (entity.Count() > 0)
                    return BaseResponse<List<SelectListItem>>.Success(entity.ToList());
                return BaseResponse<List<SelectListItem>>.NoContent(new List<SelectListItem>());
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SelectListItem>>.InternalServerError(message: ex.Message,fullMsg:ex.StackTrace);
            }
        }


        public BaseResponse<List<HotelMerchantUserViewModel>> GetAllUserOfMerchantByRole(HotelMerchantUserRequestGetAllUserWithRolesOfMerchantModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<List<HotelMerchantUserViewModel>>.BadRequest();
                var entity = _db.HotelMerchantUsers.AsNoTracking().Where(x => x.MerchantFid == model.MerchantId && x.MerchantUserRoleFid== model.Role && x.Deleted == false).Select(s => _mapper.Map<HotelMerchantUsers, HotelMerchantUserViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<HotelMerchantUserViewModel>>.Success(entity.ToList());
                else
                    return BaseResponse<List<HotelMerchantUserViewModel>>.NoContent(new List<HotelMerchantUserViewModel>());

            }
            catch (Exception ex)
            {
                return BaseResponse<List<HotelMerchantUserViewModel>>.InternalServerError(message: ex.Message);
            }
        }


        public async Task<BaseResponse<bool>> CreateMerchantUser(HotelMerchantUserCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest(false);
                var entity = new HotelMerchantUsers();
                entity.InjectFrom(model);
                
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;
                await _db.HotelMerchantUsers.AddAsync(entity);
                await _db.SaveChangesAsync();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }


        public async Task<BaseResponse<bool>> UpdateMerchantUser(HotelMerchantUserUpdateModel model)
        {
            try
            {
                if( model==null )
                    return BaseResponse<bool>.BadRequest();
                var entity = _db.HotelMerchantUsers.Find(model.Id);
                if (entity != null)
                {
                    entity.InjectFrom(model);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _db.HotelMerchantUsers.Update(entity);
                    await _db.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                else
                    return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }



        public async Task<BaseResponse<bool>> DeleteMerchantUser(int id)
        {
            try
            {
                var entity = _db.HotelMerchantUsers.Find(id);
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _db.HotelMerchantUsers.Update(entity);
                    await _db.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                else
                    return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message);
            }
        }

        
    }
}
