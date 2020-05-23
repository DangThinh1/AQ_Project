using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string strSubject, string strBody);
        int SendEmail(string strSubject, string strBody);
    }
}
