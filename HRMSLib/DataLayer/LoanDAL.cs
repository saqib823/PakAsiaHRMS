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
    public static class LoanDAL
    {
        // ================================
        // APPLY / UPDATE LOAN
        // ================================
        public static bool SaveLoan(
            int EmployeeID,
            string LoanType,
            decimal LoanAmount,
            int DurationMonths,
            DateTime StartDate,
            int CreatedBy,
            int? LoanID = null)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_SaveLoan");

            db.AddInParameter(cmd, "@Mode", DbType.Int32, LoanID.HasValue ? 2 : 1);
            db.AddInParameter(cmd, "@LoanID", DbType.Int32, LoanID);
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, EmployeeID);
            db.AddInParameter(cmd, "@LoanType", DbType.String, LoanType);
            db.AddInParameter(cmd, "@LoanAmount", DbType.Decimal, LoanAmount);
            db.AddInParameter(cmd, "@DurationMonths", DbType.Int32, DurationMonths);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, StartDate);
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, CreatedBy);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        // ================================
        // PAGED LOAN LIST
        // ================================
        public static DataTable GetLoansPaged(
            int pageNumber,
            int pageSize,
            string searchText,
            int? employeeId,
            out int totalRecords)
        {
            totalRecords = 0;

            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_Loans_Select_Paged");

            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@SearchText", DbType.String, searchText ?? "");
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);

            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 &&
                ds.Tables[0].Columns.Contains("TotalRecords"))
            {
                totalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecords"]);
            }

            return ds.Tables[0];
        }

        // ================================
        // GET LOAN BY ID
        // ================================
        public static DataRow GetLoanById(int loanId)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_GetLoanById");

            db.AddInParameter(cmd, "@LoanID", DbType.Int32, loanId);

            DataSet ds = db.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];

            return null;
        }

        // ================================
        // APPROVE / REJECT LOAN
        // ================================
        public static bool UpdateLoanStatus(int loanId, string status, int approvedBy)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_UpdateLoanStatus");

            db.AddInParameter(cmd, "@LoanID", DbType.Int32, loanId);
            db.AddInParameter(cmd, "@Status", DbType.String, status);
            db.AddInParameter(cmd, "@ApprovedBy", DbType.Int32, approvedBy);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        // ================================
        // DELETE LOAN (ONLY IF PENDING)
        // ================================
        public static bool DeleteLoan(int loanId)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetSqlStringCommand(
                "DELETE FROM Loans WHERE LoanID=@LoanID AND Status='Pending'");

            db.AddInParameter(cmd, "@LoanID", DbType.Int32, loanId);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        // ================================
        // LOAN DEDUCTIONS (PAYROLL)
        // ================================
        public static DataTable GetLoanDeductions(int loanId)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetSqlStringCommand(
                "SELECT * FROM LoanDeductions WHERE LoanID=@LoanID ORDER BY DeductionMonth");

            db.AddInParameter(cmd, "@LoanID", DbType.Int32, loanId);
            return db.ExecuteDataSet(cmd).Tables[0];
        }

        public static void MarkDeductionPaid(int deductionId)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetSqlStringCommand(
                @"UPDATE LoanDeductions 
                  SET IsDeducted=1, DeductedDate=GETDATE()
                  WHERE DeductionID=@DeductionID");

            db.AddInParameter(cmd, "@DeductionID", DbType.Int32, deductionId);
            db.ExecuteNonQuery(cmd);
        }
    }
}
