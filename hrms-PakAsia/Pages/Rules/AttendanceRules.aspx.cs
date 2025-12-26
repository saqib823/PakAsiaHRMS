using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Rules
{
    public partial class AttendanceRules : System.Web.UI.Page
    {
        private AttendanceRulesDAL dal = new AttendanceRulesDAL();
        private int PageSize = 10;

        private int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }

        private string SearchText
        {
            get => ViewState["SearchText"] != null ? ViewState["SearchText"].ToString() : string.Empty;
            set => ViewState["SearchText"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRules();
            }
        }

        private void BindRules()
        {
            int totalRecords;
            DataTable dt = dal.GetRules(SearchText, CurrentPage, PageSize, out totalRecords);

            rptRules.DataSource = dt;
            rptRules.DataBind();

            int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            lblPageInfo.Text = $"Page {CurrentPage} of {totalPages}";

            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
        }

        protected void btnSaveRule_Click(object sender, EventArgs e)
        {
            int ruleID = string.IsNullOrEmpty(hfRuleID.Value) ? 0 : Convert.ToInt32(hfRuleID.Value);

            // Late / EarlyLeave fields
            int? graceMinutes = string.IsNullOrEmpty(txtLateGrace.Text) ? null : (int?)Convert.ToInt32(txtLateGrace.Text);
            int? lateAllowance = string.IsNullOrEmpty(txtLateAllowance.Text) ? null : (int?)Convert.ToInt32(txtLateAllowance.Text);
            int? halfDayThreshold = string.IsNullOrEmpty(txtLateHalfDay.Text) ? null : (int?)Convert.ToInt32(txtLateHalfDay.Text);
            int? absentThreshold = string.IsNullOrEmpty(txtLateAbsent.Text) ? null : (int?)Convert.ToInt32(txtLateAbsent.Text);
            int? allowedEarlyLeaves = string.IsNullOrEmpty(txtAllowedEarlyLeaves.Text) ? null : (int?)Convert.ToInt32(txtAllowedEarlyLeaves.Text);

            // Overtime fields
            int? preShiftOT = string.IsNullOrEmpty(txtPreShiftOT.Text) ? null : (int?)Convert.ToInt32(txtPreShiftOT.Text);
            int? postShiftOT = string.IsNullOrEmpty(txtPostShiftOT.Text) ? null : (int?)Convert.ToInt32(txtPostShiftOT.Text);
            int? maxOTHours = string.IsNullOrEmpty(txtMaxOT.Text) ? null : (int?)Convert.ToInt32(txtMaxOT.Text);
            bool weekendOT = chkWeekendOT.Checked;
            bool holidayOT = chkHolidayOT.Checked;

            // Weekly Off fields
            string weeklyOffPattern = ddlWeeklyPattern.SelectedValue;
            int? branchHolidayID = string.IsNullOrEmpty(txtBranchHolidayID.Text) ? null : (int?)Convert.ToInt32(txtBranchHolidayID.Text);

            dal.SaveRule(
                ruleID,
                ddlRuleType.SelectedValue,
                txtRuleName.Text.Trim(),
                graceMinutes,
                lateAllowance,
                halfDayThreshold,
                absentThreshold,
                allowedEarlyLeaves,
                preShiftOT,
                postShiftOT,
                maxOTHours,
                weekendOT,
                holidayOT,
                weeklyOffPattern,
                branchHolidayID,
                "CurrentUser"
            );

            ClearForm();
            BindRules();
        }

        protected void rptRules_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int ruleID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRule")
            {
                DataRow dr = dal.GetRuleByID(ruleID);
                if (dr != null)
                {
                    hfRuleID.Value = dr["RuleID"].ToString();
                    ddlRuleType.SelectedValue = dr["RuleType"].ToString();
                    txtRuleName.Text = dr["RuleName"].ToString();

                    // Show values based on RuleType
                    txtLateGrace.Text = dr["GraceMinutes"]?.ToString() ?? "";
                    txtLateAllowance.Text = dr["LateAllowance"]?.ToString() ?? "";
                    txtLateHalfDay.Text = dr["HalfDayThreshold"]?.ToString() ?? "";
                    txtLateAbsent.Text = dr["AbsentThreshold"]?.ToString() ?? "";
                    txtAllowedEarlyLeaves.Text = dr["AllowedEarlyLeaves"]?.ToString() ?? "";

                    txtPreShiftOT.Text = dr["PreShiftOT"]?.ToString() ?? "";
                    txtPostShiftOT.Text = dr["PostShiftOT"]?.ToString() ?? "";
                    txtMaxOT.Text = dr["MaxOTHours"]?.ToString() ?? "";
                    chkWeekendOT.Checked = dr["WeekendOT"] != DBNull.Value && (bool)dr["WeekendOT"];
                    chkHolidayOT.Checked = dr["HolidayOT"] != DBNull.Value && (bool)dr["HolidayOT"];

                    ddlWeeklyPattern.SelectedValue = dr["WeeklyOffPattern"]?.ToString() ?? "";
                    txtBranchHolidayID.Text = dr["BranchHolidayID"]?.ToString() ?? "";
                }
            }
            else if (e.CommandName == "DeleteRule")
            {
                dal.DeleteRule(ruleID);
                BindRules();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1) { CurrentPage--; BindRules(); }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++; BindRules();
        }

        private void ClearForm()
        {
            hfRuleID.Value = "0";
            ddlRuleType.SelectedIndex = 0;
            txtRuleName.Text = string.Empty;

            txtLateGrace.Text = txtLateAllowance.Text = txtLateHalfDay.Text = txtLateAbsent.Text = txtAllowedEarlyLeaves.Text = "";
            txtPreShiftOT.Text = txtPostShiftOT.Text = txtMaxOT.Text = "";
            chkWeekendOT.Checked = chkHolidayOT.Checked = false;
            ddlWeeklyPattern.SelectedIndex = 0;
            txtBranchHolidayID.Text = "";
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler source, string eventArgument)
        {
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.StartsWith("search$"))
            {
                SearchText = eventArgument.Split('$')[1];
                CurrentPage = 1;
                BindRules();
            }

            base.RaisePostBackEvent(source, eventArgument);
        }

        protected void ddlRuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlLate.Visible = ddlRuleType.SelectedValue == "Late";
            pnlEarlyLeave.Visible = ddlRuleType.SelectedValue == "EarlyLeave";
            pnlOvertime.Visible = ddlRuleType.SelectedValue == "Overtime";
            pnlWeeklyOff.Visible = ddlRuleType.SelectedValue == "WeeklyOff";
        }
    }
}
