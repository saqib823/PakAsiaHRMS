using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Shifts
{
    public partial class Shifts : System.Web.UI.Page
    {
        private const int PageSize = 10;

        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] == null ? 1 : (int)ViewState["CurrentPage"]; }
            set { ViewState["CurrentPage"] = value; }
        }

        private int TotalRecords
        {
            get { return ViewState["TotalRecords"] == null ? 0 : (int)ViewState["TotalRecords"]; }
            set { ViewState["TotalRecords"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadShiftTypes();
                LoadShiftsPaged();
            }
        }

        private void LoadShiftTypes()
        {
            ddlShiftType.DataSource = ShiftDAL.GetShiftTypes();
            ddlShiftType.DataTextField = "Name";
            ddlShiftType.DataValueField = "ID";
            ddlShiftType.DataBind();

            ddlShiftType.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem("-- Select Shift Type --", "0"));
        }

        
        private void LoadShiftsPaged()
        {
            DataTable dt = ShiftDAL.GetShiftsPaged(CurrentPage, PageSize, out int totalRecords);

            TotalRecords = totalRecords;
            rptShifts.DataSource = dt;
            rptShifts.DataBind();

            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            lblPageInfo.Text = $"Page {CurrentPage} of {totalPages}";

            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            LoadShiftsPaged();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            LoadShiftsPaged();
        }
        protected void rptShifts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int shiftId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditShift")
            {
                LoadShiftForEdit(shiftId);
            }
            else if (e.CommandName == "DeleteShift")
            {
                ShiftDAL.DeleteShift(shiftId);

                if ((CurrentPage - 1) * PageSize >= TotalRecords - 1 && CurrentPage > 1)
                    CurrentPage--;

                LoadShiftsPaged();
            }
        }
        private void LoadShiftForEdit(int shiftId)
        {
            DataRow dr = ShiftDAL.GetShiftByID(shiftId);

            if (dr == null) return;

            hfShiftID.Value = shiftId.ToString();
            txtShiftName.Text = dr["ShiftName"].ToString();
            ddlShiftType.SelectedValue = dr["ShiftTypeID"].ToString();
            txtGraceMinutes.Text = dr["GraceMinutes"].ToString();
            txtMinWorkMinutes.Text = dr["MinWorkMinutes"].ToString();

            txtStartTime.Text = dr["StartTime"] == DBNull.Value ? "" :
                TimeSpan.Parse(dr["StartTime"].ToString()).ToString(@"hh\:mm");

            txtEndTime.Text = dr["EndTime"] == DBNull.Value ? "" :
                TimeSpan.Parse(dr["EndTime"].ToString()).ToString(@"hh\:mm");

            chkCrossMidnight.Checked = Convert.ToBoolean(dr["IsCrossMidnight"]);

            btnSave.Text = "Update Shift";
            btnSave.CssClass = "btn btn-warning px-4";
        }
        private void ResetForm()
        {
            hfShiftID.Value = "0";
            txtShiftName.Text = "";
            ddlShiftType.SelectedIndex = 0;
            txtGraceMinutes.Text = "";
            txtMinWorkMinutes.Text = "";
            txtStartTime.Text = "";
            txtEndTime.Text = "";
            chkCrossMidnight.Checked = false;

            btnSave.Text = "Save Shift";
            btnSave.CssClass = "btn btn-success px-4";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            TimeSpan? startTime = null;
            TimeSpan? endTime = null;

            if (!string.IsNullOrEmpty(txtStartTime.Text))
                startTime = TimeSpan.Parse(txtStartTime.Text);

            if (!string.IsNullOrEmpty(txtEndTime.Text))
                endTime = TimeSpan.Parse(txtEndTime.Text);

            Shift shift = new Shift
            {
                ShiftID = Convert.ToInt32(hfShiftID.Value), // KEY LINE
                ShiftName = txtShiftName.Text.Trim(),
                ShiftTypeID = Convert.ToInt32(ddlShiftType.SelectedValue),
                StartTime = startTime,
                EndTime = endTime,
                GraceMinutes = string.IsNullOrEmpty(txtGraceMinutes.Text) ? 0 : Convert.ToInt32(txtGraceMinutes.Text),
                MinWorkMinutes = string.IsNullOrEmpty(txtMinWorkMinutes.Text) ? 0 : Convert.ToInt32(txtMinWorkMinutes.Text),
                IsCrossMidnight = chkCrossMidnight.Checked
            };

            if (shift.ShiftID == 0)
            {
                // INSERT
                ShiftDAL.InsertShift(shift);
            }
            else
            {
                // UPDATE
                ShiftDAL.UpdateShift(shift);
            }

            ResetForm();
            LoadShiftsPaged();
        }

    }
}