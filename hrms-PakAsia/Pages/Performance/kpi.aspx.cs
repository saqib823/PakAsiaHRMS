using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Performance
{
    public partial class kpi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDDLs();
                LoadKPIList();
            }
        }
        private void LoadDDLs()
        {
            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("Select One", "0"));

            ddlMonth.DataSource = CommonDAL.GetMonths();
            ddlMonth.DataBind();
            ddlMonth.Items.Insert(0, new ListItem("Select One", "0"));
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            decimal attendance = ToDecimal(txtAttendance.Text);
            decimal punctuality = ToDecimal(txtPunctuality.Text);
            decimal task = ToDecimal(txtTaskCompletion.Text);
            decimal overtime = ToDecimal(txtOvertime.Text);

            decimal finalScore =
                (attendance * 0.30m) +
                (punctuality * 0.20m) +
                (task * 0.40m) +
                (overtime * 0.10m);

            txtFinalScore.Text = finalScore.ToString("0.00");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal finalScore = ToDecimal(txtFinalScore.Text);
            string grade = GetGrade(finalScore);

            HRMSLib.DataLayer.KPIDAL.SaveEmployeeKPI(
                employeeId: Convert.ToInt32(ddlEmployee.SelectedValue),
                year: DateTime.Now.Year,
                month: Convert.ToInt32(ddlMonth.SelectedValue),
                attendance: ToDecimal(txtAttendance.Text),
                punctuality: ToDecimal(txtPunctuality.Text),
                taskCompletion: ToDecimal(txtTaskCompletion.Text),
                overtime: ToDecimal(txtOvertime.Text),
                finalScore: finalScore,
                grade: grade,
                createdBy: Convert.ToInt32(Session["UserID"])
            );

            LoadKPIList();
            ShowAlert("KPI saved successfully", "success");
        }
        private void LoadKPIList()
        {
            rptKPI.DataSource = KPIDAL.GetEmployeeKPI();
            rptKPI.DataBind();
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            rptKPI.DataSource =
              KPIDAL.GetEmployeeKPI(txtSearch.Text);

            rptKPI.DataBind();
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

        private void ShowAlert(string message, string cssClass)
        {
            var alertScript = $@"
                <div class='alert alert-{cssClass} alert-dismissible fade show' role='alert'>
                    {message}
                    <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                </div>
                <script>
                    setTimeout(function() {{
                        var alert = document.querySelector('.alert');
                        if(alert) alert.remove();
                    }}, 5000);
                </script>";

            phAlert.Controls.Clear();
            phAlert.Controls.Add(new LiteralControl(alertScript));
        }

    }
}