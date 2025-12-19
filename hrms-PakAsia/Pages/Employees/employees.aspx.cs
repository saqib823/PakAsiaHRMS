using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hrms_PakAsia.Pages.Employees
{
    public partial class employees : System.Web.UI.Page
    {
        private LoggedInUser _currentUser;
        private const string UPLOADS_FOLDER = "~/Uploads/";
        private const string ALLOWED_IMAGE_EXTENSIONS = ".jpg,.jpeg,.png";
        private const string ALLOWED_DOCUMENT_EXTENSIONS = ".pdf,.doc,.docx";
        private long _currentEmployeeId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckSession();
                InitializePage();

                // Check if employee ID is passed in query string
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    _currentEmployeeId = Convert.ToInt64(Request.QueryString["id"]);
                    ddlSelectEmployee.SelectedValue = _currentEmployeeId.ToString();
                    LoadEmployee(_currentEmployeeId);
                    ShowAlert($"Employee #{_currentEmployeeId} loaded successfully.", "info");
                }
            }
            else
            {
                CheckSession();
                _currentUser = GetSessionData();

                // Get selected employee from dropdown
                if (!string.IsNullOrEmpty(ddlSelectEmployee.SelectedValue) && ddlSelectEmployee.SelectedValue != "0")
                {
                    _currentEmployeeId = Convert.ToInt64(ddlSelectEmployee.SelectedValue);
                }
            }
        }

        #region Initialization Methods
        private void InitializePage()
        {
            try
            {
                _currentUser = GetSessionData();
                BindEmployeesDropdown();
                BindDropdowns();
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert("Error initializing page. Please try again.", "danger");
            }
        }

        private void BindEmployeesDropdown()
        {
            try
            {
                ddlSelectEmployee.DataSource = CommonDAL.GetEmployees(); 
                ddlSelectEmployee.DataTextField = "Name";
                ddlSelectEmployee.DataValueField = "ID";
                ddlSelectEmployee.DataBind();
                ddlSelectEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to bind employees dropdown");
            }
        }

        private void BindDropdowns()
        {
            var dropdowns = new Dictionary<DropDownList, Func<object>>
            {
                { ddlDepartment, () => CommonDAL.GetDepartments() },
                { ddlBranch, () => CommonDAL.GetBranches() },
                { ddlDesignation, () => CommonDAL.GetDesignation() },
                { ddlReportingManager, () => CommonDAL.GetEmployees() },
                { ddlTitle, () => CommonDAL.GetTitles() },
                { ddlGender, () => CommonDAL.GetGender() },
                { ddlMaritalStatus, () => CommonDAL.GetMaritalStatus() },
                { BloodGroup, () => CommonDAL.GetBloodGroup() },
                { ddlShift, () => CommonDAL.GetShiftTiming() },
                { ddlWorkDays, () => CommonDAL.GetWorkDays() },
                { ddlAttendanceMethod, () => CommonDAL.GetAttendanceMethod() },
                { ddlPaymentMethod, () => CommonDAL.GetPaymentMethod() },
                { PayrollCycle, () => CommonDAL.GetPayrollCycle() },
                { ddlSalaryType, () => CommonDAL.GetPayrollCycle() }
            };

            foreach (var dropdown in dropdowns)
            {
                try
                {
                    dropdown.Key.DataSource = dropdown.Value();
                    dropdown.Key.DataBind();
                    dropdown.Key.Items.Insert(0, new ListItem("Select One", "0"));
                }
                catch (Exception ex)
                {
                    LogError(ex, $"Failed to bind {dropdown.Key.ID}");
                }
            }

            lbWeeklyOff.DataSource = CommonDAL.GetDays();
            lbWeeklyOff.DataBind();
        }
        #endregion

        #region Session Management
        private void CheckSession()
        {
            _currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
            if (_currentUser == null)
            {
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private LoggedInUser GetSessionData() =>
            HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
        #endregion

        #region Employee Selection
        protected void ddlSelectEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectEmployee.SelectedValue != "0")
            {
                _currentEmployeeId = Convert.ToInt64(ddlSelectEmployee.SelectedValue);
                LoadEmployee(_currentEmployeeId);
                ShowAlert($"Employee #{_currentEmployeeId} loaded successfully.", "info");
            }
            else
            {
                ResetForm();
                ShowAlert("Please select an employee to continue.", "warning");
            }
        }
        #endregion

        #region Individual Section Submit Handlers
        // Basic Info Handler
        protected async void btnSubmitBasicInfo_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (!ValidateBasicInfo())
                {
                    ShowAlert("Please fill all required fields in Basic Information.", "warning");
                    return;
                }

                var basicInfo = await SaveEmployeeBasicInfo();
                if (basicInfo > 0)
                {
                    ShowAlert("Basic information saved successfully!", "success");
                    // Refresh employee dropdown to show updated info
                    BindEmployeesDropdown();
                    ddlSelectEmployee.SelectedValue = basicInfo.ToString();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert($"Error saving basic info: {ex.Message}", "danger");
            }
        }

        // Contact Info Handler
        protected async void btnSubmitContactInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEmployeeId == 0)
                {
                    ShowAlert("Please select an employee first.", "warning");
                    return;
                }

                if (!ValidateContactInfo())
                {
                    ShowAlert("Please fill all required fields in Contact Information.", "warning");
                    return;
                }

                await SaveEmployeeContactInfo(_currentEmployeeId);
                ShowAlert("Contact information saved successfully!", "success");
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert($"Error saving contact info: {ex.Message}", "danger");
            }
        }

        // Employment Info Handler
        protected async void btnSubmitEmploymentInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEmployeeId == 0)
                {
                    ShowAlert("Please select an employee first.", "warning");
                    return;
                }

                if (!ValidateEmploymentInfo())
                {
                    ShowAlert("Please fill all required fields in Employment Information.", "warning");
                    return;
                }

                await SaveEmployeeEmploymentInfo(_currentEmployeeId);
                ShowAlert("Employment information saved successfully!", "success");
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert($"Error saving employment info: {ex.Message}", "danger");
            }
        }

        // Attendance Info Handler
        protected async void btnSubmitAttendanceInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEmployeeId == 0)
                {
                    ShowAlert("Please select an employee first.", "warning");
                    return;
                }

                await SaveEmployeeAttendanceInfo(_currentEmployeeId);
                ShowAlert("Attendance information saved successfully!", "success");
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert($"Error saving attendance info: {ex.Message}", "danger");
            }
        }

        // Payroll Info Handler
        protected async void btnSubmitPayrollInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEmployeeId == 0)
                {
                    ShowAlert("Please select an employee first.", "warning");
                    return;
                }

                if (!ValidatePayrollInfo())
                {
                    ShowAlert("Please fill all required fields in Payroll Information.", "warning");
                    return;
                }

                await SaveEmployeePayrollInfo(_currentEmployeeId);
                ShowAlert("Payroll information saved successfully!", "success");
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert($"Error saving payroll info: {ex.Message}", "danger");
            }
        }

        // Legal Info Handler
        protected async void btnSubmitLegalInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEmployeeId == 0)
                {
                    ShowAlert("Please select an employee first.", "warning");
                    return;
                }

                await SaveEmployeeLegalInfo(_currentEmployeeId);
                ShowAlert("Legal documents saved successfully!", "success");
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert($"Error saving legal info: {ex.Message}", "danger");
            }
        }

        // Complete All Info Handler
        protected async void btnSubmitAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                {
                    ShowAlert("Please fill all required fields correctly.", "warning");
                    return;
                }

                if (_currentEmployeeId == 0)
                {
                    // Create new employee
                    var basicInfo = await SaveEmployeeBasicInfo();
                    if (basicInfo <= 0) return;

                    _currentEmployeeId = basicInfo;
                    ddlSelectEmployee.SelectedValue = basicInfo.ToString();
                }

                await Task.WhenAll(
                    SaveEmployeeContactInfo(_currentEmployeeId),
                    SaveEmployeeEmploymentInfo(_currentEmployeeId),
                    SaveEmployeeAttendanceInfo(_currentEmployeeId),
                    SaveEmployeePayrollInfo(_currentEmployeeId),
                    SaveEmployeeLegalInfo(_currentEmployeeId)
                );

                ShowAlert("All employee information saved successfully!", "success");
                BindEmployeesDropdown();
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowAlert($"Error saving employee: {ex.Message}", "danger");
            }
        }
        #endregion

        #region Validation Methods for Individual Sections
        private bool ValidateBasicInfo()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(FullName.Text))
                errors.Add("Full Name is required");

            if (string.IsNullOrWhiteSpace(GuardianName.Text))
                errors.Add("Father/Spouse Name is required");

            if (!IsValidCNIC(txtCNIC.Text))
                errors.Add("Invalid CNIC format (XXXXX-XXXXXXX-X)");

            if (!IsValidDate(txtDOB.Text))
                errors.Add("Invalid Date of Birth");

            if (errors.Any())
            {
                ShowAlert(string.Join("<br/>", errors), "warning");
                return false;
            }

            return true;
        }

        private bool ValidateContactInfo()
        {
            var errors = new List<string>();

            if (!IsValidEmail(txtPersonalEmail.Text))
                errors.Add("Invalid Personal Email");

            if (!IsValidPhone(txtPhone.Text))
                errors.Add("Invalid Phone Number (must start with 03)");

            if (errors.Any())
            {
                ShowAlert(string.Join("<br/>", errors), "warning");
                return false;
            }

            return true;
        }

        private bool ValidateEmploymentInfo()
        {
            var errors = new List<string>();

            if (ddlDepartment.SelectedValue == "0")
                errors.Add("Department is required");

            if (ddlDesignation.SelectedValue == "0")
                errors.Add("Designation is required");

            if (!IsValidDate(txtJoiningDate.Text))
                errors.Add("Invalid Joining Date");

            if (errors.Any())
            {
                ShowAlert(string.Join("<br/>", errors), "warning");
                return false;
            }

            return true;
        }

        private bool ValidatePayrollInfo()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtBasicSalary.Text) || !decimal.TryParse(txtBasicSalary.Text, out _))
                errors.Add("Basic Salary is required and must be a valid number");

            if (ddlPaymentMethod.SelectedValue == "0")
                errors.Add("Payment Method is required");

            if (errors.Any())
            {
                ShowAlert(string.Join("<br/>", errors), "warning");
                return false;
            }

            return true;
        }
        #endregion

        #region Data Saving Methods (Same as before but with employee ID parameter removed where not needed)
        private async Task<long> SaveEmployeeBasicInfo()
        {
            var filePath = await UploadProfilePictureAsync();

            var employee = new EmployeeMaster();
            return await employee.EmployeeBasicInfoAsync(
                txtEmpID.Text.Trim(),
                ddlTitle.SelectedItem?.Text,
                FullName.Text.Trim(),
                GuardianName.Text.Trim(),
                ParseDate(txtDOB.Text),
                ddlGender.SelectedValue,
                FormatCNIC(txtCNIC.Text),
                ParseDate(txtCnicExpiry.Text),
                ddlMaritalStatus.SelectedValue,
                txtNationality.Text.Trim(),
                txtReligion.Text.Trim(),
                filePath,
                BloodGroup.SelectedValue,
                _currentUser?.UserID.ToString()
            );
        }

        private async Task SaveEmployeeContactInfo(long employeeId)
        {
            var employee = new EmployeeMaster();
            await employee.EmployeeContactsAsync(
                employeeId.ToString(),
                txtPersonalEmail.Text.Trim(),
                txtOfficialEmail.Text.Trim(),
                txtPhone.Text.Trim(),
                txtAlternatePhone.Text.Trim(),
                txtEmergencyContact.Text.Trim(),
                txtEmergencyName.Text.Trim(),
                txtEmergencyRelationship.Text.Trim(),
                txtPermAddress1.Text.Trim(),
                txtPermAddress2.Text.Trim(),
                txtPermCity.Text.Trim(),
                txtPermState.Text.Trim(),
                txtPermPostalCode.Text.Trim(),
                txtCurrentAddress1.Text.Trim(),
                txtCurrentAddress2.Text.Trim(),
                txtCurrentState.Text.Trim()
            );
        }

        private async Task SaveEmployeeEmploymentInfo(long employeeId)
        {
            var employee = new EmployeeMaster();
            await employee.EmployeeEmploymentAsync(
                employeeId,
                ParseInt(ddlDepartment.SelectedValue),
                ParseInt(ddlDesignation.SelectedValue),
                ParseInt(ddlReportingManager.SelectedValue),
                ddlEmploymentType.SelectedValue,
                ddlEmpStatus.SelectedValue,
                ParseDate(txtJoiningDate.Text),
                ParseNullableDate(txtConfirmationDate.Text),
                ParseNullableDate(txtContractEndDate.Text),
                ParseNullableDate(txtProbationEndDate.Text),
                txtWorkLocation.Text.Trim(),
                txtJobDescription.Text.Trim()
            );
            LeaveDAL.GenerateYearlyLeaveBalance(Convert.ToInt32(employeeId), DateTime.Now.Year);
        }

        private async Task SaveEmployeeAttendanceInfo(long employeeId)
        {
            var weeklyOffDays = lbWeeklyOff.Items.Cast<ListItem>()
                .Where(i => i.Selected)
                .Select(i => i.Value)
                .ToList();

            var employee = new EmployeeMaster();
            await employee.EmployeeAttendanceAsync(
                employeeId,
                ParseInt(ddlShift.SelectedValue),
                weeklyOffDays,
                ParseInt(ddlAttendanceMethod.SelectedValue),
                ParseInt(txtAllowedLate.Text),
                ParseInt(txtAllowedEarlyLeaveCont.Text),
                txtBiometricID.Text.Trim()
            );
        }

        private async Task SaveEmployeePayrollInfo(long employeeId)
        {
            var basicSalary = ParseDecimal(txtBasicSalary.Text);
            var houseRent = ParseDecimal(txtHouseRent.Text);
            var medicalAllowance = ParseDecimal(txtMedicalAllowance.Text);
            var transportAllowance = ParseDecimal(txtTransportAllowance.Text);
            var otherAllowances = ParseDecimal(txtOtherAllowances.Text);
            var overtimeRate = ParseDecimal(OvertimeRate.Text);

            var grossSalary = basicSalary + houseRent + medicalAllowance +
                            transportAllowance + otherAllowances;
            txtGrossSalary.Text = grossSalary.ToString("N2");

            var employee = new EmployeeMaster();
            await employee.EmployeePayrollAsync(
                employeeId,
                ParseInt(ddlSalaryType.SelectedValue),
                basicSalary, houseRent, medicalAllowance,
                transportAllowance, otherAllowances, grossSalary,
                overtimeRate, ParseInt(PayrollCycle.SelectedValue),
                ddlPaymentMethod.SelectedValue, txtBankName.Text.Trim(),
                txtSalaryAccount.Text.Trim(), TaxDeduction.Text.Trim(),
                EOBIRegistered.Text.Trim(), SocialSecurity.Text.Trim()
            );
        }

        private async Task SaveEmployeeLegalInfo(long employeeId)
        {
            string legalFolder = Server.MapPath($"{UPLOADS_FOLDER}Legal/");
            string educationFolder = Server.MapPath($"{UPLOADS_FOLDER}Education/");

            EnsureDirectoryExists(legalFolder);
            EnsureDirectoryExists(educationFolder);

            var contractPath = await SaveFileAsync(fileContract, legalFolder, $"{employeeId}_Contract");
            var cnicFrontPath = await SaveFileAsync(fileCNICFront, legalFolder, $"{employeeId}_CNICFront");
            var cnicBackPath = await SaveFileAsync(fileCNICBack, legalFolder, $"{employeeId}_CNICBack");

            var educationPaths = await SaveMultipleFilesAsync(fileEducation, educationFolder, employeeId.ToString(), "Education");
            var experiencePaths = await SaveMultipleFilesAsync(fileExperience, educationFolder, employeeId.ToString(), "Experience");
            var otherDocsPaths = await SaveMultipleFilesAsync(fileOtherDocs, educationFolder, employeeId.ToString(), "OtherDocs");

            var employee = new EmployeeMaster();
            await employee.EmployeeLegalAsync(
                employeeId,
                contractPath, cnicFrontPath, cnicBackPath,
                educationPaths, experiencePaths, otherDocsPaths,
                ddlNDA.SelectedValue, ddlTerms.SelectedValue,
                false, null
            );
        }
        #endregion

        #region File Upload Methods (Same as before)
        private async Task<string> UploadProfilePictureAsync()
        {
            if (fileProfilePic.PostedFile == null || fileProfilePic.PostedFile.ContentLength <= 0)
                return string.Empty;

            var file = fileProfilePic.PostedFile;
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!ALLOWED_IMAGE_EXTENSIONS.Split(',').Contains(ext))
                throw new InvalidOperationException("Only JPG, JPEG, and PNG images are allowed");

            var uploadFolder = Server.MapPath($"{UPLOADS_FOLDER}UserImages/");
            EnsureDirectoryExists(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadFolder, fileName);

            await Task.Run(() => file.SaveAs(fullPath));
            return $"{UPLOADS_FOLDER}UserImages/{fileName}";
        }

        private async Task<string> SaveFileAsync(FileUpload fileControl, string uploadFolder, string baseName)
        {
            if (!fileControl.HasFile || fileControl.PostedFile.ContentLength <= 0)
                return null;

            var file = fileControl.PostedFile;
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!IsValidExtension(ext))
                throw new InvalidOperationException("Invalid file extension");

            var fileName = $"{baseName}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
            var fullPath = Path.Combine(uploadFolder, fileName);

            await Task.Run(() => file.SaveAs(fullPath));
            return $"{UPLOADS_FOLDER}Legal/{fileName}";
        }

        private async Task<List<string>> SaveMultipleFilesAsync(FileUpload fileControl, string uploadFolder,
            string employeeId, string fileType)
        {
            var savedFiles = new List<string>();

            if (!fileControl.HasFiles) return savedFiles;

            var tasks = fileControl.PostedFiles.Cast<HttpPostedFile>()
                .Select(async file =>
                {
                    var ext = Path.GetExtension(file.FileName).ToLower();
                    if (!IsValidExtension(ext)) return null;

                    var fileName = $"{employeeId}_{fileType}_{Guid.NewGuid():N}{ext}";
                    var fullPath = Path.Combine(uploadFolder, fileName);

                    await Task.Run(() => file.SaveAs(fullPath));
                    return $"{UPLOADS_FOLDER}Education/{fileName}";
                });

            var results = await Task.WhenAll(tasks);
            return results.Where(r => r != null).ToList();
        }

        private bool IsValidExtension(string extension) =>
            (ALLOWED_IMAGE_EXTENSIONS + "," + ALLOWED_DOCUMENT_EXTENSIONS)
            .Split(',')
            .Contains(extension.ToLower());
        #endregion

        #region Utility Methods (Same as before)
        private DateTime ParseDate(string dateString) =>
            DateTime.TryParse(dateString, out var date) ? date : DateTime.MinValue;

        private DateTime? ParseNullableDate(string dateString) =>
            DateTime.TryParse(dateString, out var date) ? date : (DateTime?)null;

        private decimal ParseDecimal(string value) =>
            decimal.TryParse(value, out var result) ? result : 0;

        private int ParseInt(string value) =>
            int.TryParse(value, out var result) ? result : 0;

        private string FormatCNIC(string cnic) =>
            cnic?.Replace("-", "")?.Trim();

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private bool IsValidCNIC(string cnic) =>
            Regex.IsMatch(cnic?.Trim() ?? "", @"^\d{5}-\d{7}-\d{1}$");

        private bool IsValidEmail(string email) =>
            Regex.IsMatch(email?.Trim() ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

        private bool IsValidPhone(string phone) =>
            Regex.IsMatch(phone?.Trim() ?? "", @"^03\d{9}$");

        private bool IsValidDate(string date) =>
            DateTime.TryParse(date, out _);

        private void ResetForm()
        {
            var controls = new Control[] {
                txtEmpID, FullName, GuardianName, txtDOB, txtCNIC, txtCnicExpiry,
                txtNationality, txtReligion, txtPersonalEmail, txtOfficialEmail,
                txtPhone, txtAlternatePhone, txtEmergencyContact, txtEmergencyName,
                txtEmergencyRelationship, txtPermAddress1, txtPermAddress2, txtPermCity,
                txtPermState, txtPermPostalCode, txtCurrentAddress1, txtCurrentAddress2,
                txtCurrentState, txtCurrentCity, txtCurrentPostalCode, txtWorkLocation,
                txtJobDescription, txtBasicSalary, txtHouseRent, txtMedicalAllowance,
                txtTransportAllowance, txtOtherAllowances, OvertimeRate, txtBankName,
                txtSalaryAccount, TaxDeduction, EOBIRegistered, SocialSecurity,
                txtBiometricID, txtAllowedLate, txtAllowedEarlyLeaveCont, txtHalfDayHours
            };

            foreach (var control in controls)
            {
                if (control is TextBox textBox)
                    textBox.Text = string.Empty;
            }

            var dropdowns = new DropDownList[] {
                ddlTitle, ddlGender, ddlMaritalStatus, BloodGroup, ddlDepartment,
                ddlDesignation, ddlBranch, ddlReportingManager, ddlEmploymentType,
                ddlEmpStatus, ddlShift, ddlWorkDays, ddlAttendanceMethod,
                ddlPaymentMethod, PayrollCycle, ddlSalaryType, ddlNDA, ddlTerms
            };

            foreach (var dropdown in dropdowns)
                dropdown.SelectedIndex = 0;

            lbWeeklyOff.ClearSelection();
        }

        private bool ValidateForm()
        {
            return ValidateBasicInfo() && ValidateContactInfo() &&
                   ValidateEmploymentInfo() && ValidatePayrollInfo();
        }
        #endregion

        #region UI Methods (Same as before)
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

        private void LogError(Exception ex, string context = null)
        {
            // Implement logging here
        }

        protected void LoadEmployee(long employeeId)
        {
            DataTable dt = EmployeeMaster.GetEmployeeProfile(employeeId);
            if (dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];

            /* ================= BASIC INFORMATION ================= */
            txtEmpID.Text = r["EmployeeNo"].ToString();
            FullName.Text = r["FullName"].ToString();
            GuardianName.Text = r["FatherOrSpouseName"].ToString();

            SetDropDownValue(ddlGender, r["GenderID"]);
            SetDropDownValue(ddlMaritalStatus, r["MaritalStatusID"]);
            SetDropDownValue(BloodGroup, r["BloodGroupID"]);

            txtCNIC.Text = r["CNIC"].ToString();
            SetDate(txtCnicExpiry, r["CNICExpiryDate"]);
            SetDate(txtDOB, r["DateOfBirth"]);

            /* ================= CONTACT ================= */
            txtPersonalEmail.Text = r["PersonalEmail"].ToString();
            txtOfficialEmail.Text = r["OfficialEmail"].ToString();
            txtPhone.Text = r["MobileNumber"].ToString();
            txtAlternatePhone.Text = r["AlternateMobileNumber"].ToString();

            txtEmergencyContact.Text = r["EmergencyContactNumber"].ToString();
            txtEmergencyName.Text = r["EmergencyContactName"].ToString();
            txtEmergencyRelationship.Text = r["EmergencyContactRelation"].ToString();

            /* ================= ADDRESSES ================= */
            txtPermAddress1.Text = r["PermanentAddress"].ToString();
            txtPermAddress2.Text = r["PermanentAddress"].ToString();
            txtPermCity.Text = r["City"].ToString();
            txtPermState.Text = r["Province"].ToString();
            txtPermPostalCode.Text = r["PostalCode"].ToString();

            txtCurrentAddress1.Text = r["CurrentAddress"].ToString();
            txtCurrentAddress2.Text = r["CurrentAddress"].ToString();
            txtCurrentCity.Text = r["City"].ToString();
            txtCurrentState.Text = r["Province"].ToString();
            txtCurrentPostalCode.Text = r["PostalCode"].ToString();

            /* ================= EMPLOYMENT ================= */
            SetDropDownValue(ddlDepartment, r["DepartmentID"]);
            SetDropDownValue(ddlDesignation, r["DesignationID"]);
            SetDropDownValue(ddlReportingManager, r["ReportingManagerID"]);

            ddlEmploymentType.SelectedValue = r["EmploymentType"].ToString();
            ddlEmpStatus.SelectedValue = r["EmploymentStatus"].ToString();

            SetDate(txtJoiningDate, r["JoiningDate"]);
            SetDate(txtConfirmationDate, r["ConfirmationDate"]);
            SetDate(txtContractEndDate, r["ContractEndDate"]);
            SetDate(txtProbationEndDate, r["ProbationEndDate"]);

            txtWorkLocation.Text = r["WorkLocation"].ToString();
            txtJobDescription.Text = r["EmployeeCategory"].ToString();

            /* ================= ATTENDANCE ================= */
            SetDropDownValue(ddlShift, r["Shift"]);
            SetDropDownValue(ddlWorkDays, r["WorkDays"]);
            SetDropDownValue(ddlAttendanceMethod, r["AttendancePolicyID"]);

            txtBiometricID.Text = r["BiometricMachineUserID"].ToString();
            SetMultiSelect(lbWeeklyOff, r["WeeklyOffDayID"].ToString());

            txtAllowedLate.Text = r["AllowedLateCount"].ToString();
            txtAllowedEarlyLeaveCont.Text = r["AllowedEarlyLeaveCount"].ToString();
            txtHalfDayHours.Text = r["HalfDayHours"].ToString();

            /* ================= PAYROLL ================= */
            txtBasicSalary.Text = r["BasicSalaryOrDailyWage"].ToString();
            txtHouseRent.Text = r["HouseRent"].ToString();
            txtMedicalAllowance.Text = r["MedicalAllowance"].ToString();
            txtTransportAllowance.Text = r["TransportAllowance"].ToString();
            txtOtherAllowances.Text = r["OtherAllowances"].ToString();

            txtBankName.Text = r["BankName"].ToString();
            txtSalaryAccount.Text = r["BankAccountOrIBAN"].ToString();

            txtGrossSalary.Text = r["GrossSalary"].ToString();
            OvertimeRate.Text = r["OvertimeRate"].ToString();

            SetDropDownValue(ddlPaymentMethod, r["SalaryPaymentMethod"]);
            SetDropDownValue(PayrollCycle, r["PayrollCycle"]);
            SetDropDownValue(ddlSalaryType, r["SalaryTypeID"]);

            TaxDeduction.Text = r["TaxStatusOrNTN"].ToString();
            EOBIRegistered.Text = r["EOBINumber"].ToString();
            SocialSecurity.Text = r["SocialSecurityNumber"].ToString();
        }

        private void SetDropDownValue(DropDownList ddl, object value)
        {
            if (value == null) return;
            string v = value.ToString();
            if (ddl.Items.FindByValue(v) != null)
                ddl.SelectedValue = v;
        }

        private void SetDate(TextBox txt, object value)
        {
            if (value == DBNull.Value) return;
            DateTime dt;
            if (DateTime.TryParse(value.ToString(), out dt))
                txt.Text = dt.ToString("yyyy-MM-dd");
        }

        private void SetMultiSelect(ListBox lb, string csv)
        {
            if (string.IsNullOrEmpty(csv)) return;

            var values = csv.Split(',');
            foreach (ListItem item in lb.Items)
                item.Selected = values.Contains(item.Value);
        }
        #endregion

        #region Event Handlers
        protected void btnSaveDraft_Click(object sender, EventArgs e) { }
        protected void cbSameAsPermanent_CheckedChanged(object sender, EventArgs e) { }
        protected void btnCancel_Click(object sender, EventArgs e) => ResetForm();
        protected void txtSearchEmployee_TextChanged(object sender, EventArgs e) { }
        protected void rptEmployees_ItemCommand(object source, RepeaterCommandEventArgs e) { }
        protected void btnPrev_Click(object sender, EventArgs e) { }
        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e) { }
        protected void btnNext_Click(object sender, EventArgs e) { }
        #endregion
    }
}