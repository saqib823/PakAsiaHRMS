<%@ Page Async="true" Title="Employee Management" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="employees.aspx.cs" Inherits="hrms_PakAsia.Pages.Employees.employees" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="upEmployee" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <main class="main mt-10" id="top">
            <div class="container-fluid">
                <div class="row py-5">
                    
                    <!-- Page Heading -->
                    <div class="col-12 text-center mb-4">
                        <h3 class="mt-3">Employee Management</h3>
                        <p class="text-body-tertiary">Complete employee information</p>
                    </div>

                    <!-- Employee Selection Section -->
                    <div class="col-12 mb-4">
                        <div class="card">
                            <div class="card-header py-3">
                                <h5 class="mb-0"><i class="uil uil-search me-2"></i>Select Employee</h5>
                            </div>
                            <div class="card-body">
                                <div class="row align-items-center">
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <span class="input-group-text bg-primary text-white">
                                                <i class="uil uil-users-alt"></i>
                                            </span>
                                            <asp:DropDownList ID="ddlSelectEmployee" runat="server" 
                                                CssClass="form-control" 
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectEmployee_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 text-end">
                                        <a href="?id=0" class="btn btn-primary">
                                            <i class="uil uil-plus me-1"></i> Create New Employee
                                        </a>
                                    </div>
                                </div>
                                <div class="mt-2">
                                    <small class="form-text text-muted">
                                        <i class="uil uil-info-circle me-1"></i>Select an existing employee or create a new one
                                    </small>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Alert Placeholder -->
                    <div class="col-12 mb-3">
                        <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>
                    </div>

                    <!-- Form Sections with Accordion -->
                    <div class="col-12">
                        <!-- Using Phoenix Accordion Structure -->
                        <div class="accordion" id="employeeAccordion">
                            
                            <!-- 1️⃣ Basic Info Section -->
                            <div class="accordion-item border-top">
                                <h2 class="accordion-header" id="headingBasicInfo">
                                    <button class="accordion-button" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#collapseBasicInfo" aria-expanded="true" 
                                        aria-controls="collapseBasicInfo">
                                        <i class="uil uil-user-circle me-2 text-primary"></i><strong>Basic Information</strong>
                                    </button>
                                </h2>
                                <div id="collapseBasicInfo" class="accordion-collapse collapse show" 
                                    aria-labelledby="headingBasicInfo" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body pt-0">
                                        <h5 class="mb-4 text-primary">Personal Details</h5>
                                        <div class="row">
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Employee No. <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmpID" runat="server" placeholder="EMP-001"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Title</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlTitle" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Full Name <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control" ID="FullName" runat="server" placeholder="First name" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Father/Spouse Name <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control" ID="GuardianName" runat="server" placeholder="Last name" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Date of Birth <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtDOB" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Gender <span class="text-danger">*</span></label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlGender" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">CNIC <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtCNIC" runat="server" MaxLength="15" placeholder="12345-1234567-1" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">CNIC Expiry <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtCnicExpiry" runat="server" MaxLength="15" required="true" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Marital Status</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlMaritalStatus" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Nationality</label>
                                                <asp:TextBox CssClass="form-control" ID="txtNationality" runat="server" placeholder="Pakistani"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Religion</label>
                                                <asp:TextBox CssClass="form-control" ID="txtReligion" runat="server" placeholder="Religion"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Blood Group</label>
                                                <asp:DropDownList CssClass="form-control" ID="BloodGroup" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label" for="fileProfilePic">Profile Picture</label>
                                                <input class="form-control" id="fileProfilePic" type="file" runat="server" accept="image/*" />
                                                <small class="form-text text-muted">Max 5MB, JPG/PNG only</small>
                                            </div>
                                        </div>
                                        
                                        <!-- Save Basic Info Button -->
                                        <div class="text-end mt-4 border-top pt-3">
                                            <asp:Button ID="btnSubmitBasicInfo" runat="server"
                                                Text="Save Basic Information" CssClass="btn btn-outline-primary px-4"
                                                OnClick="btnSubmitBasicInfo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 2️⃣ Contact & Address Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="headingContactAddress">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#collapseContactAddress" aria-expanded="false" 
                                        aria-controls="collapseContactAddress">
                                        <i class="uil uil-phone me-2 text-primary"></i><strong>Contact & Address</strong>
                                    </button>
                                </h2>
                                <div id="collapseContactAddress" class="accordion-collapse collapse" 
                                    aria-labelledby="headingContactAddress" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body pt-0">
                                        <h5 class="mb-4 text-primary">Contact Information</h5>
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Personal Email <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control" ID="txtPersonalEmail" runat="server" TextMode="Email" placeholder="personal@example.com" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Official Email</label>
                                                <asp:TextBox CssClass="form-control" ID="txtOfficialEmail" runat="server" TextMode="Email" placeholder="official@company.com"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Phone Number <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control" ID="txtPhone" runat="server" MaxLength="11" placeholder="03XX-XXXXXXX" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Alternate Phone</label>
                                                <asp:TextBox CssClass="form-control" ID="txtAlternatePhone" runat="server" MaxLength="11" placeholder="03XX-XXXXXXX"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Emergency Contact</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmergencyContact" runat="server" placeholder="Emergency number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Emergency Name</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmergencyName" runat="server" placeholder="Contact person name"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Relationship</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmergencyRelationship" runat="server" placeholder="Father/Mother/Spouse"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <h5 class="mt-5 mb-4 text-primary">Permanent Address</h5>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Address Line 1</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermAddress1" runat="server" placeholder="House #, Street"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Address Line 2</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermAddress2" runat="server" placeholder="Sector, Area"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">City</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermCity" runat="server" placeholder="City"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">State/Province</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermState" runat="server" placeholder="Province"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Postal Code</label>
                                                <asp:TextBox CssClass="form-control" ID="txtPermPostalCode" runat="server" placeholder="Postal code"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <div class="form-check mt-3">
                                            <asp:CheckBox ID="cbSameAsPermanent" runat="server" CssClass="form-check-input" AutoPostBack="true" OnCheckedChanged="cbSameAsPermanent_CheckedChanged" />
                                            <label class="form-check-label" for="<%= cbSameAsPermanent.ClientID %>">
                                                <i class="uil uil-copy me-1"></i>Same as permanent address
                                            </label>
                                        </div>
                                        
                                        <h5 class="mt-5 mb-4 text-primary">Current Address</h5>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Address Line 1</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentAddress1" runat="server" placeholder="House #, Street"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Address Line 2</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentAddress2" runat="server" placeholder="Sector, Area"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">City</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentCity" runat="server" placeholder="City"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">State/Province</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentState" runat="server" placeholder="Province"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Postal Code</label>
                                                <asp:TextBox CssClass="form-control" ID="txtCurrentPostalCode" runat="server" placeholder="Postal code"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <!-- Save Contact Info Button -->
                                        <div class="text-end mt-4 border-top pt-3">
                                            <asp:Button ID="btnSubmitContactInfo" runat="server"
                                                Text="Save Contact Information" CssClass="btn btn-outline-primary px-4"
                                                OnClick="btnSubmitContactInfo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 3️⃣ Employment Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="headingEmployment">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#collapseEmployment" aria-expanded="false" 
                                        aria-controls="collapseEmployment">
                                        <i class="uil uil-briefcase me-2 text-primary"></i><strong>Employment</strong>
                                    </button>
                                </h2>
                                <div id="collapseEmployment" class="accordion-collapse collapse" 
                                    aria-labelledby="headingEmployment" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body pt-0">
                                        <h5 class="mb-4 text-primary">Job Details</h5>
                                        <div class="row">
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Department <span class="text-danger">*</span></label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlDepartment" DataTextField="Name" DataValueField="ID" runat="server" required="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Designation <span class="text-danger">*</span></label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlDesignation" DataTextField="Name" DataValueField="ID" runat="server" required="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Branch <span class="text-danger">*</span></label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlBranch" DataTextField="Name" DataValueField="ID" runat="server" required="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Reporting Manager</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlReportingManager" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Employment Type</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlEmploymentType" runat="server"><asp:ListItem Value="">Select</asp:ListItem>
                                                    <asp:ListItem>Permanent</asp:ListItem>
                                                    <asp:ListItem>Contract</asp:ListItem>
                                                    <asp:ListItem>Probation</asp:ListItem>
                                                    <asp:ListItem>Intern</asp:ListItem>
                                                    <asp:ListItem>Part-time</asp:ListItem></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Joining Date <span class="text-danger">*</span></label>
                                                <asp:TextBox CssClass="form-control" ID="txtJoiningDate" runat="server" TextMode="Date" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Confirmation Date</label>
                                                <asp:TextBox CssClass="form-control" ID="txtConfirmationDate" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Contract End Date</label>
                                                <asp:TextBox CssClass="form-control" ID="txtContractEndDate" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Employee Status</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlEmpStatus" runat="server"><asp:ListItem Value="Active" Selected="True">Active</asp:ListItem>
                                                    <asp:ListItem>Inactive</asp:ListItem>
                                                    <asp:ListItem>Suspended</asp:ListItem>
                                                    <asp:ListItem>Terminated</asp:ListItem>
                                                    <asp:ListItem>Resigned</asp:ListItem></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Work Location</label>
                                                <asp:TextBox CssClass="form-control" ID="txtWorkLocation" runat="server" placeholder="Office location"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Probation End</label>
                                                <asp:TextBox CssClass="form-control" ID="txtProbationEndDate" runat="server" TextMode="Date" Rows="2"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Employee Category</label>
                                                <asp:TextBox CssClass="form-control" ID="txtJobDescription" runat="server" placeholder="Enter Employee Category"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <!-- Save Employment Button -->
                                        <div class="text-end mt-4 border-top pt-3">
                                            <asp:Button ID="btnSubmitEmploymentInfo" runat="server"
                                                Text="Save Employment Information" CssClass="btn btn-outline-primary px-4"
                                                OnClick="btnSubmitEmploymentInfo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 4️⃣ Attendance Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="headingAttendance">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#collapseAttendance" aria-expanded="false" 
                                        aria-controls="collapseAttendance">
                                        <i class="uil uil-clock me-2 text-primary"></i><strong>Attendance</strong>
                                    </button>
                                </h2>
                                <div id="collapseAttendance" class="accordion-collapse collapse" 
                                    aria-labelledby="headingAttendance" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body pt-0">
                                        <h5 class="mb-4 text-primary">Attendance Settings</h5>
                                        <div class="row">
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Shift Timing</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlShift" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Work Days</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlWorkDays" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Attendance Method</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlAttendanceMethod" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Biometric ID</label>
                                                <asp:TextBox CssClass="form-control" ID="txtBiometricID" runat="server" placeholder="Biometric ID"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Weekly Off Days</label>
                                                <asp:ListBox CssClass="form-control" ID="lbWeeklyOff" runat="server" DataTextField="Name" DataValueField="ID" SelectionMode="Multiple" Rows="3"></asp:ListBox>
                                                <small class="form-text text-muted">Hold Ctrl to select multiple</small>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Allowed Late Minutes</label>
                                                <asp:TextBox CssClass="form-control" ID="txtAllowedLate" runat="server" TextMode="Number" placeholder="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Allowed Early Leave Count</label>
                                                <asp:TextBox CssClass="form-control" ID="txtAllowedEarlyLeaveCont" runat="server" TextMode="Number" placeholder="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Half Day Hours</label>
                                                <asp:TextBox CssClass="form-control" ID="txtHalfDayHours" runat="server" TextMode="Number" placeholder="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <!-- Save Attendance Button -->
                                        <div class="text-end mt-4 border-top pt-3">
                                            <asp:Button ID="btnSubmitAttendanceInfo" runat="server"
                                                Text="Save Attendance Information" CssClass="btn btn-outline-primary px-4"
                                                OnClick="btnSubmitAttendanceInfo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 5️⃣ Payroll Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="headingPayroll">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#collapsePayroll" aria-expanded="false" 
                                        aria-controls="collapsePayroll">
                                        <i class="uil uil-money-bill me-2 text-primary"></i><strong>Payroll</strong>
                                    </button>
                                </h2>
                                <div id="collapsePayroll" class="accordion-collapse collapse" 
                                    aria-labelledby="headingPayroll" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body pt-0">
                                        <h5 class="mb-4 text-primary">Salary Structure</h5>
                                        <div class="row">
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Basic Salary <span class="text-danger">*</span></label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtBasicSalary" runat="server" TextMode="Number" step="0.01" placeholder="0.00" required="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">House Rent</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtHouseRent" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Medical Allowance</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtMedicalAllowance" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Transport Allowance</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtTransportAllowance" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Other Allowances</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtOtherAllowances" runat="server" TextMode="Number" step="0.01" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Salary Account No.</label>
                                                <asp:TextBox CssClass="form-control" ID="txtSalaryAccount" runat="server" placeholder="Bank account number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Bank Name</label>
                                                <asp:TextBox CssClass="form-control" ID="txtBankName" runat="server" placeholder="Bank name"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Payment Method <span class="text-danger">*</span></label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlPaymentMethod" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Tax Status/NTN</label>
                                                <asp:TextBox CssClass="form-control" ID="TaxDeduction" runat="server" placeholder="NTN Number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">EOBI Number</label>
                                                <asp:TextBox CssClass="form-control" ID="EOBIRegistered" runat="server" placeholder="EOBI Number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Social Security</label>
                                                <asp:TextBox CssClass="form-control" ID="SocialSecurity" runat="server" placeholder="Social Security Number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Overtime Rate</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="OvertimeRate" TextMode="Number" runat="server" placeholder="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Payroll Cycle</label>
                                                <asp:DropDownList CssClass="form-control" ID="PayrollCycle" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Salary Type</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlSalaryType" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mb-3">
                                                <label class="form-label">Gross Salary</label>
                                                <div class="input-group">
                                                    <span class="input-group-text">PKR</span>
                                                    <asp:TextBox CssClass="form-control" ID="txtGrossSalary" runat="server" ReadOnly="true" placeholder="Auto-calculated"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <!-- Save Payroll Button -->
                                        <div class="text-end mt-4 border-top pt-3">
                                            <asp:Button ID="btnSubmitPayrollInfo" runat="server"
                                                Text="Save Payroll Information" CssClass="btn btn-outline-primary px-4"
                                                OnClick="btnSubmitPayrollInfo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 6️⃣ Legal Section -->
                            <div class="accordion-item">
                                <h2 class="accordion-header" id="headingLegal">
                                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" 
                                        data-bs-target="#collapseLegal" aria-expanded="false" 
                                        aria-controls="collapseLegal">
                                        <i class="uil uil-file-contract me-2 text-primary"></i><strong>Legal Documents</strong>
                                    </button>
                                </h2>
                                <div id="collapseLegal" class="accordion-collapse collapse" 
                                    aria-labelledby="headingLegal" data-bs-parent="#employeeAccordion">
                                    <div class="accordion-body pt-0">
                                        <h5 class="mb-4 text-primary">Required Documents</h5>
                                        <div class="row">
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Contract File</label>
                                                <asp:FileUpload ID="fileContract" runat="server" CssClass="form-control" />
                                                <small class="form-text text-muted">PDF, DOC, DOCX only</small>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">CNIC Front</label>
                                                <asp:FileUpload ID="fileCNICFront" runat="server" CssClass="form-control" />
                                                <small class="form-text text-muted">JPG, PNG, PDF only</small>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">CNIC Back</label>
                                                <asp:FileUpload ID="fileCNICBack" runat="server" CssClass="form-control" />
                                                <small class="form-text text-muted">JPG, PNG, PDF only</small>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Educational Certificates</label>
                                                <asp:FileUpload ID="fileEducation" runat="server" CssClass="form-control" AllowMultiple="true" />
                                                <small class="form-text text-muted">Multiple files allowed</small>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Experience Letters</label>
                                                <asp:FileUpload ID="fileExperience" runat="server" CssClass="form-control" AllowMultiple="true" />
                                                <small class="form-text text-muted">Multiple files allowed</small>
                                            </div>
                                            <div class="col-md-4 mb-3">
                                                <label class="form-label">Other Documents</label>
                                                <asp:FileUpload ID="fileOtherDocs" runat="server" CssClass="form-control" AllowMultiple="true" />
                                                <small class="form-text text-muted">Multiple files allowed</small>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Non-Disclosure Agreement</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlNDA" runat="server">
                                                    <asp:ListItem Value="">Select Status</asp:ListItem>
                                                    <asp:ListItem>Signed</asp:ListItem>
                                                    <asp:ListItem>Pending</asp:ListItem>
                                                    <asp:ListItem>Not Required</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Terms & Conditions</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlTerms" runat="server">
                                                    <asp:ListItem Value="">Select Status</asp:ListItem>
                                                    <asp:ListItem>Accepted</asp:ListItem>
                                                    <asp:ListItem>Pending</asp:ListItem>
                                                    <asp:ListItem>Declined</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        
                                        <!-- Save Legal Button -->
                                        <div class="text-end mt-4 border-top pt-3">
                                            <asp:Button ID="btnSubmitLegalInfo" runat="server"
                                                Text="Save Legal Documents" CssClass="btn btn-outline-primary px-4"
                                                OnClick="btnSubmitLegalInfo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Main Action Buttons -->
                    <div class="col-12 mt-5">
                        <div class="card border">
                            <div class="card-body text-center">
                                <h5 class="card-title text-primary mb-3">Complete Employee Setup</h5>
                                <p class="card-text text-muted mb-4">
                                    Save all information at once or save each section individually using the buttons above.
                                </p>
                                <div class="d-flex justify-content-center gap-3">
                                    <asp:Button ID="btnCancel" runat="server"
                                        Text="Cancel" CssClass="btn btn-secondary px-4" 
                                        OnClick="btnCancel_Click" UseSubmitBehavior="false" />

                                    <asp:Button ID="btnSubmitAll" runat="server"
                                        Text="Save All Information" CssClass="btn btn-primary px-5"
                                        OnClick="btnSubmitAll_Click" />
                                </div>
                                <small class="form-text text-muted mt-3 d-block">
                                    <i class="uil uil-info-circle me-1"></i>Make sure to save each section before proceeding to the next
                                </small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </ContentTemplate>
    
    <Triggers>
        <asp:PostBackTrigger ControlID="btnSubmitAll" />
        <asp:PostBackTrigger ControlID="btnSubmitBasicInfo" />
        <asp:PostBackTrigger ControlID="btnSubmitContactInfo" />
        <asp:PostBackTrigger ControlID="btnSubmitEmploymentInfo" />
        <asp:PostBackTrigger ControlID="btnSubmitAttendanceInfo" />
        <asp:PostBackTrigger ControlID="btnSubmitPayrollInfo" />
        <asp:PostBackTrigger ControlID="btnSubmitLegalInfo" />
        <asp:AsyncPostBackTrigger ControlID="ddlSelectEmployee" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cbSameAsPermanent" EventName="CheckedChanged" />
    </Triggers>
</asp:UpdatePanel>

<!-- JavaScript for Accordion and Form Enhancement -->
<script type="text/javascript">
    // Auto-calculate gross salary
    function calculateGrossSalary() {
        var basic = parseFloat(document.getElementById('<%= txtBasicSalary.ClientID %>').value) || 0;
        var houseRent = parseFloat(document.getElementById('<%= txtHouseRent.ClientID %>').value) || 0;
        var medical = parseFloat(document.getElementById('<%= txtMedicalAllowance.ClientID %>').value) || 0;
        var transport = parseFloat(document.getElementById('<%= txtTransportAllowance.ClientID %>').value) || 0;
        var other = parseFloat(document.getElementById('<%= txtOtherAllowances.ClientID %>').value) || 0;
        
        var gross = basic + houseRent + medical + transport + other;
        document.getElementById('<%= txtGrossSalary.ClientID %>').value = gross.toFixed(2);
    }

    // Attach event listeners for salary calculation
    document.addEventListener('DOMContentLoaded', function() {
        var salaryFields = [
            '<%= txtBasicSalary.ClientID %>',
            '<%= txtHouseRent.ClientID %>',
            '<%= txtMedicalAllowance.ClientID %>',
            '<%= txtTransportAllowance.ClientID %>',
            '<%= txtOtherAllowances.ClientID %>'
        ];
        
        salaryFields.forEach(function(fieldId) {
            var field = document.getElementById(fieldId);
            if (field) {
                field.addEventListener('input', calculateGrossSalary);
            }
        });
        
        // Calculate on page load
        calculateGrossSalary();
    });

    // Form validation before submission
    function validateSection(sectionName) {
        var isValid = true;
        var errorMessages = [];
        
        if (sectionName === 'basic') {
            // Basic info validation
            var fullName = document.getElementById('<%= FullName.ClientID %>');
            var guardianName = document.getElementById('<%= GuardianName.ClientID %>');
            var cnic = document.getElementById('<%= txtCNIC.ClientID %>');
            var dob = document.getElementById('<%= txtDOB.ClientID %>');
            
            if (!fullName.value.trim()) {
                errorMessages.push("Full Name is required");
                isValid = false;
            }
            if (!guardianName.value.trim()) {
                errorMessages.push("Father/Spouse Name is required");
                isValid = false;
            }
            if (!cnic.value.match(/^\d{5}-\d{7}-\d{1}$/)) {
                errorMessages.push("Invalid CNIC format (XXXXX-XXXXXXX-X)");
                isValid = false;
            }
        }
        
        if (errorMessages.length > 0) {
            alert("Please fix the following errors:\n\n" + errorMessages.join("\n"));
        }
        
        return isValid;
    }

    // Copy permanent address to current address
    function copyAddressToCurrent() {
        if (document.getElementById('<%= cbSameAsPermanent.ClientID %>').checked) {
            document.getElementById('<%= txtCurrentAddress1.ClientID %>').value = 
                document.getElementById('<%= txtPermAddress1.ClientID %>').value;
            document.getElementById('<%= txtCurrentAddress2.ClientID %>').value = 
                document.getElementById('<%= txtPermAddress2.ClientID %>').value;
            document.getElementById('<%= txtCurrentCity.ClientID %>').value = 
                document.getElementById('<%= txtPermCity.ClientID %>').value;
            document.getElementById('<%= txtCurrentState.ClientID %>').value = 
                document.getElementById('<%= txtPermState.ClientID %>').value;
            document.getElementById('<%= txtCurrentPostalCode.ClientID %>').value = 
                document.getElementById('<%= txtPermPostalCode.ClientID %>').value;
        }
    }

    // Format CNIC input
    function formatCNIC(input) {
        var value = input.value.replace(/\D/g, '');
        if (value.length > 5) {
            value = value.substring(0, 5) + '-' + value.substring(5);
        }
        if (value.length > 13) {
            value = value.substring(0, 13) + '-' + value.substring(13, 14);
        }
        input.value = value;
    }

    // Phone number validation
    function validatePhone(input) {
        var value = input.value.replace(/\D/g, '');
        if (value.length === 11 && value.startsWith('03')) {
            input.classList.remove('is-invalid');
            input.classList.add('is-valid');
        } else {
            input.classList.remove('is-valid');
            input.classList.add('is-invalid');
        }
    }
</script>
</asp:Content>