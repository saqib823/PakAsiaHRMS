using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages
{
    public partial class LoanManagement : hrms_PakAsia.BasePage
    {
        private int PageSize => 10;
        private int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }
        private int TotalRecords
        {
            get => ViewState["TotalRecords"] != null ? (int)ViewState["TotalRecords"] : 0;
            set => ViewState["TotalRecords"] = value;
        }

        private LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();

            if (!IsPostBack)
            {
                BindEmployees();
                BindLoans();
                // landing logged by BasePage.OnLoad
            }
        }

        private LoggedInUser GetSessionData()
        {
            return HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
        }

        private void CheckSession()
        {
            if (Session["LoggedInUser"] == null)
                Response.Redirect("~/Default.aspx");
        }

        private void BindEmployees()
        {
            if (currentUser.RoleId == 1)
            {
                ddlEmployee.DataSource = CommonDAL.GetEmployees();
            }
            else
            {
                ddlEmployee.DataSource = CommonDAL.GetEmployee(currentUser.UserID.ToString());
            }

            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }

        // ===== Save or Update Loan =====
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? loanId = ViewState["EditLoanID"] as int?;

            bool isSaved = LoanDAL.SaveLoan(
                EmployeeID: Convert.ToInt32(ddlEmployee.SelectedValue),
                LoanType: txtLoanType.Text,
                LoanAmount: Convert.ToDecimal(txtLoanAmount.Text),
                DurationMonths: Convert.ToInt32(txtDuration.Text),
                StartDate: Convert.ToDateTime(txtStartDate.Text),
                CreatedBy: currentUser.UserID,
                LoanID: loanId
            );

            if (isSaved)
            {
                LogAction(loanId.HasValue ? "Update Loan" : "Apply Loan",
                    recordId: loanId?.ToString() ?? string.Empty,
                    newData: $"EmployeeID={ddlEmployee.SelectedValue};Type={txtLoanType.Text};Amount={txtLoanAmount.Text};Duration={txtDuration.Text};StartDate={txtStartDate.Text}",
                    remarks: "Loan saved from UI");
                ShowAlert(loanId.HasValue ? "Loan updated successfully" : "Loan applied successfully", "success");
                ClearForm();
                BindLoans();
            }
            else
            {
                ShowAlert("Error saving loan. Please try again.", "danger");
            }
        }

        private void BindLoans()
        {
            int? employeeId = currentUser.RoleId != 1 ? currentUser.UserID : (int?)null;

            // Declare the out variable
            int totalRecords;

            // C# 5 compatible: positional arguments only
            DataTable dt = LoanDAL.GetLoansPaged(
                CurrentPage,        // pageNumber
                PageSize,           // pageSize
                txtSearch.Text.Trim(), // searchText
                employeeId,         // employeeId
                out totalRecords    // out parameter
            );

            TotalRecords = totalRecords;
            rptLoans.DataSource = dt;
            rptLoans.DataBind();
        }


        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindLoans();
            LogAction("Search Loans", remarks: $"Search='{txtSearch.Text?.Trim()}'");
        }

        protected void rptLoans_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int loanId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "EditLoan":
                    LoadLoanForEdit(loanId);
                    break;
                case "Approve":
                    LoanDAL.UpdateLoanStatus(loanId, "Approved", currentUser.UserID);
                    LogAction("Approve Loan", recordId: loanId.ToString(), remarks: "Loan approved");
                    ShowAlert("Loan approved", "success");
                    BindLoans();
                    break;
                case "Reject":
                    LoanDAL.UpdateLoanStatus(loanId, "Rejected", currentUser.UserID);
                    LogAction("Reject Loan", recordId: loanId.ToString(), remarks: "Loan rejected");
                    ShowAlert("Loan rejected", "danger");
                    BindLoans();
                    break;
                case "DeleteLoan":
                    LoanDAL.DeleteLoan(loanId);
                    LogAction("Delete Loan", recordId: loanId.ToString(), remarks: "Loan deleted");
                    ShowAlert("Loan deleted successfully", "warning");
                    BindLoans();
                    break;
            }
        }

        private void LoadLoanForEdit(int loanId)
        {
            DataRow dr = LoanDAL.GetLoanById(loanId);
            if (dr == null) return;

            ddlEmployee.SelectedValue = dr["EmployeeID"].ToString();
            txtLoanType.Text = dr["LoanType"].ToString();
            txtLoanAmount.Text = dr["Amount"].ToString();
            txtDuration.Text = dr["Duration"].ToString();
            txtStartDate.Text = Convert.ToDateTime(dr["StartDate"]).ToString("yyyy-MM-dd");

            ViewState["EditLoanID"] = loanId;
            ShowAlert("Loan loaded for editing", "info");
        }

        protected void rptLoans_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            string status = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Status"));

            LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
            LinkButton btnApprove = e.Item.FindControl("btnApprove") as LinkButton;
            LinkButton btnReject = e.Item.FindControl("btnReject") as LinkButton;

            // Hide all buttons by default
            if (btnEdit != null) btnEdit.Visible = false;
            if (btnDelete != null) btnDelete.Visible = false;
            if (btnApprove != null) btnApprove.Visible = false;
            if (btnReject != null) btnReject.Visible = false;

            // Normal user can edit/delete only pending loans
            if (currentUser.RoleId != 1)
            {
                if (status == "Pending")
                {
                    if (btnEdit != null) btnEdit.Visible = true;
                    if (btnDelete != null) btnDelete.Visible = true;
                }
                return;
            }

            // Admin can approve/reject pending loans
            if (status == "Pending")
            {
                if (btnApprove != null) btnApprove.Visible = true;
                if (btnReject != null) btnReject.Visible = true;
            }

            // Admin can also edit/delete any loan
            if (btnEdit != null) btnEdit.Visible = true;
            if (btnDelete != null) btnDelete.Visible = true;
        }

        void ClearForm()
        {
            ddlEmployee.SelectedIndex = 0;
            txtLoanType.Text = "";
            txtLoanAmount.Text = "";
            txtDuration.Text = "";
            txtStartDate.Text = "";
            ViewState["EditLoanID"] = null;
        }

        void ShowAlert(string msg, string css)
        {
            phAlert.Controls.Clear();
            phAlert.Controls.Add(new Literal
            {
                Text = $@"
                    <div id='autoAlert' class='alert alert-{css} alert-dismissible fade show' role='alert'>
                        {msg}
                    </div>
                    <script>
                        setTimeout(function() {{
                            var a=document.getElementById('autoAlert');
                            if(a){{a.classList.remove('show'); a.classList.add('hide');}}
                        }}, 3000);
                    </script>"
            });
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
