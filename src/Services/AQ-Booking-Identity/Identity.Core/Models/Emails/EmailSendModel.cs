using System.Collections.Generic;

namespace Identity.Core.Models.Emails
{
    public class SendMailModel
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
    }
}
