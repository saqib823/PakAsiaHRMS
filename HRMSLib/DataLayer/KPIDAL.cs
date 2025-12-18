using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public class KPIDAL
    {
        private static Database db =>
            new DatabaseProviderFactory().Create("defaultDB");

        // SAVE KPI
        public static void SaveEmployeeKPI(
            int employeeId,
            int year,
            int month,
            decimal attendance,
            decimal punctuality,
            decimal taskCompletion,
            decimal overtime,
            decimal finalScore,
            string grade,
            int createdBy)
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
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

            db.ExecuteNonQuery(cmd);
        }

        // GET KPI LIST
        public static DataTable GetEmployeeKPI(string searchText = null)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_GetEmployeeKPI");

            if (!string.IsNullOrEmpty(searchText))
                db.AddInParameter(cmd, "@Search", DbType.String, searchText);

            DataSet ds = db.ExecuteDataSet(cmd);
            return ds.Tables[0];
        }
    }
}
