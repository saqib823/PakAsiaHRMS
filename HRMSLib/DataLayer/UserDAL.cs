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
    [Serializable]
    public class UserDAL
    {
        public string SaveUserData(string username, string password, string firstName, string lastName, string email, string cnic,
            string phone, string roleId, string departmentId, string createdBy, string designation, byte[] fileBytes, 
            string contentType)
        {
            try
            {
                // Create database instance
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                // Create stored procedure command
                DbCommand cmd = db.GetStoredProcCommand("SP_SaveUserData");

                // Add parameters
                db.AddInParameter(cmd, "@Mode", DbType.String, 1);
                db.AddInParameter(cmd, "@UserID", DbType.String, null);
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
                db.AddInParameter(cmd, "@fileBytes", DbType.Binary, (object)fileBytes ?? DBNull.Value);
                db.AddInParameter(cmd, "@contentType", DbType.String, contentType);
                db.AddInParameter(cmd, "@Password", DbType.String, BCrypt.Net.BCrypt.HashPassword(password));
                // Execute
                int result = db.ExecuteNonQuery(cmd);

                return result.ToString();
            }
            catch (Exception ex)
            {
                // You can log ex.Message here if needed
                throw ex;
            }
        }

    }
}
