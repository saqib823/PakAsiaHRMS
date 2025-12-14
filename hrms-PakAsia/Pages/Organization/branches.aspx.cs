using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Organization
{
    public partial class branches : System.Web.UI.Page
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
                BindBranches();
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
            BranchName.Text = "";
            ddlActive.SelectedValue = "1";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? BranchID = ViewState["BranchID"] as int?;
            BranchDAL dal = new BranchDAL(); // You’ll provide this DAL
            bool IsSaved = false;

            if (BranchID.HasValue)
            {
                // UPDATE
                IsSaved = dal.SaveBranch(
                    2, // Mode 2 = Update
                    BranchID.Value,
                    BranchName.Text,
                    Location.Text,
                    Convert.ToInt32(ddlActive.SelectedValue),
                    GetCurrentUserID()
                );

                ViewState["BranchID"] = null;
                ShowAlert("Branch updated successfully", "success");
            }
            else
            {
                // INSERT
                IsSaved = dal.SaveBranch(
                    1, // Mode 1 = Insert
                    null,
                    BranchName.Text,
                    Location.Text,
                    Convert.ToInt32(ddlActive.SelectedValue),
                    GetCurrentUserID()
                );

                ShowAlert("Branch created successfully", "success");
            }

            ClearForm();
            BindBranches();
        }

        protected void rptBranches_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int BranchID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditBranch")
            {
                LoadBranchForEdit(BranchID);
            }
            else if (e.CommandName == "DeleteBranch")
            {
                DeleteBranch(BranchID);
            }
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument);
                BindBranches();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindBranches();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindBranches();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindBranches();
        }

        private void BindBranches()
        {
            BranchDAL dal = new BranchDAL();
            int total;
            DataTable dt = dal.GetBranchesPaged(
                CurrentPage,
                PageSize,
                txtSearch.Text.Trim(),
                "BranchName",
                "ASC",
                out total
            );

            TotalRecords = total;

            rptBranches.DataSource = dt;
            rptBranches.DataBind();

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

        private void LoadBranchForEdit(int BranchID)
        {
            BranchDAL dal = new BranchDAL();
            DataRow dr = dal.GetBranchById(BranchID);

            if (dr == null) return;

            BranchName.Text = dr["BranchName"].ToString();
            Location.Text = dr["Location"].ToString();
            ddlActive.SelectedValue = dr["Status"].ToString();

            ViewState["BranchID"] = BranchID;

            ShowAlert("Branch loaded for editing", "info");
        }

        private void DeleteBranch(int BranchID)
        {
            BranchDAL dal = new BranchDAL();
            dal.DeleteBranch(BranchID);

            ShowAlert("Branch deleted successfully", "warning");
            BindBranches();
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
            setTimeout(function() {{
                var alert = document.getElementById('autoAlert');
                if (alert) {{
                    alert.classList.remove('show');
                    alert.classList.add('hide');
                }}
            }}, 3000);
        </script>"
            });
        }

        // Dummy method for current user id, replace with actual session
        private int GetCurrentUserID()
        {
            LoggedInUser currentUser =
                 HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
            return currentUser.UserID;
        }
    }
}
