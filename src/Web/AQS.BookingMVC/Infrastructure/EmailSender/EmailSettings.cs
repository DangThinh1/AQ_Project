namespace AQS.BookingMVC.Infrastructure.EmailSender
{
    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderEmailPassword { get; set; }
        public string SenderDisplayName { get; set; }
        public string CcEmail { get; set; }
        public string CcDisplayName { get; set; }
        public string BccEmail { get; set; }
        public string BccDisplayName { get; set; }
        public string ToEmail { get; set; }
    }
}
