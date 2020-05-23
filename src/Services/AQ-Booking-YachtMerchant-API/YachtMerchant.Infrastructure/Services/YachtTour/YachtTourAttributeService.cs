using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AutoMapper;
using Identity.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Models.YachtTourAttributes;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourAttributeService : IYachtTourAttributeService
    {
        private readonly IMapper _mapper;
        private readonly YachtOperatorDbContext _db;
        public YachtTourAttributeService(YachtOperatorDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public BaseResponse<List<YachtTourAttributeViewModel>> All()
        {
            try
            {
                var entities = _db.YachtTourAttributes
                    .AsNoTracking()
                    .Where(k => !k.Deleted)
                    .Select(k => _mapper.Map<YachtTourAttributes, YachtTourAttributeViewModel>(k))
                    .OrderBy(k => k.OrderBy)
                    .ThenBy(k=> k.Id)
                    .ToList();
                return entities != null
                    ? BaseResponse<List<YachtTourAttributeViewModel>>.Success(entities)
                    : BaseResponse<List<YachtTourAttributeViewModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<List<YachtTourAttributeViewModel>>.InternalServerError(message: ex.Message, fullMsg:ex.StackTrace);
            }
        }

        public BaseResponse<bool> Create(YachtTourAttributeCreateModel model)
        {
            try
            {
                var entity = GenerateForCreate(model);
                _db.YachtTourAttributes.Add(entity);
                var result = _db.SaveChanges();
                return result > 0
                    ? BaseResponse<bool>.Success(true)
                    : BaseResponse<bool>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(YachtTourAttributeUpdateModel model, int id)
        {
            try
            {
                var entity = GenerateForUpdate(_db.YachtTourAttributes.FirstOrDefault(k => !k.Deleted && k.Id == id));
                entity.InjectFrom(model);
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

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var entity = GenerateForUpdate(_db.YachtTourAttributes.FirstOrDefault(k => !k.Deleted && k.Id == id));
                entity.Deleted = true;
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

        #region Private Methods

        private YachtTourAttributes GenerateForCreate(YachtTourAttributeCreateModel model)
        {
            var entity = new YachtTourAttributes();
            entity.InjectFrom(model);
            entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            entity.LastModifiedBy = userId;
            entity.CreatedBy = userId;
            entity.LastModifiedDate = now;
            entity.CreatedDate = now;
            return entity;
        }

        private YachtTourAttributes GenerateForUpdate(YachtTourAttributes entity)
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