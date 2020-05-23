using System;
using System.Linq;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Currencies;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Infrastructure.Databases.Entities;
using AQConfigurations.Infrastructure.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class CurrencyService : ICurrencyService
    {
        private const string CURRENCY_VALUE_GROUP = "CURRENCY";
        private readonly IMapper _mapper;

        private readonly AQConfigurationsDbContext _commonContext;
        public CurrencyService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _mapper = mapper;
            _commonContext = commonContext;
        }

        public BaseResponse<List<CurrencyViewModel>> All()
        {
            try
            {
                var currencies = _commonContext.Currencies
                    .AsNoTracking()
                    .Select(k=> _mapper.Map<Currencies, CurrencyViewModel>(k))
                    .ToList();
                if (currencies != null)
                    return BaseResponse<List<CurrencyViewModel>>.Success(currencies);

                return BaseResponse<List<CurrencyViewModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<List<CurrencyViewModel>>.InternalServerError(ex);
            }
        }

        public BaseResponse<CurrencyViewModel> Find(string currencyCode)
        {
            try
            {
                if(string.IsNullOrEmpty(currencyCode))
                    return BaseResponse<CurrencyViewModel>.BadRequest();

                var crrEntity = _commonContext.Currencies.Find(currencyCode);
                if(crrEntity != null)
                {
                    var crrViewModel = _mapper.Map<Currencies, CurrencyViewModel>(crrEntity);
                    if (crrViewModel != null)
                        return BaseResponse<CurrencyViewModel>.Success(crrViewModel);
                }

                return BaseResponse<CurrencyViewModel>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<CurrencyViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CurrencyViewModel> FindByCountryName(string countryName)
        {
            try
            {
                if (string.IsNullOrEmpty(countryName))
                  return BaseResponse<CurrencyViewModel>.BadRequest();

                var crrViewModel = (from c in _commonContext.Currencies
                                    join v in _commonContext.CommonValues
                                    on c.CurrencyCode equals v.ValueString
                                    where c.Country.ToUpper() == countryName.ToUpper() && v.ValueGroup.ToUpper() == CURRENCY_VALUE_GROUP.ToUpper()
                                    select new CurrencyViewModel
                                    {
                                        CurrencyCode = c.CurrencyCode,
                                        CultureCode = c.CultureCode,
                                        ResourceKey = c.ResourceKey,
                                        Country = c.Country,
                                        City = string.Empty,
                                        Text = v.Text
                                    }).FirstOrDefault();

                if (crrViewModel != null)
                    return BaseResponse<CurrencyViewModel>.Success(crrViewModel);

                return BaseResponse<CurrencyViewModel>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<CurrencyViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
