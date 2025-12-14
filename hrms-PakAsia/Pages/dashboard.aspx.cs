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
            CheckSession();
        }
        public void CheckSession()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
            if (currentUser == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

    }
}