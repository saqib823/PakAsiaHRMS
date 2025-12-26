using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public class KPIDAL
    {
        private static Database db =
            new DatabaseProviderFactory().Create("defaultDB");

        // SAVE KPI
        public static void SaveEmployeeKPI(
            int employeeId, int year, int month,
            decimal attendance, decimal punctuality,
            decimal taskCompletion, decimal overtime,
            decimal finalScore, string grade,
            string periodType, int? quarter, int createdBy)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_SaveEmployeeKPI");

            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);
            db.AddInParameter(cmd, "@KPIYear", DbType.Int32, year);
            db.AddInParameter(cmd, "@KPIMonth", DbType.Int32, month);
            db.AddInParameter(cmd, "@AttendancePct", DbType.Decimal, attendance);
            db.AddInParameter(cmd, "@PunctualityPct", DbType.Decimal, punctuality);
            db.AddInParameter(cmd, "@TaskCompletion", DbType.Decimal, taskCompletion);
            db.AddInParameter(cmd, "@OvertimeHours", DbType.Decimal, overtime);
            db.AddInParameter(cmd, "@FinalScore", DbType.Decimal, finalScore);
            db.AddInParameter(cmd, "@Grade", DbType.String, grade);
            db.AddInParameter(cmd, "@PeriodType", DbType.String, periodType);
            db.AddInParameter(cmd, "@Quarter", DbType.Int32, quarter);
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

            db.ExecuteNonQuery(cmd);
        }

        // PAGED KPI LIST
        public static DataTable GetEmployeeKPI(
            string search, int pageIndex, int pageSize,
            out int totalRows)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_GetEmployeeKPI_Paged");

            db.AddInParameter(cmd, "@Search", DbType.String,
                string.IsNullOrEmpty(search) ? null : search);
            db.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

            DataSet ds = db.ExecuteDataSet(cmd);
            totalRows = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            return ds.Tables[0];
        }

        // DELETE
        public static void DeleteKPI(int kpiId)
        {
            DbCommand cmd = db.GetSqlStringCommand(
                "DELETE FROM EmployeeKPI WHERE KPIID=@ID");
            db.AddInParameter(cmd, "@ID", DbType.Int32, kpiId);
            db.ExecuteNonQuery(cmd);
        }

        // GOAL %
        public static decimal GetGoalAchievement(int empId, int year)
        {
            DbCommand cmd = db.GetStoredProcCommand("usp_GetGoalAchievement");
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, empId);
            db.AddInParameter(cmd, "@Year", DbType.Int32, year);
            object r = db.ExecuteScalar(cmd);
            return r == null ? 0 : Convert.ToDecimal(r);
        }
    }
}
