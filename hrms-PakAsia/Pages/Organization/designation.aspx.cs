using HRMSLib.BusinessLogic;
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
    public partial class designation : System.Web.UI.Page
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
            CheckSession();
            if (!IsPostBack)
            {
                CurrentPage = 1;
                BindDesignations();
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
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private void ClearForm()
        {
            Designation.Text = "";
            ddlActive.SelectedValue = "1";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? DesignationID = ViewState["DesignationID"] as int?;
            DesignationDAL dal = new DesignationDAL();
            bool IsSaved = false;
            if (DesignationID.HasValue)
            {
                // UPDATE
                IsSaved = dal.DesignationData(
                    2,
                    Designation.Text,
                    ddlActive.SelectedValue,
                    DesignationID.Value.ToString()
                );

                ViewState["DepartmentID"] = null;
                ShowAlert("Department updated successfully", "success");
            }
            else
            {
                // INSERT
                IsSaved = dal.DesignationData(
                    1,
                     Designation.Text,
                     ddlActive.SelectedValue,
                     ""
                );

                ShowAlert("Department created successfully", "success");
            }

            ClearForm();
            BindDesignations();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindDesignations();
        }

        protected void rptDesignation_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int DesignationID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditDesignation")
            {
                LoadDesignationForEdit(DesignationID);
            }
            else if (e.CommandName == "DeleteDesignation")
            {
                DeleteUser(DesignationID);
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindDesignations();
            }
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument);
                BindDesignations();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindDesignations();
        }
        private void BindDesignations()
        {
            DesignationDAL dal = new DesignationDAL();

            int total;
            var dt = dal.GetDesignationsPaged(
                CurrentPage,
                PageSize,
                txtSearch.Text.Trim(),
                "DesignationName",
                "ASC",
                out total);

            TotalRecords = total;

            rptDesignation.DataSource = dt;
            rptDesignation.DataBind();

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
        private void LoadDesignationForEdit(int DesignationID)
        {
            DesignationDAL dal = new DesignationDAL();
            DataRow dr = dal.GetDesignationById(DesignationID);

            if (dr == null) return;

            Designation.Text = dr["DesignationName"].ToString();
            ddlActive.SelectedValue = dr["Status"].ToString();

            // Store UserID for update
            ViewState["DesignationID"] = DesignationID;

            ShowAlert("Designation loaded for editing", "info");
        }
        private void DeleteUser(int DesignationID)
        {
            DesignationDAL dal = new DesignationDAL();
            dal.DeleteDesignation(DesignationID);

            ShowAlert("Designation deleted successfully", "warning");
            BindDesignations();
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