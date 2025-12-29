using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Payroll
{
    public partial class ProcessPayroll : System.Web.UI.Page
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

            if (ds != null && ds.Tables.Count > 0)
            {
                gvAttendance.DataSource = ds.Tables[0];
                gvAttendance.DataBind();

                if (ds.Tables.Count > 1)
                {
                    lblGross.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["MonthlyGrossSalary"]).ToString("N2");
                    lblEarned.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["EarnedSalary"]).ToString("N2");
                    lblNet.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["NetPayableSalary"]).ToString("N2");
                }
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
