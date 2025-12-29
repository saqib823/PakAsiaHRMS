using System;
using System.Data;
using System.ServiceProcess;
using System.Data.SqlClient;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.IO;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private static Database db =>
                     new DatabaseProviderFactory().Create("defaultDB");
        public Service1()
        {
            InitializeComponent();
        }

        private Timer _timer;

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(1000); // 1000 ms = 1 second
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ProcessAttendance();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        private void ProcessAttendance()
        {
            using (DbCommand cmd = db.GetStoredProcCommand("dbo.SP_SubmitAttendanceLogsRealtime"))
            {
                db.ExecuteNonQuery(cmd);
            }
        }
        private void LogError(Exception ex)
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "log.txt";

            File.AppendAllText(logPath,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                + " | "
                + ex.ToString()
                + Environment.NewLine
                + "------------------------------------"
                + Environment.NewLine
            );
        }
        protected override void OnStop()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
