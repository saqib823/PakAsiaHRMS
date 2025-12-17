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
                                                <label class="form-label">Employee No.</label>
                                                <asp:TextBox CssClass="form-control" ID="txtEmpID" runat="server" placeholder="EMP-001" ></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Title</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlTitle" DataTextField="Name" DataValueField="ID"  runat="server">
                                                    
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Full Name *</label>
                                                <asp:TextBox CssClass="form-control" ID="FullName" runat="server" placeholder="First name" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Father or Spouse Name *</label>
                                                <asp:TextBox CssClass="form-control" ID="GuardianName" runat="server" placeholder="Last name" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Date of Birth</label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtDOB" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Gender</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlGender" DataTextField="Name" DataValueField="ID"  runat="server">
                                                 
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">CNIC *</label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtCNIC" runat="server" MaxLength="15" placeholder="12345-1234567-1" required="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">CNIC Expiry *</label>
                                                <asp:TextBox CssClass="form-control datetimepicker" ID="txtCnicExpiry" runat="server" MaxLength="15" required="true" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Marital Status</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlMaritalStatus" DataTextField="Name" DataValueField="ID"  runat="server">
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
                                                <label class="form-label">Blood Group</label>
                                                <asp:DropDownList CssClass="form-control" ID="BloodGroup" DataTextField="Name" DataValueField="ID"  runat="server"  >

                                                </asp:DropDownList>
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
                                                <asp:DropDownList CssClass="form-control" ID="ddlShift" DataTextField="Name" DataValueField="ID"  runat="server">
                                                 
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Work Days</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlWorkDays" DataTextField="Name" DataValueField="ID"  runat="server">
                                                   
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Attendance Method</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlAttendanceMethod" DataTextField="Name" DataValueField="ID"  runat="server">
                                               
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Biometric ID</label>
                                                <asp:TextBox CssClass="form-control" ID="txtBiometricID" runat="server" placeholder="Biometric ID"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Weekly Off Days</label>
                                                <asp:ListBox CssClass="form-control" ID="lbWeeklyOff" runat="server" DataTextField="Name" DataValueField="ID"  SelectionMode="Multiple">
                                                  
                                                </asp:ListBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Allowed Late Minutes</label>
                                                <asp:TextBox CssClass="form-control" ID="txtAllowedLate" runat="server" TextMode="Number" placeholder="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Allowed Early Leave Count</label>
                                                <asp:TextBox CssClass="form-control" ID="txtAllowedEarlyLeaveCont" runat="server" TextMode="Number" placeholder="15"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
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
                                                <asp:DropDownList CssClass="form-control" ID="ddlPaymentMethod" DataTextField="Name" DataValueField="ID" runat="server">
                                                   
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Tax Status Or NTN</label>
                                                <asp:TextBox CssClass="form-control" ID="TaxDeduction" runat="server">
                                                    
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">EOBI Number</label>
                                                <asp:TextBox CssClass="form-control" ID="EOBIRegistered" runat="server">
                                                    
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Social Security</label>
                                                <asp:TextBox CssClass="form-control" ID="SocialSecurity" runat="server">
                                                    
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Overtime Rate</label>
                                                <asp:TextBox CssClass="form-control" ID="OvertimeRate" Text="Number" runat="server">
                                                    
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Payroll Cycle</label>
                                                <asp:DropDownList CssClass="form-control" ID="PayrollCycle" DataTextField="Name" DataValueField="ID" runat="server" >
                                                 
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Salary Type</label>
                                                <asp:DropDownList CssClass="form-control" ID="ddlSalaryType" DataTextField="Name" DataValueField="ID" runat="server">
                                                   
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 mt-3">
                                                <label class="form-label">Gross Salary</label>
                                                <asp:TextBox CssClass="form-control" ID="txtGrossSalary" runat="server">
                                                    
                                                </asp:TextBox>
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
                                                <asp:FileUpload ID="fileContract" runat="server" CssClass="form-control" />
                                            </div>

                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">CNIC Front</label>
                                                <asp:FileUpload ID="fileCNICFront" runat="server" CssClass="form-control" />
                                            </div>

                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">CNIC Back</label>
                                                <asp:FileUpload ID="fileCNICBack" runat="server" CssClass="form-control" />
                                            </div>

                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Educational Certificates</label>
                                                <asp:FileUpload ID="fileEducation" runat="server" CssClass="form-control" AllowMultiple="true" />
                                            </div>

                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Experience Letters</label>
                                                <asp:FileUpload ID="fileExperience" runat="server" CssClass="form-control" AllowMultiple="true" />
                                            </div>

                                            <div class="col-md-4 mt-3">
                                                <label class="form-label">Other Documents</label>
                                                <asp:FileUpload ID="fileOtherDocs" runat="server" CssClass="form-control" AllowMultiple="true" />
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

                         

                        </div>
                        <!-- End Accordion -->

                        <!-- Action Buttons -->
                        <div class="text-end mt-4">
                            <asp:Button ID="btnCancel" runat="server"
                                Text="Cancel" CssClass="btn btn-secondary me-2 mb-3" 
                                OnClick="btnCancel_Click" UseSubmitBehavior="false" />

                            

                            <asp:Button ID="btnSubmit" runat="server"
                                Text="Submit Employee" CssClass="btn btn-primary mb-3"
                                OnClick="btnSubmit_Click" />
                        </div>

                        <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

                       
                    </div>
                </div>
            </main>
        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
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