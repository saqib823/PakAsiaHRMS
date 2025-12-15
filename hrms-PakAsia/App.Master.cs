using HRMSLib.BusinessLogic;
using System;
using System.Web;

namespace hrms_PakAsia
{
    public partial class App : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoggedInUser currentUser =
                    HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

                if (currentUser != null && !string.IsNullOrWhiteSpace(currentUser.filePath))
                {
                    imgProfile.Src = $"{currentUser.filePath}";
                    imgNav.Src = $"{currentUser.filePath}";


                    FullName.InnerHtml = currentUser.FirstName +" "+ currentUser.LastName;
                }
                else
                {
                    imgProfile.Src = "assets/img/team/default-user.png";
                }
            }
        }
    }
}
