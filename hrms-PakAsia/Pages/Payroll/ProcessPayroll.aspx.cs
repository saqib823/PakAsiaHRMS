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
    public partial class ProcessPayroll : System.Web.UI.Page
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
                rpt.Load(Server.MapPath("~/Reports/payroll.rpt"));

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
    }
}
