//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Helpers
{
    public static class EmailHelpers
    {
        
        public static void SendEmail(string msg)
        {
            try
            {
                //string URL = HttpContext.Current.Request.Url.AbsoluteUri;

                string html = "<body bgcolor='white'>";
                html += "<span><H3 style='color:maroon;'>" + System.DateTime.Now.ToString("dd-MMM-yyyy / hh:mm:ss tt") + "</H3></span>";
                html += "<span><H3 style='color:red;'>Server Error in " + "Window Service" + "<hr width=100% size=1 color=silver></H3>";
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

                message.CC.Add(new MailAddress("aqbooking@aqbooking.com", "aq"));
                message.CC.Add(new MailAddress("aqbooking@aqbooking.com", "aq"));

                message.IsBodyHtml = true;
                //message.Subject = "VMS 4.0 Application Error";
                message.Subject = string.Format("{0}", "AQ - Payment Gateway Window Service");
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
    }
}
