using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Leaves
{
    public partial class leavemanagement : System.Web.UI.Page
    {
        private const int PageSize = 10;

        protected int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindLeaves();
        }

        #region Bind

        private void BindLeaves()
        {
            DataSet ds = LeaveDAL.GetLeaves(
                txtSearchLeave.Text.Trim(),
                CurrentPage,
                PageSize
            );

            rptLeaves.DataSource = ds.Tables[0];
            rptLeaves.DataBind();

            int totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            SetupPager(totalRecords);
        }

        #endregion

        #region Pager

        private void SetupPager(int totalRecords)
        {
            int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

            lblPageInfoLeave.Text = totalPages == 0
                ? "No records found"
                : $"Page {CurrentPage} of {totalPages}";

            btnPrevLeave.Enabled = CurrentPage > 1;
            btnNextLeave.Enabled = CurrentPage < totalPages;

            List<object> pages = new List<object>();
            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(new
                {
                    PageNumber = i,
                    IsCurrent = (i == CurrentPage)
                });
            }

            rptPagerLeave.DataSource = pages;
            rptPagerLeave.DataBind();
        }

        protected void rptPagerLeave_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            CurrentPage = Convert.ToInt32(e.CommandArgument);
            BindLeaves();
        }

        protected void btnPrevLeave_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            BindLeaves();
        }

        protected void btnNextLeave_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindLeaves();
        }

        #endregion

        #region Search

        protected void txtSearchLeave_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindLeaves();
        }

        #endregion

        #region Actions

        protected void rptLeaves_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int leaveId = Convert.ToInt32(e.CommandArgument);
            int approverId = Convert.ToInt32(Session["EmployeeID"]);

            switch (e.CommandName)
            {
                case "ApproveLeave":
                    LeaveDAL.ApproveRejectLeave(leaveId, approverId, "Approved");
                    break;

                case "RejectLeave":
                    LeaveDAL.ApproveRejectLeave(leaveId, approverId, "Rejected");
                    break;

                case "EncashLeave":
                    LeaveDAL.EncashLeave(leaveId);
                    break;
            }

            BindLeaves();
        }

        #endregion

        #region UI Helpers

        protected void rptLeaves_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string status = DataBinder.Eval(e.Item.DataItem, "Status").ToString();

                LinkButton btnApprove = (LinkButton)e.Item.FindControl("btnApprove");
                LinkButton btnReject = (LinkButton)e.Item.FindControl("btnReject");

                if (status != "Pending")
                {
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                }
            }
        }

        protected string ShowEmptyMessageLeave()
        {
            if (rptLeaves.Items.Count == 0)
                return "<div class='text-center text-muted py-3'>No leave records found.</div>";

            return string.Empty;
        }

        #endregion

        #region Navigation

        protected void btnApplyLeave_Click(object sender, EventArgs e)
        {
            Response.Redirect("applyleave.aspx");
        }

        #endregion
    }
}