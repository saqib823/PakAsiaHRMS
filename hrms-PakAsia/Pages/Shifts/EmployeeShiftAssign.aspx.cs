using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Shifts
{
    public partial class EmployeeShiftAssign : System.Web.UI.Page
    {
        private const int PageSize = 10;

        int PageIndex
        {
            get => ViewState["PageIndex"] == null ? 1 : (int)ViewState["PageIndex"];
            set => ViewState["PageIndex"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
                LoadShifts();
                BindGrid();
            }
        }

        private void LoadEmployees()
        {
            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
        }

        private void LoadShifts()
        {
            ddlShift.DataSource = ShiftDAL.GetShiftTypes();
            ddlShift.DataTextField = "Name";
            ddlShift.DataValueField = "ID";
            ddlShift.DataBind();
            ddlShift.Items.Insert(0, new ListItem("-- Select Shift --", ""));
        }

        private void BindGrid()
        {
            int totalRecords;
            rptShifts.DataSource = ShiftDAL.GetPaged(PageIndex, PageSize, out totalRecords);
            rptShifts.DataBind();

            lblPage.Text = $"Page {PageIndex}";
            btnPrev.Enabled = PageIndex > 1;
            btnNext.Enabled = PageIndex * PageSize < totalRecords;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ShiftDAL.Save(
                Convert.ToInt32(hfShiftID.Value),
                Convert.ToInt32(ddlEmployee.SelectedValue),
                Convert.ToInt32(ddlShift.SelectedValue),
                DateTime.Parse(txtFromDate.Text),
                string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : DateTime.Parse(txtToDate.Text)
            );

            hfShiftID.Value = "0";
            BindGrid();
        }

        protected void rptShifts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Edit")
            {
                var r = ShiftDAL.GetById(id);
                hfShiftID.Value = id.ToString();
                ddlEmployee.SelectedValue = r["EmployeeID"].ToString();
                ddlShift.SelectedValue = r["ShiftID"].ToString();
                txtFromDate.Text = Convert.ToDateTime(r["EffectiveFrom"]).ToString("yyyy-MM-dd");
                txtToDate.Text = r["EffectiveTo"] == DBNull.Value ? "" : Convert.ToDateTime(r["EffectiveTo"]).ToString("yyyy-MM-dd");
            }
            else if (e.CommandName == "Delete")
            {
                ShiftDAL.Delete(id);
                BindGrid();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (PageIndex > 1) PageIndex--;
            BindGrid();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            PageIndex++;
            BindGrid();
        }
    }
}
