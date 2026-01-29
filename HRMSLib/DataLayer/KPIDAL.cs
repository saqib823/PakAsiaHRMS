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

        public static void SaveEmployeeKPI(
     int employeeId, DateTime From, DateTime To,
     decimal attendance, decimal punctuality,
     decimal taskCompletion, decimal overtime,
     decimal finalScore, string grade,
     int createdBy, decimal appraisalpct, decimal currentbasic, decimal appraised)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_SaveEmployeeKPI");

            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);
            db.AddInParameter(cmd, "@From", DbType.DateTime, From);
            db.AddInParameter(cmd, "@To", DbType.DateTime, To);
            db.AddInParameter(cmd, "@AttendancePct", DbType.Decimal, attendance);
            db.AddInParameter(cmd, "@PunctualityPct", DbType.Decimal, punctuality);
            db.AddInParameter(cmd, "@TaskCompletion", DbType.Decimal, taskCompletion);
            db.AddInParameter(cmd, "@OvertimeHours", DbType.Decimal, overtime);
            db.AddInParameter(cmd, "@FinalScore", DbType.Decimal, finalScore);
            db.AddInParameter(cmd, "@Grade", DbType.String, grade);
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);
            db.AddInParameter(cmd, "@AppraisalPct", DbType.Decimal, appraisalpct);
            db.AddInParameter(cmd, "@CurrentBasic", DbType.Decimal, currentbasic);
            db.AddInParameter(cmd, "@Appraised", DbType.Decimal, appraised);

            db.ExecuteNonQuery(cmd);
        }
        public static void UpdateEmployeeAppraisedSalary(
    long employeeId,
    decimal appraisedSalary,
    decimal appraisedAmount)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                DbCommand cmd = db.GetStoredProcCommand("SP_UpdateEmployeeAppraisedSalary");

                db.AddInParameter(cmd, "@EmployeeID", DbType.Int64, employeeId);
                db.AddInParameter(cmd, "@AppraisedSalary", DbType.Decimal, appraisedSalary);
                db.AddInParameter(cmd, "@AppraisedAmount", DbType.Decimal, appraisedAmount);

                db.ExecuteNonQuery(cmd);
            }
            
            catch (Exception ex)
            {
                throw; // preserves original stack trace
            }
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
        public static decimal GetEmployeeBasicSalary(int EmployeeID)
        {
           try
            {
                // Create database instance from configuration
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                // SQL query to fetch EmployeeID
                string query = @"SELECT [BasicSalaryOrDailyWage] FROM [EmployeePayroll] WHERE [EmployeeID] = @EmployeeID";

                // Create a command
                DbCommand cmd = db.GetSqlStringCommand(query);

                // Add parameter
                db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, EmployeeID);

                // Execute scalar to get single value
                object result = db.ExecuteScalar(cmd);

                // Check for null and convert to long
                if (result != null && result != DBNull.Value)
                    return Convert.ToDecimal(result);

                return 0; // not found
            }
            catch (DbException dbEx)
            {
                // Handle database-specific errors
                throw new ApplicationException("Database operation failed.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle other errors
                throw;
            }
        }
    }
}
