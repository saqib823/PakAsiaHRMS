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
    [Serializable]
    public class UserDAL
    {
        public bool SaveUserData(int mode, string username, string password, string firstName, string lastName, string email, string cnic,
            string phone, string roleId, string departmentId, string createdBy, string designation,string filePath,
            string contentType,int? UserID, string Branch)
        {
            try
            {
                // Create database instance
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                // Create stored procedure command
                DbCommand cmd = db.GetStoredProcCommand("SP_SaveUserData");

                // Add parameters
                db.AddInParameter(cmd, "@Mode", DbType.String, mode);
                db.AddInParameter(cmd, "@UserID", DbType.Int32, UserID);
                db.AddInParameter(cmd, "@UserName", DbType.String, username);
                db.AddInParameter(cmd, "@FirstName", DbType.String, firstName);
                db.AddInParameter(cmd, "@LastName", DbType.String, lastName);
                db.AddInParameter(cmd, "@EmailAddress", DbType.String, email);
                db.AddInParameter(cmd, "@cnic", DbType.String, cnic);
                db.AddInParameter(cmd, "@phonenumber", DbType.String, phone);
                db.AddInParameter(cmd, "@RoleId", DbType.String, roleId);
                db.AddInParameter(cmd, "@DepartmentId", DbType.String, departmentId);
                db.AddInParameter(cmd, "@Active", DbType.String, 1);
                db.AddInParameter(cmd, "@CreatedBy", DbType.String, 001);
                db.AddInParameter(cmd, "@filePath", DbType.String, filePath);
                db.AddInParameter(cmd, "@contentType", DbType.String, contentType);
                db.AddInParameter(cmd, "@Password", DbType.String, BCrypt.Net.BCrypt.HashPassword(password));
                db.AddInParameter(cmd, "@Designation", DbType.String, designation);
                db.AddInParameter(cmd, "@Branch", DbType.String, Branch);
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

        public DataTable GetUsersPaged(
            int pageNumber,
            int pageSize,
            string searchText,
            string sortField,
            string sortOrder,
            out int totalRecords)
        {
            totalRecords = 0;

            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_UsersData_Select");

            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@SearchText", DbType.String, searchText ?? "");
            db.AddInParameter(cmd, "@SortField", DbType.String, sortField);
            db.AddInParameter(cmd, "@SortOrder", DbType.String, sortOrder);

            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0 && dt.Columns.Contains("TotalRecords"))
                {
                    totalRecords = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);
                }

                return dt;
            }

            return new DataTable();
        }

        public DataRow GetUserById(int userId)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");

            using (DbCommand cmd = db.GetStoredProcCommand("SP_GetUserById"))
            {
                db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
            }

            return null;
        }

        public void DeleteUser(int userId)
        {
            // Create database instance from Enterprise Library
            Database db = new DatabaseProviderFactory().Create("defaultDB");

            // SQL command (can also be stored procedure)
            string sql = "DELETE FROM Userinformation WHERE UserID = @UserID";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);
                db.ExecuteNonQuery(cmd);
            }
        }



        public LoggedInUser LoginUser(string email, string password)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                // 1️⃣ Try UserInformation first
                LoggedInUser user = TryLoginUserInformation(db, email, password);
                if (user != null)
                {
                    HttpContext.Current.Session["LoggedInUser"] = user;
                    return user;
                }

                // 2️⃣ If not found or password mismatch, try Employee
                user = TryLoginEmployee(db, email, password);
                if (user != null)
                {
                    HttpContext.Current.Session["LoggedInUser"] = user;
                    return user;
                }

                return null; // Login failed
            }
            catch
            {
                throw;
            }
        }
        private LoggedInUser TryLoginUserInformation(Database db, string email, string password)
        {
            string sql = "SELECT * FROM UserInformation WHERE EmailAddress = @EmailAddress";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(cmd, "@EmailAddress", DbType.String, email);

                DataSet ds = db.ExecuteDataSet(cmd);
                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                DataRow dr = ds.Tables[0].Rows[0];

                if (!BCrypt.Net.BCrypt.Verify(password, dr["Password"].ToString()))
                    return null;

                return new LoggedInUser
                {
                    UserID = Convert.ToInt32(dr["UserID"]),
                    RoleId = Convert.ToInt32(dr["RoleId"]),
                    UserName = dr["UserName"].ToString(),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    EmailAddress = dr["EmailAddress"].ToString(),
                    Active = Convert.ToBoolean(dr["Active"]),
                    PrimaryDepartmentId = Convert.ToInt32(dr["PrimaryDepartmentId"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    CreatedBy = dr["CreatedBy"].ToString(),
                    Cnic = dr["Cnic"].ToString(),
                    PhoneNumber = dr["PhoneNumber"].ToString(),
                    Designation = dr["Designation"].ToString(),
                    filePath = dr["ImageData"]?.ToString(),
                    ImageType = dr["ImageType"]?.ToString()
                };
            }
        }
        private LoggedInUser TryLoginEmployee(Database db, string email, string password)
        {
            using (DbCommand cmd = db.GetStoredProcCommand("dbo.usp_GetEmployeeByEmail"))
            {
                db.AddInParameter(cmd, "@EmailAddress", DbType.String, email);

                DataSet ds = db.ExecuteDataSet(cmd);
                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                DataRow dr = ds.Tables[0].Rows[0];

                if (!BCrypt.Net.BCrypt.Verify(password, dr["Password"].ToString()))
                    return null;

                return new LoggedInUser
                {
                    UserID = Convert.ToInt32(dr["EmployeeID"]),
                    RoleId = Convert.ToInt32(dr["RoleId"]),
                    UserName = dr["FullName"].ToString(),
                    FirstName = dr["FullName"].ToString(),
                    LastName = dr["FatherOrSpouseName"].ToString(),
                    EmailAddress = dr["EmailAddress"].ToString(),
                    Active = Convert.ToBoolean(dr["Active"]),
                    PrimaryDepartmentId = dr["DepartmentID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DepartmentID"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    CreatedBy = dr["CreatedBy"].ToString(),
                    Cnic = dr["CNIC"].ToString(),
                    PhoneNumber = dr["MobileNumber"].ToString(),
                    Designation = dr["DesignationID"]?.ToString(),
                    filePath = dr["PhotographPath"]?.ToString()
                };
            }
        }

        public List<RoleRights> GetRoleRights(int roleId)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.usp_GetRoleAllowedMenus"))
                {
                    db.AddInParameter(cmd, "@RoleId", DbType.Int32, roleId);

                    DataSet ds = db.ExecuteDataSet(cmd);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        List<RoleRights> roleRightsList = new List<RoleRights>();

                        foreach (DataRow row in dt.Rows)
                        {
                            roleRightsList.Add(new RoleRights
                            {
                                MenuId = Convert.ToInt32(row["MenuId"]),
                                MenuHref = row["MenuHref"].ToString()
                            });
                        }


                        // Store in session
                        HttpContext.Current.Session["RoleRights"] = roleRightsList;

                            return roleRightsList;
                        
                    }
                }

                return null; // Login failed

            }
            catch (Exception)
            {

                throw;
            }
           
        }

    }
}




