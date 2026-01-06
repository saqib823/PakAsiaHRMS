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
            

            DataSet ds = dal.ProcessEmployeePayslip(empID);

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
    }
}