using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;

namespace hrms_PakAsia.Pages.Asset
{
    public partial class AddAsset : System.Web.UI.Page
    {
        AssetDAL dal = new AssetDAL();
        LoggedInUser currentUser = null;
        protected async void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();

            if (!IsPostBack)
                LoadAssets();
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
            dal.SaveAssetMaster(
                Convert.ToInt32(hfAssetID.Value),
                txtAssetName.Text.Trim(),
                ddlStatus.SelectedValue == "1"
            );

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
                dal.DeleteAssetMaster(Convert.ToInt32(e.CommandArgument));
                LoadAssets();
            }
        }
    }
}
