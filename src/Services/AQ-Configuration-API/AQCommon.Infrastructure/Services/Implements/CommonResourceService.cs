using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Core.Models.CommonResources;
using AQConfigurations.Infrastructure.Databases.Entities;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Omu.ValueInjecter;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class CommonResourceService : ICommonResourceService
    {
        private readonly AQConfigurationsDbContext _commonContext;
        private readonly IMapper _mapper;
        public CommonResourceService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _commonContext = commonContext;
            _mapper = mapper;
        }

        public BaseResponse<List<CommonResourceViewModel>> GetAllResource(int languageID, List<string> type = null)
        {
            try
            {
                var typesearch = new List<string>() { "COMMON" };
                if (type != null && type.Count > 0)
                {
                    for (int i = 0; i < type.Count; i++)
                    {
                        type[i] = type[i].ToUpper();
                    }
                    typesearch = type;
                }

                var resources = _commonContext.CommonResources
                                .AsNoTracking()
                                .Where(k => typesearch.Contains(k.TypeOfResource.ToUpper()) && k.LanguageFid == languageID)
                                .Select(k=> _mapper.Map<CommonResources, CommonResourceViewModel>(k))
                                .ToList();

                if (resources != null)
                    return BaseResponse<List<CommonResourceViewModel>>.Success(resources);

                return BaseResponse<List<CommonResourceViewModel>>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonResourceViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<string> GetResourceValue(int lang, string resourceKey)
        {
            try
            {
                if (string.IsNullOrEmpty(resourceKey))
                    return BaseResponse<string>.BadRequest();

                var resourceValue = _commonContext.CommonResources
                                    .AsNoTracking()
                                    .Where(k => k.LanguageFid == lang && k.ResourceKey.Trim().ToUpper() == resourceKey.Trim().ToUpper())
                                    .Select(k => k.ResourceValue)
                                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(resourceValue))
                    return BaseResponse<string>.Success(resourceValue);

                return BaseResponse<string>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError(ex.Message, ex.StackTrace);
            }
        }

        public BaseResponse<bool> Create(CommonResourceCreateModel model)
        {
            try
            {
                if(model != null && !string.IsNullOrEmpty(model.ResourceKey) && !string.IsNullOrEmpty(model.ResourceValue))
                {
                    var entity = new CommonResources();
                    entity.InjectFrom(model);
                    _commonContext.CommonResources.Add(entity);
                    _commonContext.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Update(CommonResourceUpdateModel model)
        {
            try
            {
                var entity = _commonContext.CommonResources
                    .FirstOrDefault(k=> k.ResourceKey.ToUpper() == model.ResourceKey.ToUpper() && k.LanguageFid == model.LanguageFid);
                if(entity != null)
                {
                    entity.InjectFrom(model);
                    _commonContext.CommonResources.Update(entity);
                    _commonContext.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }
    }
}
