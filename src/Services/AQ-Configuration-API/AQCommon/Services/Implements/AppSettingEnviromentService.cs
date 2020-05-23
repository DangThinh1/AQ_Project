using System;
using Microsoft.Extensions.Configuration;
using AQConfigurations.Core.Services.Interfaces;

namespace AQConfigurations.Core.Services.Implements
{
    public class AppSettingEnvironmentService : IAppSettingEnvironmentService
    {
        private const string ENVIRONMENT_SCHEME = "SettingEnvironment";
        private readonly IConfiguration _configuration;
        public string EnvironmentScheme { get; }
        public int Enviroment => GetEnvironment(EnvironmentScheme);

        public AppSettingEnvironmentService(IConfiguration configuration, string environmentScheme = ENVIRONMENT_SCHEME)
        {
            _configuration = configuration ?? throw new ArgumentNullException("IConfiguration can not be null");
            EnvironmentScheme = environmentScheme;
        }

        #region Private methods

        private int GetEnvironment(string scheme)
        {
            MakeSureIConfigurationIsNotNull();
            var environmentStringValue = _configuration.GetSection(scheme).Value;
            int enviroment;
            //If value is null or NOT a number
            if (int.TryParse(environmentStringValue, out enviroment))
                return enviroment;
            return 0;
        }

        private bool IsNumber(string strValue) => int.TryParse(strValue, out int a);
        private void MakeSureIConfigurationIsNotNull()
        {
            if(_configuration == null)
                throw new ArgumentNullException("IConfiguration can not be null");
        }
        #endregion Private methods
    }
}