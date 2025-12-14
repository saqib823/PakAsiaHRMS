using HRMSLib.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web;

namespace HRMSLib.DataLayer
{
    public class BranchDAL
    {
        LoggedInUser currentUser =
                   HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

        // Get paginated list of branches
        public DataTable GetBranchesPaged(
            int pageNumber,
            int pageSize,
            string searchText,
            string sortField,
            string sortOrder,
            out int totalRecords)
        {
            totalRecords = 0;

            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_Branches_Select");

            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@SearchText", DbType.String, searchText);


            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                totalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecords"]);
                return ds.Tables[0];
            }

            return new DataTable();
        }

        // Insert / Update / Delete branch
        public bool SaveBranch(int mode, int? BranchID, string BranchName, string Location, int Status, int UserID)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("SP_SaveBranchData");

                db.AddInParameter(cmd, "@Mode", DbType.Int32, mode);
                db.AddInParameter(cmd, "@BranchID", DbType.Int32, BranchID.HasValue ? BranchID.Value : (object)DBNull.Value);
                db.AddInParameter(cmd, "@BranchName", DbType.String, BranchName);
                db.AddInParameter(cmd, "@Location", DbType.String, Location);
                db.AddInParameter(cmd, "@Status", DbType.Int32, Status);
                db.AddInParameter(cmd, "@UserID", DbType.Int32, UserID);

                int rowsAffected = db.ExecuteNonQuery(cmd);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Get branch by ID
        public DataRow GetBranchById(int BranchID)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            using (DbCommand cmd = db.GetStoredProcCommand("SP_GetBranchById"))
            {
                db.AddInParameter(cmd, "@BranchID", DbType.Int32, BranchID);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
            }

            return null;
        }

        // Delete branch
        public void DeleteBranch(int BranchID)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            string sql = "DELETE FROM Branches WHERE BranchID = @BranchID";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(cmd, "@BranchID", DbType.Int32, BranchID);
                db.ExecuteNonQuery(cmd);
            }
        }
    }
}
