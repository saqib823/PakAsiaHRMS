using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;

namespace hrms_PakAsia.Pages.Asset
{
    public partial class AddAsset : hrms_PakAsia.BasePage
    {
        AssetDAL dal = new AssetDAL();
        LoggedInUser currentUser = null;
    protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();

            if (!IsPostBack)
        LoadAssets();
            // landing logged by BasePage.OnLoad
        }
        public LoggedInUser GetSessionData()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            return currentUser;
        }
        

        public void CheckSession()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;

            if (currentUser == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        //test
        void LoadAssets()
        {
            rptAssets.DataSource = dal.GetAllAssets();
            rptAssets.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // capture old data for update
            string oldDataJson = null;
            int assetId = Convert.ToInt32(hfAssetID.Value);
            if (assetId != 0)
            {
                var existing = dal.GetAssetMasterById(assetId);
                if (existing != null)
                {
                    var oldObj = new
                    {
                        AssetID = existing["AssetID"],
                        AssetName = existing["AssetName"],
                        IsActive = Convert.ToBoolean(existing["IsActive"]) ? 1 : 0
                    };
                    oldDataJson = new JavaScriptSerializer().Serialize(oldObj);
                }
            }

            dal.SaveAssetMaster(
                assetId,
                txtAssetName.Text.Trim(),
                ddlStatus.SelectedValue == "1"
            );

            // audit - include oldData and newData JSON
            var newObj = new { AssetID = hfAssetID.Value, AssetName = txtAssetName.Text.Trim(), IsActive = ddlStatus.SelectedValue == "1" ? 1 : 0 };
            string newDataJson = new JavaScriptSerializer().Serialize(newObj);
            LogAction(assetId == 0 ? "Insert Asset" : "Update Asset", recordId: assetId.ToString(), oldData: oldDataJson, newData: newDataJson, remarks: "Asset saved from UI");

            hfAssetID.Value = "0";
            txtAssetName.Text = "";
            ddlStatus.SelectedIndex = 0;
            LoadAssets();
        }

        protected void rptAssets_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                var row = dal.GetAssetMasterById(Convert.ToInt32(e.CommandArgument));
                if (row == null) return;

                hfAssetID.Value = row["AssetID"].ToString();
                txtAssetName.Text = row["AssetName"].ToString();
                ddlStatus.SelectedValue = Convert.ToBoolean(row["IsActive"]) ? "1" : "0";
            }

            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                // capture existing data before delete
                string oldData = null;
                var existing = dal.GetAssetMasterById(id);
                if (existing != null)
                {
                    var oldObj = new
                    {
                        AssetID = existing["AssetID"],
                        AssetName = existing["AssetName"],
                        IsActive = Convert.ToBoolean(existing["IsActive"]) ? 1 : 0
                    };
                    oldData = new JavaScriptSerializer().Serialize(oldObj);
                }

                dal.DeleteAssetMaster(id);
                LogAction("Delete Asset", recordId: id.ToString(), oldData: oldData, remarks: "Asset deleted from UI");
                LoadAssets();
            }
        }
    }
}
