using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;

namespace hrms_PakAsia.Pages.Payroll
{
    public partial class SalaryStructure : System.Web.UI.Page
    {
        PayrollDAL dal = new PayrollDAL();
        int PageSize = 10;

        int PageIndex
        {
            get { return ViewState["PageIndex"] == null ? 1 : (int)ViewState["PageIndex"]; }
            set { ViewState["PageIndex"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
                LoadPayrollPeriods();
                LoadSalaryStructures();
                SetDefaultDates();
            }
        }

        void SetDefaultDates()
        {
            if (string.IsNullOrEmpty(txtEffectiveFrom.Text))
                txtEffectiveFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(txtReportStartDate.Text))
                txtReportStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(txtReportEndDate.Text))
                txtReportEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        void LoadEmployees()
        {
            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }

        void LoadPayrollPeriods()
        {
            ddlPayrollPeriod.DataSource = dal.GetPayrollPeriods("Draft");
            ddlPayrollPeriod.DataTextField = "PeriodName";
            ddlPayrollPeriod.DataValueField = "PeriodID";
            ddlPayrollPeriod.DataBind();
            ddlPayrollPeriod.Items.Insert(0, new ListItem("-- Select Period --", "0"));
        }

        void LoadSalaryStructures()
        {
            var ds = dal.GetSalaryStructuresPaged(txtSearch.Text.Trim(), PageIndex, PageSize);

            gvSalary.DataSource = ds.Tables[0];
            gvSalary.DataBind();

            int totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

            lblPageInfo.Text = $"Page {PageIndex} of {totalPages}";
            btnPrev.Enabled = PageIndex > 1;
            btnNext.Enabled = PageIndex < totalPages;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlEmployee.SelectedValue == "0")
                {
                    ShowMessage("Please select an employee!", "warning");
                    return;
                }

                var model = new SalaryStructureModel
                {
                    SalaryID = Convert.ToInt32(hfSalaryID.Value),
                    EmployeeID = Convert.ToInt32(ddlEmployee.SelectedValue),
                    EffectiveFrom = Convert.ToDateTime(txtEffectiveFrom.Text),
                    Basic = Convert.ToDecimal(string.IsNullOrEmpty(txtBasic.Text) ? "0" : txtBasic.Text),
                    HouseRent = Convert.ToDecimal(string.IsNullOrEmpty(txtHouseRent.Text) ? "0" : txtHouseRent.Text),
                    Utilities = Convert.ToDecimal(string.IsNullOrEmpty(txtUtilities.Text) ? "0" : txtUtilities.Text),
                    Medical = Convert.ToDecimal(string.IsNullOrEmpty(txtMedical.Text) ? "0" : txtMedical.Text),
                    Fuel = Convert.ToDecimal(string.IsNullOrEmpty(txtFuel.Text) ? "0" : txtFuel.Text),
                    Transport = Convert.ToDecimal(string.IsNullOrEmpty(txtTransport.Text) ? "0" : txtTransport.Text),
                    Mobile = Convert.ToDecimal(string.IsNullOrEmpty(txtMobile.Text) ? "0" : txtMobile.Text),
                    Bonus = Convert.ToDecimal(string.IsNullOrEmpty(txtBonus.Text) ? "0" : txtBonus.Text),
                    Commission = Convert.ToDecimal(string.IsNullOrEmpty(txtCommission.Text) ? "0" : txtCommission.Text),
                    Incentives = Convert.ToDecimal(string.IsNullOrEmpty(txtIncentives.Text) ? "0" : txtIncentives.Text),
                    CustomAllowances = txtCustomAllowances.Text,
                    Deductions = txtDeductions.Text,
                    IsActive = ddlStatus.SelectedValue == "1",
                    CreatedBy = Convert.ToInt32(Session["UserID"])
                };

                int salaryId = dal.SaveSalaryStructure(model);

                ShowMessage("Salary structure saved successfully!", "success");
                ClearForm();
                LoadSalaryStructures();
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving salary structure: " + ex.Message, "danger");
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                decimal basic = Convert.ToDecimal(string.IsNullOrEmpty(txtBasic.Text) ? "0" : txtBasic.Text);
                decimal houseRent = Convert.ToDecimal(string.IsNullOrEmpty(txtHouseRent.Text) ? "0" : txtHouseRent.Text);
                decimal utilities = Convert.ToDecimal(string.IsNullOrEmpty(txtUtilities.Text) ? "0" : txtUtilities.Text);
                decimal medical = Convert.ToDecimal(string.IsNullOrEmpty(txtMedical.Text) ? "0" : txtMedical.Text);
                decimal fuel = Convert.ToDecimal(string.IsNullOrEmpty(txtFuel.Text) ? "0" : txtFuel.Text);
                decimal transport = Convert.ToDecimal(string.IsNullOrEmpty(txtTransport.Text) ? "0" : txtTransport.Text);
                decimal mobile = Convert.ToDecimal(string.IsNullOrEmpty(txtMobile.Text) ? "0" : txtMobile.Text);
                decimal bonus = Convert.ToDecimal(string.IsNullOrEmpty(txtBonus.Text) ? "0" : txtBonus.Text);
                decimal commission = Convert.ToDecimal(string.IsNullOrEmpty(txtCommission.Text) ? "0" : txtCommission.Text);
                decimal incentives = Convert.ToDecimal(string.IsNullOrEmpty(txtIncentives.Text) ? "0" : txtIncentives.Text);

                decimal grossSalary = basic + houseRent + utilities + medical +
                                     fuel + transport + mobile + bonus +
                                     commission + incentives;

                txtGrossSalary.Text = grossSalary.ToString("N2");
            }
            catch (Exception)
            {
                txtGrossSalary.Text = "0.00";
            }
        }

        protected void btnProcessPayroll_Click(object sender, EventArgs e)
        {
            try
            {
                int periodId = Convert.ToInt32(ddlPayrollPeriod.SelectedValue);
                int processedBy = Convert.ToInt32(Session["UserID"]);

                if (periodId > 0)
                {
                    string result = dal.ProcessPayroll(periodId, processedBy);
                    ShowMessage(result, "success");
                    LoadPayrollPeriods();
                }
                else
                {
                    ShowMessage("Please select a payroll period!", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error processing payroll: " + ex.Message, "danger");
            }
        }

        protected void btnGenerateReports_Click(object sender, EventArgs e)
        {
            divReports.Visible = !divReports.Visible;
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string reportType = ddlReportType.SelectedValue;
                int periodId = Convert.ToInt32(ddlPayrollPeriod.SelectedValue);

                if (periodId == 0 && reportType != "YTD")
                {
                    ShowMessage("Please select a payroll period!", "warning");
                    return;
                }

                DateTime? startDate = null;
                DateTime? endDate = null;

                if (reportType == "YTD")
                {
                    startDate = Convert.ToDateTime(txtReportStartDate.Text);
                    endDate = Convert.ToDateTime(txtReportEndDate.Text);
                }

                DataSet ds = dal.GetPayrollReport(reportType, periodId, startDate, endDate, null);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string fileName = $"{reportType}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    ExportToExcel(ds.Tables[0], fileName);
                }
                else
                {
                    ShowMessage("No data found for the selected criteria!", "info");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error generating report: " + ex.Message, "danger");
            }
        }

        protected void btnBankFile_Click(object sender, EventArgs e)
        {
            try
            {
                int periodId = Convert.ToInt32(ddlPayrollPeriod.SelectedValue);

                if (periodId > 0)
                {
                    DataSet ds = dal.GenerateBankFile(periodId);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string fileName = $"BankFile_{DateTime.Now:yyyyMMddHHmmss}.txt";
                        ExportToText(ds.Tables[0], fileName);
                    }
                    else
                    {
                        ShowMessage("No bank data found for the selected period!", "info");
                    }
                }
                else
                {
                    ShowMessage("Please select a payroll period!", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error generating bank file: " + ex.Message, "danger");
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var ds = dal.GetSalaryStructuresPaged(txtSearch.Text.Trim(), 1, 1000);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string fileName = $"SalaryStructures_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    ExportToExcel(ds.Tables[0], fileName);
                }
                else
                {
                    ShowMessage("No data to export!", "info");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error exporting to Excel: " + ex.Message, "danger");
            }
        }

        protected void gvSalary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                int salaryId = Convert.ToInt32(e.CommandArgument);
                LoadSalaryForEdit(salaryId);
            }
            else if (e.CommandName == "DeleteRecord")
            {
                int salaryId = Convert.ToInt32(e.CommandArgument);
                dal.DeleteSalary(salaryId);
                ShowMessage("Salary structure deleted successfully!", "success");
                LoadSalaryStructures();
            }
        }

        protected void gvSalary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSalary.PageIndex = e.NewPageIndex;
            LoadSalaryStructures();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
            LoadSalaryStructures();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            PageIndex--;
            LoadSalaryStructures();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            PageIndex++;
            LoadSalaryStructures();
        }

        #region Helper Methods

        private void LoadSalaryForEdit(int salaryId)
        {
            DataRow row = dal.GetSalaryById(salaryId);
            if (row != null)
            {
                hfSalaryID.Value = row["SalaryID"].ToString();
                ddlEmployee.SelectedValue = row["EmployeeID"].ToString();
                txtEffectiveFrom.Text = Convert.ToDateTime(row["EffectiveFrom"]).ToString("yyyy-MM-dd");

                txtBasic.Text = row["Basic"].ToString();
                txtHouseRent.Text = row["HouseRent"].ToString();
                txtUtilities.Text = row["Utilities"].ToString();
                txtMedical.Text = row["Medical"].ToString();
                txtFuel.Text = row["Fuel"].ToString();
                txtTransport.Text = row["Transport"].ToString();
                txtMobile.Text = row["Mobile"].ToString();
                txtBonus.Text = row["Bonus"].ToString();
                txtCommission.Text = row["Commission"].ToString();
                txtIncentives.Text = row["Incentives"].ToString();
                txtCustomAllowances.Text = row["CustomAllowances"].ToString();
                txtDeductions.Text = row["Deductions"].ToString();
                txtGrossSalary.Text = row["GrossSalary"].ToString();

                ddlStatus.SelectedValue = Convert.ToBoolean(row["IsActive"]) ? "1" : "0";
            }
        }

        private void ClearForm()
        {
            hfSalaryID.Value = "0";
            ddlEmployee.SelectedIndex = 0;
            txtEffectiveFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtBasic.Text = txtHouseRent.Text = txtUtilities.Text = txtMedical.Text = "0";
            txtFuel.Text = txtTransport.Text = txtMobile.Text = "0";
            txtBonus.Text = txtCommission.Text = txtIncentives.Text = "0";
            txtCustomAllowances.Text = "";
            txtDeductions.Text = "";
            txtGrossSalary.Text = "0.00";
            ddlStatus.SelectedValue = "1";
        }

        private void ShowMessage(string message, string type)
        {
            string script = $@"toastr.{type}('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowToastr", script, true);
        }

        private void ExportToExcel(DataTable dt, string fileName)
        {
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", $"attachment; filename={fileName}");

            using (StringWriter sw = new StringWriter())
            {
                sw.Write("<table border='1'>");

                // Write headers
                sw.Write("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    sw.Write($"<th>{column.ColumnName}</th>");
                }
                sw.Write("</tr>");

                // Write data
                foreach (DataRow row in dt.Rows)
                {
                    sw.Write("<tr>");
                    foreach (object item in row.ItemArray)
                    {
                        sw.Write($"<td>{item}</td>");
                    }
                    sw.Write("</tr>");
                }

                sw.Write("</table>");
                Response.Write(sw.ToString());
            }

            Response.End();
        }

        private void ExportToText(DataTable dt, string fileName)
        {
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.AddHeader("content-disposition", $"attachment; filename={fileName}");

            using (StreamWriter sw = new StreamWriter(Response.OutputStream))
            {
                // Write header
                string header = string.Join(",", dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName));
                sw.WriteLine(header);

                // Write data
                foreach (DataRow row in dt.Rows)
                {
                    string line = string.Join(",", row.ItemArray.Select(item => item.ToString()));
                    sw.WriteLine(line);
                }
            }

            Response.End();
        }

        #endregion

        protected void gvSalary_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}