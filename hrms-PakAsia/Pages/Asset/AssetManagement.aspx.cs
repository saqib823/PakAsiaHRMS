using System;
using System.Data;
using HRMSLib.DataLayer;

namespace hrms_PakAsia.Pages.Asset
{
    public partial class AssetManagement : System.Web.UI.Page
    {
        AssetDAL dal = new AssetDAL();
        private int PageSize = 10;

        private int PageIndex
        {
            get { return ViewState["PageIndex"] != null ? (int)ViewState["PageIndex"] : 1; }
            set { ViewState["PageIndex"] = value; }
        }

        private string SearchTerm
        {
            get { return ViewState["SearchTerm"]?.ToString() ?? string.Empty; }
            set { ViewState["SearchTerm"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
                LoadAssets();
            }
        }

        void LoadEmployees()
        {
            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        }

        void LoadAssets()
        {
            ddlAsset.DataSource = dal.GetAllAssets();
            ddlAsset.DataTextField = "AssetName";
            ddlAsset.DataValueField = "AssetID";
            ddlAsset.DataBind();
            ddlAsset.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

            int totalRecords;
            rptAssets.DataSource = dal.GetAssetRecords(PageIndex, PageSize, SearchTerm, out totalRecords);
            rptAssets.DataBind();

            lblPageInfo.Text = $"Page {PageIndex} of {Math.Ceiling((double)totalRecords / PageSize)}";
            btnPrev.Enabled = PageIndex > 1;
            btnNext.Enabled = PageIndex * PageSize < totalRecords;
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (PageIndex > 1) PageIndex--;
            LoadAssets();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            PageIndex++;
            LoadAssets();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            PageIndex = 1;
            LoadAssets();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            dal.SaveAsset(
                Convert.ToInt32(hfAssetID.Value),
                Convert.ToInt32(ddlEmployee.SelectedValue),
                Convert.ToInt32(ddlAsset.SelectedValue),
                Convert.ToDateTime(txtIssueDate.Text),
                string.IsNullOrEmpty(txtReturnDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtReturnDate.Text),
                ddlCondition.SelectedValue,
                string.IsNullOrEmpty(txtDeduction.Text) ? 0 : Convert.ToDecimal(txtDeduction.Text)
            );

            hfAssetID.Value = "0";
            ClearForm();
            LoadAssets();
        }

        protected void rptAssets_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            int assetRecordID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Edit")
            {
                DataRow row = dal.GetAssetById(assetRecordID);
                if (row == null) return;

                hfAssetID.Value = row["AssetRecordID"].ToString();
                ddlEmployee.SelectedValue = row["EmployeeID"].ToString();
                ddlAsset.SelectedValue = row["AssetID"].ToString();
                txtIssueDate.Text = Convert.ToDateTime(row["IssueDate"]).ToString("yyyy-MM-dd");
                txtReturnDate.Text = row["ReturnDate"] != DBNull.Value ? Convert.ToDateTime(row["ReturnDate"]).ToString("yyyy-MM-dd") : "";
                ddlCondition.SelectedValue = row["Condition"].ToString();
                txtDeduction.Text = row["Deduction"].ToString();
            }

            if (e.CommandName == "Delete")
            {
                dal.DeleteAsset(assetRecordID);
                LoadAssets();
            }
        }

        void ClearForm()
        {
            ddlEmployee.SelectedIndex = 0;
            ddlAsset.SelectedIndex = 0;
            txtIssueDate.Text = "";
            txtReturnDate.Text = "";
            ddlCondition.SelectedIndex = 0;
            txtDeduction.Text = "";
        }
    }
}
