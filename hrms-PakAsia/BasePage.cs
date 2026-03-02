using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Web;
using System.Web.UI;

namespace hrms_PakAsia
{
    /// <summary>
    /// Base page that centralizes audit logging for pages.
    /// Pages can inherit this class and call LogAction(...) for specific actions.
    /// On first load (not postback) it will also insert a 'Landed on page' audit record.
    /// </summary>
    public class BasePage : Page
    {
        protected LoggedInUser CurrentUser => HttpContext.Current?.Session?["LoggedInUser"] as LoggedInUser;

        protected virtual string DefaultAuditModuleName =>
            Page?.AppRelativeVirtualPath ?? Request?.Url?.AbsolutePath ?? string.Empty;

        protected virtual string DefaultAuditTableName => string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Log landing on page once per initial load.
            try
            {
                if (!IsPostBack && CurrentUser != null)
                {
                    string module = DefaultAuditModuleName;
                    AuditDAL.InsertAuditLog(
                        userId: CurrentUser.UserID,
                        userName: CurrentUser.UserName,
                        moduleName: module,
                        tableName: DefaultAuditTableName,
                        recordId: string.Empty,
                        actionType: "Landed",
                        oldData: null,
                        newData: null,
                        ipAddress: AuditDAL.GetClientIp(),
                        machineName: HttpContext.Current?.Session?["UserMachine"]?.ToString() ?? string.Empty,
                        browserInfo: AuditDAL.GetClientBrowser(),
                        remarks: $"{CurrentUser.UserName} landed on {module}"
                    );
                }
            }
            catch
            {
                // never throw from logging
            }
        }

        /// <summary>
        /// Helper to insert an audit log for an action on the current page.
        /// </summary>
        protected void LogAction(string actionType, string recordId = "", string oldData = null, string newData = null, string remarks = "")
        {
            try
            {
                if (CurrentUser == null) return;
                string module = DefaultAuditModuleName;
                AuditDAL.InsertAuditLog(
                    userId: CurrentUser.UserID,
                    userName: CurrentUser.UserName,
                    moduleName: module,
                    tableName: DefaultAuditTableName,
                    recordId: recordId,
                    actionType: actionType,
                    oldData: oldData,
                    newData: newData,
                    ipAddress: AuditDAL.GetClientIp(),
                    machineName: HttpContext.Current?.Session?["UserMachine"]?.ToString() ?? string.Empty,
                    browserInfo: AuditDAL.GetClientBrowser(),
                    remarks: remarks
                );
            }
            catch
            {
                // ignore logging failures
            }
        }

        protected void LogAction(
            string actionType,
            string recordId,
            string oldData,
            string newData,
            string remarks,
            string tableName,
            string moduleName
        )
        {
            try
            {
                if (CurrentUser == null) return;
                AuditDAL.InsertAuditLog(
                    userId: CurrentUser.UserID,
                    userName: CurrentUser.UserName,
                    moduleName: moduleName ?? DefaultAuditModuleName,
                    tableName: tableName ?? DefaultAuditTableName,
                    recordId: recordId ?? string.Empty,
                    actionType: actionType,
                    oldData: oldData,
                    newData: newData,
                    ipAddress: AuditDAL.GetClientIp(),
                    machineName: HttpContext.Current?.Session?["UserMachine"]?.ToString() ?? string.Empty,
                    browserInfo: AuditDAL.GetClientBrowser(),
                    remarks: remarks
                );
            }
            catch
            {
                // ignore logging failures
            }
        }
    }
}
