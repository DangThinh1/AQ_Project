namespace YachtMerchant.Infrastructure.AppSetting
{
    public class ApiServer
    {
        public string ServerName { get; set; }
        public int Server { get; set; }
        public Server IdentityApi { get; set; }
        public Server FileStreamApi { get; set; }
        public Server AdminApi { get; set; }
        public Server ConfigurationApi { get; set; }
        public Server ConnectionString { get; set; }
    }

    public class Server
    {
        public string Server_LOCAL { get; set; }
        public string Server_VN { get; set; }
        public string Server_BETA { get; set; }
        public string Server_LIVE { get; set; }
    }
}