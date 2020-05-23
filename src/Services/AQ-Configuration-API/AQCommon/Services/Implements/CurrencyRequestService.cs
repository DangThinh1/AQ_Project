using System;
using System.Collections.Generic;
using APIHelpers.Response;
using AQConfigurations.Core.Models.Currencies;
using AQConfigurations.Core.Services.Interfaces;

namespace AQConfigurations.Core.Services.Implements
{
    public class CurrencyRequestService : ConfigurationsRequestServiceBase, ICurrencyRequestService
    {
        private const string ALL_URL = "api/Currencies";
        private const string FIND_URL = "api/Currencies/{0}";
        private const string FIND_BY_COUNTRY_NAME_URL = "api/Currencies/CountryName/{0}";
        public CurrencyRequestService() : base()
        {
            Console.Write("CurrencyRequestService Init:  _configurationsHost = " + _configurationsHost);
        }

        public BaseResponse<List<CurrencyViewModel>> All(string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = ALL_URL;

                string url = _configurationsHost + actionUrl;
                var response = Get<List<CurrencyViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CurrencyViewModel>>.InternalServerError(ex);
            }
        }

        public BaseResponse<CurrencyViewModel> Find(string currencyCode, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(FIND_URL, currencyCode);

                string url = _configurationsHost + actionUrl;
                Console.Write("CurrencyRequestService Find:  url = " + _configurationsHost);
                var response = Get<CurrencyViewModel>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CurrencyViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CurrencyViewModel> FindByCountryName(string countryName, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(FIND_BY_COUNTRY_NAME_URL, countryName);

                string url = _configurationsHost + actionUrl;
                Console.Write("CurrencyRequestService FindByCountryName:  url = " + _configurationsHost);
                var response = Get<CurrencyViewModel>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CurrencyViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}