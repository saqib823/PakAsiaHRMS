using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Payroll
{
    public partial class ProcessPayroll : hrms_PakAsia.BasePage
    {
        LoggedInUser currentUser = null;

        private readonly PayrollDAL dal = new PayrollDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                LoadEmployees();
                txtEffectiveFrom.Text = DateTime.Now.ToString("yyyy-MM-01");
                txtEffectiveTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
        private void LoadEmployees()
        {
            var employees = CommonDAL.GetEmployees();
            if (employees != null)
            {
                ddlEmployee.DataSource = employees;
                ddlEmployee.DataTextField = "Name";
                ddlEmployee.DataValueField = "ID";
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
            }

            var branches = CommonDAL.GetBranches();
            if (branches != null)
            {
                ddlBranch.DataSource = branches;
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataValueField = "ID";
                ddlBranch.DataBind();
            }

            ddlPayrollCycle.Items.Clear();
            ddlPayrollCycle.Items.Insert(0, new ListItem("Monthly", "2"));
            ddlPayrollCycle.Items.Insert(1, new ListItem("Wagges", "1"));
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0") return;

            string empID = ddlEmployee.SelectedValue;
            DateTime from = Convert.ToDateTime(txtEffectiveFrom.Text);
            DateTime to = Convert.ToDateTime(txtEffectiveTo.Text);

            DataSet ds = dal.ProcessEmployeePayroll(empID, from, to);
            LogAction("Calculate Payroll", recordId: empID, remarks: $"Calculated payroll for {empID} from {from} to {to}");

            // Create typed dataset instance
            hrms_PakAsia.Dataset.Payroll payrollDS = new hrms_PakAsia.Dataset.Payroll();

            // ===== Summary Table =====
            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                payrollDS.dtMonthlyAttendance.Clear();
                payrollDS.dtMonthlyAttendance.Merge(ds.Tables[0]);
            }

            // ===== Monthly Attendance Table =====
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                payrollDS.dtSummary.Clear();
                payrollDS.dtSummary.Merge(ds.Tables[1]);
            }
            // ================= EXPORT TO PDF =================
            ReportDocument rpt = new ReportDocument();

            try
            {
                rpt.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                rpt.PrintOptions.PaperSize = PaperSize.PaperA4;
                rpt.Load(Server.MapPath("~/Reports/PayrollReport.rpt"));

                // VERY IMPORTANT: Typed DataSet
                rpt.SetDataSource(payrollDS);

                // Prevent DB login prompt
                rpt.DataSourceConnections.Clear();

                using (Stream pdfStream = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline; filename=PayrollSlip.pdf");

                    pdfStream.CopyTo(Response.OutputStream);
                    Response.Flush();

                    // IMPORTANT: Complete request safely
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
            }


            if (ds != null && ds.Tables.Count > 0)
            {
                //gvAttendance.DataSource = ds.Tables[0];
                //gvAttendance.DataBind();

                //if (ds.Tables.Count > 1)
                //{
                //    lblGross.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["MonthlyGrossSalary"]).ToString("N2");
                //    lblEarned.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["EarnedSalary"]).ToString("N2");
                //    lblNet.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["NetPayableSalary"]).ToString("N2");
                //}
            }
        }

        // Optional: color code rows based on Status
        protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = e.Row.Cells[2].Text; // Status column
                switch (status)
                {
                    case "OFF":
                        e.Row.BackColor = System.Drawing.Color.LightGray;
                        break;
                    case "ABSENT":
                        e.Row.BackColor = System.Drawing.Color.LightCoral;
                        break;
                    case "WORKED_OFF":
                        e.Row.BackColor = System.Drawing.Color.LightGreen;
                        break;
                }
            }
        }

        protected void btnBranchPayroll_Click(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedValue == "0" || ddlBranch.SelectedValue == "") return;
            if (ddlPayrollCycle.SelectedValue == "0" || ddlPayrollCycle.SelectedValue == "") return;
            string branchID = ddlBranch.SelectedValue;
            string PayrollCycle = ddlPayrollCycle.SelectedValue;
            DateTime from = Convert.ToDateTime(dateFrom.Text);
            DateTime to = Convert.ToDateTime(dateTo.Text);

            DataSet ds = dal.ProcessBranchPayroll(Convert.ToInt64(branchID), from, to, Convert.ToInt64(PayrollCycle));

            DataTable finalTable = new DataTable();

            // Clone structure from first table
            if (ds.Tables.Count > 0)
            {
                finalTable = ds.Tables[0].Clone();

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        finalTable.ImportRow(row);
                    }
                }
            }
            decimal totalNetPayable = 0;

            foreach (DataRow row in finalTable.Rows)
            {
                if (row["NetPayable"] != DBNull.Value)
                    totalNetPayable += Convert.ToDecimal(row["NetPayable"]);
            }

            // Option A: Put total in every row
            foreach (DataRow row in finalTable.Rows)
            {
                row["BranchCost"] = totalNetPayable;
            }
            hrms_PakAsia.Dataset.BranchPayroll BranchPayroll = new hrms_PakAsia.Dataset.BranchPayroll();

            // ===== Summary Table =====
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                BranchPayroll.dtBranchPayroll.Clear();
                BranchPayroll.dtBranchPayroll.Merge(finalTable);
            }
            ReportDocument rpt = new ReportDocument();

            try
            {
                rpt.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                rpt.PrintOptions.PaperSize = PaperSize.PaperA4;
                rpt.Load(Server.MapPath("~/Reports/BranchPayroll.rpt"));

                // VERY IMPORTANT: Typed DataSet
                rpt.SetDataSource(BranchPayroll);

                // Prevent DB login prompt
                rpt.DataSourceConnections.Clear();

                using (Stream pdfStream = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline; filename=Branch_PayrollSlip.pdf");

                    pdfStream.CopyTo(Response.OutputStream);
                    Response.Flush();

                    // IMPORTANT: Complete request safely
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
            }
                LogAction("Export Branch Payroll PDF", recordId: branchID, remarks: $"Exported branch payroll PDF for branch {branchID} from {from} to {to}");


            if (ds != null && ds.Tables.Count > 0)
            {
                //gvAttendance.DataSource = ds.Tables[0];
                //gvAttendance.DataBind();

                //if (ds.Tables.Count > 1)
                //{
                //    lblGross.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["MonthlyGrossSalary"]).ToString("N2");
                //    lblEarned.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["EarnedSalary"]).ToString("N2");
                //    lblNet.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["NetPayableSalary"]).ToString("N2");
                //}
            }
        }
    }
}
