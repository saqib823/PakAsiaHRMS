using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages
{
    public partial class roles : hrms_PakAsia.BasePage
    {
        private int PageSize => 10;
        LoggedInUser currentUser = null;

        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        private int TotalRecords
        {
            get { return ViewState["TotalRecords"] != null ? (int)ViewState["TotalRecords"] : 0; }
            set { ViewState["TotalRecords"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            if (!IsPostBack)
            {
                CurrentPage = 1;
                BindRoles();
                // landing logged by BasePage.OnLoad
            }
            currentUser = GetSessionData();

        }
        public LoggedInUser GetSessionData()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            return currentUser;
        }

        public void CheckSession()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            if (currentUser == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? RoleID = ViewState["RoleID"] as int?;
            RoleDAL dal = new RoleDAL();
            bool IsSaved = false;
            if (RoleID.HasValue)
            {
                // UPDATE
                IsSaved = dal.RoleData(
                    2,
                    RoleName.Text,
                    ddlActive.SelectedValue,
                    RoleID.Value.ToString()
                );

                ViewState["RoleID"] = null;
                ShowAlert("Role updated successfully", "success");
                LogAction("Update Role", recordId: RoleID.Value.ToString(), newData: $"RoleName={RoleName.Text};Status={ddlActive.SelectedValue}", remarks: "Role updated from UI");
            }
            else
            {
                // INSERT
                IsSaved = dal.RoleData(
                    1,
                     RoleName.Text,
                     ddlActive.SelectedValue,
                     ""
                );

                ShowAlert("Role created successfully", "success");
                LogAction("Insert Role", newData: $"RoleName={RoleName.Text};Status={ddlActive.SelectedValue}", remarks: "Role created from UI");
            }

            ClearForm();
            BindRoles();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void rptRole_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int RoleID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRole")
            {
                LoadRoleForEdit(RoleID);
            }
            else if (e.CommandName == "DeleteRole")
            {
                DeleteRole(RoleID);
            }
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument);
                BindRoles();
            }
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindRoles();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindRoles();
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindRoles();
            LogAction("Search Roles", remarks: $"Search='{txtSearch.Text?.Trim()}'");
        }
        private void ClearForm()
        {
            RoleName.Text = "";
            ddlActive.SelectedValue = "1";
        }
        private void BindRoles()
        {
            RoleDAL dal = new RoleDAL();

            int total;
            var dt = dal.GetRolesPaged(
                CurrentPage,
                PageSize,
                txtSearch.Text.Trim(),
                "RoleName",
                "ASC",
                out total);

            TotalRecords = total;

            rptRole.DataSource = dt;
            rptRole.DataBind();

            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            lblPageInfo.Text = $"Page {CurrentPage} of {totalPages} (Total: {TotalRecords})";

            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
            BindPager();

        }
        private void BindPager()
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            List<object> pages = new List<object>();

            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(new
                {
                    PageNumber = i,
                    IsCurrent = (i == CurrentPage)
                });
            }

            rptPager.DataSource = pages;
            rptPager.DataBind();

            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
        }
        private void LoadRoleForEdit(int RoleID)
        {
            RoleDAL dal = new RoleDAL();
            DataRow dr = dal.GetRoleById(RoleID);

            if (dr == null) return;

            RoleName.Text = dr["RoleName"].ToString();
            ddlActive.SelectedValue = dr["Status"].ToString();

            // Store UserID for update
            ViewState["RoleID"] = RoleID;

            ShowAlert("Role loaded for editing", "info");
        }
        private void DeleteRole(int RoleID)
        {
            RoleDAL dal = new RoleDAL();
            dal.DeleteRoles(RoleID);

            ShowAlert("Role deleted successfully", "warning");
            LogAction("Delete Role", recordId: RoleID.ToString(), remarks: "Role deleted from UI");
            BindRoles();
        }
        private void ShowAlert(string message, string css)
        {
            phAlert.Controls.Clear();

            phAlert.Controls.Add(new Literal
            {
                Text = $@"
        <div id='autoAlert' class='alert alert-{css} alert-dismissible fade show' role='alert'>
            {message}
        </div>

        <script>
            setTimeout(function () {{
                var alert = document.getElementById('autoAlert');
                if (alert) {{
                    alert.classList.remove('show');
                    alert.classList.add('hide');
                }}
            }}, 3000); // 3 seconds
        </script>"
            });
        }
    }
}