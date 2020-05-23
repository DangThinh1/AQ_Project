using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using static AQ_PGW.Common.DTO;

namespace AQ_PGW.Common.Important
{
    public static class EmailHelpers
    {
        public static void SendEmail(ErrorInfo errorInfo)
        {
            try
            {
                string exception = "";

                if (errorInfo.Exception != null)
                    exception += errorInfo.Exception.Message.ToString();

                if (errorInfo.Exception.InnerException != null)
                {
                    string innerExceptionMessage = errorInfo.Exception.InnerException.Message;
                    if (errorInfo.Exception.InnerException.InnerException != null)
                        innerExceptionMessage = errorInfo.Exception.InnerException.InnerException.Message;

                    exception += Environment.NewLine + innerExceptionMessage;
                }


                if (errorInfo.Exception.TargetSite != null)
                {
                    string declaringTypeName = errorInfo.Exception.TargetSite.DeclaringType != null ? errorInfo.Exception.TargetSite.DeclaringType.Name : "";
                    exception += Environment.NewLine + "TargetSite: " + declaringTypeName + "," + errorInfo.Exception.TargetSite.Name + Environment.NewLine;
                }

                if (errorInfo.Exception.StackTrace != null)
                    //errorInfo.Exception = errorInfo.Exception.InnerException;
                    exception += Environment.NewLine + errorInfo.Exception.StackTrace;

                //string html = File.ReadAllText("EmailTemplate.html");
                //html = string.Format(html, System.DateTime.Now.ToString("dd-MMM-yyyy / hh:mm:ss tt"), errorInfo.Section, errorInfo.Username, errorInfo.Exception.ToLogString(""));

                string html = "<body bgcolor='white'>";
                html += "<span><H3 style='color:maroon;'>" + System.DateTime.Now.ToString("dd-MMM-yyyy / hh:mm:ss tt") + "</H3></span>";
                html += "<span><H3 style='color:maroon;'>" + errorInfo.Useragent + "</H3></span>";
                html += "<span><H3 style='color:red;'>Server Error in " + errorInfo.Section + "<hr width=100% size=1 color=silver></H3>";
                html += "<font face='Arial, Helvetica, Geneva, SunSans - Regular, sans - serif '>";
                html += "<b style='font-family:'Verdana';font-weight:normal;color:black;'> Username: </b>" + errorInfo.Username + "<br><br>";
                html += "<b style='font-family:'Verdana';font-weight:normal;color:black;'> Error Message:</b> <br><br>";
                html += "<table width=100% bgcolor='#ffffcc'><tr><td><code><pre>";
                //html += errorInfo.Exception.ToLogString("");
                html += exception;
                html += "</pre></code></td></tr></table>";
                html += "</font>";
                html += "</body>";

                MailMessage message = new MailMessage();

                message.From = new MailAddress("aqbooking@aqbooking.com", "SYSLOGS");
                message.To.Add(new MailAddress("aqbooking@aqbooking.com"));

                message.CC.Add(new MailAddress("aqbooking@aqbooking.com", "AQ Booking"));
                message.CC.Add(new MailAddress("aqbooking@aqbooking.com", "AQ Booking"));                

                message.IsBodyHtml = true;
                message.Subject = string.Format("{0}", "AQ API");
                message.Body = html;

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("aqbooking@aqbooking.com", "Sysadmin2010"),
                    EnableSsl = true
                };

                smtp.Send(message);
            }
            catch { }
        }

        public static void SendEmail(string msg)
        {

            try
            {
                string URL = "";//HttpContext.Current.Request.Url.AbsoluteUri;

                string html = "<body bgcolor='white'>";
                html += "<span><H3 style='color:maroon;'>" + System.DateTime.Now.ToString("dd-MMM-yyyy / hh:mm:ss tt") + "</H3></span>";
                html += "<span><H3 style='color:red;'>Server Error in " + URL + "<hr width=100% size=1 color=silver></H3>";
                html += "<font face='Arial, Helvetica, Geneva, SunSans - Regular, sans - serif '>";
                html += "<b style='font-family:'Verdana';font-weight:normal;color:black;'> Error Message:</b> <br><br>";
                html += "<table width=100% bgcolor='#ffffcc'><tr><td><code><pre>";
                html += msg;
                html += "</pre></code></td></tr></table>";
                html += "</font>";
                html += "</body>";

                MailMessage message = new MailMessage();

                message.From = new MailAddress("aqbooking@aqbooking.com", "SYSLOGS");
                message.To.Add(new MailAddress("aqbooking@aqbooking.com"));

                message.CC.Add(new MailAddress("aqbooking@aqbooking.com", "AQ Booking"));
                message.CC.Add(new MailAddress("aqbooking@aqbooking.com", "AQ Booking"));

                message.IsBodyHtml = true;
                message.Subject = string.Format("{0}", "AQ API");
                message.Body = html;

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("aqbooking@aqbooking.com", "Sysadmin2010"),
                    EnableSsl = true
                };

                smtp.Send(message);
            }
            catch (Exception ex) { }
        }

        public static bool SendEmail(string emailTo, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();

                message.From = new MailAddress("it_vn2@aqbooking.com", "AQ API");
                message.To.Add(new MailAddress(emailTo));

                message.Bcc.Add(new MailAddress("it_vn2@aqbooking.com", "IT VN 2"));
                message.Bcc.Add(new MailAddress("it_vn2@aqbooking.com", "IT VN 2"));
                message.Bcc.Add(new MailAddress("it_vn2@aqbooking.com", "IT VN 2"));
                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("it_vn2@aqbooking.com", "Sysadmin2010"),
                    EnableSsl = true
                };

                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
