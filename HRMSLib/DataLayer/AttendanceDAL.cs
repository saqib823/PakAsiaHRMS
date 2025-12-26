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
    public static class AttendanceDAL
    {
        private static Database db =>
           new DatabaseProviderFactory().Create("defaultDB");

        public static DataTable GetAttendanceLogs(
       string search,
       DateTime? startDate,
       DateTime? endDate,
       int pageIndex,
       int pageSize,
       out int totalRecords)
        {
            totalRecords = 0;
            DbCommand cmd = db.GetStoredProcCommand("usp_GetAttendanceLogs");

            db.AddInParameter(cmd, "@Search", DbType.String, search);
            db.AddInParameter(cmd, "@StartDate", DbType.DateTime, startDate);
            db.AddInParameter(cmd, "@EndDate", DbType.DateTime, endDate);
            db.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

            DataSet ds = db.ExecuteDataSet(cmd);
            DataTable data = ds.Tables[0];

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);

            return data;
        }

    }
}
