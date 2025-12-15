using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace hrms_PakAsia.Pages
{
    public partial class signup : System.Web.UI.Page
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
        LoggedInUser currentUser = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            if (!IsPostBack)
            {
                InitialDataBindings();
                CurrentPage = 1;
                BindUsers();
            }
            currentUser = GetSessionData();

        }
        public void CheckSession()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            if (currentUser == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        public LoggedInUser GetSessionData()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            return currentUser;
        }

        private void InitialDataBindings()
        {
            try
            {
                ddlDepartment.DataSource = CommonDAL.GetDepartments();
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("Select One", "0"));

                ddlRole.DataSource = CommonDAL.GetRoles();
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("Select One", "0"));

                ddlBranch.DataSource = CommonDAL.GetBranches();
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem("Select One", "0"));

                Designation.DataSource = CommonDAL.GetDesignation();
                Designation.DataBind();
                Designation.Items.Insert(0, new ListItem("Select One", "0"));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? userId = ViewState["EditUserID"] as int?;

            string filePath = "";
            string contentType = "";

            if (customFile.PostedFile != null && customFile.PostedFile.ContentLength > 0)
            {
                // 1. Validate extension
                string ext = Path.GetExtension(customFile.PostedFile.FileName).ToLower();
                string[] allowedExt = { ".jpg", ".jpeg", ".png" };

                if (!allowedExt.Contains(ext))
                {
                    ShowAlert("Only JPG and PNG images are allowed", "danger");
                    return;
                }

                // 2. Create uploads folder if not exists
                string uploadFolder = Server.MapPath("~/Uploads/UserImages/");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                // 3. Generate unique filename
                string fileName = Guid.NewGuid().ToString() + ext;

                // 4. Full physical path
                string fullPath = Path.Combine(uploadFolder, fileName);

                // 5. Save file
                customFile.PostedFile.SaveAs(fullPath);

                // 6. Save relative path (for DB)
                filePath = "~/Uploads/UserImages/" + fileName;

                // 7. Content type (optional)
                contentType = customFile.PostedFile.ContentType;
            }

            UserDAL dal = new UserDAL();
            bool IsSaved = false;
            if (userId.HasValue)
            {
                // UPDATE
                IsSaved = dal.SaveUserData(
                    2,
                   UserName.Text.Trim(),
                   Password.Text,
                   FirstName.Text.Trim(),
                   LastName.Text.Trim(),
                   EmailAddress.Text.Trim(),
                   Cnic.Text.Trim(),
                   PhoneNumber.Text.Trim(),
                   ddlRole.SelectedValue,
                   ddlDepartment.SelectedValue,
                   currentUser.UserID.ToString(),
                   Designation.SelectedValue.Trim(),
                   filePath,
                   contentType,
                   userId,
                   ddlBranch.SelectedValue
               );

                ViewState["EditUserID"] = null;
                ShowAlert("User updated successfully", "success");
            }
            else
            {
                // INSERT
                IsSaved = dal.SaveUserData(
                    1,
                    UserName.Text.Trim(),
                    Password.Text,
                    FirstName.Text.Trim(),
                    LastName.Text.Trim(),
                    EmailAddress.Text.Trim(),
                    Cnic.Text.Trim(),
                    PhoneNumber.Text.Trim(),
                    ddlRole.SelectedValue,
                    ddlDepartment.SelectedValue,
                   currentUser.UserID.ToString(),
                    Designation.SelectedValue.Trim(),
                    filePath,
                    contentType,
                    userId,
                    ddlBranch.SelectedValue
                );

                ShowAlert("User created successfully", "success");
            }

            ClearForm();
            BindUsers();
        }

        // Optional helper method to clear the form
        private void ClearForm()
        {
            UserName.Text = "";
            EmailAddress.Text = "";
            FirstName.Text = "";
            LastName.Text = "";
            Designation.SelectedValue = "0";
            Cnic.Text = "";
            PhoneNumber.Text = "";
            Password.Text = "";
            ddlDepartment.SelectedIndex = 0;
            ddlRole.SelectedIndex = 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void BindUsers()
        {
            UserDAL dal = new UserDAL();

            int total;
            var dt = dal.GetUsersPaged(
                CurrentPage,
                PageSize,
                txtSearch.Text.Trim(),
                "UserName",
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

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindUsers();
        }
        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument);
                BindUsers();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindUsers();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindUsers();
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

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditUser")
            {
                LoadUserForEdit(userId);
            }
            else if (e.CommandName == "DeleteUser")
            {
                DeleteUser(userId);
            }
        }
        private void LoadUserForEdit(int userId)
        {
            UserDAL dal = new UserDAL();
            DataRow dr = dal.GetUserById(userId);

            if (dr == null) return;

            UserName.Text = dr["UserName"].ToString();
            EmailAddress.Text = dr["EmailAddress"].ToString();
            FirstName.Text = dr["FirstName"].ToString();
            LastName.Text = dr["LastName"].ToString();
            string designationId = dr["Designation"]?.ToString();

            if (!string.IsNullOrEmpty(designationId) &&
                Designation.Items.FindByValue(designationId) != null)
            {
                Designation.SelectedValue = designationId;
            }
            else
            {
                Designation.SelectedIndex = 0; // "Select One"
            }
            Cnic.Text = dr["CNIC"].ToString();
            PhoneNumber.Text = dr["PhoneNumber"].ToString();
           

            ddlDepartment.SelectedValue = dr["PrimaryDepartmentId"].ToString();
            Designation.SelectedValue = dr["Designation"].ToString();
            string branchId = dr["Branch"]?.ToString();

            if (!string.IsNullOrEmpty(branchId) &&
                ddlBranch.Items.FindByValue(branchId) != null)
            {
                ddlBranch.SelectedValue = branchId;
            }
            else
            {
                ddlBranch.SelectedIndex = 0; // "Select One"
            }
            ddlRole.SelectedValue = dr["RoleID"].ToString();

            // Store UserID for update
            ViewState["EditUserID"] = userId;

            ShowAlert("User loaded for editing", "info");
        }

        private void DeleteUser(int userId)
        {
            UserDAL dal = new UserDAL();
            dal.DeleteUser(userId);

            ShowAlert("User deleted successfully", "warning");
            BindUsers();
        }
    }
}
