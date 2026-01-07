using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MenuItem = HRMSLib.DataLayer.MenuItem;

namespace hrms_PakAsia.Pages
{
    public partial class Rights : Page
    {
        private readonly RoleDAL roleDAL = new RoleDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRoles();
                BindMenuTree();
            }
        }

        #region ROLES

        private void BindRoles()
        {
            ddlRoles.DataSource = CommonDAL.GetRoles();
            ddlRoles.DataTextField = "Name";
            ddlRoles.DataValueField = "ID";
            ddlRoles.DataBind();
            ddlRoles.Items.Insert(0, new ListItem("Select One", "0"));
        }

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            UncheckAllNodes(tvFolders.Nodes);

            int roleId = Convert.ToInt32(ddlRoles.SelectedValue);
            if (roleId == 0) return;

            List<int> assignedMenus = roleDAL.GetRoleMenuRights(roleId);

            foreach (TreeNode node in tvFolders.Nodes)
                CheckNodeRecursive(node, assignedMenus);
        }

        #endregion

        #region TREE BINDING (DB DRIVEN)

        private void BindMenuTree()
        {
            tvFolders.Nodes.Clear();

            List<MenuItem> menus = MenuDAL.GetMenus();
            var rootMenus = menus.Where(m => m.ParentMenuId == null).ToList();

            foreach (var menu in rootMenus)
            {
                TreeNode node = CreateNode(menu);
                tvFolders.Nodes.Add(node);
                AddChildNodes(node, menus);
            }

            tvFolders.ExpandAll();
        }

        private TreeNode CreateNode(MenuItem menu)
        {
            return new TreeNode(menu.MenuText, menu.MenuId.ToString())
            {
                SelectAction = TreeNodeSelectAction.None
            };
        }

        private void AddChildNodes(TreeNode parentNode, List<MenuItem> menus)
        {
            int parentId = int.Parse(parentNode.Value);

            var children = menus.Where(m => m.ParentMenuId == parentId);

            foreach (var child in children)
            {
                TreeNode childNode = CreateNode(child);
                parentNode.ChildNodes.Add(childNode);
                AddChildNodes(childNode, menus);
            }
        }

        #endregion

        #region CHECKBOX SYNC

        protected void tvFolders_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            // Parent → children
            SetChildNodes(e.Node, e.Node.Checked);

            // Child → parent
            UpdateParentNode(e.Node.Parent);
        }

        private void SetChildNodes(TreeNode parent, bool isChecked)
        {
            foreach (TreeNode child in parent.ChildNodes)
            {
                child.Checked = isChecked;
                SetChildNodes(child, isChecked);
            }
        }

        private void UpdateParentNode(TreeNode parent)
        {
            if (parent == null) return;

            parent.Checked = parent.ChildNodes.Cast<TreeNode>().Any(n => n.Checked);
            UpdateParentNode(parent.Parent);
        }

        #endregion

        #region SAVE RIGHTS

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int roleId = Convert.ToInt32(ddlRoles.SelectedValue);
            if (roleId == 0)
            {
                ShowMessage("Please select a role");
                return;
            }

            List<int> selectedMenuIds = tvFolders.CheckedNodes
                .Cast<TreeNode>()
                .Select(n => int.Parse(n.Value))
                .ToList();

            roleDAL.SaveRoleMenuRights(roleId, selectedMenuIds);

            ShowMessage("Rights saved successfully");
        }

        #endregion

        #region HELPERS

        private void CheckNodeRecursive(TreeNode node, List<int> assignedMenus)
        {
            if (assignedMenus.Contains(int.Parse(node.Value)))
                node.Checked = true;

            foreach (TreeNode child in node.ChildNodes)
                CheckNodeRecursive(child, assignedMenus);
        }

        private void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                UncheckAllNodes(node.ChildNodes);
            }
        }

        private void ShowMessage(string message)
        {
            ClientScript.RegisterStartupScript(
                GetType(),
                "msg",
                $"alert('{message}');",
                true);
        }

        #endregion
    }
}
