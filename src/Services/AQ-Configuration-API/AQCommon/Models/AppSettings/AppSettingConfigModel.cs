using System.Collections.Generic;

namespace AQConfigurations.Core.Models.AppSettings
{
    public class AppSettingConfigModel
    {
        public string EnvironmentScheme { get; set; }
        public int EnvironmentValue { get; set; }
        public string BinFolderPath { get; set; }
        public List<object> Settings { get; set; }
        public List<CustomSettingModel> CustomSettings { get; set; }
    }

    public class CustomSettingModel
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public CustomSettingModel(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
