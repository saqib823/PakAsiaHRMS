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
    class EmployeeMaster
    {
        public static DataSet View_DeptList(string Search)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("pr_ProcedureName");
                db.AddInParameter(cmd, "Search", DbType.String, Search);
                return db.ExecuteDataSet(cmd);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}