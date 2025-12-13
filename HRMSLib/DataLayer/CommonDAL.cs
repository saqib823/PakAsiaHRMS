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
        public static DataSet GetRoles()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT RoleId ID, RoleName Name FROM Roles WHERE Status = 1 ORDER BY RoleName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
