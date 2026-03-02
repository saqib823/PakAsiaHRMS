using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Performance
{
    public partial class kpi : hrms_PakAsia.BasePage
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
                // landing logged by BasePage.OnLoad
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
            LogAction("Search KPI", remarks: $"Search='{txtSearch.Text?.Trim()}'");
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Parse Employee & Dates
                string employeeNo = ddlEmployee.SelectedValue.Trim(); // Assuming you have txtEmployee for EmpNo
                DateTime fromDate = DateTime.Parse(txtFrom.Text); // From date textbox
                DateTime toDate = DateTime.Parse(txtTo.Text);     // To date textbox

                // 2️⃣ Fetch KPI from database using stored procedure
                DataRow kpiRow = AttendanceDAL.GetEmployeeAttendancePercentages(employeeNo, fromDate, toDate);

                decimal attendance = 0, punctuality = 0, overtime = 0;

                if (kpiRow != null)
                {
                    // Pull attendance, punctuality, overtime from DB
                    attendance = kpiRow["AttendancePercentage"] != DBNull.Value ? Convert.ToDecimal(kpiRow["AttendancePercentage"]) : 0;
                    punctuality = kpiRow["PunctualityPercentage"] != DBNull.Value ? Convert.ToDecimal(kpiRow["PunctualityPercentage"]) : 0;
                    overtime = kpiRow["OvertimeHours"] != DBNull.Value ? Convert.ToDecimal(kpiRow["OvertimeHours"]) : 0;

                    // Display in textboxes (optional)
                    txtAttendance.Text = attendance.ToString("0.00");
                    txtPunctuality.Text = punctuality.ToString("0.00");
                    txtOvertime.Text = overtime.ToString("0.00");
                }

                // 3️⃣ Parse Task Completion and Goal Achievement
                decimal taskCompletion = 0, goal = 0;

                decimal.TryParse(txtTaskCompletion.Text, out taskCompletion); // % or points
                decimal.TryParse(txtGoal.Text, out goal);                       // % or points

                // 4️⃣ Normalize overtime (0-100 scale)
                decimal maxOvertimeHours = 40; // adjust as per policy
                decimal overtimeScore = Math.Min(overtime / maxOvertimeHours * 100, 100);

                // 5️⃣ Assign weights
                decimal weightAttendance = 0.3m;
                decimal weightPunctuality = 0.2m;
                decimal weightOvertime = 0.1m;
                decimal weightTask = 0.2m;
                decimal weightGoal = 0.2m;

                // 6️⃣ Calculate final score
                decimal finalScore = (attendance * weightAttendance) +
                                     (punctuality * weightPunctuality) +
                                     (overtimeScore * weightOvertime) +
                                     (taskCompletion * weightTask) +
                                     (goal * weightGoal);

                // 7️⃣ Display rounded score
                txtFinalScore.Text = finalScore.ToString("0.00");
                LogAction("Calculate KPI", recordId: ddlEmployee.SelectedValue, remarks: $"Calculated KPI from {txtFrom.Text} to {txtTo.Text}");
            }
            catch (Exception ex)
            {
                txtFinalScore.Text = "Error: " + ex.Message;
            }
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
            int empID = CommonDAL.GetEmployeeIdByEmpNo(ddlEmployee.SelectedValue);
            if (empID == 0)
            {
                ShowAlert("Employee ID Not Found","warning");
                return;
            }
            KPIDAL.SaveEmployeeKPI(
                employeeId: empID, //Emp ID against Emp NO
                From: From,
                To: To,
                attendance: ToDecimal(txtAttendance.Text),
                punctuality: ToDecimal(txtPunctuality.Text),
                taskCompletion: ToDecimal(txtTaskCompletion.Text),
                overtime: ToDecimal(txtOvertime.Text),
                finalScore: finalScore,
                grade: GetGrade(finalScore),
                createdBy: Convert.ToInt32(Session["UserID"]),
                appraisalpct : Convert.ToDecimal(txtAppraisal.Text),
                currentbasic : Convert.ToDecimal(ltCurrentBasicSalary.Text),
                appraised : Convert.ToDecimal(ltAppraisedSalary.Text)

            );
            LogAction("Save KPI", recordId: empID.ToString(), newData: $"From={From:yyyy-MM-dd};To={To:yyyy-MM-dd};FinalScore={finalScore:0.00};Grade={GetGrade(finalScore)}", remarks: "KPI saved");
            decimal appraisalPercentage = Convert.ToDecimal(txtAppraisal.Text);
            decimal basicSalary = KPIDAL.GetEmployeeBasicSalary(empID);
            decimal appraisalAmount = basicSalary * appraisalPercentage / 100;

            ltCurrentBasicSalary.Text = basicSalary.ToString();
            ltAppraisedSalary.Text = (appraisalAmount + basicSalary).ToString();
            KPIDAL.UpdateEmployeeAppraisedSalary(empID, Convert.ToDecimal(ltAppraisedSalary.Text), appraisalAmount);
            ClearForm();
            LoadKPIList();
            ShowAlert("KPI saved successfully", "success");
        }

        protected void DeleteKPI(object sender, CommandEventArgs e)
        {
            int kpiId = Convert.ToInt32(e.CommandArgument);
            KPIDAL.DeleteKPI(kpiId);
            LogAction("Delete KPI", recordId: kpiId.ToString(), remarks: "KPI deleted");
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

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(txtFrom.Text, out DateTime fromDate) ||
               !DateTime.TryParse(txtTo.Text, out DateTime toDate))
            {
                ShowAlert("Invalid date range", "danger");
                return;
            }

            string employeeID = ddlEmployee.SelectedValue;

            DataRow dr = AttendanceDAL.GetEmployeeAttendancePercentages(employeeID, fromDate, toDate);

            if (dr == null) return;

            txtAttendance.Text = dr["AttendancePercentage"].ToString();
            txtPunctuality.Text = dr["PunctualityPercentage"].ToString();
            txtOvertime.Text = dr["OvertimeHours"].ToString();
        }

        protected void txtAppraisal_TextChanged(object sender, EventArgs e)
        {
            int empID = CommonDAL.GetEmployeeIdByEmpNo(ddlEmployee.SelectedValue);
            decimal appraisalPercentage = Convert.ToDecimal(txtAppraisal.Text);
            decimal basicSalary = KPIDAL.GetEmployeeBasicSalary(empID);
            decimal appraisalAmount = basicSalary * appraisalPercentage / 100;

            ltCurrentBasicSalary.Text = basicSalary.ToString();
            ltAppraisedSalary.Text = (appraisalAmount + basicSalary).ToString();
        }
    }
}
