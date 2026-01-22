<%@ Page Title="Process Payroll" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="ProcessPayroll.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Payroll.ProcessPayroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid mt-10">

        <!-- ================= SALARY STRUCTURE FORM ================= -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0">Process Payroll</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3">
                            <!-- Employee -->
                            <div class="col-md-4">
                                <label class="form-label">Employee <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-select"  data-choices="data-choices" data-options='{"removeItemButton":true,"placeholder":true}' required />
                            </div>
                            <!-- From Date -->
                            <div class="col-md-4">
                                <label class="form-label">From <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>
                            <!-- To Date -->
                            <div class="col-md-4">
                                <label class="form-label">To <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEffectiveTo" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>
                        </div>

                        <div class="mt-4 text-end">
                            <asp:Button ID="btnCalculate" runat="server" CssClass="btn btn-info px-4" Text="Generate" OnClick="btnCalculate_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0">Process Branch Payroll</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3">
                            <!-- Employee -->
                            <div class="col-md-4">
                                <label class="form-label">Branch <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-select"  data-choices="data-choices" data-options='{"removeItemButton":true,"placeholder":true}' required />
                            </div>
                            <!-- From Date -->
                            <div class="col-md-4">
                                <label class="form-label">From <span class="text-danger">*</span></label>
                                <asp:TextBox ID="dateFrom" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>
                            <!-- To Date -->
                            <div class="col-md-4">
                                <label class="form-label">To <span class="text-danger">*</span></label>
                                <asp:TextBox ID="dateTo" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">To <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlPayrollCycle" runat="server" CssClass="form-select"  data-choices="data-choices" data-options='{"removeItemButton":true,"placeholder":true}' required />
                            </div>
                        </div>

                        <div class="mt-4 text-end">
                            <asp:Button ID="btnBranchPayroll" runat="server" CssClass="btn btn-info px-4" Text="Generate" OnClick="btnBranchPayroll_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- ================= PAYROLL REPORT ================= -->
       <%-- <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0">Payroll Details</h5>
                    </div>
                    <div class="card-body">

                        <asp:GridView ID="gvAttendance" runat="server" CssClass="table table-bordered table-striped"
                            AutoGenerateColumns="False" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="WorkDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="DayName" HeaderText="Day" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="InTime" HeaderText="In Time" DataFormatString="{0:HH:mm}" />
                                <asp:BoundField DataField="OutTime" HeaderText="Out Time" DataFormatString="{0:HH:mm}" />
                                <asp:BoundField DataField="WorkHours" HeaderText="Hours Worked" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="WorkedMinutes" HeaderText="Minutes Worked" />
                                <asp:BoundField DataField="DaySalary" HeaderText="Salary" DataFormatString="{0:N2}" />
                            </Columns>
                        </asp:GridView>

                        <div class="mt-3 row">
                            <div class="col-md-4">
                                <label>Monthly Gross Salary:</label>
                                <asp:Label ID="lblGross" runat="server" CssClass="fw-bold"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <label>Earned Salary:</label>
                                <asp:Label ID="lblEarned" runat="server" CssClass="fw-bold"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <label>Net Payable Salary:</label>
                                <asp:Label ID="lblNet" runat="server" CssClass="fw-bold"></asp:Label>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>--%>

    </div>

</asp:Content>
