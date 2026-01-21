using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(email.Text) && !string.IsNullOrWhiteSpace(password.Text))
            {
                UserDAL dal = new UserDAL();
                LoggedInUser currentUser = dal.LoginUser(email.Text, password.Text);
                if (currentUser == null)
                {
                    ShowAlert("User not exist!", "danger");
                    return;
                }
                if (currentUser.RoleId == null || currentUser.RoleId == 0)
                {
                    ShowAlert("You have no role assigned! Contact with Admin", "danger");
                    return;
                }
                List<RoleRights> currentRoleRights = dal.GetRoleRights(currentUser.RoleId);
                if (currentRoleRights == null)
                {
                    ShowAlert("Your Role has no rights! Contact with Admin", "danger");
                    return;
                }

                if (currentUser != null && currentRoleRights != null && currentRoleRights.Count > 0)
                {
                    // Get allowed URLs (ignore '#')
                    var allowedUrls = currentRoleRights
                                        .Where(r => !string.IsNullOrEmpty(r.MenuHref) && r.MenuHref != "#")
                                        .Select(r => VirtualPathUtility.ToAbsolute(NormalizeMenuUrl(r.MenuHref)))
                                        .ToList();

                    // Priority dashboard pages
                    string adminDashboard = "~/Pages/dashboard.aspx";
                    string userDashboard = "~/Pages/UserDashboard.aspx";

                    string redirectUrl = null;

                    // If admin dashboard exists in role rights
                    if (allowedUrls.Any(u => u.Equals(
                            VirtualPathUtility.ToAbsolute(adminDashboard),
                            StringComparison.OrdinalIgnoreCase)))
                    {
                        redirectUrl = adminDashboard;
                    }
                    // If user dashboard exists in role rights
                    else if (allowedUrls.Any(u => u.Equals(
                            VirtualPathUtility.ToAbsolute(userDashboard),
                            StringComparison.OrdinalIgnoreCase)))
                    {
                        redirectUrl = userDashboard;
                    }
                    // Otherwise redirect to first allowed page
                    else
                    {
                        redirectUrl = allowedUrls.First();
                    }

                    Response.Redirect(redirectUrl, true);
                }
                else
                {
                    ShowAlert("Invalid Credentials", "danger");
                    return;
                }

            
            }
        }
        private string NormalizeMenuUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url == "#")
                return null;

            // If only filename is stored → make it app-relative
            if (!url.StartsWith("~/") && !url.StartsWith("/"))
            {
                url = "~/" + url;
            }

            return VirtualPathUtility.ToAbsolute(url);
        }

        private void ShowAlert(string message, string css)
        {
            phAlert.Controls.Clear();

            phAlert.Controls.Add(new Literal
            {
                Text = $@"
        <div id='autoAlert' class='alert alert-{css} alert-dismissible fade show' role='alert'>
            {message}
        </div>

        <script>
            setTimeout(function () {{
                var alert = document.getElementById('autoAlert');
                if (alert) {{
                    alert.classList.remove('show');
                    alert.classList.add('hide');
                }}
            }}, 3000); // 3 seconds
        </script>"
            });
        }
    }
}