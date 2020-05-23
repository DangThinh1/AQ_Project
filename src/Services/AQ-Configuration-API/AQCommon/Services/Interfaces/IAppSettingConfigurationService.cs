using Microsoft.Extensions.Configuration;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface IAppSettingConfigurationService
    {
        string BinFolderPath { get; }
        void Create(string scheme, object value, string filePath = "", int delay = 200);
        void CreateDevelopment(string scheme, object entry, string path = "", int delay = 200);
    }
}