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
    public partial class ProcessPayslip : hrms_PakAsia.BasePage
    {
        private readonly PayrollDAL dal = new PayrollDAL();
        LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                LoadEmployees();
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

            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();

            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));

            ddlEmplyeeClearance.DataSource = CommonDAL.GetEmployees();
            ddlEmplyeeClearance.DataTextField = "Name";
            ddlEmplyeeClearance.DataValueField = "ID";
            ddlEmplyeeClearance.DataBind();

            ddlEmplyeeClearance.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0") return;

            string empID = ddlEmployee.SelectedValue;
            

            DataSet ds = dal.ProcessEmployeePayslip(empID);
            LogAction("Export Payslip PDF", recordId: empID, remarks: $"Generated payslip PDF for employee {empID}");

            // Create typed dataset instance
            hrms_PakAsia.Dataset.Payslip dtPayslip = new hrms_PakAsia.Dataset.Payslip();

            // ===== Summary Table =====
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dtPayslip.dtPayslip.Clear();
                dtPayslip.dtPayslip.Merge(ds.Tables[0]);
            }

            // ================= EXPORT TO PDF =================
            ReportDocument rpt = new ReportDocument();

            try
            {
                rpt.Load(Server.MapPath("~/Reports/payslip.rpt"));

                // VERY IMPORTANT: Typed DataSet
                rpt.SetDataSource(dtPayslip);

                // Prevent DB login prompt
                rpt.DataSourceConnections.Clear();

                using (Stream pdfStream = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline; filename=PaySlip.pdf");

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
               
            }
        }

        protected void btnProcessClearance_Click(object sender, EventArgs e)
        {
            if (ddlEmplyeeClearance.SelectedValue == "0")
                return;

            string empID = ddlEmplyeeClearance.SelectedValue;

            DateTime from, to;

            if (!DateTime.TryParse(txtEffectiveFrom.Text, out from) ||
                !DateTime.TryParse(txtEffectiveTo.Text, out to))
                return;

            // Get data
            DataSet dsPayroll = dal.ProcessEmployeePayroll(empID, from, to);
            DataSet dsPayslip = dal.ProcessEmployeePayslip(empID);
            LogAction("Export Clearance Report PDF", recordId: empID, remarks: $"Generated clearance report for employee {empID} from {from:yyyy-MM-dd} to {to:yyyy-MM-dd}");

            // ===== Create ONE Typed Dataset =====
            hrms_PakAsia.Dataset.Clearence payrollDS = new hrms_PakAsia.Dataset.Clearence();

            // ===== Fill Monthly Attendance =====
            if (dsPayroll != null &&
                dsPayroll.Tables.Count > 0 &&
                dsPayroll.Tables[0].Rows.Count > 0)
            {
                payrollDS.dtMonthlyAttendance.Clear();
                payrollDS.dtMonthlyAttendance.Merge(dsPayroll.Tables[0]);
            }

            // ===== Fill Summary =====
            if (dsPayroll != null &&
                dsPayroll.Tables.Count > 1 &&
                dsPayroll.Tables[1].Rows.Count > 0)
            {
                payrollDS.dtSummary.Clear();
                payrollDS.dtSummary.Merge(dsPayroll.Tables[1]);
            }

            // ===== Fill Payslip =====
            if (dsPayslip != null &&
                dsPayslip.Tables.Count > 0 &&
                dsPayslip.Tables[0].Rows.Count > 0)
            {
                payrollDS.dtPayslip.Clear();
                payrollDS.dtPayslip.Merge(dsPayslip.Tables[0]);
            }

            // ================= EXPORT TO PDF =================
            ReportDocument rpt = new ReportDocument();

            try
            {
                rpt.Load(Server.MapPath("~/Reports/ClearanceReport.rpt"));

                rpt.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                rpt.PrintOptions.PaperSize = PaperSize.PaperA4;

                // 🔥 SET DATASOURCE ONLY ONCE
                rpt.SetDataSource(payrollDS);

                // Prevent DB login popup
                rpt.DataSourceConnections.Clear();

                using (Stream pdfStream = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline; filename=ClearanceReport.pdf");

                    pdfStream.CopyTo(Response.OutputStream);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                // Optional logging
                throw;
            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
            }
        }

    }
}