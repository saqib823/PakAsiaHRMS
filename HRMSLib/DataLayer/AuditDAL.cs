using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web;

namespace HRMSLib.DataLayer
{
    public static class AuditDAL
    {
        private static Database db => new DatabaseProviderFactory().Create("defaultDB");

        /// <summary>
        /// Insert an audit log entry into HRMS_AuditLogs table
        /// </summary>
        public static void InsertAuditLog(
            int? userId,
            string userName,
            string moduleName,
            string tableName,
            string recordId,
            string actionType,
            string oldData = null,
            string newData = null,
            string ipAddress = null,
            string machineName = null,
            string browserInfo = null,
            string remarks = null
        )
        {
            // Create a command for stored procedure
            DbCommand cmd = db.GetStoredProcCommand("SP_HRMS_Audit_Insert");

            // Add parameters
            db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);
            db.AddInParameter(cmd, "@UserName", DbType.String, userName);
            db.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);
            db.AddInParameter(cmd, "@TableName", DbType.String, tableName);
            db.AddInParameter(cmd, "@RecordID", DbType.String, recordId);
            db.AddInParameter(cmd, "@ActionType", DbType.String, actionType);
            db.AddInParameter(cmd, "@OldData", DbType.String, oldData);
            db.AddInParameter(cmd, "@NewData", DbType.String, newData);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, ipAddress);
            db.AddInParameter(cmd, "@MachineName", DbType.String, machineName);
            db.AddInParameter(cmd, "@BrowserInfo", DbType.String, browserInfo);
            db.AddInParameter(cmd, "@Remarks", DbType.String, remarks);

            // Execute the stored procedure
            db.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// Returns all audit logs (for UI filtering/paging).
        /// </summary>
        public static DataTable GetAllAuditLogs()
        {
            DbCommand cmd = db.GetSqlStringCommand("SELECT * FROM HRMS_AuditLogs ORDER BY 1 DESC");
            var ds = db.ExecuteDataSet(cmd);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
        public static string GetClientIp()
        {
            try
            {
                var req = HttpContext.Current?.Request;
                if (req == null) return string.Empty;

                // Try common headers in order
                string ip = req.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip))
                    ip = req.ServerVariables["HTTP_CLIENT_IP"];

                if (string.IsNullOrEmpty(ip))
                    ip = req.ServerVariables["REMOTE_ADDR"];

                // Fallback to Request.UserHostAddress
                if (string.IsNullOrEmpty(ip) && !string.IsNullOrEmpty(req.UserHostAddress))
                    ip = req.UserHostAddress;

                if (string.IsNullOrEmpty(ip)) return string.Empty;

                // If multiple IPs are forwarded, take the first one
                if (ip.Contains(",")) ip = ip.Split(',')[0].Trim();

                return ip;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string GetClientBrowser()
        {
            try
            {
                var req = HttpContext.Current?.Request;
                var browser = req?.Browser;
                if (browser != null)
                {
                    // Normalize Edge vs Chrome reporting
                    string name = browser.Browser;
                    string userAgent = req?.UserAgent ?? string.Empty;
                    if (userAgent.IndexOf("Edg", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        name = "Edge";
                    }
                    return $"{name} {browser.MajorVersion}.{browser.MinorVersion}";
                }

                // Fallback to user agent string if browser info not available
                if (req != null && !string.IsNullOrEmpty(req.UserAgent))
                    return req.UserAgent;

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
