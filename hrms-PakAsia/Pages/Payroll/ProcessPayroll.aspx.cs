using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Payroll
{
    public partial class ProcessPayroll : hrms_PakAsia.BasePage
    {
        private LoggedInUser currentUser;
        private readonly PayrollDAL dal = new PayrollDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();

            if (!IsPostBack)
            {
                LoadEmployees();
                InitializeDefaults();
            }
        }

        private void InitializeDefaults()
        {
            txtEffectiveFrom.Text = DateTime.Now.ToString("yyyy-MM-01");
            txtEffectiveTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

            dateFrom.Text = DateTime.Now.ToString("yyyy-MM-01");
            dateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ddlBranch.SelectedIndex = 0;
            ddlPayrollCycle.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;

            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Insert(0, new ListItem("-- Select Branch First --", "0"));
        }

        private LoggedInUser GetSessionData()
        {
            return HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
        }

        private void CheckSession()
        {
            if (GetSessionData() == null)
                Response.Redirect("~/Default.aspx");
        }

        private void LoadEmployees()
        {
            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-- All Employee --", "0"));

            ddlBranch.DataSource = CommonDAL.GetBranches();
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items.Insert(0, new ListItem("-- All Branch --", "0"));

            ddlPayrollCycle.Items.Clear();
            ddlPayrollCycle.Items.Add(new ListItem("Monthly", "2"));
            ddlPayrollCycle.Items.Add(new ListItem("Wages", "1"));
            ddlPayrollCycle.Items.Insert(0, new ListItem("-- All Cycles --", "0"));

            ddlGender.DataSource = CommonDAL.GetGender();
            ddlGender.DataTextField = "Name";
            ddlGender.DataValueField = "ID";
            ddlGender.DataBind();
            ddlGender.Items.Insert(0, new ListItem("-- All Genders --", "0"));
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDepartment.Items.Clear();

            if (ddlBranch.SelectedValue == "0")
            {
                ddlDepartment.Items.Insert(0, new ListItem("-- Select Branch First --", "0"));
                return;
            }

            var departments = CommonDAL.GetBranchDepartments(Convert.ToInt64(ddlBranch.SelectedValue));

            if (departments != null)
            {
                ddlDepartment.DataSource = departments;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "ID";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("-- All Departments --", "0"));
            }
            else
            {
                ddlDepartment.Items.Insert(0, new ListItem("-- No Departments Found --", "0"));
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0")
                return;

            DateTime from, to;
            if (!DateTime.TryParse(txtEffectiveFrom.Text, out from) ||
                !DateTime.TryParse(txtEffectiveTo.Text, out to))
                return;

            DataSet ds = dal.ProcessEmployeePayroll(ddlEmployee.SelectedValue, from, to);

            if (ds == null || ds.Tables.Count == 0)
                return;

            var payrollDS = new hrms_PakAsia.Dataset.Payroll();

            if (ds.Tables.Count > 0)
                payrollDS.dtMonthlyAttendance.Merge(ds.Tables[0]);

            if (ds.Tables.Count > 1)
                payrollDS.dtSummary.Merge(ds.Tables[1]);

            ExportToPdf("~/Reports/PayrollReport.rpt", payrollDS, "PayrollSlip.pdf");

            LogAction("Calculate Payroll", ddlEmployee.SelectedValue,
                $"Payroll calculated from {from} to {to}");
        }

        protected void btnBranchPayroll_Click(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedValue == "0" || ddlPayrollCycle.SelectedValue == "0")
                return;

            DateTime from, to;
            if (!DateTime.TryParse(dateFrom.Text, out from) ||
                !DateTime.TryParse(dateTo.Text, out to))
                return;

            long branchID = Convert.ToInt64(ddlBranch.SelectedValue);
            long payrollCycle = Convert.ToInt64(ddlPayrollCycle.SelectedValue);

            long? departmentID = ddlDepartment.SelectedValue == "0" ? (long?)null : Convert.ToInt64(ddlDepartment.SelectedValue);
            long? gender = ddlGender.SelectedValue == "0" ? (long?)null : Convert.ToInt64(ddlGender.SelectedValue);

            DataSet ds = dal.ProcessBranchPayroll(branchID, from, to, payrollCycle, departmentID, gender);

            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable finalTable = ds.Tables[0].Clone();

            foreach (DataTable table in ds.Tables)
                foreach (DataRow row in table.Rows)
                    finalTable.ImportRow(row);

            finalTable = finalTable.DefaultView.ToTable(true);

            decimal totalNet = finalTable.AsEnumerable()
                .Where(r => r["NetPayable"] != DBNull.Value)
                .Sum(r => Convert.ToDecimal(r["NetPayable"]));

            foreach (DataRow row in finalTable.Rows)
                row["BranchCost"] = totalNet;

            var branchDS = new hrms_PakAsia.Dataset.BranchPayroll();
            branchDS.dtBranchPayroll.Merge(finalTable);

            ExportToPdf("~/Reports/BranchPayroll.rpt", branchDS, "Branch_PayrollSlip.pdf");

            LogAction("Export Branch Payroll", branchID.ToString(),
                $"From {from} to {to}");
        }

        private void ExportToPdf(string reportPath, object dataSource, string fileName)
        {
            using (ReportDocument rpt = new ReportDocument())
            {
                rpt.Load(Server.MapPath(reportPath));
                rpt.SetDataSource(dataSource);
                rpt.DataSourceConnections.Clear();

                using (Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"inline; filename={fileName}");
                    stream.CopyTo(Response.OutputStream);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
    }
}