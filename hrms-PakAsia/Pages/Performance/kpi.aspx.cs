using HRMSLib.DataLayer;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Performance
{
    public partial class kpi : Page
    {
        private const int PageSize = 10;

        private int PageIndex
        {
            get => ViewState["PageIndex"] == null ? 1 : (int)ViewState["PageIndex"];
            set => ViewState["PageIndex"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDDLs();
                LoadKPIList();
            }
        }

        #region LOADERS

        private void LoadDDLs()
        {
            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));

            ddlMonth.DataSource = CommonDAL.GetMonths();
            ddlMonth.DataTextField = "Name";
            ddlMonth.DataValueField = "ID";
            ddlMonth.DataBind();
            ddlMonth.Items.Insert(0, new ListItem("-- Select Month --", "0"));
        }

        private void LoadKPIList()
        {
            int total;
            rptKPI.DataSource = KPIDAL.GetEmployeeKPI(
                txtSearch.Text.Trim(),
                PageIndex,
                PageSize,
                out total
            );
            rptKPI.DataBind();
        }

        private void LoadGoalAchievement()
        {
            if (ddlEmployee.SelectedValue == "0") return;

            txtGoal.Text = KPIDAL
                .GetGoalAchievement(Convert.ToInt32(ddlEmployee.SelectedValue), DateTime.Now.Year)
                .ToString("0.00");
        }

        #endregion

        #region EVENTS

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGoalAchievement();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            PageIndex = 1;
            LoadKPIList();
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            decimal finalScore =
                (ToDecimal(txtAttendance.Text) * 0.25m) +
                (ToDecimal(txtPunctuality.Text) * 0.20m) +
                (ToDecimal(txtTaskCompletion.Text) * 0.30m) +
                (ToDecimal(txtGoal.Text) * 0.15m) +
                (ToDecimal(txtOvertime.Text) * 0.10m);

            txtFinalScore.Text = finalScore.ToString("0.00");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0" || ddlMonth.SelectedValue == "0")
            {
                ShowAlert("Please select Employee and Month", "warning");
                return;
            }

            decimal finalScore = ToDecimal(txtFinalScore.Text);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            string periodType = ddlPeriodType.SelectedValue;

            int? quarter = null;

            if (periodType == "Q")
            {
                quarter = GetQuarter(month);
            }

            KPIDAL.SaveEmployeeKPI(
                employeeId: Convert.ToInt32(ddlEmployee.SelectedValue),
                year: DateTime.Now.Year,
                month: month,
                attendance: ToDecimal(txtAttendance.Text),
                punctuality: ToDecimal(txtPunctuality.Text),
                taskCompletion: ToDecimal(txtTaskCompletion.Text),
                overtime: ToDecimal(txtOvertime.Text),
                finalScore: finalScore,
                grade: GetGrade(finalScore),
                periodType: periodType,
                quarter: quarter,
                createdBy: Convert.ToInt32(Session["UserID"])
            );

            ClearForm();
            LoadKPIList();
            ShowAlert("KPI saved successfully", "success");
        }

        protected void DeleteKPI(object sender, CommandEventArgs e)
        {
            KPIDAL.DeleteKPI(Convert.ToInt32(e.CommandArgument));
            LoadKPIList();
        }

        #endregion

        #region HELPERS

        private int GetQuarter(int month)
        {
            return (month - 1) / 3 + 1;
        }

        private string GetGrade(decimal score)
        {
            if (score >= 90) return "A";
            if (score >= 75) return "B";
            if (score >= 60) return "C";
            return "D";
        }

        private decimal ToDecimal(string value)
        {
            decimal.TryParse(value, out decimal result);
            return result;
        }

        private void ClearForm()
        {
            txtAttendance.Text = txtPunctuality.Text =
            txtTaskCompletion.Text = txtOvertime.Text =
            txtFinalScore.Text = string.Empty;
        }

        private void ShowAlert(string message, string cssClass)
        {
            phAlert.Controls.Clear();
            phAlert.Controls.Add(new Literal
            {
                Text = $@"
                <div class='alert alert-{cssClass} alert-dismissible fade show'>
                    {message}
                    <button type='button' class='btn-close' data-bs-dismiss='alert'></button>
                </div>"
            });
        }

        #endregion
    }
}
