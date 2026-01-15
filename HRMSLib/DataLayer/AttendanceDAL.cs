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

        public static DataTable GetAttendanceLogs(
       string search,
       DateTime? startDate,
       DateTime? endDate,
       int pageIndex,
       int pageSize,
       out int totalRecords)
        {
            totalRecords = 0;
            DbCommand cmd = db.GetStoredProcCommand("usp_GetAttendanceLogs");

            db.AddInParameter(cmd, "@Search", DbType.String, search);
            db.AddInParameter(cmd, "@StartDate", DbType.DateTime, startDate);
            db.AddInParameter(cmd, "@EndDate", DbType.DateTime, endDate);
            db.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

            DataSet ds = db.ExecuteDataSet(cmd);
            DataTable data = ds.Tables[0];

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);

            return data;
        }
        public static bool SaveAttendance(
           int mode,
           int? attendanceLogId,
           string empNo,
           string fullName,
           DateTime punchDate,
           DateTime punchDateTime,
           string punchType,
           string verifyMode,
           int createdBy)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_Save");

            db.AddInParameter(cmd, "@Mode", DbType.Int32, mode);
            db.AddInParameter(cmd, "@AttendanceLogID", DbType.Int32, attendanceLogId);
            db.AddInParameter(cmd, "@EmpNo", DbType.String, empNo);
            db.AddInParameter(cmd, "@FullName", DbType.String, fullName);
            db.AddInParameter(cmd, "@PunchDate", DbType.Date, punchDate);
            db.AddInParameter(cmd, "@PunchDateTime", DbType.DateTime, punchDateTime);
            db.AddInParameter(cmd, "@PunchType", DbType.String, punchType);
            db.AddInParameter(cmd, "@VerifyModeName", DbType.String, verifyMode);
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        public static DataTable GetAttendancePaged(int page, int size, string search, out int total)
        {
            total = 0;

            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_List");
            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, page);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, size);
            db.AddInParameter(cmd, "@SearchText", DbType.String, search ?? "");

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

            if (dt.Rows.Count > 0)
                total = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);

            return dt;
        }

        public static DataRow GetById(int id)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_GetById");
            db.AddInParameter(cmd, "@AttendanceLogID", DbType.Int32, id);

            DataSet ds = db.ExecuteDataSet(cmd);
            return ds.Tables[0].Rows[0];
        }

        public static void Delete(int id)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_Delete");
            db.AddInParameter(cmd, "@AttendanceLogID", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);
        }
    }

}

