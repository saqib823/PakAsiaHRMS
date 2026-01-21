<%@ Page Title="View Employee" Language="C#" MasterPageFile="~/App.master"
    AutoEventWireup="true" CodeBehind="viewemployee.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Employees.viewemployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Employee Profile</h3>

    <!-- PROFILE HEADER -->
    <div class="card mb-3">
        <div class="card-body row align-items-center">
            <div class="col-md-2 text-center">
                <asp:Image ID="imgEmployee" runat="server"
                    CssClass="img-thumbnail" />
            </div>
            <div class="col-md-10">
                <h4>
                    <asp:Label ID="lblName" runat="server" /></h4>
                <p class="mb-1"><b>Employee No:</b>
                    <asp:Label ID="lblEmpNo" runat="server" /></p>
                <p class="mb-0"><b>Designation:</b>
                    <asp:Label ID="lblDesignation" runat="server" /></p>
                <p><b>Department:</b>
                    <asp:Label ID="lblDepartment" runat="server" /></p>
            </div>
        </div>
    </div>

    <!-- BASIC INFO -->
    <div class="card mb-3">
        <div class="card-header">Basic Information</div>
        <div class="card-body row">
            <div class="col-md-4"><b>Father / Spouse:</b>
                <asp:Label ID="lblGuardian" runat="server" /></div>
            <div class="col-md-4"><b>CNIC:</b>
                <asp:Label ID="lblCNIC" runat="server" /></div>
            <div class="col-md-4"><b>CNIC Expiry:</b>
                <asp:Label ID="lblCNICExpiry" runat="server" /></div>
            <div class="col-md-4"><b>DOB:</b>
                <asp:Label ID="lblDOB" runat="server" /></div>
            <div class="col-md-4"><b>Gender:</b>
                <asp:Label ID="lblGender" runat="server" /></div>
            <div class="col-md-4"><b>Marital Status:</b>
                <asp:Label ID="lblMarital" runat="server" /></div>
            <div class="col-md-4"><b>Blood Group:</b>
                <asp:Label ID="lblBlood" runat="server" /></div>
        </div>
    </div>

    <!-- CONTACT INFO -->
    <div class="card mb-3">
        <div class="card-header">Contact Information</div>
        <div class="card-body row">
            <div class="col-md-4"><b>Mobile:</b>
                <asp:Label ID="lblMobile" runat="server" /></div>
            <div class="col-md-4"><b>Alt Mobile:</b>
                <asp:Label ID="lblAltMobile" runat="server" /></div>
            <div class="col-md-4"><b>Personal Email:</b>
                <asp:Label ID="lblPersonalEmail" runat="server" /></div>
            <div class="col-md-4"><b>Official Email:</b>
                <asp:Label ID="lblOfficialEmail" runat="server" /></div>
            <div class="col-md-4"><b>City:</b>
                <asp:Label ID="lblCity" runat="server" /></div>
            <div class="col-md-4"><b>Province:</b>
                <asp:Label ID="lblProvince" runat="server" /></div>
            <div class="col-md-12"><b>Permanent Address:</b>
                <asp:Label ID="lblPermanentAddress" runat="server" /></div>
            <div class="col-md-12"><b>Current Address:</b>
                <asp:Label ID="lblCurrentAddress" runat="server" /></div>
        </div>
    </div>

    <!-- EMPLOYMENT -->
    <div class="card mb-3">
        <div class="card-header">Employment Information</div>
        <div class="card-body row">
            <div class="col-md-4"><b>Employment Type:</b>
                <asp:Label ID="lblEmploymentType" runat="server" /></div>
            <div class="col-md-4"><b>Status:</b>
                <asp:Label ID="lblEmploymentStatus" runat="server" /></div>
            <div class="col-md-4"><b>Joining Date:</b>
                <asp:Label ID="lblJoining" runat="server" /></div>
            <div class="col-md-4"><b>Confirmation Date:</b>
                <asp:Label ID="lblConfirmation" runat="server" /></div>
            <div class="col-md-4"><b>Contract End:</b>
                <asp:Label ID="lblContractEnd" runat="server" /></div>
            <div class="col-md-4"><b>Work Location:</b>
                <asp:Label ID="lblLocation" runat="server" /></div>
        </div>
    </div>

    <!-- ATTENDANCE -->
    <div class="card mb-3">
        <div class="card-header">Attendance Configuration</div>
        <div class="card-body row">
            <div class="col-md-4"><b>Shift:</b>
                <asp:Label ID="lblShift" runat="server" /></div>
            <div class="col-md-4"><b>Weekly Off:</b>
                <asp:Label ID="lblWeeklyOff" runat="server" /></div>
            <div class="col-md-4"><b>Biometric ID:</b>
                <asp:Label ID="lblBioID" runat="server" /></div>
            <div class="col-md-4"><b>Allowed Late:</b>
                <asp:Label ID="lblAllowedLate" runat="server" /></div>
            <div class="col-md-4"><b>Allowed Early:</b>
                <asp:Label ID="lblAllowedEarly" runat="server" /></div>
            <div class="col-md-4"><b>Half Day Hours:</b>
                <asp:Label ID="lblHalfDayHours" runat="server" /></div>
        </div>
    </div>

    <!-- PAYROLL -->
    <div class="card mb-3">
        <div class="card-header">Payroll Information</div>
        <div class="card-body row">
            <div class="col-md-4"><b>Salary Type:</b>
                <asp:Label ID="lblSalaryType" runat="server" /></div>
            <div class="col-md-4"><b>Basic Salary:</b>
                <asp:Label ID="lblBasicSalary" runat="server" /></div>
            <div class="col-md-4"><b>Gross Salary:</b>
                <asp:Label ID="lblGrossSalary" runat="server" /></div>
            <div class="col-md-4"><b>OT Rate:</b>
                <asp:Label ID="lblOTRate" runat="server" /></div>
            <div class="col-md-4"><b>Bank:</b>
                <asp:Label ID="lblBank" runat="server" /></div>
            <div class="col-md-4"><b>Account / IBAN:</b>
                <asp:Label ID="lblAccount" runat="server" /></div>
        </div>
    </div>
    <div class="card mb-3">
    <div class="card-header">Documents & Agreements</div>

    <div class="card-body row">

        <div class="col-md-4 mb-3">
            <b>Contract File:</b><br />
            <asp:Label ID="lblContractFile" runat="server" Text="Not Uploaded" />
            <br />
            <asp:LinkButton ID="btnDownloadContract" runat="server"
                CssClass="btn btn-sm btn-primary mt-1"
                OnClick="Download_Click"
                Visible="false">Download</asp:LinkButton>
        </div>

        <div class="col-md-4 mb-3">
            <b>CNIC Front:</b><br />
            <asp:Label ID="lblCNICFront" runat="server" Text="Not Uploaded" />
            <br />
            <asp:LinkButton ID="btnDownloadCNICFront" runat="server"
                CssClass="btn btn-sm btn-primary mt-1"
                OnClick="Download_Click"
                Visible="false">Download</asp:LinkButton>
        </div>

        <div class="col-md-4 mb-3">
            <b>CNIC Back:</b><br />
            <asp:Label ID="lblCNICBack" runat="server" Text="Not Uploaded" />
            <br />
            <asp:LinkButton ID="btnDownloadCNICBack" runat="server"
                CssClass="btn btn-sm btn-primary mt-1"
                OnClick="Download_Click"
                Visible="false">Download</asp:LinkButton>
        </div>

        <div class="col-md-4 mb-3">
            <b>Educational Certificates:</b><br />
            <asp:Label ID="lblEducation" runat="server" Text="Not Uploaded" />
            <br />
            <asp:LinkButton ID="btnDownloadEducation" runat="server"
                CssClass="btn btn-sm btn-primary mt-1"
                OnClick="Download_Click"
                Visible="false">Download</asp:LinkButton>
        </div>

        <div class="col-md-4 mb-3">
            <b>Experience Letters:</b><br />
            <asp:Label ID="lblExperience" runat="server" Text="Not Uploaded" />
            <br />
            <asp:LinkButton ID="btnDownloadExperience" runat="server"
                CssClass="btn btn-sm btn-primary mt-1"
                OnClick="Download_Click"
                Visible="false">Download</asp:LinkButton>
        </div>

        <div class="col-md-4 mb-3">
            <b>Other Documents:</b><br />
            <asp:Label ID="lblOtherDocs" runat="server" Text="Not Uploaded" />
            <br />
            <asp:LinkButton ID="btnDownloadOtherDocs" runat="server"
                CssClass="btn btn-sm btn-primary mt-1"
                OnClick="Download_Click"
                Visible="false">Download</asp:LinkButton>
        </div>

        <div class="col-md-4 mb-3">
            <b>Non-Disclosure Agreement:</b><br />
            <asp:Label ID="lblNDAStatus" runat="server" />
        </div>

        <div class="col-md-4 mb-3">
            <b>Appointment Letter:</b><br />
            <asp:Label ID="lblAppointmentIssued" runat="server" />
        </div>

        <div class="col-md-4 mb-3">
            <b>Contract Start Date:</b><br />
            <asp:Label ID="lblContractStartDate" runat="server" />
        </div>

        <div class="col-md-12 mb-3">
            <b>Terms & Conditions:</b><br />
            <asp:Label ID="lblTerms" runat="server" />
        </div>

    </div>
</div>


</asp:Content>
