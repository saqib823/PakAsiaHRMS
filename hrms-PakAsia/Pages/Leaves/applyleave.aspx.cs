using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Leaves
{
    public partial class applyleave : System.Web.UI.Page
    {
        LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                chkCarryForward.Visible = false;

                LoadLeaveTypes();
                LoadEmployees();
            }
        }

        private LoggedInUser GetSessionData()
        {
            return HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
        }

        private void CheckSession()
        {
            if (Session["LoggedInUser"] == null)
                Response.Redirect("~/Default.aspx");
        }

        private void LoadLeaveTypes()
        {
            ddlLeaveType.DataSource = CommonDAL.GetLeaveTypes(); 
            ddlLeaveType.DataTextField = "Name";
            ddlLeaveType.DataValueField = "ID";
            ddlLeaveType.DataBind();
            ddlLeaveType.Items.Insert(0, new ListItem("-- Select Leave --", "0"));
        }
        private void LoadEmployees()
        {
            if (currentUser.RoleId == 1)
            {
                ddlEmployees.DataSource = CommonDAL.GetEmployees(); ;
                ddlEmployees.DataTextField = "Name";
                ddlEmployees.DataValueField = "ID";
                ddlEmployees.DataBind();
                ddlEmployees.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
            }
            else
            {
                ddlEmployees.DataSource = CommonDAL.GetEmployee(currentUser.UserID.ToString()); ;
                ddlEmployees.DataTextField = "Name";
                ddlEmployees.DataValueField = "ID";
                ddlEmployees.DataBind();
                ddlEmployees.Items.Insert(0, new ListItem("-- Select Employee --", "0"));

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlEmployees.SelectedValue == "0" || ddlEmployees.SelectedValue == "")
            {
                ShowAlert("Please select Employee", "warning");
                return;
            }
            if (ddlLeaveType.SelectedValue == "0" || ddlLeaveType.SelectedValue == "")
            {
                ShowAlert("Please select leave type", "warning");
                return;
            }
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (string.IsNullOrEmpty(txtStartDate.Text) || string.IsNullOrEmpty(txtEndDate.Text))
            {

            }
            else
            {
                startDate = Convert.ToDateTime(txtStartDate.Text);
                endDate = Convert.ToDateTime(txtEndDate.Text);
            }
            if (endDate < startDate)
            {
                ShowAlert("End date cannot be earlier than start date", "danger");
                return;
            }

            var result = LeaveDAL.ApplyLeave(
                Convert.ToInt32(ddlEmployees.SelectedValue),
                Convert.ToInt32(ddlLeaveType.SelectedValue),
                startDate,
                endDate,
                txtReason.Text.Trim(),
                chkCarryForward.Checked,
                chkEncash.Checked
            );

            ShowAlert(result.ResultMessage,
                result.ResultCode > 0 ? "success" : "danger");

            if (result.ResultCode > 0)
            {
                ddlLeaveType.SelectedIndex = 0;
                ddlEmployees.SelectedIndex = 0;
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtReason.Text = "";
            }
        }

        private void ShowAlert(string message, string type)
        {
            phAlert.Controls.Clear();
            phAlert.Controls.Add(new Literal
            {
                Text = $@"
                <div id='autoAlert' class='alert alert-{type} alert-dismissible fade show'>
                    {message}
                </div>
                <script>
                    setTimeout(function(){{
                        var a=document.getElementById('autoAlert');
                        if(a) a.remove();
                    }},3000);
                </script>"
            });
        }

        protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LeaveDAL.CheckCarryForward(Convert.ToInt32(ddlLeaveType.SelectedValue)))
            {
                chkCarryForward.Visible = true;
                txtStartDate.Visible = true;
                txtEndDate.Visible = true;

                chkEncash.Visible = true;
            }
            else
            {
                chkCarryForward.Visible = false;
                chkEncash.Visible = true;
            }
        }

        protected void chkEncash_CheckedChanged(object sender, EventArgs e)
        {
            chkCarryForward.Visible = false;
            chkCarryForward.Checked = false;

            chkEncash.Visible = true;
        }

        protected void chkCarryForward_CheckedChanged(object sender, EventArgs e)
        {
            chkEncash.Visible = false;
            chkEncash.Checked = false;

            chkCarryForward.Visible = true;

        }

        protected void ddlEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt64(ddlEmployees.SelectedValue) != 0)
            {
                ddlLeaveType.DataSource = CommonDAL.GetEmployeeLeavesTypeBalance(Convert.ToInt64(ddlEmployees.SelectedValue));
                ddlLeaveType.DataTextField = "Name";
                ddlLeaveType.DataValueField = "ID";
                ddlLeaveType.DataBind();
                ddlLeaveType.Items.Insert(0, new ListItem("-- Select Leave --", "0"));
            }
        }
    }
}
