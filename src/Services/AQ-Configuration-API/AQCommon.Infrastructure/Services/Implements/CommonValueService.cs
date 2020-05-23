using System;
using AutoMapper;
using System.Linq;
using ExtendedUtility;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using AQConfigurations.Core.Enums;
using Microsoft.EntityFrameworkCore;
using AQConfigurations.Core.Models.CommonValues;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Infrastructure.Databases.Entities;
using AQConfigurations.Infrastructure.Services.Interfaces;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class CommonValueService : ICommonValueService
    {
        private readonly AQConfigurationsDbContext _commonContext;
        private readonly IMapper _mapper;

        public CommonValueService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _commonContext = commonContext;
            _mapper = mapper;
        }

        public BaseResponse<bool> Create(CommonValueCreateModel model)
        {
            try
            {
                var entity = _mapper.Map<CommonValueCreateModel, CommonValues>(model);
                entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                _commonContext.CommonValues.Add(entity);
                _commonContext.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<CommonValueViewModel> Find(int id, int lang = 1)
        {
            try
            {
                var cmValue = _commonContext.CommonValues
                    .AsNoTracking()
                    .Select(k => _mapper.Map<CommonValues, CommonValueViewModel>(k))
                    .FirstOrDefault(k=> k.Id == id);
                if (cmValue != null)
                {
                    cmValue = LoadResouceValue(cmValue, lang);
                    return BaseResponse<CommonValueViewModel>.Success(cmValue);
                }

                return BaseResponse<CommonValueViewModel>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(ex);
            }
        }

        public BaseResponse<List<CommonValueViewModel>> GetListCommonValueByGroup(string valueGroup, int lang = 1, SortTypeEnum sortType = SortTypeEnum.Ascending)
        {
            try
            {
                var result = _commonContext.CommonValues
                    .AsNoTracking()
                    .Where(e => valueGroup.ToUpper().Equals(e.ValueGroup.ToUpper()))
                    .Select(k => _mapper.Map<CommonValues, CommonValueViewModel>(k))
                    .OrderBy("OrderBy " + sortType.ToString())
                    .ToList();
                result = LoadResouceValue(result, lang);
                return BaseResponse<List<CommonValueViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonValueViewModel>>.InternalServerError(new List<CommonValueViewModel>(), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CommonValueViewModel> GetCommonValueByGroupDouble(string valueGroup, double valueDouble, int lang = 1)
        {
            try
            {
                if (!valueGroup.HaveValue())
                    return BaseResponse<CommonValueViewModel>.NotFound();

                var result = _commonContext.CommonValues
                    .AsNoTracking()
                    .Select(k => _mapper.Map<CommonValues, CommonValueViewModel>(k))
                    .FirstOrDefault(e => valueGroup.ToUpper().Equals(e.ValueGroup.ToUpper()) && e.ValueDouble == valueDouble);
                result = LoadResouceValue(result, lang);
                return BaseResponse<CommonValueViewModel>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CommonValueViewModel> GetCommonValueByGroupInt(string valueGroup, int valueInt, int lang = 1)
        {
            try
            {
                if (!valueGroup.HaveValue())
                    return BaseResponse<CommonValueViewModel>.NotFound(null);
                var result = _commonContext.CommonValues
                    .AsNoTracking()
                    .Select(k => _mapper.Map<CommonValues, CommonValueViewModel>(k))
                    .FirstOrDefault(e => valueGroup.ToUpper().Equals(e.ValueGroup.ToUpper()) && e.ValueInt == valueInt);
                result = LoadResouceValue(result, lang);
                return BaseResponse<CommonValueViewModel>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CommonValueViewModel> GetCommonValueByGroupString(string valueGroup, string valueString, int lang = 1)
        {
            try
            {
                if (!valueGroup.HaveValue() || !valueString.HaveValue())
                    return BaseResponse<CommonValueViewModel>.NotFound();
                var result = _commonContext.CommonValues
                    .AsNoTracking()
                    .Select(k => _mapper.Map<CommonValues, CommonValueViewModel>(k))
                        .FirstOrDefault(e => valueGroup.ToUpper().Equals(e.ValueGroup.ToUpper()) && e.ValueString == valueString);
                result = LoadResouceValue(result, lang);
                return BaseResponse<CommonValueViewModel>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CommonValueViewModel>> GetAllCommonValue(int lang = 1)
        {
            try
            {
                var result = _commonContext.CommonValues
                    .AsNoTracking()
                    .Select(k => _mapper.Map<CommonValues, CommonValueViewModel>(k)).ToList();
                result = LoadResouceValue(result, lang);
                return BaseResponse<List<CommonValueViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonValueViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        private CommonValueViewModel LoadResouceValue(CommonValueViewModel model, int lang)
        {
            if(!string.IsNullOrEmpty(model.ResourceKey))
            {
                var commonResouce = _commonContext.CommonResources.FirstOrDefault(k => k.ResourceKey.ToUpper() == model.ResourceKey.ToUpper() && k.LanguageFid == lang);
                if (commonResouce != null && !string.IsNullOrEmpty(commonResouce.ResourceValue))
                    model.ResourceValue = commonResouce.ResourceValue;
                else
                    model.ResourceValue = model.ResourceKey;
            }
            return model;
        }

        private List<CommonValueViewModel> LoadResouceValue(List<CommonValueViewModel> models, int lang)
        {
            var result = new List<CommonValueViewModel>();
            foreach(var m in models)
            {
                result.Add(LoadResouceValue(m, lang));
            }
            return result;
        }
    }
}