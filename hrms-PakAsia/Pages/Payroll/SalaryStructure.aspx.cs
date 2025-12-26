using System;
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
                LoadSalaryStructures();
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

        void LoadSalaryStructures()
        {
            var ds = dal.GetSalaryStructuresPaged(
                txtSearch.Text.Trim(),
                PageIndex,
                PageSize
            );

            rptSalary.DataSource = ds.Tables[0];
            rptSalary.DataBind();

            int totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

            lblPageInfo.Text = $"Page {PageIndex} of {totalPages}";

            btnPrev.Enabled = PageIndex > 1;
            btnNext.Enabled = PageIndex < totalPages;
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
            LoadSalaryStructures();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            dal.SaveSalaryStructure(
                Convert.ToInt32(hfSalaryID.Value),
                Convert.ToInt32(ddlEmployee.SelectedValue),
                Convert.ToDateTime(txtEffectiveFrom.Text),
                Convert.ToDecimal(txtBasic.Text),
                Convert.ToDecimal(txtHouseRent.Text),
                Convert.ToDecimal(txtUtilities.Text),
                Convert.ToDecimal(txtMedical.Text),
                Convert.ToDecimal(txtFuel.Text),
                Convert.ToDecimal(txtTransport.Text),
                Convert.ToDecimal(txtMobile.Text),
                Convert.ToDecimal(txtBonus.Text),
                Convert.ToDecimal(txtCommission.Text),
                Convert.ToDecimal(txtIncentives.Text),
                txtCustomAllowances.Text,
                txtDeductions.Text,
                ddlStatus.SelectedValue == "1"
            );

            ClearForm();
            LoadSalaryStructures();
        }

        protected void rptSalary_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                var row = dal.GetSalaryById(Convert.ToInt32(e.CommandArgument));
                if (row == null) return;

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

                ddlStatus.SelectedValue = Convert.ToBoolean(row["IsActive"]) ? "1" : "0";
            }

            if (e.CommandName == "Delete")
            {
                dal.DeleteSalary(Convert.ToInt32(e.CommandArgument));
                LoadSalaryStructures();
            }
            if (e.CommandName == "ExportPDF")
            {
                
                var row = dal.GetSalaryById(Convert.ToInt32(e.CommandArgument));
                byte[] pdfBytes = PayrollPDFHelper.GeneratePayslip(row);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", $"attachment;filename=Payslip_{row["FullName"]}.pdf");
                Response.BinaryWrite(pdfBytes);
                Response.End();
                LoadSalaryStructures();
            }
            if (e.CommandName == "ExportExcel")
            {
                var row = dal.GetSalaryById(Convert.ToInt32(e.CommandArgument));
                byte[] file = PayrollPDFHelper.ExportToExcel(row, "SalaryStructures");

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=SalaryStructures.xlsx");
                Response.BinaryWrite(file);
                Response.End();
                LoadSalaryStructures();
            }
        }

        void ClearForm()
        {
            hfSalaryID.Value = "0";
            ddlEmployee.SelectedIndex = 0;
            txtEffectiveFrom.Text = "";
            txtBasic.Text = txtHouseRent.Text = txtUtilities.Text = txtMedical.Text = "0";
            txtFuel.Text = txtTransport.Text = txtMobile.Text = "0";
            txtBonus.Text = txtCommission.Text = txtIncentives.Text = "0";
            txtCustomAllowances.Text = "";
            txtDeductions.Text = "";
            ddlStatus.SelectedValue = "1";
        }
    }
}
