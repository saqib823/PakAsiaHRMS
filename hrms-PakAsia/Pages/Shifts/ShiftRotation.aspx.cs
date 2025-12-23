using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Shifts
{
    public partial class ShiftRotation : System.Web.UI.Page
    {
        private int PageSize = 10;
        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
                BindRepeater();
            }
        }

        private void LoadDropdowns()
        {
            // Load Employees
            ddlEmployee.DataSource = CommonDAL.GetEmployees();
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));

            // Load Shifts
            int total;
            ddlShift.DataSource = ShiftDAL.GetShiftTypes();
            ddlShift.DataTextField = "Name";
            ddlShift.DataValueField = "ID";
            ddlShift.DataBind();
            ddlShift.Items.Insert(0, new ListItem("--Select Shift--", "0"));
        }

        private void BindRepeater()
        {
            DataTable dt = ShiftDAL.GetAllRotationsPaged(CurrentPage, PageSize, out int totalRecords);
            rptRotations.DataSource = dt;
            rptRotations.DataBind();

            int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            lblPageInfo.Text = $"Page {CurrentPage} of {totalPages}";

            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int empId = Convert.ToInt32(ddlEmployee.SelectedValue);
            int shiftId = Convert.ToInt32(ddlShift.SelectedValue);
            DateTime rotationDate = Convert.ToDateTime(txtDate.Text);

            if (empId == 0 || shiftId == 0)
                return;

            if (string.IsNullOrEmpty(hfRotationID.Value) || hfRotationID.Value == "0")
            {
                ShiftDAL.InsertRotation(empId, shiftId, rotationDate);
            }
            else
            {
                int rotationId = Convert.ToInt32(hfRotationID.Value);
                ShiftDAL.UpdateRotation(rotationId, empId, shiftId, rotationDate);
            }

            ResetForm();
            BindRepeater();
        }

        protected void rptRotations_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int rotationId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Delete")
            {
                ShiftDAL.DeleteRotation(rotationId);
                BindRepeater();
            }
            else if (e.CommandName == "Edit")
            {
                DataRow dr = ShiftDAL.GetRotationById(rotationId);
                if (dr != null)
                {
                    ddlEmployee.SelectedValue = dr["EmployeeID"].ToString();
                    ddlShift.SelectedValue = dr["ShiftID"].ToString();
                    txtDate.Text = Convert.ToDateTime(dr["RotationDate"]).ToString("yyyy-MM-dd");
                    hfRotationID.Value = rotationId.ToString();
                }
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                BindRepeater();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindRepeater();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            ddlEmployee.SelectedIndex = 0;
            ddlShift.SelectedIndex = 0;
            txtDate.Text = "";
            hfRotationID.Value = "0";
            }
    }
}
