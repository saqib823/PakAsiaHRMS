using HRMSLib.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HRMSLib.DataLayer
{
    public class RoleDAL
    {
        private static Database db =>
                   new DatabaseProviderFactory().Create("defaultDB");
        LoggedInUser currentUser =
                   HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
        public DataTable GetRolesPaged(
        int pageNumber,
        int pageSize,
        string searchText,
        string sortField,
        string sortOrder,
        out int totalRecords)
        {
            totalRecords = 0;

            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_RoleData_Select");

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

        public bool RoleData(int mode, string RoleName, string Status, string RoleID)
        {
            try
            {
                // Create stored procedure command
                DbCommand cmd = db.GetStoredProcCommand("SP_SaveRolesData");

                // Add parameters
                db.AddInParameter(cmd, "@Mode", DbType.Int32, mode);
                db.AddInParameter(cmd, "@RoleName", DbType.String, RoleName);
                db.AddInParameter(cmd, "@Status", DbType.String, Status);
                db.AddInParameter(cmd, "@UserID", DbType.Int32, currentUser.UserID);
                db.AddInParameter(cmd, "@RoleID", DbType.String, RoleID);

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

        public DataRow GetRoleById(int RoleID)
        {

            using (DbCommand cmd = db.GetStoredProcCommand("SP_GetRoleById"))
            {
                db.AddInParameter(cmd, "@RoleID", DbType.Int32, RoleID);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
            }

            return null;
        }
        public void DeleteRoles(int DepartmentID)
        {
            // SQL command (can also be stored procedure)
            string sql = "DELETE FROM Roles WHERE RoleID = @RoleID";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(cmd, "@RoleID", DbType.Int32, DepartmentID);
                db.ExecuteNonQuery(cmd);
            }
        }
        public static bool SaveRoleRights(List<string> selectedPages, int roleId)
        {
            if (selectedPages == null)
                selectedPages = new List<string>();

            try
            {
                // 1️⃣ Create TVP DataTable (MUST match SQL TYPE)
                DataTable dtRights = new DataTable();
                dtRights.Columns.Add("PageRight", typeof(string));

                foreach (string page in selectedPages)
                    dtRights.Rows.Add(page);


                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // 2️⃣ Get SP Command
                            DbCommand cmd = db.GetStoredProcCommand("usp_SaveRoleRights");

                            // 3️⃣ Add normal parameter
                            db.AddInParameter(cmd, "@RoleID", DbType.Int32, roleId);

                            // 4️⃣ CAST TO SqlCommand FOR TVP (IMPORTANT)
                            SqlCommand sqlCmd = cmd as SqlCommand;
                            if (sqlCmd == null)
                                throw new InvalidOperationException("Command is not SqlCommand");

                            // 5️⃣ Add TVP properly
                            SqlParameter tvpParam = new SqlParameter("@PageRights", SqlDbType.Structured)
                            {
                                TypeName = "dbo.PageRightTableType", // MUST MATCH SQL TYPE
                                Value = dtRights
                            };

                            sqlCmd.Parameters.Add(tvpParam);

                            // 6️⃣ Execute
                            db.ExecuteNonQuery(cmd, tran);

                            tran.Commit();
                            return true;
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
