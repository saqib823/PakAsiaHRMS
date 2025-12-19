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
    }
}
