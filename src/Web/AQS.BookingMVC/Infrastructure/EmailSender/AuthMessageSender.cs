using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.EmailSender
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }
        public Task SendEmailAsync(string strSubject, string strBody)
        {

            Execute(strSubject, strBody).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string strSubject, string strBody)
        {
            try
            {
                #region Indentify
                string strSenderDisplayName = string.Empty;
                string strCCDisplayName = string.Empty;
                string strBccDisplayName = string.Empty;
                if (!string.IsNullOrEmpty(_emailSettings.SenderEmail))
                {
                    //Display Name
                    strSenderDisplayName = string.IsNullOrEmpty(_emailSettings.SenderDisplayName)
                            ? _emailSettings.SenderEmail.Trim()
                            : _emailSettings.SenderDisplayName.Trim();
                }
                if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                {
                    //Display Name
                    strCCDisplayName = string.IsNullOrEmpty(_emailSettings.CcDisplayName)
                           ? _emailSettings.CcEmail.Trim()
                           : _emailSettings.CcDisplayName.Trim();
                }
                if (!string.IsNullOrEmpty(_emailSettings.BccEmail))
                {
                    //Display Name
                    strBccDisplayName = string.IsNullOrEmpty(_emailSettings.BccDisplayName)
                            ? _emailSettings.BccEmail.Trim()
                            : _emailSettings.BccDisplayName.Trim();
                }

                #endregion
                
                if (!string.IsNullOrEmpty(_emailSettings.ToEmail)&&!string.IsNullOrEmpty(strSubject)&& !string.IsNullOrEmpty(strBody))
                {
                    using (var mailMsg = new MailMessage())
                    {
                        mailMsg.To.Add(new MailAddress(_emailSettings.ToEmail));
                        mailMsg.From = new MailAddress(_emailSettings.SenderEmail.Trim(), strSenderDisplayName);

                        //Bcc
                        if (!string.IsNullOrEmpty(_emailSettings.BccEmail))
                        {
                            MailAddress mailBccAdd = new MailAddress(_emailSettings.BccEmail.Trim(), strBccDisplayName);
                            mailMsg.Bcc.Add(mailBccAdd);
                        }
                        //CC
                        if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                        {
                            MailAddress mailCcAdd = new MailAddress(_emailSettings.CcEmail.Trim(), strCCDisplayName);
                            mailMsg.CC.Add(mailCcAdd);
                        }
                        mailMsg.Subject = strSubject;
                        mailMsg.Body = strBody;
                        mailMsg.IsBodyHtml = true;
                        using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain.Trim(), _emailSettings.PrimaryPort)) //465 - SSL | 587 - TLS
                        {
                            smtp.EnableSsl = false;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(_emailSettings.SenderEmail.Trim(), _emailSettings.SenderEmailPassword.Trim());
                            await smtp.SendMailAsync(mailMsg);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        public int SendEmail(string strSubject, string strBody)
        {
            int iRs = 0;
            try
            {
                #region Indentify
                string strSenderDisplayName = string.Empty;
                string strCCDisplayName = string.Empty;
                string strBccDisplayName = string.Empty;
                if (!string.IsNullOrEmpty(_emailSettings.SenderEmail))
                {
                    //Display Name
                    strSenderDisplayName = string.IsNullOrEmpty(_emailSettings.SenderDisplayName)
                            ? _emailSettings.SenderEmail.Trim()
                            : _emailSettings.SenderDisplayName.Trim();
                }
                if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                {
                    //Display Name
                    strCCDisplayName = string.IsNullOrEmpty(_emailSettings.CcDisplayName)
                           ? _emailSettings.CcEmail.Trim()
                           : _emailSettings.CcDisplayName.Trim();
                }
                if (!string.IsNullOrEmpty(_emailSettings.BccEmail))
                {
                    //Display Name
                    strBccDisplayName = string.IsNullOrEmpty(_emailSettings.BccDisplayName)
                            ? _emailSettings.BccEmail.Trim()
                            : _emailSettings.BccDisplayName.Trim();
                }

                #endregion

                if (!string.IsNullOrEmpty(_emailSettings.ToEmail) && !string.IsNullOrEmpty(strSubject) && !string.IsNullOrEmpty(strBody))
                {

                    using (var mailMsg = new MailMessage())
                    {
                        mailMsg.To.Add(new MailAddress(_emailSettings.ToEmail));
                        mailMsg.From = new MailAddress(_emailSettings.SenderEmail.Trim(), strSenderDisplayName);

                        //Bcc
                        if (!string.IsNullOrEmpty(_emailSettings.BccEmail))
                        {
                            MailAddress mailBccAdd = new MailAddress(_emailSettings.BccEmail.Trim(), strBccDisplayName);
                            mailMsg.Bcc.Add(mailBccAdd);
                        }
                        //CC
                        if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                        {
                            MailAddress mailCcAdd = new MailAddress(_emailSettings.CcEmail.Trim(), strCCDisplayName);
                            mailMsg.CC.Add(mailCcAdd);
                        }
                        mailMsg.Subject = strSubject;
                        mailMsg.Body = strBody;
                        mailMsg.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain.Trim(), _emailSettings.PrimaryPort)) //465 - SSL | 587 - TLS
                        {
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(_emailSettings.SenderEmail.Trim(), _emailSettings.SenderEmailPassword.Trim());
                            smtp.Send(mailMsg);
                        }
                    }
                    iRs = 1;
                }

            }
            catch (Exception ex)
            {
                iRs = 0;
            }
            return iRs;
        }

    }
}
