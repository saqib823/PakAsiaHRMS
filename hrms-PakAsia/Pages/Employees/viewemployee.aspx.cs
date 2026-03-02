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
    public partial class viewemployee : hrms_PakAsia.BasePage
    {
        LoggedInUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();

            if (!IsPostBack)
            {
                long empId = Convert.ToInt64(Request.QueryString["id"]);
                LoadEmployee(empId);
                LogAction("View Employee Profile", recordId: empId.ToString(), remarks: $"Viewed employee profile #{empId}");
            }
            currentUser = GetSessionData();

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

            BindFile(r["ContractFile"], lblContractFile, btnDownloadContract);
            BindFile(r["CNICFrontFile"], lblCNICFront, btnDownloadCNICFront);
            BindFile(r["CNICBackFile"], lblCNICBack, btnDownloadCNICBack);
            BindFile(r["EducationCertificates"], lblEducation, btnDownloadEducation);
            BindFile(r["ExperienceLetters"], lblExperience, btnDownloadExperience);
            BindFile(r["OtherDocuments"], lblOtherDocs, btnDownloadOtherDocs);

            lblNDAStatus.Text = r["NDASigned"].ToString();
            lblTerms.Text = r["TermsAccepted"].ToString();

            // FIXED: Check for DBNull before conversion
            lblAppointmentIssued.Text = r["AppointmentLetterIssued"] != DBNull.Value
                ? (Convert.ToBoolean(r["AppointmentLetterIssued"]) ? "Issued" : "Not Issued")
                : "Not Issued";

            lblContractStartDate.Text = r["ContractStartDate"] == DBNull.Value
                ? "-"
                : Convert.ToDateTime(r["ContractStartDate"]).ToString("dd-MMM-yyyy");
        }

        private string FormatDate(object value)
        {
            if (value == DBNull.Value) return "-";
            return Convert.ToDateTime(value).ToString("dd-MMM-yyyy");
        }
        private void BindFile(object value, Label lbl, LinkButton btn)
        {
            if (value != DBNull.Value && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                lbl.Text = System.IO.Path.GetFileName(value.ToString());
                btn.Visible = true;
                btn.CommandArgument = value.ToString(); // store ~/Uploads/...
            }
            else
            {
                lbl.Text = "Not Uploaded";
                btn.Visible = false;
            }
        }
        protected void Download_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            // DB value example: ~/Uploads/Education/82_Education_xxx.png
            string virtualPath = btn.CommandArgument;

            string fullPath = Server.MapPath(virtualPath);

            if (System.IO.File.Exists(fullPath))
            {
                LogAction("Download Employee Document", recordId: lblEmpNo.Text, remarks: $"Downloaded file {virtualPath}");
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader(
                    "Content-Disposition",
                    "attachment; filename=" + System.IO.Path.GetFileName(fullPath)
                );
                Response.TransmitFile(fullPath);
                Response.End();
            }
        }



    }
}