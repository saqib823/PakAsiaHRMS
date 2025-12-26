<%@ Page Title="Attendance Rules" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="AttendanceRules.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Rules.AttendanceRules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid">

    <!-- ================= RULE FORM ================= -->
    <div class="row mt-10">
        <div class="col-12">

            <div class="card shadow-sm mb-4">
                <div class="card-header text-white">
                    <h5 class="mb-0">Attendance Rules Master</h5>
                </div>

                <div class="card-body">

                    <div class="row g-3">

                        <!-- Rule Type -->
                        <div class="col-md-4">
                            <label class="form-label">Rule Type</label>
                            <asp:DropDownList ID="ddlRuleType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRuleType_SelectedIndexChanged"
                                CssClass="form-select">
                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                <asp:ListItem Value="Late">Late</asp:ListItem>
                                <asp:ListItem Value="EarlyLeave">Early Leave</asp:ListItem>
                                <asp:ListItem Value="Absence">Absence</asp:ListItem>
                                <asp:ListItem Value="Overtime">Overtime</asp:ListItem>
                                <asp:ListItem Value="WeeklyOff">Weekly Off</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Rule Name -->
                        <div class="col-md-4">
                            <label class="form-label">Rule Name</label>
                            <asp:TextBox ID="txtRuleName" runat="server"
                                CssClass="form-control"
                                placeholder="Enter Rule Name" />
                        </div>

                        <!-- Rule Config / Description -->
    

<!-- ================= Late Fields ================= -->
<asp:Panel ID="pnlLate" runat="server" Visible="false" CssClass="row g-3 mt-3">
    <div class="col-md-3">
        <label>Grace Minutes</label>
        <asp:TextBox ID="txtLateGrace" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3">
        <label>Late Allowance</label>
        <asp:TextBox ID="txtLateAllowance" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3">
        <label>Half Day Threshold (min)</label>
        <asp:TextBox ID="txtLateHalfDay" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3">
        <label>Absent Threshold (min)</label>
        <asp:TextBox ID="txtLateAbsent" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
</asp:Panel>

<!-- ================= Early Leave Fields ================= -->
<asp:Panel ID="pnlEarlyLeave" runat="server" Visible="false" CssClass="row g-3 mt-3">
    <div class="col-md-3">
        <label>Grace Minutes</label>
        <asp:TextBox ID="txtEarlyGrace" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3">
        <label>Allowed Early Leaves</label>
        <asp:TextBox ID="txtAllowedEarlyLeaves" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3">
        <label>Half Day Threshold (min)</label>
        <asp:TextBox ID="txtEarlyHalfDay" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
</asp:Panel>

<!-- ================= Overtime Fields ================= -->
<asp:Panel ID="pnlOvertime" runat="server" Visible="false" CssClass="row g-3 mt-3">
    <div class="col-md-3">
        <label>Pre-Shift OT (min)</label>
        <asp:TextBox ID="txtPreShiftOT" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3">
        <label>Post-Shift OT (min)</label>
        <asp:TextBox ID="txtPostShiftOT" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3">
        <label>Max OT Hours</label>
        <asp:TextBox ID="txtMaxOT" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
    <div class="col-md-3 d-flex align-items-center">
        <div class="form-check me-2">
            <asp:CheckBox ID="chkWeekendOT" runat="server" CssClass="form-check-input" />
            <label class="form-check-label">Weekend OT</label>
        </div>
        <div class="form-check">
            <asp:CheckBox ID="chkHolidayOT" runat="server" CssClass="form-check-input" />
            <label class="form-check-label">Holiday OT</label>
        </div>
    </div>
</asp:Panel>

<!-- ================= Weekly Off Fields ================= -->
<asp:Panel ID="pnlWeeklyOff" runat="server" Visible="false" CssClass="row g-3 mt-3">
    <div class="col-md-4">
        <label>Weekly Off Pattern</label>
        <asp:DropDownList ID="ddlWeeklyPattern" runat="server" CssClass="form-select">
            <asp:ListItem Value="Fri/Sat">Fri/Sat</asp:ListItem>
            <asp:ListItem Value="Sun">Sun</asp:ListItem>
            <asp:ListItem Value="Sat/Sun">Sat/Sun</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="col-md-4">
        <label>Branch Holiday ID</label>
        <asp:TextBox ID="txtBranchHolidayID" runat="server" CssClass="form-control" TextMode="Number" />
    </div>
</asp:Panel>
    

                    </div>

                    <!-- Buttons -->
                    <div class="mt-4 text-end">
                        <asp:Button ID="btnSaveRule" runat="server"
                            CssClass="btn btn-success px-4"
                            Text="Save Rule"
                            OnClick="btnSaveRule_Click" />
                    </div>

                </div>
            </div>

        </div>
    </div>

  <!-- ================= RULE GRID ================= -->
<div class="row">
    <div class="col-12">

        <div class="card shadow-sm">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h6 class="mb-0">Existing Rules</h6>

                <!-- Global Search -->
                <div class="d-flex">
                    <input type="text" id="txtSearch" class="form-control form-control-sm"
                           placeholder="Search Rules..." onkeyup="searchRules(this.value)" />
                </div>
            </div>

            <div class="card-body table-responsive">

                <asp:Repeater ID="rptRules" runat="server" OnItemCommand="rptRules_ItemCommand">
                    <HeaderTemplate>
                        <table class="table table-striped table-hover align-middle">
                            <thead>
                                <tr>
                                    <th>Rule Type</th>
                                    <th>Rule Name</th>
                                    <th style="width:120px;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("RuleType") %></td>
                            <td><%# Eval("RuleName") %></td>
                            <td>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CssClass="text-primary me-2"
                                    CommandName="EditRule"
                                    CommandArgument='<%# Eval("RuleID") %>'>
                                    <i class="uil uil-edit"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CssClass="text-danger"
                                    CommandName="DeleteRule"
                                    CommandArgument='<%# Eval("RuleID") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this rule?');">
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

                <!-- ================= Pagination ================= -->
                <div class="d-flex justify-content-between align-items-center mt-3">
                    <asp:LinkButton ID="btnPrev" runat="server"
                        CssClass="btn btn-outline-secondary btn-sm"
                        OnClick="btnPrev_Click">Previous</asp:LinkButton>

                    <asp:Label ID="lblPageInfo" runat="server" CssClass="fw-bold"></asp:Label>

                    <asp:LinkButton ID="btnNext" runat="server"
                        CssClass="btn btn-outline-secondary btn-sm"
                        OnClick="btnNext_Click">Next</asp:LinkButton>
                </div>

            </div>
        </div>

    </div>
</div>

<asp:HiddenField ID="hfRuleID" runat="server" Value="0" />

<!-- ================= JS ================= -->
<script type="text/javascript">
    function searchRules(searchText) {
        __doPostBack('<%= rptRules.UniqueID %>', 'search$' + searchText);
    }
</script>

</div>

</asp:Content>
