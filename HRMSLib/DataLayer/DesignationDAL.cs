using HRMSLib.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HRMSLib.DataLayer
{
    public class DesignationDAL
    {
        LoggedInUser currentUser =
                     HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
        public DataTable GetDesignationsPaged(
        int pageNumber,
        int pageSize,
        string searchText,
        string sortField,
        string sortOrder,
        out int totalRecords)
        {
            totalRecords = 0;

            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_DesignationsData_Select");

            db.AddInParameter(cmd, "@Page_Number", DbType.Int32, pageNumber);
            db.AddInParameter(cmd, "@Page_Size", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@SearchText", DbType.String, searchText);
            db.AddInParameter(cmd, "@SortField", DbType.String, sortField);
            db.AddInParameter(cmd, "@SortOrder", DbType.String, sortOrder);

            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                totalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecords"]);
                return ds.Tables[0];
            }

            return new DataTable();
        }
        public bool DesignationData(int mode, string DesignationName, string Status, string DesignationID)
        {
            try
            {
                // Create database instance
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                // Create stored procedure command
                DbCommand cmd = db.GetStoredProcCommand("SP_SaveDesignationData");

                // Add parameters
                db.AddInParameter(cmd, "@Mode", DbType.Int32, mode);
                db.AddInParameter(cmd, "@DesignationName", DbType.String, DesignationName);
                db.AddInParameter(cmd, "@Status", DbType.String, Status);
                db.AddInParameter(cmd, "@UserID", DbType.Int32, currentUser.UserID);
                db.AddInParameter(cmd, "@DesignationID", DbType.String, DesignationID);

                // Execute
                int rowsAffected = db.ExecuteNonQuery(cmd);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // You can log ex.Message here if needed
                throw ex;
            }
        }
        public DataRow GetDesignationById(int DesignationID)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");

            using (DbCommand cmd = db.GetStoredProcCommand("SP_GetDesignationById"))
            {
                db.AddInParameter(cmd, "@DesignationID", DbType.Int32, DesignationID);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
            }

            return null;
        }
        public void DeleteDesignation(int DesignationID)
        {
            // Create database instance from Enterprise Library
            Database db = new DatabaseProviderFactory().Create("defaultDB");

            // SQL command (can also be stored procedure)
            string sql = "DELETE FROM Designations WHERE DesignationID = @DesignationID";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(cmd, "@DesignationID", DbType.Int32, DesignationID);
                db.ExecuteNonQuery(cmd);
            }
        }
    }
}
