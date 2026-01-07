using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSLib.DataLayer;
namespace HRMSLib.BusinessLogic
{
    public class MenuHelper
    {
        public static bool CheckPageAccess(int roleId, string pagePath)
        {
            try
            {
                MenuDAL menuDAL = new MenuDAL();
                var menuItems = menuDAL.GetMenuByRoleId(roleId);

                if (menuItems == null || menuItems.Count == 0)
                    return false;

                string normalizedPagePath = NormalizePath(pagePath);

                foreach (var item in menuItems)
                {
                    if (NormalizePath(item.MenuHref) == normalizedPagePath)
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;

            path = path.Trim().ToLower();

            if (path.StartsWith("~/"))
                path = path.Substring(2);

            return path;
        }
    }
}