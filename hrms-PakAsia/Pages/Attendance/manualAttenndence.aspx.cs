using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Attendance
{
    public partial class manualAttenndence : System.Web.UI.Page
    {
        private int PageSize => 10;

        private int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }

        private int TotalRecords
        {
            get => ViewState["TotalRecords"] != null ? (int)ViewState["TotalRecords"] : 0;
            set => ViewState["TotalRecords"] = value;
        }

        LoggedInUser currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = Session["LoggedInUser"] as LoggedInUser;
            if (currentUser == null) Response.Redirect("~/Default.aspx");

            if (!IsPostBack)
            {
                ddlEmployees.DataSource = CommonDAL.GetEmployees_EmpNO_DDL();
                ddlEmployees.DataBind();
                ddlEmployees.Items.Insert(0, new ListItem("Select One", "0"));

                CurrentPage = 1;
                BindAttendance();
            }
        }

        #region Save / Clear
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Get current attendance ID (null if adding new)
                int? attendanceId = ViewState["EditAttendanceID"] as int?;

                // Parse date and time inputs
                if (!DateTime.TryParse(txtPunchDate.Text, out DateTime punchDate))
                {
                    ShowAlert("Invalid punch date.", "danger");
                    return;
                }

                if (!TimeSpan.TryParse(txtPunchTime.Text, out TimeSpan punchTime))
                {
                    ShowAlert("Invalid punch time.", "danger");
                    return;
                }

                // Determine mode: 1 = Insert, 2 = Update
                int mode = attendanceId.HasValue ? 2 : 1;

                if (mode == 2 && !attendanceId.HasValue)
                {
                    ShowAlert("Cannot update: Attendance ID missing.", "danger");
                    return;
                }

                // Save attendance
                bool success = AttendanceDAL.SaveAttendance(
                    mode,
                    attendanceId,
                    ddlEmployees.SelectedValue,
                    ddlEmployees.SelectedItem.Text,
                    punchDate,
                    punchTime,
                    ddlPunchType.SelectedValue,
                    "Manual Entry",
                    currentUser.UserID
                );

                    ShowAlert(
                        mode == 2 ? "Attendance updated successfully." : "Attendance added successfully.",
                        "success"
                    );
                
               
                // Clear edit state and form
                ViewState["EditAttendanceID"] = null;
                ClearForm();

                // Rebind the table
                BindAttendance();
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message, "danger");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            ddlEmployees.SelectedValue = "0";
            txtPunchDate.Text = "";
            txtPunchTime.Text = "";
            ddlPunchType.SelectedIndex = 0;
        }
        #endregion

        #region Bind Attendance / Paging
        private void BindAttendance()
        {
            int total;
            DataTable dt = AttendanceDAL.GetAttendancePaged(CurrentPage, PageSize, txtSearch.Text.Trim(), out total);
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
            var pages = new List<object>();
            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(new { PageNumber = i, IsCurrent = i == CurrentPage });
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
            if (CurrentPage > 1) CurrentPage--;
            BindAttendance();
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
        #endregion

        #region Repeater Edit / Delete
        protected void rptAttendance_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Edit")
            {
                DataRow dr = AttendanceDAL.GetById(id);
                if (dr == null) return;

                ddlEmployees.SelectedValue = dr["EmpNo"].ToString();
                txtPunchDate.Text = Convert.ToDateTime(dr["PunchDate"]).ToString("yyyy-MM-dd");

                // TimeSpan from SQL TIME(7)
                txtPunchTime.Text = DateTime.Today
                    .Add((TimeSpan)dr["PunchDateTime"])
                    .ToString("HH:mm");

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
        #endregion

        #region Alert
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
        #endregion
    }
}
