using System;
using System.Linq;
using APIHelpers.Response;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Core.Models.CommonLanguages;
using AQConfigurations.Infrastructure.Services.Interfaces;
using AutoMapper;
using AQConfigurations.Infrastructure.Databases.Entities;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class CommonLanguagesServices : ICommonLanguagesServices
    {
        private readonly IMapper _mapper;
        private readonly AQConfigurationsDbContext _commonContext;
        public CommonLanguagesServices(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _mapper = mapper;
            _commonContext = commonContext;
        }

        public BaseResponse<List<CommonLanguagesViewModel>> GetAllLanguages()
        {
            try
            {
                var result = _commonContext.CommonLanguages
                    .AsNoTracking()
                    .Where(k => !k.Deleted)
                    .Select(k => _mapper.Map<CommonLanguages, CommonLanguagesViewModel>(k))
                    .ToList();

                return BaseResponse<List<CommonLanguagesViewModel>>.Success(result);
            }
            catch(Exception ex)
            {
                return BaseResponse<List<CommonLanguagesViewModel>>.InternalServerError(message:ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<string> GetLanguageCommonValue(int languageId)
        {
            try
            {
                var query = from l in _commonContext.CommonLanguages.AsNoTracking().Where(l => l.Deleted == false && l.Id == languageId)
                            join c in _commonContext.CommonValues.AsNoTracking() on l.ResourceKey equals c.ResourceKey
                            select c.Text;
                var result = query.FirstOrDefault();
                return BaseResponse<string>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}