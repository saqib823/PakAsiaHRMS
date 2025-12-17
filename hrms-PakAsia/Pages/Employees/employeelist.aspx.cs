using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Employees
{
    public partial class employeelist : System.Web.UI.Page
    {
        private int PageSize = 10;
        private int TotalRecords = 0;
        private EmployeeMaster employeeDAL;

        // ViewState Properties
        private int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }

        private string SortField
        {
            get => ViewState["SortField"]?.ToString() ?? "EmployeeID";
            set => ViewState["SortField"] = value;
        }

        private string SortOrder
        {
            get => ViewState["SortOrder"]?.ToString() ?? "DESC";
            set => ViewState["SortOrder"] = value;
        }

        private string SearchText
        {
            get => ViewState["SearchText"]?.ToString() ?? string.Empty;
            set => ViewState["SearchText"] = value;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            employeeDAL = new EmployeeMaster();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckSession();
                BindEmployees();
            }
        }

        private void CheckSession()
        {
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/Default.aspx?returnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
            }
        }

        private void BindEmployees()
        {
            try
            {
                DataTable dt = employeeDAL.GetEmployees(
                    CurrentPage,
                    PageSize,
                    SearchText,
                    SortField,
                    SortOrder,
                    out TotalRecords
                );

                rptEmployees.DataSource = dt;
                rptEmployees.DataBind();

                BindPager();
                UpdatePageInfo();
                UpdateNavigationButtons();
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading employees: " + ex.Message, "danger");
            }
        }

        private void BindPager()
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            if (totalPages == 0) totalPages = 1;

            // Show limited page numbers (max 7)
            int startPage = Math.Max(1, CurrentPage - 3);
            int endPage = Math.Min(totalPages, CurrentPage + 3);

            var pagerList = new List<dynamic>();

            for (int i = startPage; i <= endPage; i++)
            {
                pagerList.Add(new { PageNumber = i, IsCurrent = i == CurrentPage });
            }

            rptPager.DataSource = pagerList;
            rptPager.DataBind();
        }

        private void UpdatePageInfo()
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            int startRecord = ((CurrentPage - 1) * PageSize) + 1;
            int endRecord = Math.Min(CurrentPage * PageSize, TotalRecords);

            if (TotalRecords == 0)
            {
                lblPageInfo.Text = "No records found";
            }
            else
            {
                lblPageInfo.Text = $"Showing {startRecord} to {endRecord} of {TotalRecords} entries";
            }
        }

        private void UpdateNavigationButtons()
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
        }

        protected string ShowEmptyMessage()
        {
            if (rptEmployees.Items.Count == 0)
            {
                return @"<tr>
                            <td colspan='5' class='text-center py-4'>
                                <div class='text-muted'>
                                    <i class='uil uil-users-alt fs-1'></i>
                                    <h5 class='mt-2'>No employees found</h5>
                                    <p>" + (string.IsNullOrEmpty(SearchText) ?
                                        "No employees in the system" :
                                        $"No employees found matching '{SearchText}'") + @"</p>
                                </div>
                            </td>
                         </tr>";
            }
            return string.Empty;
        }

        protected void rptEmployees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // You can add row-specific logic here
                // Example: Highlight based on status
                DataRowView row = (DataRowView)e.Item.DataItem;
                // if (row["Status"].ToString() == "Inactive")
                // {
                //     e.Item.CssClass = "table-secondary";
                // }
            }
        }

        protected void txtSearchEmployee_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            SearchText = txtSearchEmployee.Text.Trim();
            BindEmployees();
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument);
                BindEmployees();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindEmployees();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

            if (CurrentPage < totalPages)
            {
                CurrentPage++;
                BindEmployees();
            }
        }

        protected void rptEmployees_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                long employeeID = Convert.ToInt64(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "ViewEmployee":
                        Response.Redirect($"EmployeeView.aspx?ID={employeeID}", false);
                        Context.ApplicationInstance.CompleteRequest();
                        break;

                    case "EditEmployee":
                        Response.Redirect($"EmployeeEdit.aspx?ID={employeeID}", false);
                        Context.ApplicationInstance.CompleteRequest();
                        break;

                    case "DeleteEmployee":
                        DeleteEmployee(employeeID);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error processing request:" + ex.Message, "danger");
            }
        }

        private void DeleteEmployee(long employeeID)
        {
            try
            {
                employeeDAL.DeleteEmployee(employeeID);
                ShowAlert("Employee deleted succesfully!", "success");
            }
            catch (Exception ex)
            {
                ShowAlert("Error deleting employee: " + ex.Message, "danger");
            }
        }

        // Optional: Add sorting functionality
        protected void SortEmployees(string field)
        {
            if (SortField == field)
            {
                SortOrder = SortOrder == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                SortField = field;
                SortOrder = "ASC";
            }

            BindEmployees();
        }
        private void ShowAlert(string message, string cssClass)
        {
            var alertScript = $@"
                <div class='alert alert-{cssClass} alert-dismissible fade show' role='alert'>
                    {message}
                    <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                </div>
                <script>
                    setTimeout(function() {{
                        var alert = document.querySelector('.alert');
                        if(alert) alert.remove();
                    }}, 5000);
                </script>";

            phAlert.Controls.Clear();
            phAlert.Controls.Add(new LiteralControl(alertScript));
        }

    }
}