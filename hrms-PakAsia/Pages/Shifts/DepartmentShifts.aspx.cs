using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Shifts
{
    public partial class DepartmentShifts : System.Web.UI.Page
    {
        LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();
            if (!IsPostBack)
            {
                LoadDepartments();
                LoadShifts();
                LoadDepartmentShifts();
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
        // ================= LOAD DROPDOWNS =================

        private void LoadDepartments()
        {
            ddlDepartment.DataSource = CommonDAL.GetDepartments();
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("Select One", "0"));

        }

        private void LoadShifts()
        {
            ddlShift.DataSource = CommonDAL.GetShiftTiming();
            ddlShift.DataBind();
            ddlShift.Items.Insert(0, new ListItem("Select One", "0"));
        }

        // ================= GRID =================

        private void LoadDepartmentShifts()
        {
            DataTable dt = ShiftDAL.GetDepartmentShifts();
            rptDepartmentShifts.DataSource = dt;
            rptDepartmentShifts.DataBind();
        }

        // ================= SAVE / UPDATE =================

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
            int shiftId = Convert.ToInt32(ddlShift.SelectedValue);
            bool isDefault = chkDefault.Checked;

            if (departmentId == 0 || shiftId == 0)
                return;

            int departmentShiftId = Convert.ToInt32(hfDepartmentShiftID.Value);

            if (departmentShiftId > 0)
            {
                // UPDATE
                ShiftDAL.UpdateDepartmentShift(
                    departmentShiftId,
                    departmentId,
                    shiftId,
                    isDefault
                );
            }
            else
            {
                // INSERT
                ShiftDAL.InsertDepartmentShift(
                    departmentId,
                    shiftId,
                    isDefault
                );
            }

            ClearForm();
            LoadDepartmentShifts();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        // ================= EDIT / DELETE =================

        protected void rptDepartmentShifts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                DataRow dr = ShiftDAL.GetDepartmentShiftById(id);
                if (dr != null)
                {
                    hfDepartmentShiftID.Value = id.ToString();
                    ddlDepartment.SelectedValue = dr["DepartmentID"].ToString();
                    ddlShift.SelectedValue = dr["ShiftID"].ToString();
                    chkDefault.Checked = Convert.ToBoolean(dr["IsDefault"]);

                    btnSave.Text = "Update";
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                ShiftDAL.DeleteDepartmentShift(id);
                LoadDepartmentShifts();
            }
        }

        // ================= HELPERS =================

        private void ClearForm()
        {
            ddlDepartment.SelectedIndex = 0;
            ddlShift.SelectedIndex = 0;
            chkDefault.Checked = false;
            hfDepartmentShiftID.Value = "0";
            btnSave.Text = "Save";
        }
    }
}
