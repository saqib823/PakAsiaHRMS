using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Payroll
{
    public partial class ProcessPayslip : System.Web.UI.Page
    {
        private readonly PayrollDAL dal = new PayrollDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
                txtEffectiveFrom.Text = DateTime.Now.ToString("yyyy-MM-01");
                txtEffectiveTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void LoadEmployees()
        {

            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();

            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0") return;

            string empID = ddlEmployee.SelectedValue;
            DateTime from = Convert.ToDateTime(txtEffectiveFrom.Text);
            DateTime to = Convert.ToDateTime(txtEffectiveTo.Text);

            DataSet ds = dal.ProcessEmployeePayroll(empID, from, to);

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
                rpt.Load(Server.MapPath("~/Reports/payslip.rpt"));

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
               
            }
        }        
    }
}