<%@ Page Async="true" Title="Employee Management" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="employees.aspx.cs" Inherits="hrms_PakAsia.Pages.Employees.employees" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upEmployee" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <main class="main mt-10" id="top">
                <div class="container">
                    <div class="row py-5">
                        
                        <!-- Page Heading -->
                        <div class="text-center mb-4">
                            <h3 class="mt-3">Employee Management</h3>
                            <p class="text-body-tertiary">Complete employee information</p>
                        </div>

                        <!-- Form Sections with Accordion -->
                        <div class="accordion" id="employeeAccordion">
                            
                            <!-- 1️⃣ Basic Info Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#basicInfo">
                                        <i class="uil uil-user-circle me-2"></i>Basic Information
                                    </button>
                                </h2>
                                <div id="basicInfo" class="accordion-collapse collapse show" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <div class="row">
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Employee ID</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmpID" runat="server" placeholder="EMP-001" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Title</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlTitle" runat="server">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                    <asp:ListItem>Mr.</asp:ListItem>
                                                    <asp:ListItem>Ms.</asp:ListItem>
                                                    <asp:ListItem>Mrs.</asp:ListItem>
                                                    <asp:ListItem>Dr.</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Full Name *</label>
                                                <asp:TextBox CssClass="form-control" ID="txtFirstName" runat="server" placeholder="First name" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Father or Spouse Name *</label>
                                                <asp:TextBox CssClass="form-control" ID="txtLastName" runat="server" placeholder="Last name" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Date of Birth</label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtDOB" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Gender</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlGender" runat="server">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                    <asp:ListItem>Male</asp:ListItem>
                                                    <asp:ListItem>Female</asp:ListItem>
                                                    <asp:ListItem>Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">CNIC *</label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtCNIC" runat="server" MaxLength="15" placeholder="12345-1234567-1" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">CNIC Expiry *</label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtCnicExpiry" runat="server" MaxLength="15" required="true" TextMode="DateTimeLocal"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Marital Status</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlMaritalStatus" runat="server">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                    <asp:ListItem>Single</asp:ListItem>
                                                    <asp:ListItem>Married</asp:ListItem>
                                                    <asp:ListItem>Divorced</asp:ListItem>
                                                    <asp:ListItem>Widowed</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Nationality</label>
                                                <asp:TextBox CssClass="form-control" ID="txtNationality" runat="server" placeholder="Pakistani"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Religion</label>
                                                <asp:TextBox CssClass="form-control" ID="txtReligion" runat="server" placeholder="Religion"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label" for="fileProfilePic">Profile Picture</label>
                                                <input class="form-control" id="fileProfilePic" type="file" runat="server" accept="image/*" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 2️⃣ Contact & Address Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#contactAddress">
                                        <i class="uil uil-phone me-2"></i>Contact & Address
                                    </button>
                                </h2>
                                <div id="contactAddress" class="accordion-collapse collapse" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <h5 class="mb-3">Contact Information</h5>
                                        <div class="row">
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Personal Email *</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPersonalEmail" runat="server" TextMode="Email" placeholder="personal@example.com" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Official Email</label>
                                                <asp:TextBox CssClass="form-control" ID="txtOfficialEmail" runat="server" TextMode="Email" placeholder="official@company.com"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Phone Number *</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPhone" runat="server" MaxLength="11" placeholder="03XX-XXXXXXX" required="true"></asp:TextBox>
                                            </div> 
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Alternate Phone Number *</label>
                                                <asp:TextBox CssClass="form-control" ID="txtAlternatePhone" runat="server" MaxLength="11" placeholder="03XX-XXXXXXX" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Emergency Contact</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmergencyContact" runat="server" placeholder="Emergency number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Emergency Contact Name</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmergencyName" runat="server" placeholder="Contact person name"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Relationship</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmergencyRelationship" runat="server" placeholder="Father/Mother/Spouse"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <h5 class="mt-4 mb-3">Permanent Address</h5>
                                        <div class="row">
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Address Line 1</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermAddress1" runat="server" placeholder="House #, Street"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Address Line 2</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermAddress2" runat="server" placeholder="Sector, Area"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">City</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermCity" runat="server" placeholder="City"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">State/Province</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermState" runat="server" placeholder="Province"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Postal Code</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermPostalCode" runat="server" placeholder="Postal code"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <div class="form-check mt-3">
                                            <asp:CheckBox ID="cbSameAsPermanent" runat="server" CssClass="form-check-input" AutoPostBack="true" OnCheckedChanged="cbSameAsPermanent_CheckedChanged" />
                                            <label class="form-check-label" for="<%= cbSameAsPermanent.ClientID %>">
                                                Same as permanent address
                                            </label>
                                        </div>
                                        
                                        <h5 class="mt-4 mb-3">Current Address</h5>
                                        <div class="row">
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Address Line 1</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentAddress1" runat="server" placeholder="House #, Street"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Address Line 2</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentAddress2" runat="server" placeholder="Sector, Area"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">City</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentCity" runat="server" placeholder="City"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">State/Province</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentState" runat="server" placeholder="Province"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Postal Code</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentPostalCode" runat="server" placeholder="Postal code"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 3️⃣ Employment Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#employment">
                                        <i class="uil uil-briefcase me-2"></i>Employment
                                    </button>
                                </h2>
                                <div id="employment" class="accordion-collapse collapse" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <div class="row">
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Department *</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlDepartment" DataTextField="Name" DataValueField="ID" runat="server" required="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Designation *</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlDesignation" DataTextField="Name" DataValueField="ID" runat="server" required="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Branch *</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlBranch" DataTextField="Name" DataValueField="ID" runat="server" required="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Reporting Manager</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlReportingManager" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Employment Type</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlEmploymentType" runat="server">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                    <asp:ListItem>Permanent</asp:ListItem>
                                                    <asp:ListItem>Contract</asp:ListItem>
                                                    <asp:ListItem>Probation</asp:ListItem>
                                                    <asp:ListItem>Intern</asp:ListItem>
                                                    <asp:ListItem>Part-time</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Joining Date *</label>
                                                <asp:TextBox CssClass="form-control" ID="txtJoiningDate" runat="server" TextMode="Date" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Confirmation Date</label>
                                                <asp:TextBox CssClass="form-control" ID="txtConfirmationDate" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Contract End Date</label>
                                                <asp:TextBox CssClass="form-control" ID="txtContractEndDate" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Employee Status</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlEmpStatus" runat="server">
                                                    <asp:ListItem Value="Active" Selected="True">Active</asp:ListItem>
                                                    <asp:ListItem>Inactive</asp:ListItem>
                                                    <asp:ListItem>Suspended</asp:ListItem>
                                                    <asp:ListItem>Terminated</asp:ListItem>
                                                    <asp:ListItem>Resigned</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Work Location</label>
                                                <asp:TextBox CssClass="form-control" ID="txtWorkLocation" runat="server" placeholder="Office location"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Probation End</label>
                                                <asp:TextBox CssClass="form-control" ID="txtProbationEndDate" runat="server" TextMode="DateTimeLocal" Rows="2" ></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Employee Category</label>
                                                <asp:TextBox CssClass="form-control" ID="txtJobDescription" runat="server" placeholder="Enter Employee Category"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 4️⃣ Attendance Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#attendance">
                                        <i class="uil uil-clock me-2"></i>Attendance
                                    </button>
                                </h2>
                                <div id="attendance" class="accordion-collapse collapse" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <div class="row">
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Shift Timing</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlShift" runat="server">
                                                    <asp:ListItem Value="">Select Shift</asp:ListItem>
                                                    <asp:ListItem>Morning (9AM-5PM)</asp:ListItem>
                                                    <asp:ListItem>Evening (2PM-10PM)</asp:ListItem>
                                                    <asp:ListItem>Night (10PM-6AM)</asp:ListItem>
                                                    <asp:ListItem>Flexible</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Work Days</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlWorkDays" runat="server">
                                                    <asp:ListItem Value="5">5 Days (Mon-Fri)</asp:ListItem>
                                                    <asp:ListItem Value="6">6 Days (Mon-Sat)</asp:ListItem>
                                                    <asp:ListItem Value="7">7 Days (All week)</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Attendance Method</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlAttendanceMethod" runat="server">
                                                    <asp:ListItem Value="">Select Method</asp:ListItem>
                                                    <asp:ListItem>Biometric</asp:ListItem>
                                                    <asp:ListItem>Mobile App</asp:ListItem>
                                                    <asp:ListItem>Manual</asp:ListItem>
                                                    <asp:ListItem>RFID Card</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Biometric ID</label>
                                                <asp:TextBox CssClass="form-control" ID="txtBiometricID" runat="server" placeholder="Biometric ID"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Weekly Off Days</label>
                                                <asp:ListBox CssClass="form-control" ID="lbWeeklyOff" runat="server" SelectionMode="Multiple">
                                                    <asp:ListItem>Monday</asp:ListItem>
                                                    <asp:ListItem>Tuesday</asp:ListItem>
                                                    <asp:ListItem>Wednesday</asp:ListItem>
                                                    <asp:ListItem>Thursday</asp:ListItem>
                                                    <asp:ListItem>Friday</asp:ListItem>
                                                    <asp:ListItem>Saturday</asp:ListItem>
                                                    <asp:ListItem>Sunday</asp:ListItem>
                                                </asp:ListBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Allowed Late Minutes</label>
                                                <asp:TextBox CssClass="form-control" ID="txtAllowedLate" runat="server" TextMode="Number" placeholder="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Half Day Hours</label>
                                                <asp:TextBox CssClass="form-control" ID="txtHalfDayHours" runat="server" TextMode="Number" placeholder="4"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 5️⃣ Payroll Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#payroll">
                                        <i class="uil uil-money-bill me-2"></i>Payroll
                                    </button>
                                </h2>
                                <div id="payroll" class="accordion-collapse collapse" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <div class="row">
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Basic Salary *</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtBasicSalary" runat="server" TextMode="Number" step="0.01" placeholder="0.00" required="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">House Rent</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtHouseRent" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Medical Allowance</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtMedicalAllowance" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Transport Allowance</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtTransportAllowance" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Other Allowances</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtOtherAllowances" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Salary Account No.</label>
                                                <asp:TextBox CssClass="form-control" ID="txtSalaryAccount" runat="server" placeholder="Bank account number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Bank Name</label>
                                                <asp:TextBox CssClass="form-control" ID="txtBankName" runat="server" placeholder="Bank name"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Payment Method</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlPaymentMethod" runat="server">
                                                    <asp:ListItem Value="">Select Method</asp:ListItem>
                                                    <asp:ListItem>Bank Transfer</asp:ListItem>
                                                    <asp:ListItem>Cash</asp:ListItem>
                                                    <asp:ListItem>Cheque</asp:ListItem>
                                                    <asp:ListItem>Mobile Wallet</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Tax Deduction</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlTaxDeduction" runat="server">
                                                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">EOBI Registered</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlEOBIRegistered" runat="server">
                                                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Social Security</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlSocialSecurity" runat="server">
                                                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 6️⃣ Legal Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#legal">
                                        <i class="uil uil-file-contract me-2"></i>Legal
                                    </button>
                                </h2>
                                <div id="legal" class="accordion-collapse collapse" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <div class="row">
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Contract File</label>
                                                <input class="form-control" id="fileContract" type="file" runat="server" accept=".pdf,.doc,.docx" />
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">CNIC Front</label>
                                                <input class="form-control" id="fileCNICFront" type="file" runat="server" accept="image/*,.pdf" />
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">CNIC Back</label>
                                                <input class="form-control" id="fileCNICBack" type="file" runat="server" accept="image/*,.pdf" />
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Educational Certificates</label>
                                                <input class="form-control" id="fileEducation" type="file" runat="server" accept=".pdf,.jpg,.jpeg,.png" multiple="multiple" />
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Experience Letters</label>
                                                <input class="form-control" id="fileExperience" type="file" runat="server" accept=".pdf,.jpg,.jpeg,.png" multiple="multiple" />
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Other Documents</label>
                                                <input class="form-control" id="fileOtherDocs" type="file" runat="server" accept=".pdf,.jpg,.jpeg,.png,.doc,.docx" multiple="multiple" />
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Non-Disclosure Agreement</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlNDA" runat="server">
                                                    <asp:ListItem Value="">Select Status</asp:ListItem>
                                                    <asp:ListItem>Signed</asp:ListItem>
                                                    <asp:ListItem>Pending</asp:ListItem>
                                                    <asp:ListItem>Not Required</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Terms & Conditions</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlTerms" runat="server">
                                                    <asp:ListItem Value="">Select Status</asp:ListItem>
                                                    <asp:ListItem>Accepted</asp:ListItem>
                                                    <asp:ListItem>Pending</asp:ListItem>
                                                    <asp:ListItem>Declined</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 7️⃣ System Access Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#systemAccess">
                                        <i class="uil uil-lock-access me-2"></i>System Access
                                    </button>
                                </h2>
                                <div id="systemAccess" class="accordion-collapse collapse" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <div class="row">
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">System Username</label>
                                                <asp:TextBox CssClass="form-control" ID="txtSystemUsername" runat="server" placeholder="Username for login"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Initial Password</label>
                                                <asp:TextBox CssClass="form-control" ID="txtInitialPassword" runat="server" TextMode="Password" placeholder="Temporary password"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">User Role *</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlUserRole" DataTextField="Name" DataValueField="ID" runat="server" required="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Access Level</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlAccessLevel" runat="server">
                                                    <asp:ListItem Value="Full">Full Access</asp:ListItem>
                                                    <asp:ListItem Value="Limited">Limited Access</asp:ListItem>
                                                    <asp:ListItem Value="ReadOnly">Read Only</asp:ListItem>
                                                    <asp:ListItem Value="NoAccess">No Access</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-12 mt-3">
                                                <label class="form-label">System Permissions</label>
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="cbHRModule" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%= cbHRModule.ClientID %>">HR Module</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="cbPayrollModule" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%= cbPayrollModule.ClientID %>">Payroll Module</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="cbAttendanceModule" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%= cbAttendanceModule.ClientID %>">Attendance Module</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="cbLeaveModule" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%= cbLeaveModule.ClientID %>">Leave Module</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="cbReportsModule" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%= cbReportsModule.ClientID %>">Reports Module</label>
                                                </div>
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Email Access</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlEmailAccess" runat="server">
                                                    <asp:ListItem Value="">Select Option</asp:ListItem>
                                                    <asp:ListItem>Company Email Created</asp:ListItem>
                                                    <asp:ListItem>Personal Email Only</asp:ListItem>
                                                    <asp:ListItem>No Email Required</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">System Access Status</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlSystemAccessStatus" runat="server">
                                                    <asp:ListItem Value="Active" Selected="True">Active</asp:ListItem>
                                                    <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                                                    <asp:ListItem Value="Suspended">Suspended</asp:ListItem>
                                                    <asp:ListItem Value="Pending">Pending Activation</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 8️⃣ Performance & Notes Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#performance">
                                        <i class="uil uil-chart-line me-2"></i>Performance & Notes
                                    </button>
                                </h2>
                                <div id="performance" class="accordion-collapse collapse" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body">
                                        <div class="row">
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Key Performance Indicators</label>
                                                <asp:TextBox CssClass="form-control" ID="txtKPI" runat="server" TextMode="MultiLine" Rows="3" placeholder="Main KPIs"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Performance Rating</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlPerformanceRating" runat="server">
                                                    <asp:ListItem Value="">Select Rating</asp:ListItem>
                                                    <asp:ListItem>Excellent (5)</asp:ListItem>
                                                    <asp:ListItem>Good (4)</asp:ListItem>
                                                    <asp:ListItem>Average (3)</asp:ListItem>
                                                    <asp:ListItem>Below Average (2)</asp:ListItem>
                                                    <asp:ListItem>Poor (1)</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Last Appraisal Date</label>
                                                <asp:TextBox CssClass="form-control" ID="txtLastAppraisal" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Strengths</label>
                                                <asp:TextBox CssClass="form-control" ID="txtStrengths" runat="server" TextMode="MultiLine" Rows="2" placeholder="Employee strengths"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 mt-3">
                                                <label class="form-label">Areas for Improvement</label>
                                                <asp:TextBox CssClass="form-control" ID="txtImprovementAreas" runat="server" TextMode="MultiLine" Rows="2" placeholder="Areas needing improvement"></asp:TextBox>
                                            </div>
                                            <div class="col-md-12 mt-3">
                                                <label class="form-label">Training & Development</label>
                                                <asp:TextBox CssClass="form-control" ID="txtTraining" runat="server" TextMode="MultiLine" Rows="3" placeholder="Training attended/required"></asp:TextBox>
                                            </div>
                                            <div class="col-md-12 mt-3">
                                                <label class="form-label">Notes & Remarks</label>
                                                <asp:TextBox CssClass="form-control" ID="txtNotes" runat="server" TextMode="MultiLine" Rows="4" placeholder="Additional notes, remarks, or comments"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- End Accordion -->

                        <!-- Action Buttons -->
                        <div class="text-end mt-4">
                            <asp:Button ID="btnCancel" runat="server"
                                Text="Cancel" CssClass="btn btn-secondary me-2 mb-3" 
                                OnClick="btnCancel_Click" UseSubmitBehavior="false" />

                            <asp:Button ID="btnSaveDraft" runat="server"
                                Text="Save as Draft" CssClass="btn btn-warning me-2 mb-3"
                                OnClick="btnSaveDraft_Click" />

                            <asp:Button ID="btnSubmit" runat="server"
                                Text="Submit Employee" CssClass="btn btn-primary mb-3"
                                OnClick="btnSubmit_Click" />
                        </div>

                        <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

                        <!-- Employee List Table -->
                        <div id="employeeTable" class="mt-5">
                            <div class="search-box mb-3 mx-auto">
                                <div class="position-relative">
                                    <asp:TextBox ID="txtSearchEmployee" runat="server"
                                        CssClass="form-control search-input search form-control-sm"
                                        Placeholder="Search employees..."
                                        AutoPostBack="true"
                                        OnTextChanged="txtSearchEmployee_TextChanged" />              
                                    <svg class="svg-inline--fa fa-magnifying-glass search-box-icon" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="magnifying-glass" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                                        <path fill="currentColor" d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352a144 144 0 1 0 0-288 144 144 0 1 0 0 288z"></path>
                                    </svg>
                                </div>
                            </div>
                            
                            <div class="table-responsive">
                                <asp:Repeater ID="rptEmployees" runat="server" OnItemCommand="rptEmployees_ItemCommand">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-sm fs-9 mb-0">
                                            <thead>
                                                <tr>
                                                    <th>Emp ID</th>
                                                    <th>Name</th>
                                                    <th>Department</th>
                                                    <th>Designation</th>
                                                    <th>Joining Date</th>
                                                    <th>Status</th>
                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("EmployeeID") %></td>
                                            <td><%# Eval("FullName") %></td>
                                            <td><%# Eval("DepartmentName") %></td>
                                            <td><%# Eval("DesignationName") %></td>
                                            <td><%# Eval("JoiningDate", "{0:dd-MMM-yyyy}") %></td>
                                            <td><%# Eval("Status") %></td>
                                            <td class="text-center">
                                                <asp:LinkButton
                                                    ID="btnView"
                                                    runat="server"
                                                    CssClass="text-info me-2"
                                                    CommandName="ViewEmployee"
                                                    CommandArgument='<%# Eval("EmployeeID") %>'
                                                    ToolTip="View Details">
                                                    <i class="uil uil-eye"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton
                                                    ID="btnEdit"
                                                    runat="server"
                                                    CssClass="text-primary me-2"
                                                    CommandName="EditEmployee"
                                                    CommandArgument='<%# Eval("EmployeeID") %>'
                                                    ToolTip="Edit Employee">
                                                    <i class="uil uil-edit"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton
                                                    ID="btnDelete"
                                                    runat="server"
                                                    CssClass="text-danger"
                                                    CommandName="DeleteEmployee"
                                                    CommandArgument='<%# Eval("EmployeeID") %>'
                                                    ToolTip="Delete Employee"
                                                    OnClientClick="return confirm('Are you sure you want to delete this employee?');">
                                                    <i class="uil uil-trash-alt"></i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            
                            <!-- Pagination -->
                            <div class="d-flex justify-content-between mt-3">
                                <span class="d-none d-sm-inline-block" data-list-info="data-list-info">
                                    <asp:Label ID="lblPageInfo" runat="server" CssClass="text-muted"></asp:Label>
                                </span>
                                <div class="d-flex justify-content-center mt-3">
                                    <asp:LinkButton ID="btnPrev" runat="server"
                                        CssClass="btn btn-outline-secondary btn-sm me-1"
                                        OnClick="btnPrev_Click">
                                        «
                                    </asp:LinkButton>

                                    <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server"
                                                CommandName="Page"
                                                CommandArgument='<%# Eval("PageNumber") %>'
                                                CssClass='<%# (bool)Eval("IsCurrent") 
                                                            ? "btn btn-primary btn-sm me-1" 
                                                            : "btn btn-outline-secondary btn-sm me-1" %>'>
                                                <%# Eval("PageNumber") %>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                    <asp:LinkButton ID="btnNext" runat="server"
                                        CssClass="btn btn-outline-secondary btn-sm"
                                        OnClick="btnNext_Click">
                                        »
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </main>
        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="btnSaveDraft" />
            <asp:AsyncPostBackTrigger ControlID="cbSameAsPermanent" EventName="CheckedChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- JavaScript for Accordion -->
    <script type="text/javascript">
        function toggleAccordion(sectionId) {
            var element = document.getElementById(sectionId);
            if (element.classList.contains('show')) {
                element.classList.remove('show');
            } else {
                element.classList.add('show');
            }
        }
    </script>
</asp:Content>