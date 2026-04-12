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
            if (currentUser.RoleId == 1 || currentUser.RoleId == 6)
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
            int? employeeId = (currentUser.RoleId != 1 && currentUser.RoleId != 6)
                ? currentUser.UserID
                : (int?)null;
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
            
            // Update pagination labels
            UpdatePaginationLabels();
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
                case "Print":
                    PrintLoanDetails(loanId);
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
            LinkButton btnPrint = e.Item.FindControl("btnPrint") as LinkButton;

            // Hide all buttons by default
            if (btnEdit != null) btnEdit.Visible = false;
            if (btnDelete != null) btnDelete.Visible = false;
            if (btnApprove != null) btnApprove.Visible = false;
            if (btnReject != null) btnReject.Visible = false;
            if (btnPrint != null) btnPrint.Visible = true; // Print button is always visible

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

        private void PrintLoanDetails(int loanId)
        {
            DataRow dr = LoanDAL.GetLoanById(loanId);
            if (dr == null) return;

            string printHtml = GeneratePrintHtml(dr);
            
            // Register startup script to open print dialog
            string script = $@"
                var printWindow = window.open('', '_blank');
                printWindow.document.write(`{printHtml.Replace("`", "\\`").Replace("\r", "").Replace("\n", "\\n")}`);
                printWindow.document.close();
                printWindow.print();
            ";
            
            ScriptManager.RegisterStartupScript(this, GetType(), "PrintLoan", script, true);
            
            LogAction("Print Loan", recordId: loanId.ToString(), remarks: "Loan details printed");
        }

        private string GeneratePrintHtml(DataRow loan)
        {
            string logoPath = ResolveUrl("~/assets/img/icons/logo.png"); // Correct logo path
            string companyName = "PakAsia HRMS";
            string printDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            
            return $@"
<!DOCTYPE html>
<html>
<head>
    <title>Loan Details - {loan["EmployeeNo"]} - {loan["LoanType"]}</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            color: #333;
        }}
        .header {{
            text-align: center;
            border-bottom: 2px solid #333;
            padding-bottom: 20px;
            margin-bottom: 30px;
        }}
        .logo {{
            max-width: 150px;
            margin-bottom: 10px;
        }}
        .company-name {{
            font-size: 24px;
            font-weight: bold;
            margin: 10px 0;
        }}
        .document-title {{
            font-size: 20px;
            font-weight: bold;
            color: #666;
            margin: 10px 0;
        }}
        .loan-details {{
            margin: 30px 0;
        }}
        .detail-row {{
            margin: 10px 0;
            display: flex;
            justify-content: space-between;
            border-bottom: 1px solid #eee;
            padding: 8px 0;
        }}
        .detail-label {{
            font-weight: bold;
            width: 40%;
        }}
        .detail-value {{
            width: 60%;
            text-align: right;
        }}
        .status-approved {{
            color: #28a745;
            font-weight: bold;
        }}
        .status-pending {{
            color: #ffc107;
            font-weight: bold;
        }}
        .status-rejected {{
            color: #dc3545;
            font-weight: bold;
        }}
        .footer {{
            margin-top: 50px;
            text-align: center;
            font-size: 12px;
            color: #666;
            border-top: 1px solid #eee;
            padding-top: 20px;
        }}
        @media print {{
            body {{ margin: 0; }}
            .no-print {{ display: none; }}
        }}
    </style>
</head>
<body>
    <div class='header'>
        <img src='{logoPath}' alt='Company Logo' class='logo' onerror=""this.style.display='none'"" />
        <div class='company-name'>{companyName}</div>
        <div class='document-title'>Loan Details</div>
    </div>

    <div class='loan-details'>
        <div class='detail-row'>
            <span class='detail-label'>Employee Name:</span>
            <span class='detail-value'>{loan["EmployeeName"]}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Employee No:</span>
            <span class='detail-value'>{loan["EmployeeNo"]}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Department:</span>
            <span class='detail-value'>{loan["DepartmentName"] ?? "N/A"}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Designation:</span>
            <span class='detail-value'>{loan["DesignationName"] ?? "N/A"}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Loan Type:</span>
            <span class='detail-value'>{loan["LoanType"]}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Loan Amount:</span>
            <span class='detail-value'>{Convert.ToDecimal(loan["LoanAmount"]):N2}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Duration (Months):</span>
            <span class='detail-value'>{loan["DurationMonths"]}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Monthly Deduction:</span>
            <span class='detail-value'>{Convert.ToDecimal(loan["MonthlyDeduction"]):N2}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Start Date:</span>
            <span class='detail-value'>{Convert.ToDateTime(loan["StartDate"]):yyyy-MM-dd}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Status:</span>
            <span class='detail-value status-{loan["Status"].ToString().ToLower()}'>{loan["Status"]}</span>
        </div>
        <div class='detail-row'>
            <span class='detail-label'>Loan ID:</span>
            <span class='detail-value'>{loan["LoanID"]}</span>
        </div>
    </div>

    <div class='footer'>
        <div>Printed on: {printDate}</div>
        <div>This is a system-generated document</div>
    </div>
</body>
</html>";
        }

        protected void btnPagination_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string command = btn.CommandArgument;
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            switch (command)
            {
                case "First":
                    CurrentPage = 1;
                    break;
                case "Previous":
                    if (CurrentPage > 1)
                        CurrentPage--;
                    break;
                case "Next":
                    if (CurrentPage < totalPages)
                        CurrentPage++;
                    break;
                case "Last":
                    CurrentPage = totalPages;
                    break;
            }

            BindLoans();
        }

        private void UpdatePaginationLabels()
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            int startRecord = TotalRecords == 0 ? 0 : ((CurrentPage - 1) * PageSize) + 1;
            int endRecord = Math.Min(CurrentPage * PageSize, TotalRecords);

            lblCurrentPage.Text = CurrentPage.ToString();
            lblTotalPages.Text = totalPages.ToString();
            lblTotalRecords.Text = TotalRecords.ToString();
            lblStartRecord.Text = startRecord.ToString();
            lblEndRecord.Text = endRecord.ToString();

            // Enable/disable pagination buttons
            btnFirst.Enabled = CurrentPage > 1;
            btnPrevious.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
            btnLast.Enabled = CurrentPage < totalPages;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
