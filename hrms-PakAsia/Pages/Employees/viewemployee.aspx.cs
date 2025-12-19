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
    public partial class viewemployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long empId = Convert.ToInt64(Request.QueryString["id"]);
                LoadEmployee(empId);
            }
        }

        protected void LoadEmployee(long employeeId)
        {
            DataTable dt = EmployeeMaster.GetEmployeeProfile(employeeId);
            if (dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];

            // IMAGE
            imgEmployee.ImageUrl = string.IsNullOrEmpty(r["PhotographPath"].ToString())
                ? "~/assets/img/user.png"
                : r["PhotographPath"].ToString();

            // HEADER
            lblEmpNo.Text = r["EmployeeNo"].ToString();
            lblName.Text = r["FullName"].ToString();
            lblGuardian.Text = r["FatherOrSpouseName"].ToString();
            lblDepartment.Text = r["DepartmentName"].ToString();
            lblDesignation.Text = r["DesignationName"].ToString();

            // BASIC
            lblCNIC.Text = r["CNIC"].ToString();
            lblCNICExpiry.Text = FormatDate(r["CNICExpiryDate"]);
            lblDOB.Text = FormatDate(r["DateOfBirth"]);
            lblGender.Text = r["Gender"].ToString();
            lblMarital.Text = r["MaritalStatus"].ToString();
            lblBlood.Text = r["BloodGroup"].ToString();

            // CONTACT
            lblMobile.Text = r["MobileNumber"].ToString();
            lblAltMobile.Text = r["AlternateMobileNumber"].ToString();
            lblPersonalEmail.Text = r["PersonalEmail"].ToString();
            lblOfficialEmail.Text = r["OfficialEmail"].ToString();
            lblCity.Text = r["City"].ToString();
            lblProvince.Text = r["Province"].ToString();
            lblPermanentAddress.Text = r["PermanentAddress"].ToString();
            lblCurrentAddress.Text = r["CurrentAddress"].ToString();

            // EMPLOYMENT
            lblEmploymentType.Text = r["EmploymentType"].ToString();
            lblEmploymentStatus.Text = r["EmploymentStatus"].ToString();
            lblJoining.Text = FormatDate(r["JoiningDate"]);
            lblConfirmation.Text = FormatDate(r["ConfirmationDate"]);
            lblContractEnd.Text = FormatDate(r["ContractEndDate"]);
            lblLocation.Text = r["WorkLocation"].ToString();

            // ATTENDANCE
            lblShift.Text = r["ShiftID"].ToString();
            lblWeeklyOff.Text = r["WeeklyOffDay"].ToString();
            lblBioID.Text = r["BiometricMachineUserID"].ToString();
            lblAllowedLate.Text = r["AllowedLateCount"].ToString();
            lblAllowedEarly.Text = r["AllowedEarlyLeaveCount"].ToString();
            lblHalfDayHours.Text = r["HalfDayHours"].ToString();

            // PAYROLL
            lblSalaryType.Text = r["SalaryType"].ToString();
            lblBasicSalary.Text = r["BasicSalaryOrDailyWage"].ToString();
            lblGrossSalary.Text = r["GrossSalary"].ToString();
            lblOTRate.Text = r["OvertimeRate"].ToString();
            lblBank.Text = r["BankName"].ToString();
            lblAccount.Text = r["BankAccountOrIBAN"].ToString();
        }

        private string FormatDate(object value)
        {
            if (value == DBNull.Value) return "-";
            return Convert.ToDateTime(value).ToString("dd-MMM-yyyy");
        }

    }
}