using APIHelpers.Response;
using AQConfigurations.Core.Models.PortalLanguages;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Infrastructure.Databases.Entities;
using AQConfigurations.Infrastructure.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class PortalLanguageService : IPortalLanguageService
    {
        private readonly IMapper _mapper;
        private readonly AQConfigurationsDbContext _commonContext;

        public PortalLanguageService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _mapper = mapper;
            _commonContext = commonContext;
        }

        public BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByPortalUID(string portalUniqueID)
        {
            try
            {
                var listLanguage = _commonContext.PortalLanguageControls
                .AsNoTracking()
                .Include(l => l.Language)
                .Where(k =>
                       k.PortalUniqueId == portalUniqueID &&
                       k.Deleted == false &&
                       k.IsActive == true &&
                       k.EffectiveDate.Date <= DateTime.Now.Date)
                .OrderBy(k => k.LanguageFid)
                .Select(k => _mapper.Map<PortalLanguageControls, PortalLanguageViewModel>(k))
                .ToList();
                if (listLanguage.Count == 0)
                {
                    listLanguage.Add(new PortalLanguageViewModel() {
                    });
                }
                return BaseResponse<List<PortalLanguageViewModel>>.Success(listLanguage);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLanguageViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByDomainId(int domainId)
        {
            try
            {
                var listLanguage = _commonContext.PortalLanguageControls
                .AsNoTracking()
                .Include(l => l.Language)
                .Where(k =>
                       k.DomainPortalFid == domainId &&
                       k.Deleted == false &&
                       k.IsActive == true &&
                       k.EffectiveDate.Date <= DateTime.Now.Date)
                .OrderBy(k => k.LanguageFid)
                .Select(k => _mapper.Map<PortalLanguageControls, PortalLanguageViewModel>(k))
                .ToList();
                if (listLanguage.Count == 0)
                {
                    listLanguage.Add(new PortalLanguageViewModel());
                }
                return BaseResponse<List<PortalLanguageViewModel>>.Success(listLanguage);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLanguageViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
