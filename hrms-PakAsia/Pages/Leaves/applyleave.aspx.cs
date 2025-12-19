using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Leaves
{
    public partial class applyleave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadLeaveTypes();
        }

        private void LoadLeaveTypes()
        {
            int empId = Convert.ToInt32(Session["EmployeeID"]);
            DataTable dt = LeaveDAL.GetEmployeeLeaveBalance(empId);

            ddlLeaveType.DataSource = dt;
            ddlLeaveType.DataTextField = "LeaveName";
            ddlLeaveType.DataValueField = "LeaveTypeID";
            ddlLeaveType.DataBind();

            ddlLeaveType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int empId = Convert.ToInt32(Session["EmployeeID"]);

            var result = LeaveDAL.ApplyLeave(
                empId,
                Convert.ToInt32(ddlLeaveType.SelectedValue),
                Convert.ToDateTime(txtStartDate.Text),
                Convert.ToDateTime(txtEndDate.Text),
                txtReason.Text.Trim()
            );

            phAlert.Controls.Add(new Literal
            {
                Text = $"<div class='alert alert-{(result.ResultCode > 0 ? "success" : "danger")}'>{result.ResultMessage}</div>"
            });

            if (result.ResultCode > 0)
            {
                ddlLeaveType.SelectedIndex = 0;
                txtStartDate.Text = txtEndDate.Text = txtReason.Text = "";
            }
        }
    }
}