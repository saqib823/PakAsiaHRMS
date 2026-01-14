using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MenuItem = HRMSLib.DataLayer.MenuItem;

namespace hrms_PakAsia
{
    public partial class App : System.Web.UI.MasterPage
    {
        private LoggedInUser currentUser = null;
        private MenuDAL menuDAL = new MenuDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
                GetCheckRightsData();   
                if (currentUser == null)
                {
                    // Redirect to login if no user session
                    Response.Redirect("~/Default.aspx", true);
                    return;
                }

                InitializeUserProfile();
                BuildDynamicMenu();
            }
        }
        public List<RoleRights> GetCheckRightsData()
        {
            List<RoleRights> currentRolesRights =
                HttpContext.Current.Session["RoleRights"] as List<RoleRights>;

            // Session expired or no rights
            if (currentRolesRights == null || currentRolesRights.Count == 0)
            {
                Response.Redirect("~/Default.aspx", true);
                return null;
            }

            // Get current page url
            string currentUrl = VirtualPathUtility.ToAbsolute(
                                    HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

            // Check permission
            bool hasAccess = currentRolesRights.Any(r =>
                !string.IsNullOrEmpty(r.MenuHref) &&
                r.MenuHref != "#" &&
                VirtualPathUtility.ToAbsolute(NormalizeUrl(r.MenuHref))
                    .Equals(currentUrl, StringComparison.OrdinalIgnoreCase));

            if (!hasAccess)
            {
                Response.Redirect("~/Default.aspx", true);
                return null;
            }

            return currentRolesRights;
        }
        private string NormalizeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            // Convert ~/ to absolute
            string normalized = VirtualPathUtility.ToAbsolute(NormalizeMenuUrl(url));

            // Remove query string
            normalized = normalized.Split('?')[0];

            // Remove file extension
            normalized = Path.ChangeExtension(normalized, null);

            return normalized.ToLowerInvariant();
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

        private void InitializeUserProfile()
        {
            try
            {
                // Set profile image
                string defaultImage = "assets/img/team/default-user.png";
                imgProfile.Src = !string.IsNullOrWhiteSpace(currentUser.filePath)
                    ? currentUser.filePath
                    : defaultImage;
                imgNav.Src = imgProfile.Src;

                // Set user name
                FullName.InnerHtml = $"{currentUser.FirstName} {currentUser.LastName}";

                // Set user role (optional)
                //UserRole.InnerHtml = currentUser.RoleName ?? "User";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in InitializeUserProfile: {ex.Message}");
            }
        }

        private void BuildDynamicMenu()
        {
            try
            {
                // Get hierarchical menu for the current user's role
                List<MenuItem> menuItems = menuDAL.GetHierarchicalMenuByRoleId(currentUser.RoleId);

                if (menuItems == null || menuItems.Count == 0)
                {
                    litMenu.Text = "<li class='nav-item'><span class='nav-link text-muted'>No menu items available</span></li>";
                    return;
                }

                // Generate menu HTML
                StringBuilder menuHtml = new StringBuilder();
                foreach (MenuItem item in menuItems)
                {
                    menuHtml.Append(GenerateMenuItemHtml(item));
                }

                litMenu.Text = menuHtml.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in BuildDynamicMenu: {ex.Message}");
                litMenu.Text = "<li class='nav-item'><span class='nav-link text-danger'>Error loading menu</span></li>";
            }
        }

        private string GenerateMenuItemHtml(MenuItem menuItem)
        {
            if (menuItem.Children.Count > 0)
            {
                // Generate dropdown menu item
                return GenerateDropdownHtml(menuItem);
            }
            else
            {
                // Generate simple menu item
                return GenerateSimpleMenuItemHtml(menuItem);
            }
        }

        private string GenerateDropdownHtml(MenuItem menuItem)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine($@"<li class='nav-item dropdown' id='li{menuItem.MenuId}'>
                <a class='nav-link dropdown-toggle lh-1' href='#' data-bs-toggle='dropdown' data-bs-auto-close='outside'>
                    {GetIconHtml(menuItem.MenuIcon)}
                    {HttpUtility.HtmlEncode(menuItem.MenuText)}
                </a>
                <ul class='dropdown-menu navbar-dropdown-caret'>");

            foreach (MenuItem child in menuItem.Children)
            {
                html.AppendLine($@"<li>
                    <a class='dropdown-item' href='{ResolveUrl(child.MenuHref)}' runat='server'>
                        {HttpUtility.HtmlEncode(child.MenuText)}
                    </a>
                </li>");
            }

            html.AppendLine("</ul></li>");
            return html.ToString();
        }

        private string GenerateSimpleMenuItemHtml(MenuItem menuItem)
        {
            return $@"<li class='nav-item' id='li{menuItem.MenuId}'>
                <a class='nav-link lh-1' href='{ResolveUrl(menuItem.MenuHref)}' runat='server'>
                    {GetIconHtml(menuItem.MenuIcon)}
                    {HttpUtility.HtmlEncode(menuItem.MenuText)}
                </a>
            </li>";
        }

        private string GetIconHtml(string iconClass)
        {
            if (string.IsNullOrWhiteSpace(iconClass))
                return string.Empty;

            return $@"<span class='{iconClass} fs-8 me-2'></span>";
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear session
                Session.Clear();
                Session.Abandon();

                // Clear authentication cookie if using forms authentication
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }
                currentUser = null;
                // Redirect to login page
                Response.Redirect("~/Default.aspx", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in lnkLogout_Click: {ex.Message}");
            }
        }

        // Helper method to check if user has access to a specific page
        public bool HasAccess(string pagePath)
        {
            try
            {
                if (currentUser == null) return false;

                List<MenuItem> menuItems = menuDAL.GetMenuByRoleId(currentUser.RoleId);
                if (menuItems == null) return false;

                string normalizedPagePath = NormalizePath(pagePath);
                return menuItems.Any(m => NormalizePath(m.MenuHref) == normalizedPagePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in HasAccess: {ex.Message}");
                return false;
            }
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;

            path = path.Trim().ToLower();

            // Remove ~/
            if (path.StartsWith("~/"))
                path = path.Substring(2);

            // Remove query string
            int queryIndex = path.IndexOf('?');
            if (queryIndex > 0)
                path = path.Substring(0, queryIndex);

            return path;
        }
    }
}