using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace hrms_PakAsia.Pages.Organization
{
    public partial class AuditLogs : hrms_PakAsia.BasePage
    {
        private DataTable AllLogs
        {
            get => ViewState["AllLogs"] as DataTable;
            set => ViewState["AllLogs"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            if (!IsPostBack)
            {
                LoadLogs();
                BindGrid();
            }
        }
        public void CheckSession()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            if (currentUser == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        private void LoadLogs()
        {
            AllLogs = AuditDAL.GetAllAuditLogs();
        }

        private void BindGrid()
        {
            if (AllLogs == null)
            {
                LoadLogs();
            }

            var query = AllLogs.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtUser.Text))
            {
                string user = txtUser.Text.Trim();
                query = query.Where(r => r.Field<string>("UserName") != null &&
                                         r.Field<string>("UserName").IndexOf(user, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(txtModule.Text))
            {
                string module = txtModule.Text.Trim();
                query = query.Where(r => r.Field<string>("ModuleName") != null &&
                                         r.Field<string>("ModuleName").IndexOf(module, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(txtAction.Text))
            {
                string action = txtAction.Text.Trim();
                query = query.Where(r => r.Field<string>("ActionType") != null &&
                                         r.Field<string>("ActionType").IndexOf(action, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(txtIP.Text))
            {
                string ip = txtIP.Text.Trim();
                query = query.Where(r => r.Field<string>("IPAddress") != null &&
                                         r.Field<string>("IPAddress").IndexOf(ip, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string s = txtSearch.Text.Trim();
                query = query.Where(r =>
                    (r.Field<string>("Remarks") ?? string.Empty).IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (r.Field<string>("OldData") ?? string.Empty).IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (r.Field<string>("NewData") ?? string.Empty).IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            DataTable filtered = query.Any() ? query.CopyToDataTable() : AllLogs.Clone();

            gvLogs.DataSource = filtered;
            gvLogs.DataBind();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvLogs.PageIndex = 0;
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtUser.Text = txtModule.Text = txtAction.Text = txtIP.Text = txtSearch.Text = string.Empty;
            gvLogs.PageIndex = 0;
            BindGrid();
        }

        protected void gvLogs_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvLogs.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}

