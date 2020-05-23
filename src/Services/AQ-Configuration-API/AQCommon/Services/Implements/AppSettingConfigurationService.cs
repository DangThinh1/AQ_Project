using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using AQConfigurations.Core.Services.Interfaces;
using System.Reflection;
using System.Threading;

namespace AQConfigurations.Core.Services.Implements
{
    public class AppSettingConfigurationService : IAppSettingConfigurationService
    {
        public string BinFolderPath { get; }
        private const string BIN_FOLDER_PATH = "bin\\Debug\\netcoreapp2.2\\";
        private readonly string _defaultAppSettingsJsonFilePath;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AppSettingConfigurationService(IConfiguration configuration, IHostingEnvironment hostingEnvironment, string binFolderPath = BIN_FOLDER_PATH)
        {
            BinFolderPath = binFolderPath;
            _configuration = configuration ?? throw new ArgumentNullException("IConfiguration can not be null");
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException("IHostingEnvironment can not be null");
            _defaultAppSettingsJsonFilePath = GetDefaulAppSettingJsonFilePath();
        }

        public void Create(string scheme, object entry, string path = "", int delay = 200)
        {
            if (_hostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(scheme))
            {
                path = !string.IsNullOrEmpty(path) ? path : _defaultAppSettingsJsonFilePath;
                AddSectionValue(scheme, entry, path);
                Thread.Sleep(delay);
            }
        }

        public void CreateDevelopment(string scheme, object entry, string path = "", int delay = 200)
        {
            if (_hostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(scheme))
            {
                path = GetDefaulAppSettingDevelopmentJsonFilePath();
                AddSectionValue(scheme, entry, path);
                Thread.Sleep(delay);
            }
        }

        #region Private methods

        private string GetDefaulAppSettingJsonFilePath()  
        {
            var directoryPath = AppContext.BaseDirectory;
            if (_hostingEnvironment.IsDevelopment())
                directoryPath = directoryPath.Replace(BinFolderPath, string.Empty);
            return Path.Combine(directoryPath, "appsettings.json");
        }

        private string GetDefaulAppSettingDevelopmentJsonFilePath()
        {
            var directoryPath = AppContext.BaseDirectory;
            if (_hostingEnvironment.IsDevelopment())
                directoryPath = directoryPath.Replace(BinFolderPath, string.Empty);
            return Path.Combine(directoryPath, "appsettings.Development.json");
        }

        private void AddSectionValue(string key, object entry, string path, int delay = 300)
        {
            path = !string.IsNullOrEmpty(path)
                 ? path
                 : _defaultAppSettingsJsonFilePath;
            if (File.Exists(path))
            {
                var jsonTextFromfile = File.ReadAllText(path);
                var jsonObjFromfile = JsonConvert.DeserializeObject<JObject>(jsonTextFromfile);

                if (entry.GetType().GetTypeInfo().IsClass)
                {
                    if (entry is string)
                        jsonObjFromfile.TryAdd(key, (string)entry);
                    else
                    {
                        var jsonString = JsonConvert.SerializeObject(entry);
                        var jsonObjToAddNew = JsonConvert.DeserializeObject<JObject>(jsonString);
                        jsonObjFromfile.TryAdd(key, jsonObjToAddNew);
                    }
                }
                else
                {
                    if (entry is bool)
                        jsonObjFromfile.TryAdd(key, (bool)entry);
                    if (entry is int)
                        jsonObjFromfile.TryAdd(key, (int)entry);
                    if (entry is uint)
                        jsonObjFromfile.TryAdd(key, (uint)entry);
                    if (entry is long)
                        jsonObjFromfile.TryAdd(key, (long)entry);
                    if (entry is ulong)
                        jsonObjFromfile.TryAdd(key, (ulong)entry);
                    if (entry is float)
                        jsonObjFromfile.TryAdd(key, (float)entry);
                    if (entry is decimal)
                        jsonObjFromfile.TryAdd(key, (decimal)entry);
                    if (entry is double)
                        jsonObjFromfile.TryAdd(key, (double)entry);
                }

                string jsonContent = JsonConvert.SerializeObject(jsonObjFromfile, Formatting.Indented);
                File.WriteAllTextAsync(path, jsonContent).Wait();
            }
        }
        private bool IsAvaliableSection(string section)
            => !string.IsNullOrEmpty(_configuration.GetSection(section).Value);

        #endregion Private methods
    }
}