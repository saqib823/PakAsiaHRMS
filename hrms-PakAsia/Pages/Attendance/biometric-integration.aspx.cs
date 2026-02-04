using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Linq;
using HRMSLib.BusinessLogic;
using System.Web;

namespace hrms_PakAsia.Pages.Attendance
{
    public partial class biometric_integration : System.Web.UI.Page
    {
        LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                ddlBranch.DataSource = CommonDAL.GetBranches();
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataValueField = "ID";
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem("Select One", ""));


              

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

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDepartment.DataSource = CommonDAL.GetBranchDepartments(Convert.ToInt64(ddlBranch.SelectedValue));
            ddlDepartment.DataTextField = "Name";
            ddlDepartment.DataValueField = "ID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("Select One", ""));
        }
    }
}
