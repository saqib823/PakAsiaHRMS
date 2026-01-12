using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public static class DashboardDAL
    {
        private static Database db =>
            new DatabaseProviderFactory().Create("defaultDB");

        /// <summary>
        /// Returns all dashboard data in multiple result sets
        /// Result Sets:
        /// 1 = Today Attendance Summary
        /// 2 = Attendance Trend (Date-wise)
        /// 3 = Asset Summary
        /// 4 = Asset Assignment Status
        /// 5 = KPI Snapshot
        /// </summary>
        public static DataSet GetDashboardData(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? kpiYear = null,
            int? kpiMonth = null)
        {
            DbCommand cmd = db.GetStoredProcCommand("usp_Dashboard_HRMS");

            db.AddInParameter(cmd, "@FromDate", DbType.Date,
                (object)fromDate ?? DBNull.Value);

            db.AddInParameter(cmd, "@ToDate", DbType.Date,
                (object)toDate ?? DBNull.Value);

            db.AddInParameter(cmd, "@KPIYear", DbType.Int32,
                (object)kpiYear ?? DBNull.Value);

            db.AddInParameter(cmd, "@KPIMonth", DbType.Int32,
                (object)kpiMonth ?? DBNull.Value);

            return db.ExecuteDataSet(cmd);
        }
    }
}
