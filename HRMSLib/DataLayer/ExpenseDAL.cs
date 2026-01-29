using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public static class ExpenseDAL
    {
        

        // Save or update expense
        public static bool SaveExpense(
            int EmployeeID,
            string EmployeeName,
            string ExpenseType,
            decimal Amount,
            DateTime ExpenseDate,
            string Description,
            string ReceiptPath,
            string Status = "Pending",
            int? ApprovedBy = null,
            DateTime? ApprovedDate = null,
            int? CreatedBy = null,
            int? ExpenseID = null)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_SaveExpense");

            // Parameters for insert/update
            db.AddInParameter(cmd, "@Mode", DbType.Int32, ExpenseID.HasValue ? 2 : 1);
            db.AddInParameter(cmd, "@ExpenseID", DbType.Int32, ExpenseID);
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, EmployeeID);
            db.AddInParameter(cmd, "@EmployeeName", DbType.String, EmployeeName);
            db.AddInParameter(cmd, "@ExpenseType", DbType.String, ExpenseType);
            db.AddInParameter(cmd, "@Amount", DbType.Decimal, Amount);
            db.AddInParameter(cmd, "@ExpenseDate", DbType.Date, ExpenseDate);
            db.AddInParameter(cmd, "@Description", DbType.String, Description);
            db.AddInParameter(cmd, "@ReceiptPath", DbType.String, ReceiptPath);
            db.AddInParameter(cmd, "@Status", DbType.String, Status);
            db.AddInParameter(cmd, "@ApprovedBy", DbType.Int32, ApprovedBy);
            db.AddInParameter(cmd, "@ApprovedDate", DbType.DateTime, ApprovedDate);
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, CreatedBy);

            int rows = db.ExecuteNonQuery(cmd);
            return rows > 0;
        }

        // Get expenses with pagination
        public static DataTable GetExpensesPaged(int pageNumber, int pageSize, string searchText,int? EmployeeID, out int totalRecords)
        {
            totalRecords = 0;
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_Expenses_Select");
            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@SearchText", DbType.String, searchText ?? "");
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, EmployeeID ?? null);

            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("TotalRecords"))
                totalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecords"]);

            return ds.Tables[0];
        }

        // Get a single expense by ID
        public static DataRow GetExpenseById(int expenseId)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_GetExpenseById");
            db.AddInParameter(cmd, "@ExpenseID", DbType.Int32, expenseId);
            DataSet ds = db.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            return null;
        }

        // Delete an expense
        public static void DeleteExpense(int expenseId)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetSqlStringCommand("DELETE FROM Expenses WHERE ExpenseID=@ExpenseID");
            db.AddInParameter(cmd, "@ExpenseID", DbType.Int32, expenseId);
            db.ExecuteNonQuery(cmd);
        }

        // Approve or disapprove an expense
        public static bool UpdateExpenseStatus(int expenseId, string status, int approvedBy)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetSqlStringCommand(@"
                UPDATE Expenses
                SET Status = @Status, ApprovedBy = @ApprovedBy, ApprovedDate = GETDATE()
                WHERE ExpenseID = @ExpenseID
            ");

            db.AddInParameter(cmd, "@Status", DbType.String, status);
            db.AddInParameter(cmd, "@ApprovedBy", DbType.Int32, approvedBy);
            db.AddInParameter(cmd, "@ExpenseID", DbType.Int32, expenseId);

            int rows = db.ExecuteNonQuery(cmd);
            return rows > 0;
        }
    }
}
