namespace AQConfigurations.Core.Models.AppSettings
{
    public class StringOption : EnvironmentOptionBase<string> {
        public StringOption() : base()
        {

        }
    }
    public class RedisCacheOption : EnvironmentOptionBase<RedisCacheModel> { }

    //Base Models
    public class RedisCacheModel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string InstanceName { get; set; }
        public string Password { get; set; }

        public RedisCacheModel()
        {
            Host = string.Empty;
            Port = 0;
            InstanceName = string.Empty;
            Password = string.Empty;
        }

    }
    public class EnvironmentOptionBase<T>
    {
        public T LOCAL { get; set; }
        public T VN { get; set; }
        public T BETA { get; set; }
        public T LIVE { get; set; }

        public EnvironmentOptionBase()
        {
            LOCAL = default(T);
            VN = default(T);
            BETA = default(T);
            LIVE = default(T);
        }
    }
}