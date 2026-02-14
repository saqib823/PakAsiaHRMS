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
            int? branchID,
            int? departmentID,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            totalRecords = 0;

            DbCommand cmd = db.GetStoredProcCommand("usp_GetAttendanceLogs");

            db.AddInParameter(cmd, "@Search", DbType.String, search);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);
            db.AddInParameter(cmd, "@BranchID", DbType.Int32, branchID);
            db.AddInParameter(cmd, "@DepartmentID", DbType.Int32, departmentID);
            db.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

            // TotalRecords is returned as a column in every row
            if (dt.Rows.Count > 0)
                totalRecords = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);

            return dt;
        }

        public static DataTable GetAttendanceLogs_ForKPIs(string empNo, DateTime from, DateTime to)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_GetAttendanceLogs");

            db.AddInParameter(cmd, "@EmployeeNo", DbType.String, empNo);
            db.AddInParameter(cmd, "@FromDate", DbType.Date, from.Date);
            db.AddInParameter(cmd, "@ToDate", DbType.Date, to.Date);

            return db.ExecuteDataSet(cmd).Tables[0];
        }
        public static DataRow GetEmployeeAttendancePercentages(string employeeNo, DateTime fromDate, DateTime toDate)
        {
            // Create database object using your default DB connection string
            Database db = new DatabaseProviderFactory().Create("defaultDB");

            // Create command for the new stored procedure
            DbCommand cmd = db.GetStoredProcCommand("SP_GetEmployeeAttendancePercentages");

            // Add input parameters
            db.AddInParameter(cmd, "@EmpNo", DbType.String, employeeNo);
            db.AddInParameter(cmd, "@FromDate", DbType.Date, fromDate);
            db.AddInParameter(cmd, "@ToDate", DbType.Date, toDate);

            // Execute and get dataset
            DataSet ds = db.ExecuteDataSet(cmd);

            // Return first row if exists
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];

            return null;
        }


        public static bool SaveAttendance(
              int mode,
              int? attendanceLogId,
              string empNo,
              string fullName,
              DateTime punchDate,
              TimeSpan punchTime,
              string punchType,
              string verifyMode,
              int createdBy)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_Save");

            db.AddInParameter(cmd, "@Mode", DbType.Int32, mode);
            db.AddInParameter(cmd, "@AttendanceLogID", DbType.Int32, attendanceLogId);
            db.AddInParameter(cmd, "@EmpNo", DbType.String, empNo);
            db.AddInParameter(cmd, "@FullName", DbType.String, fullName);
            db.AddInParameter(cmd, "@PunchDate", DbType.Date, punchDate.Date);

            // Enterprise Library does not accept TimeSpan directly for DbType.Time
            // Convert to DateTime; SQL TIME(7) will store only time portion
            DateTime punchDateTimeParam = DateTime.Today.Add(punchTime);
            db.AddInParameter(cmd, "@PunchDateTime", DbType.DateTime, punchDateTimeParam);

            db.AddInParameter(cmd, "@PunchType", DbType.String, punchType);
            db.AddInParameter(cmd, "@VerifyModeName", DbType.String, verifyMode);
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        public static DataTable GetAttendancePaged(int page, int size, string search, out int totalRecords)
        {
            totalRecords = 0;

            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_List");
            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, page);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, size);
            db.AddInParameter(cmd, "@SearchText", DbType.String, search ?? "");

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

            if (dt.Rows.Count > 0)
                totalRecords = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);

            return dt;
        }
        public static DataRow GetById(int id)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_GetById");
            db.AddInParameter(cmd, "@AttendanceLogID", DbType.Int32, id);

            DataSet ds = db.ExecuteDataSet(cmd);
            return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
        }
        public static void Delete(int id)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Attendance_Delete");
            db.AddInParameter(cmd, "@AttendanceLogID", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);
        }
        public static void DeleteAttendanceLogs()
        {
            try
            {
                DbCommand cmd = db.GetSqlStringCommand("DELETE FROM AttendanceLogs");
                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting attendance logs.", ex);
            }
        }

    }

}

