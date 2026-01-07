using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace HRMSLib.DataLayer
{
    public class MenuDAL
    {
        private static Database db =>
                 new DatabaseProviderFactory().Create("defaultDB");
        public static List<MenuItem> GetMenus()
        {
            DbCommand cmd = db.GetSqlStringCommand(
                @"SELECT 
                      MenuId,
                      ParentMenuId,
                      MenuText,
                      MenuHref
                  FROM MenuItems
                  WHERE IsActive = 1
                  ORDER BY DisplayOrder");

            DataSet ds = db.ExecuteDataSet(cmd);

            return ds.Tables[0]
                     .AsEnumerable()
                     .Select(r => new MenuItem
                     {
                         MenuId = r.Field<int>("MenuId"),
                         ParentMenuId = r.Field<int?>("ParentMenuId"),
                         MenuText = r.Field<string>("MenuText"),
                         MenuHref = r.Field<string>("MenuHref")
                     })
                     .ToList();
        }
        public List<MenuItem> GetMenuByRoleId(int roleId)
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            try
            {
                using (var command = db.GetStoredProcCommand("usp_GetMenuByRoleId"))
                {
                    db.AddInParameter(command, "@RoleId", DbType.Int32, roleId);

                    using (var reader = db.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            MenuItem item = new MenuItem
                            {
                                MenuId = Convert.ToInt32(reader["MenuId"]),
                                ParentMenuId = reader["ParentMenuId"] != DBNull.Value
                                    ? Convert.ToInt32(reader["ParentMenuId"])
                                    : (int?)null,
                                MenuText = reader["MenuText"].ToString(),
                                MenuHref = reader["MenuHref"] != DBNull.Value
                                    ? reader["MenuHref"].ToString()
                                    : "#",
                                MenuIcon = reader["MenuIcon"] != DBNull.Value
                                    ? reader["MenuIcon"].ToString()
                                    : "",
                                DisplayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                            menuItems.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Trace.TraceError($"Error in GetMenuByRoleId: {ex.Message}");
                throw;
            }

            return menuItems;
        }

        // Optional: Get hierarchical menu structure
        public List<MenuItem> GetHierarchicalMenuByRoleId(int roleId)
        {
            List<MenuItem> flatList = GetMenuByRoleId(roleId);
            return BuildMenuHierarchy(flatList);
        }

        private List<MenuItem> BuildMenuHierarchy(List<MenuItem> flatList)
        {
            Dictionary<int, MenuItem> menuDict = new Dictionary<int, MenuItem>();
            List<MenuItem> rootItems = new List<MenuItem>();

            // First pass: Create dictionary
            foreach (MenuItem item in flatList)
            {
                menuDict[item.MenuId] = item;

                if (!item.ParentMenuId.HasValue)
                {
                    rootItems.Add(item);
                }
            }

            // Second pass: Build hierarchy
            foreach (MenuItem item in flatList)
            {
                if (item.ParentMenuId.HasValue && menuDict.ContainsKey(item.ParentMenuId.Value))
                {
                    menuDict[item.ParentMenuId.Value].Children.Add(item);
                }
            }

            // Sort by display order
            rootItems.Sort((x, y) => x.DisplayOrder.CompareTo(y.DisplayOrder));
            foreach (var item in menuDict.Values)
            {
                item.Children.Sort((x, y) => x.DisplayOrder.CompareTo(y.DisplayOrder));
            }

            return rootItems;
        }

        // Optional: Get all active menus for administration
        public DataTable GetAllActiveMenus()
        {
            try
            {
                using (var command = db.GetStoredProcCommand("usp_GetAllActiveMenus"))
                {
                    return db.ExecuteDataSet(command).Tables[0];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in GetAllActiveMenus: {ex.Message}");
                throw;
            }
        }
        public List<string> GetRoleRights(int roleId)
        {
            List<string> rights = new List<string>();

            try
            {
                using (var command = db.GetStoredProcCommand("usp_GetRoleRights"))
                {
                    db.AddInParameter(command, "@RoleId", DbType.Int32, roleId);

                    using (var reader = db.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            string pagePath = reader["PagePath"] != DBNull.Value
                                ? reader["PagePath"].ToString()
                                : string.Empty;

                            if (!string.IsNullOrEmpty(pagePath))
                            {
                                rights.Add(pagePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in GetRoleRights: {ex.Message}");
                throw;
            }

            return rights;
        }

        // Save role rights
        public bool SaveRoleRights(int roleId, List<string> pagePaths)
        {
            bool success = false;

            try
            {
                // Begin transaction
                using (var connection = db.CreateConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // First, delete existing rights
                            using (var deleteCommand = db.GetStoredProcCommand("usp_DeleteRoleRights"))
                            {
                                db.AddInParameter(deleteCommand, "@RoleId", DbType.Int32, roleId);
                                db.ExecuteNonQuery(deleteCommand, transaction);
                            }

                            // Then insert new rights
                            if (pagePaths != null && pagePaths.Count > 0)
                            {
                                foreach (string pagePath in pagePaths)
                                {
                                    using (var insertCommand = db.GetStoredProcCommand("usp_InsertRoleRight"))
                                    {
                                        db.AddInParameter(insertCommand, "@RoleId", DbType.Int32, roleId);
                                        db.AddInParameter(insertCommand, "@PagePath", DbType.String, pagePath);
                                        db.AddInParameter(insertCommand, "@CreatedBy", DbType.String, "System");

                                        db.ExecuteNonQuery(insertCommand, transaction);
                                    }
                                }
                            }

                            // Commit transaction
                            transaction.Commit();
                            success = true;
                        }
                        catch
                        {
                            // Rollback on error
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in SaveRoleRights: {ex.Message}");
                success = false;
            }

            return success;
        }

        // Get all roles with rights count
        public DataTable GetRolesWithRightsCount()
        {
            try
            {
                using (var command = db.GetStoredProcCommand("usp_GetRolesWithRightsCount"))
                {
                    return db.ExecuteDataSet(command).Tables[0];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in GetRolesWithRightsCount: {ex.Message}");
                throw;
            }
        }
    }

    public class MenuItem
    {
        public int MenuId { get; set; }
        public int? ParentMenuId { get; set; }
        public string MenuText { get; set; }
        public string MenuHref { get; set; }
        public string MenuIcon { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<MenuItem> Children { get; set; }

        public MenuItem()
        {
            Children = new List<MenuItem>();
        }
    }
}