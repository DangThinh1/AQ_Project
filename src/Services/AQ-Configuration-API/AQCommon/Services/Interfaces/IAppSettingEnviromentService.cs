namespace AQConfigurations.Core.Services.Interfaces
{
    public interface IAppSettingEnvironmentService
    {
        string EnvironmentScheme { get; }
        int Enviroment { get; }
    }
}