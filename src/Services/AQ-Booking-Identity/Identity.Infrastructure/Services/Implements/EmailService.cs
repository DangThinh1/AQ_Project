using System;
using System.Net;
using System.Net.Mail;
using APIHelpers.Response;
using Identity.Core.Models.Emails;
using Microsoft.AspNetCore.Identity;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.AppSetting;
using Identity.Infrastructure.Database.Entities;
using Identity.Infrastructure.Services.Interfaces;
namespace Identity.Infrastructure.Services.Implements
{
    public class EmailService : IdentityServiceBase, IEmailService
    {
        private SmtpClient _client;

        public EmailService(
            IdentityDbContext db, 
            UserManager<Users> userManager,
            IAccountService accountService) : base(db, userManager)
        {
            _client = CreateSmtpClient(new EmailSettings() {
                Host = "smtp.gmail.com",
                Port= 587,
                UserName = "devsendergroup@gmail.com",
                Password= "sender123@group"
            });
        }

        public BaseResponse<bool> Send(SendMailModel sendModel)
        {
            try
            {
                var mailMessage = new MailMessage("devsendergroup@gmail.com", sendModel.EmailAddress);
                mailMessage.Subject = sendModel.Subject;
                mailMessage.Body = sendModel.Body;
                mailMessage.IsBodyHtml = sendModel.IsBodyHtml;
                _client.Send(mailMessage);
                return BaseResponse<bool>.Success();
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg:ex.StackTrace);
            }
        }
        private SmtpClient CreateSmtpClient(EmailSettings emailSettings)
        {
            try
            {
                return new SmtpClient()
                {
                    Host = emailSettings.Host,
                    Port = emailSettings.Port,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password)
                };
            }
            catch
            {
                return null;
            }

        }
        private MailAddress GetMailAddress(string email)
        {
            try
            {
                var user = Find(email);
                if(user == null)
                    return new MailAddress(email);
                var displayName = !string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : $"{user.FirstName} {user.LastName}";
                return new MailAddress(email, displayName);
            }
            catch
            {
                return null;
            }
        }
    }
}
