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
                LoadLeaveTypes();
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
            DataTable dt = LeaveDAL.GetEmployeeLeaveBalance(currentUser.UserID);

            ddlLeaveType.DataSource = dt;
            ddlLeaveType.DataTextField = "Name";
            ddlLeaveType.DataValueField = "ID";
            ddlLeaveType.DataBind();

            ddlLeaveType.Items.Insert(0, new ListItem("-- Select Leave --", ""));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlLeaveType.SelectedValue))
            {
                ShowAlert("Please select leave type", "warning");
                return;
            }

            if (string.IsNullOrEmpty(txtStartDate.Text) || string.IsNullOrEmpty(txtEndDate.Text))
            {
                ShowAlert("Please select start and end date", "warning");
                return;
            }

            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

            if (endDate < startDate)
            {
                ShowAlert("End date cannot be earlier than start date", "danger");
                return;
            }

            var result = LeaveDAL.ApplyLeave(
                currentUser.UserID,
                Convert.ToInt32(ddlLeaveType.SelectedValue),
                startDate,
                endDate,
                txtReason.Text.Trim()
            );

            ShowAlert(result.ResultMessage,
                result.ResultCode > 0 ? "success" : "danger");

            if (result.ResultCode > 0)
            {
                ddlLeaveType.SelectedIndex = 0;
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
    }
}
