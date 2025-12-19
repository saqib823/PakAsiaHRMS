using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public class BiometricDAL
    {
        private static Database db =>
            new DatabaseProviderFactory().Create("defaultDB");

        public static void InsertRawLog(
            string deviceSN,
            string empCode,
            DateTime punchTime,
            int status,
            int verify)
        {
            DbCommand cmd =
            db.GetStoredProcCommand("SP_InsertAttendanceRawLog");
            db.AddInParameter(cmd, "@DeviceSerialNo", DbType.String, deviceSN);
            db.AddInParameter(cmd, "@EmployeeCode", DbType.String, empCode);
            db.AddInParameter(cmd, "@PunchDateTime", DbType.DateTime, punchTime);
            db.AddInParameter(cmd, "@PunchStatus", DbType.Int32, status);
            db.ExecuteNonQuery(cmd);
        }

        public static void ProcessAttendance(
            string empCode,
            DateTime punchTime,
            int status,
            string deviceSN)
        {
            DbCommand cmd =
            db.GetStoredProcCommand("SP_ProcessAttendanceRawLog");
            db.AddInParameter(cmd, "@EmployeeCode", DbType.String, empCode);
            db.AddInParameter(cmd, "@PunchDateTime", DbType.DateTime, punchTime);
            //db.AddInParameter(cmd, "@PunchStatus", DbType.Int32, status);
            db.AddInParameter(cmd, "@DeviceSerialNo", DbType.String, deviceSN);
            db.ExecuteNonQuery(cmd);
        }
    }
}
