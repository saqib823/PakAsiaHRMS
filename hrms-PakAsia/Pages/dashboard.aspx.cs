using HRMSLib.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
            if (currentUser != null)
            {
                string name = currentUser.FirstName + " " + currentUser.LastName;
                string email = currentUser.EmailAddress;
                int roleId = currentUser.RoleId;
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}