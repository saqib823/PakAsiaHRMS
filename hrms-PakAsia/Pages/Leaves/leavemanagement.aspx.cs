using HRMSLib.BusinessLogic;
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
    public partial class leavemanagement : hrms_PakAsia.BasePage
    {
        private const int PageSize = 10;
        LoggedInUser currentUser = null;
        protected int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
                BindLeaves();
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
        #region Bind

        private void BindLeaves()
        {
            DataSet ds = new DataSet();
            if (currentUser.RoleId == 1 || currentUser.RoleId == 6)
            {
                ds = LeaveDAL.GetLeaves(
                               txtSearchLeave.Text.Trim(),
                               CurrentPage,
                               PageSize,
                               0
                           );
            }
            else
            {
                ds = LeaveDAL.GetLeaves(
                               txtSearchLeave.Text.Trim(),
                               CurrentPage,
                               PageSize,
                               currentUser.UserID
                           );
            }
           

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
            LogAction("Search Leaves", remarks: $"Search='{txtSearchLeave.Text?.Trim()}'");
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
                    LogAction("Approve Leave", recordId: leaveId.ToString(), remarks: $"Approved by {approverId}");
                    break;

                case "RejectLeave":
                    LeaveDAL.ApproveRejectLeave(leaveId, approverId, "Rejected");
                    LogAction("Reject Leave", recordId: leaveId.ToString(), remarks: $"Rejected by {approverId}");
                    break;

                case "EncashLeave":
                    LeaveDAL.EncashLeave(leaveId);
                    LeaveDAL.ApproveRejectLeave(leaveId, approverId, "Approved");
                    LogAction("Encash Leave", recordId: leaveId.ToString(), remarks: $"Encashed and approved by {approverId}");
                    break;
                case "CarryForward":
                    LeaveDAL.CarryForwardLeaves(DateTime.Now.Year, leaveId);
                    LeaveDAL.ApproveRejectLeave(leaveId, approverId, "Approved");
                    LogAction("Carry Forward Leave", recordId: leaveId.ToString(), remarks: $"Carry-forward and approved by {approverId}");
                    break;
            }

            BindLeaves();
        }

        #endregion

        #region UI Helpers

        protected void rptLeaves_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            string status = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Status"));

            bool carryForward = DataBinder.Eval(e.Item.DataItem, "CarryForward") != DBNull.Value &&
                                Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "CarryForward"));

            bool encashment = DataBinder.Eval(e.Item.DataItem, "Encashment") != DBNull.Value &&
                              Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "Encashment"));

            LinkButton btnApprove = (LinkButton)e.Item.FindControl("btnApprove");
            LinkButton btnReject = (LinkButton)e.Item.FindControl("btnReject");
            LinkButton btnEncash = (LinkButton)e.Item.FindControl("btnEncash");
            LinkButton btnCarryForward = (LinkButton)e.Item.FindControl("btnCarryForward");

            // 🔒 Default: hide all
            btnApprove.Visible = false;
            btnReject.Visible = false;
            btnEncash.Visible = false;
            btnCarryForward.Visible = false;

            // 🚫 Non-admin or non-pending → no actions
            if (currentUser.RoleId != 1 || status != "Pending")
                return;

            // 💰 Encash / Carry Forward allowed
            if (carryForward || encashment)
            {
                btnEncash.Visible = encashment;
                btnCarryForward.Visible = carryForward;
                btnReject.Visible = true;
                return;
            }

            // ✔ Normal approve / reject
            btnApprove.Visible = true;
            btnReject.Visible = true;
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