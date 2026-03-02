using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Shifts
{
    public partial class ShiftRotation : hrms_PakAsia.BasePage
    {
        private int PageSize = 10;
        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }
        LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                LoadDropdowns();
                BindRepeater();
                // landing logged by BasePage.OnLoad
            }
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
            ddlShift.DataSource = CommonDAL.GetShiftTiming();
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
                LogAction("Insert Shift Rotation", newData: $"EmployeeID={empId};ShiftID={shiftId};Date={rotationDate:yyyy-MM-dd}", remarks: "Shift rotation created");
            }
            else
            {
                int rotationId = Convert.ToInt32(hfRotationID.Value);
                ShiftDAL.UpdateRotation(rotationId, empId, shiftId, rotationDate);
                LogAction("Update Shift Rotation", recordId: rotationId.ToString(), newData: $"EmployeeID={empId};ShiftID={shiftId};Date={rotationDate:yyyy-MM-dd}", remarks: "Shift rotation updated");
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
                LogAction("Delete Shift Rotation", recordId: rotationId.ToString(), remarks: "Shift rotation deleted");
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
                    LogAction("Edit Shift Rotation", recordId: rotationId.ToString(), remarks: "Shift rotation loaded for edit");
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
