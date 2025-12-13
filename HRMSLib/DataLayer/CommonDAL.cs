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
    public class CommonDAL
    {
        public static DataSet GetDepartments()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT DepartmentID ID, DepartmentName Name FROM Departments WHERE Status = 1 ORDER BY DepartmentName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetRoles()
        {
            DataTable dt = new DataTable();
            try
            {
                // Create database object using Enterprise Library
                Database db = new DatabaseProviderFactory().Create("defaultDB");

                // SQL query
                string query = "SELECT RoleId, RoleName FROM Roles WHERE Status = 1 ORDER BY RoleName";

                // Create command
                DbCommand cmd = db.GetSqlStringCommand(query);

                // Execute and load into DataTable
                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                // Optionally log
                throw ex;
            }

            return dt;
        }
    }
}
