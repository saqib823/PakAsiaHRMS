<%@ Page Title="Biometric Integration"
    Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="biometric-integration.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Attendance.biometric_integration" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<asp:UpdatePanel ID="upBio" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<main class="main mt-10" id="top">
<div class="container">

<div class="row py-5">

    <!-- PAGE HEADER -->
    <div class="text-center mb-4">
        <h3 class="mt-3">Biometric Machine Integration</h3>
        <p class="text-body-tertiary">
            Configure devices and monitor real-time attendance sync
        </p>
    </div>

    <!-- DEVICE CONFIGURATION -->
    <div class="card mb-4">
        <div class="card-header fw-bold">
            Device Configuration
        </div>
        <div class="card-body">
            <div class="row">

                <div class="col-sm-3 mt-3">
                    <label class="form-label">Device Name</label>
                    <asp:TextBox ID="txtDeviceName" runat="server"
                        CssClass="form-control"
                        placeholder="e.g. ZKTeco Main Gate" />
                </div>

                <div class="col-sm-3 mt-3">
                    <label class="form-label">Device Type</label>
                    <asp:DropDownList ID="ddlDeviceType" runat="server"
                        CssClass="form-control">
                        <asp:ListItem Text="SDK" />
                        <asp:ListItem Text="TCP/IP" />
                        <asp:ListItem Text="HTTP Push" />
                        <asp:ListItem Text="ONVIF" />
                    </asp:DropDownList>
                </div>

                <div class="col-sm-3 mt-3">
                    <label class="form-label">IP Address</label>
                    <asp:TextBox ID="txtIPAddress" runat="server"
                        CssClass="form-control"
                        placeholder="192.168.1.100" />
                </div>

                <div class="col-sm-3 mt-3">
                    <label class="form-label">Port</label>
                    <asp:TextBox ID="txtPort" runat="server"
                        CssClass="form-control"
                        placeholder="4370" />
                </div>

                <div class="col-sm-3 mt-3">
                    <label class="form-label">Branch</label>
                    <asp:DropDownList ID="ddlBranch" runat="server"
                        CssClass="form-control" />
                </div>

                <div class="col-sm-3 mt-3">
                    <label class="form-label">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server"
                        CssClass="form-control">
                        <asp:ListItem Text="Active" />
                        <asp:ListItem Text="Inactive" />
                    </asp:DropDownList>
                </div>

                <div class="col-sm-6 mt-4 text-end">
                    <asp:Button ID="btnTestConnection" runat="server"
                        CssClass="btn btn-outline-primary me-2"
                        Text="Test Connection" />

                    <asp:Button ID="btnSaveDevice" runat="server"
                        CssClass="btn btn-primary"
                        Text="Save Device" />
                </div>

            </div>
        </div>
    </div>
                            <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

    <!-- REAL-TIME SYNC STATUS -->
    <div class="card mb-4">
        <div class="card-header fw-bold">
            Real-Time Attendance Sync
        </div>
        <div class="card-body">

            <div class="row text-center">
                <div class="col-md-3">
                    <div class="border rounded p-3">
                        <div class="fw-bold">Sync Status</div>
                        <span class="badge bg-success">Connected</span>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="border rounded p-3">
                        <div class="fw-bold">Last Sync</div>
                        <span class="text-muted">12-Sep-2025 10:32 AM</span>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="border rounded p-3">
                        <div class="fw-bold">Retry Attempts</div>
                        <span class="text-muted">Auto Enabled</span>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="border rounded p-3">
                        <div class="fw-bold">Duplicate Filter</div>
                        <span class="badge bg-info">Enabled</span>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- PUNCH TYPES CONFIG -->
    <div class="card mb-4">
        <div class="card-header fw-bold">
            Punch Types Supported
        </div>
        <div class="card-body">
            <div class="row">

                <div class="col-sm-3 form-check mt-2">
                    <asp:CheckBox ID="chkIn" runat="server" CssClass="form-check-input" Checked="true" />
                    <label class="form-check-label">IN</label>
                </div>

                <div class="col-sm-3 form-check mt-2">
                    <asp:CheckBox ID="chkOut" runat="server" CssClass="form-check-input" Checked="true" />
                    <label class="form-check-label">OUT</label>
                </div>

                <div class="col-sm-3 form-check mt-2">
                    <asp:CheckBox ID="chkBreakIn" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label">Break IN</label>
                </div>

                <div class="col-sm-3 form-check mt-2">
                    <asp:CheckBox ID="chkBreakOut" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label">Break OUT</label>
                </div>

                <div class="col-sm-6 form-check mt-3">
                    <asp:CheckBox ID="chkManualPunch" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label">
                        Allow Manual Punch (Approval Required)
                    </label>
                </div>

            </div>
        </div>
    </div>

    <!-- ATTENDANCE LOGS -->
    <div class="card">
        <div class="card-header fw-bold">
            Attendance Logs (Real-Time)
        </div>
        <div class="card-body">

            <div class="table-responsive">
                <asp:Repeater ID="rptAttendance" runat="server">
                    <HeaderTemplate>
                        <table class="table table-striped table-sm">
                            <thead>
                                <tr>
                                    <th>Employee</th>
                                    <th>Device</th>
                                    <th>Punch Type</th>
                                    <th>Date</th>
                                    <th>Time</th>
                                    <th>Source</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("EmployeeName") %></td>
                            <td><%# Eval("DeviceName") %></td>
                            <td>
                                <span class="badge bg-secondary">
                                    <%# Eval("PunchType") %>
                                </span>
                            </td>
                            <td><%# Eval("PunchDate") %></td>
                            <td><%# Eval("PunchTime") %></td>
                            <td><%# Eval("Source") %></td>
                        </tr>
                    </ItemTemplate>

                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

        </div>
    </div>

</div>
</div>
</main>

</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
