using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Organization
{
    public partial class departments : System.Web.UI.Page
    {
        private int PageSize => 10;

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
            if (!IsPostBack)
            {
                CurrentPage = 1;
                BindDepartments();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private void ClearForm()
        {
            Department.Text = "";
            ddlActive.SelectedValue = "1";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? DepartmentID = ViewState["DepartmentID"] as int?;
            DepartmentDAL dal = new DepartmentDAL();
            bool IsSaved = false;
            if (DepartmentID.HasValue)
            {
                // UPDATE
                IsSaved = dal.DepartmentData(
                    2,
                    Department.Text,
                    ddlActive.SelectedValue,
                    DepartmentID.Value.ToString()
                );

                ViewState["DepartmentID"] = null;
                ShowAlert("Department updated successfully", "success");
            }
            else
            {
                // INSERT
                IsSaved = dal.DepartmentData(
                    1,
                     Department.Text,
                     ddlActive.SelectedValue,
                     ""
                );

                ShowAlert("Department created successfully", "success");
            }

            ClearForm();
            BindDepartments();
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int DepartmentID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditDepartment")
            {
                LoadDepartmentForEdit(DepartmentID);
            }
            else if (e.CommandName == "DeleteDepartment")
            {
                DeleteUser(DepartmentID);
            }
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument);
                BindDepartments();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindDepartments();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindDepartments();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindDepartments();
        }
        private void BindDepartments()
        {
            DepartmentDAL dal = new DepartmentDAL();

            int total;
            var dt = dal.GetDepartmentsPaged(
                CurrentPage,
                PageSize,
                txtSearch.Text.Trim(),
                "DepartmentName",
                "ASC",
                out total);

            TotalRecords = total;

            rptUsers.DataSource = dt;
            rptUsers.DataBind();

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
        private void LoadDepartmentForEdit(int DepartmentID)
        {
            DepartmentDAL dal = new DepartmentDAL();
            DataRow dr = dal.GetDepartmentById(DepartmentID);

            if (dr == null) return;

            Department.Text = dr["DepartmentName"].ToString();
            ddlActive.SelectedValue = dr["Status"].ToString();
          
            // Store UserID for update
            ViewState["DepartmentID"] = DepartmentID;

            ShowAlert("Department loaded for editing", "info");
        }
        private void DeleteUser(int DepartmentID)
        {
            DepartmentDAL dal = new DepartmentDAL();
            dal.DeleteDepartment(DepartmentID);

            ShowAlert("Department deleted successfully", "warning");
            BindDepartments();
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