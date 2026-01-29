using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages
{
    public partial class SubmitExpense : System.Web.UI.Page
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
        LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                BindInitialData();
                BindExpenses();
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
        private void BindInitialData()
        {
            if (currentUser.RoleId == 1)
            {
                ddlEmployee.DataSource = CommonDAL.GetEmployees(); ;
                ddlEmployee.DataTextField = "Name";
                ddlEmployee.DataValueField = "ID";
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
            }
            else
            {
                ddlEmployee.DataSource = CommonDAL.GetEmployee(currentUser.UserID.ToString()); ;
                ddlEmployee.DataTextField = "Name";
                ddlEmployee.DataValueField = "ID";
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string receiptPath = "";
            if (fileReceipt.PostedFile != null && fileReceipt.PostedFile.ContentLength > 0)
            {
                string ext = Path.GetExtension(fileReceipt.PostedFile.FileName).ToLower();
                if (!new string[] { ".jpg", ".jpeg", ".png", ".pdf" }.Contains(ext))
                {
                    ShowAlert("Invalid file type", "danger");
                    return;
                }

                string uploadFolder = Server.MapPath("~/Uploads/Expenses/");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                string fileName = Guid.NewGuid().ToString() + ext;
                fileReceipt.PostedFile.SaveAs(Path.Combine(uploadFolder, fileName));
                receiptPath = "~/Uploads/Expenses/" + fileName;
            }

            int? expenseId = ViewState["EditExpenseID"] as int?;
            bool isSaved = ExpenseDAL.SaveExpense(
                EmployeeID: Convert.ToInt32(ddlEmployee.SelectedValue),
                EmployeeName: ddlEmployee.SelectedItem.Text,
                ExpenseType: txtExpenseType.Text,
                Amount: Convert.ToDecimal(txtAmount.Text),
                ExpenseDate: Convert.ToDateTime(txtDate.Text),
                Description: txtDescription.Text,
                ReceiptPath: receiptPath,
                Status: "Pending", // default for new/edited expense
                CreatedBy: 1,      // you can replace with logged-in user ID
                ExpenseID: expenseId
            );

            if (isSaved) ShowAlert(expenseId.HasValue ? "Expense updated successfully" : "Expense submitted successfully", "success");

            ClearForm();
            BindExpenses();
        }

        private void ClearForm()
        {
            ddlEmployee.SelectedIndex = 0;
            txtExpenseType.Text = "";
            txtAmount.Text = "";
            txtDate.Text = "";
            txtDescription.Text = "";
            ViewState["EditExpenseID"] = null;
        }

        private void BindExpenses()
        {
            int total;

            int? employeeId = null;

            // RoleId = 1 → normal user → own expenses only
            if (currentUser.RoleId != 1)
                employeeId = currentUser.UserID;

            DataTable dt = ExpenseDAL.GetExpensesPaged(
                CurrentPage,
                PageSize,
                txtSearch.Text.Trim(),
                employeeId,
                out total
            );

            TotalRecords = total;
            rptExpenses.DataSource = dt;
            rptExpenses.DataBind();
        }


        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindExpenses();
        }

        protected void rptExpenses_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int expenseId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "DeleteExpense")
            {
                ExpenseDAL.DeleteExpense(expenseId);
                ShowAlert("Expense deleted successfully", "warning");
                BindExpenses();
            }
            else if (e.CommandName == "EditExpense")
            {
                LoadExpenseForEdit(expenseId);
            }
            else if (e.CommandName == "ApproveExpense")
            {
                ExpenseDAL.UpdateExpenseStatus(expenseId, "Approved", 1); // Replace 1 with logged-in user ID
                ShowAlert("Expense approved", "success");
                BindExpenses();
            }
            else if (e.CommandName == "DisapproveExpense")
            {
                ExpenseDAL.UpdateExpenseStatus(expenseId, "Disapproved", 1);
                ShowAlert("Expense disapproved", "danger");
                BindExpenses();
            }
        }

        private void LoadExpenseForEdit(int expenseId)
        {
            DataRow dr = ExpenseDAL.GetExpenseById(expenseId);
            if (dr == null) return;

            ddlEmployee.SelectedValue = dr["EmployeeID"].ToString();
            txtExpenseType.Text = dr["ExpenseType"].ToString();
            txtAmount.Text = dr["Amount"].ToString();
            txtDate.Text = Convert.ToDateTime(dr["ExpenseDate"]).ToString("yyyy-MM-dd");
            txtDescription.Text = dr["Description"].ToString();

            ViewState["EditExpenseID"] = expenseId;
            ShowAlert("Expense loaded for editing", "info");
            
        }

        private void ShowAlert(string message, string css)
        {
            phAlert.Controls.Clear();
            phAlert.Controls.Add(new Literal
            {
                Text = $@"
                    <div id='autoAlert' class='alert alert-{css} alert-dismissible fade show' role='alert'>{message}</div>
                    <script>
                        setTimeout(function() {{
                            var a=document.getElementById('autoAlert');
                            if(a){{a.classList.remove('show'); a.classList.add('hide');}}
                        }}, 3000);
                    </script>"
            });
        }

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        protected void rptExpenses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            // Get the status of the expense
            string status = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Status"));

            // Find controls safely
            LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
            LinkButton btnApprove = e.Item.FindControl("btnApprove") as LinkButton;
            LinkButton btnReject = e.Item.FindControl("btnDisapprove") as LinkButton;

            // 🔒 Default: hide all buttons initially
            if (btnEdit != null) btnEdit.Visible = false;
            if (btnDelete != null) btnDelete.Visible = false;
            if (btnApprove != null) btnApprove.Visible = false;
            if (btnReject != null) btnReject.Visible = false;

            // ===== Normal user (RoleId != 1) =====
            if (currentUser.RoleId != 1)
            {
                // Can edit / delete only if expense is NOT approved
                if (status == "Pending")
                {
                    if (btnEdit != null) btnEdit.Visible = true;
                    if (btnDelete != null) btnDelete.Visible = true;
                }

                // Normal user cannot see approve/reject buttons
                return; // stop here for normal users
            }

            // ===== Super admin (RoleId = 1) =====
            // Can approve / reject only if status is pending
            if (status == "Pending")
            {
                if (btnApprove != null) btnApprove.Visible = true;
                if (btnReject != null) btnReject.Visible = true;
            }

            // Super admin can edit / delete any expense (optional)
            if (btnEdit != null) btnEdit.Visible = true;
            if (btnDelete != null) btnDelete.Visible = true;
        }



    }
}
