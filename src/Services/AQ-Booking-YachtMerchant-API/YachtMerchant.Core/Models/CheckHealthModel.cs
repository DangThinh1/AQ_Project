namespace YachtMerchant.Core.Models
{
    public class CheckHealthModel
    {
        public string ServerName { get; set; }
        public int Enviroment { get; set; }
        public string IdentityApi { get; set; }
        public string FileStreamApi { get; set; }
        public string AdminApi { get; set; }
        public string ConfigurationApi { get; set; }
        public string ConnectionString { get; set; }
        public bool IsGoodHealth { get; set; }

        //Test Connection
        public string TestConnect_IdentityApi { get; set; }
        public string TestConnect_FileStreamApi { get; set; }
        public string TestConnect_AdminApi { get; set; }
        public string TestConnect_ConfigurationApi { get; set; }
        public string TestConnect_Database { get; set; }
    }
}
