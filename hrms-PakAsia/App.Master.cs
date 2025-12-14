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

                if (currentUser != null && currentUser.ImageData != null)
                {
                    string base64String =
                        Convert.ToBase64String(currentUser.ImageData);

                    imgProfile.Src =
                        $"data:image/png;base64,{base64String}";
                    imgNav.Src =
                        $"data:image/png;base64,{base64String}";

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
