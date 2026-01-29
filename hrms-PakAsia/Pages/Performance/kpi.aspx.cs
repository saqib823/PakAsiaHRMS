using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Performance
{
    public partial class kpi : Page
    {
        private const int PageSize = 10;
        LoggedInUser currentUser = null;

        private int PageIndex
        {
            get => ViewState["PageIndex"] == null ? 1 : (int)ViewState["PageIndex"];
            set => ViewState["PageIndex"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                LoadDDLs();
                LoadKPIList();
            }
        }
        public LoggedInUser GetSessionData()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            return currentUser;
        }

        public void CheckSession()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            if (currentUser == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        #region LOADERS

        private void LoadDDLs()
        {
            ddlEmployee.DataSource = CommonDAL.GetEmployeeswithEmployeeNumber();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));

            
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
            if (!DateTime.TryParse(txtFrom.Text, out DateTime fromDate) ||
                !DateTime.TryParse(txtTo.Text, out DateTime toDate))
            {
                ShowAlert("Invalid date range", "danger");
                return;
            }

            string employeeID = ddlEmployee.SelectedValue;

            DataRow dr = AttendanceDAL.GetEmployeeKPI(employeeID, fromDate, toDate);

            if (dr == null) return;

            txtAttendance.Text = dr["AttendancePercentage"].ToString() + "%";
            txtPunctuality.Text = dr["PunctualityPercentage"].ToString() + "%";
            txtFinalScore.Text = dr["FinalScore"].ToString();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0" || txtFrom.Text == "" || txtTo.Text == "")
            {
                ShowAlert("Select the employee and duration", "warning");
                return;
            }

            decimal finalScore = ToDecimal(txtFinalScore.Text);
            DateTime From = Convert.ToDateTime(txtFrom.Text);
            DateTime To = Convert.ToDateTime(txtTo.Text);
            
            KPIDAL.SaveEmployeeKPI(
                employeeId: Convert.ToInt32(ddlEmployee.SelectedValue),
                From: From,
                To: To,
                attendance: ToDecimal(txtAttendance.Text),
                punctuality: ToDecimal(txtPunctuality.Text),
                taskCompletion: ToDecimal(txtTaskCompletion.Text),
                overtime: ToDecimal(txtOvertime.Text),
                finalScore: finalScore,
                grade: GetGrade(finalScore),
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
