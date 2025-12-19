using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSLib.DataLayer
{
    public class LeaveDAL
    {
        private static Database db =>
           new DatabaseProviderFactory().Create("defaultDB");

        #region Get Leave List (Paging + Search)

        public static DataSet GetLeaves(string search, int pageNumber, int pageSize)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Leave_List");

            db.AddInParameter(cmd, "@Search", DbType.String, search);
            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

            return db.ExecuteDataSet(cmd);
        }

        #endregion

        #region Approve / Reject Leave

        public static void ApproveRejectLeave(int leaveId, int approverId, string status)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Leave_ApproveReject");

            db.AddInParameter(cmd, "@EmployeeLeaveID", DbType.Int32, leaveId);
            db.AddInParameter(cmd, "@ApproverID", DbType.Int32, approverId);
            db.AddInParameter(cmd, "@Status", DbType.String, status);

            db.ExecuteNonQuery(cmd);
        }

        #endregion

        #region Leave Encashment

        public static void EncashLeave(int leaveId)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Leave_Encash");

            db.AddInParameter(cmd, "@EmployeeLeaveID", DbType.Int32, leaveId);

            db.ExecuteNonQuery(cmd);
        }

        #endregion
        public static DataTable GetEmployeeLeaveBalance(int employeeId)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Leave_GetEmployeeBalance");
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);

            return db.ExecuteDataSet(cmd).Tables[0];
        }

        public static (int ResultCode, string ResultMessage) ApplyLeave(
            int employeeId,
            int leaveTypeId,
            DateTime startDate,
            DateTime endDate,
            string reason)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Leave_Apply");

            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);
            db.AddInParameter(cmd, "@LeaveTypeID", DbType.Int32, leaveTypeId);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);
            db.AddInParameter(cmd, "@Reason", DbType.String, reason);

            db.AddOutParameter(cmd, "@ResultCode", DbType.Int32, 4);
            db.AddOutParameter(cmd, "@ResultMessage", DbType.String, 200);

            db.ExecuteNonQuery(cmd);

            return (
                (int)db.GetParameterValue(cmd, "@ResultCode"),
                db.GetParameterValue(cmd, "@ResultMessage").ToString()
            );
        }
        public static void GenerateYearlyLeaveBalance(int employeeId, int year)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Leave_GenerateYearlyBalance");

            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);
            db.AddInParameter(cmd, "@Year", DbType.Int32, year);

            db.ExecuteNonQuery(cmd);
        }
     
     


        // =========================================================
        // 4. Carry Forward Leaves (Year End)
        // =========================================================
        public static void CarryForwardLeaves(int year)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_Leave_CarryForward_YearEnd");

            db.AddInParameter(cmd, "@Year", DbType.Int32, year);

            db.ExecuteNonQuery(cmd);
        }

        // =========================================================
        // 5. Get Leave Balances (for Apply Leave UI)
        // =========================================================
        public static DataSet GetEmployeeLeaveBalances(int employeeId, int year)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_LeaveBalance_GetByEmployee");

            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);
            db.AddInParameter(cmd, "@Year", DbType.Int32, year);

            return db.ExecuteDataSet(cmd);
        }
    }
}
