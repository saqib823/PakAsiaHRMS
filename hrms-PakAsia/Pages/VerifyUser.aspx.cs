using HRMSLib.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages
{
    public partial class VerifyUser : System.Web.UI.Page
    {
        LoggedInUser currentUser;
        List<RoleRights> currentRoleRights;
        protected void Page_Load(object sender, EventArgs e)
        {
            string otp = HttpContext.Current.Session["OTP"] as string;

            if (string.IsNullOrWhiteSpace(otp))
            {
                Response.Redirect("~/Default.aspx");
            }

            currentUser = null;
            currentRoleRights = null;
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if(HttpContext.Current.Session["OTP"].ToString() == otp.Text)
            {
                currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
                currentRoleRights = HttpContext.Current.Session["RoleRights"] as List<RoleRights>;
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
            }
            else
            {
                ShowAlert("Invalid OTP!", "danger");
            }
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
    }
}