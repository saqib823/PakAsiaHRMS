using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSLib.DataLayer
{
    public static class AttendanceDAL
    {
        private static Database db =>
           new DatabaseProviderFactory().Create("defaultDB");

        public static DataTable GetRealtimeLogs()
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_GetRealtimeAttendance");
            return db.ExecuteDataSet(cmd).Tables[0];
        }
    }
}
