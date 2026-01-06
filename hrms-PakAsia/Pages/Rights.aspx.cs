using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages
{
    public partial class Rights : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTree();
            }
        }

        private void BindTree()
        {
            tvFolders.Nodes.Clear();

            string physicalRoot = Server.MapPath("~/Pages");
            string virtualRoot = "~/Pages";

            DirectoryInfo rootDir = new DirectoryInfo(physicalRoot);

            TreeNode rootNode = new TreeNode("Pages", virtualRoot)
            {
                SelectAction = TreeNodeSelectAction.None
            };

            tvFolders.Nodes.Add(rootNode);
            LoadDirectories(rootDir, rootNode, virtualRoot);
            rootNode.Expand();
        }

        private void LoadDirectories(DirectoryInfo dir, TreeNode parentNode, string virtualPath)
        {
            // Folders
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if (subDir.Name.Equals("bin", StringComparison.OrdinalIgnoreCase) ||
                    subDir.Name.Equals("obj", StringComparison.OrdinalIgnoreCase))
                    continue;

                string folderPath = virtualPath + "/" + subDir.Name;

                TreeNode dirNode = new TreeNode(subDir.Name, folderPath)
                {
                    SelectAction = TreeNodeSelectAction.None
                };

                parentNode.ChildNodes.Add(dirNode);
                LoadDirectories(subDir, dirNode, folderPath);
            }

            // Pages
            foreach (FileInfo file in dir.GetFiles("*.aspx"))
            {
                string filePath = virtualPath + "/" + file.Name;

                TreeNode fileNode = new TreeNode(file.Name, filePath)
                {
                    SelectAction = TreeNodeSelectAction.None
                };

                parentNode.ChildNodes.Add(fileNode);
            }
        }

        // 🔥 Main sync handler
        protected void tvFolders_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            // 1️⃣ Parent → Children
            CheckUncheckChildren(e.Node, e.Node.Checked);

            // 2️⃣ Child → Parent
            UpdateParentState(e.Node.Parent);
        }

        private void CheckUncheckChildren(TreeNode parent, bool isChecked)
        {
            foreach (TreeNode child in parent.ChildNodes)
            {
                child.Checked = isChecked;
                CheckUncheckChildren(child, isChecked);
            }
        }

        private void UpdateParentState(TreeNode parent)
        {
            if (parent == null) return;

            bool anyChecked = parent.ChildNodes
                .Cast<TreeNode>()
                .Any(n => n.Checked);

            parent.Checked = anyChecked;

            UpdateParentState(parent.Parent);
        }

        // 🔐 Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> selectedPages = tvFolders.CheckedNodes
                .Cast<TreeNode>()
                .Select(n => n.Value)
                .ToList();

            // TODO: Save to DB
            // Example: RoleRights(RoleId, PagePath)

            ClientScript.RegisterStartupScript(
                this.GetType(),
                "saved",
                "alert('Rights saved successfully');",
                true);
        }
    }
}
