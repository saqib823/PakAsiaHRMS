using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Attendance
{
    public partial class manualAttenndence : System.Web.UI.Page
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
                ddlEmployees.DataSource = CommonDAL.GetEmployees_EmpNO_DDL();
                ddlEmployees.DataBind();
                ddlEmployees.Items.Insert(0, new ListItem("Select One", "0"));

                CurrentPage = 1;
                BindAttendance();
            }

            currentUser = GetSessionData();
        }

        private void CheckSession()
        {
            if (Session["LoggedInUser"] == null)
                Response.Redirect("~/Default.aspx");
        }

        private LoggedInUser GetSessionData()
        {
            return Session["LoggedInUser"] as LoggedInUser;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? attendanceId = ViewState["EditAttendanceID"] as int?;

            DateTime punchDateTime = DateTime.Parse(txtPunchDate.Text + " " + txtPunchTime.Text);

            AttendanceDAL.SaveAttendance(
                attendanceId.HasValue ? 2 : 1,
                attendanceId,
                ddlEmployees.SelectedValue.Trim(),
                ddlEmployees.SelectedItem.Text.Trim(),
                DateTime.Parse(txtPunchDate.Text),
                punchDateTime,
                ddlPunchType.SelectedValue,
                "Manual Entry",
                currentUser.UserID
            );

            ViewState["EditAttendanceID"] = null;

            ShowAlert(attendanceId.HasValue ? "Attendance updated successfully" : "Attendance added successfully", "success");

            ClearForm();
            BindAttendance();
        }

        private void ClearForm()
        {
            ddlEmployees.SelectedValue = "0";
            txtPunchDate.Text = "";
            txtPunchTime.Text = "";
            ddlPunchType.SelectedIndex = 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void BindAttendance()
        {
            int total;
            DataTable dt = AttendanceDAL.GetAttendancePaged(
                CurrentPage,
                PageSize,
                txtSearch.Text.Trim(),
                out total
            );

            TotalRecords = total;

            rptAttendance.DataSource = dt;
            rptAttendance.DataBind();

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
                    IsCurrent = i == CurrentPage
                });
            }

            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindAttendance();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindAttendance();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindAttendance();
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument);
                BindAttendance();
            }
        }

        protected void rptAttendance_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Edit")
            {
                DataRow dr = AttendanceDAL.GetById(id);

                ddlEmployees.SelectedValue = dr["EmpNo"].ToString();
                txtPunchDate.Text = Convert.ToDateTime(dr["PunchDate"]).ToString("yyyy-MM-dd");
                txtPunchTime.Text = Convert.ToDateTime(dr["PunchDateTime"]).ToString("HH:mm");
                ddlPunchType.SelectedValue = dr["PunchType"].ToString();

                ViewState["EditAttendanceID"] = id;

                ShowAlert("Attendance loaded for editing", "info");
            }
            else if (e.CommandName == "Delete")
            {
                AttendanceDAL.Delete(id);
                ShowAlert("Attendance deleted successfully", "warning");
                BindAttendance();
            }
        }

        private void ShowAlert(string message, string css)
        {
            phAlert.Controls.Clear();
            phAlert.Controls.Add(new Literal
            {
                Text = $@"
                <div id='autoAlert' class='alert alert-{css} alert-dismissible fade show'>
                    {message}
                </div>
                <script>
                    setTimeout(function(){{
                        var a=document.getElementById('autoAlert');
                        if(a) a.remove();
                    }},3000);
                </script>"
            });
        }
    }
}
