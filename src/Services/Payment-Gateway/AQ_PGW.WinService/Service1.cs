using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Common.Helpers;

namespace AQ_PGW.WinService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer _timer;
        private DateTime _lastRun = DateTime.Now.AddDays(-1);
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            schedule_Timer();
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }

        void schedule_Timer()
        {
            WriteToFile("### Timer Started ###  Service is started at " + DateTime.Now);
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 10, 0, 0, 0); //Specify your scheduled time HH,MM,SS [10am and 0 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            _timer = new System.Timers.Timer(tickTime); // every 10 hour
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();

        }
        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // ignore the time, just compare the date
            if (_lastRun.Date < DateTime.Now.Date)
            {
                // stop the timer while we are running the cleanup task
                _timer.Stop();
                //
                // do cleanup stuff
                WriteToFile("Service is recall at " + DateTime.Now);
                WriteToFile("\n\n ### Begin Service withAPI ###");
                try
                {
                    var objData = RestAPI.SendGetRequest("api/Transaction/GetSchedulePaymentToday", null);
                    dynamic source = JsonConvert.DeserializeObject(objData.Content);
                    foreach (dynamic item in source.result.data)
                    {
                        var response = RestAPI.SendPostRequest("api/Transaction/ProcessSchedulePayment?transactionId=" + item.transactionId.ToString(), null);
                        var result = JsonConvert.DeserializeObject(response.Content);
                        WriteToFile($"Log : transactionId  {item.transactionId.ToString()} \n\n");
                        WriteToFile($"statusCode : {result.statusCode.ToString()}\n\n");
                        WriteToFile($"message : {result.message.ToString()}\n\n");
                    }
                    //
                }
                catch (Exception ex)
                {
                    EmailHelpers.SendEmail(ex.Message);
                }
                finally
                {
                    WriteToFile("\n\n ### End ###");
                    _lastRun = DateTime.Now;
                    _timer.Start();
                }
              
           
            }
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
