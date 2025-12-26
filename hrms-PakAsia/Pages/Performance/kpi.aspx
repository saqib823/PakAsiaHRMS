<%@ Page Title="Employee KPI" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="kpi.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Performance.kpi" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<asp:UpdatePanel runat="server">
<ContentTemplate>

<div class="container py-4 mt-10">

<h3 class="text-center mb-4">Employee KPI Evaluation</h3>

<div class="row g-3">

    <div class="col-md-3">
        <label>Employee</label>
        <asp:DropDownList ID="ddlEmployee" runat="server"
            CssClass="form-control"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged"/>
    </div>

    <div class="col-md-3">
        <label>Month</label>
        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"/>
    </div>

    <div class="col-md-3">
        <label>Period</label>
        <asp:DropDownList ID="ddlPeriodType" runat="server" CssClass="form-control">
            <asp:ListItem Text="Monthly" Value="M"/>
            <asp:ListItem Text="Quarterly" Value="Q"/>
        </asp:DropDownList>
    </div>

    <div class="col-md-3">
        <label>Goal Achievement</label>
        <asp:TextBox ID="txtGoal" runat="server" CssClass="form-control" ReadOnly="true"/>
    </div>

    <div class="col-md-3"><label>Attendance %</label><asp:TextBox ID="txtAttendance" runat="server" CssClass="form-control"/></div>
    <div class="col-md-3"><label>Punctuality %</label><asp:TextBox ID="txtPunctuality" runat="server" CssClass="form-control"/></div>
    <div class="col-md-3"><label>Task Completion %</label><asp:TextBox ID="txtTaskCompletion" runat="server" CssClass="form-control"/></div>
    <div class="col-md-3"><label>Overtime</label><asp:TextBox ID="txtOvertime" runat="server" CssClass="form-control"/></div>

    <div class="col-md-3">
        <label>Final Score</label>
        <asp:TextBox ID="txtFinalScore" runat="server" CssClass="form-control fw-bold" ReadOnly="true"/>
    </div>
</div>

<div class="text-end mt-3">
    <asp:Button ID="btnCalculate" runat="server" Text="Calculate"
        CssClass="btn btn-primary" OnClick="btnCalculate_Click"/>
    <asp:Button ID="btnSave" runat="server" Text="Save"
        CssClass="btn btn-success ms-2" OnClick="btnSave_Click"/>
</div>

<asp:PlaceHolder ID="phAlert" runat="server"/>

<hr />

<asp:TextBox ID="txtSearch" runat="server" CssClass="form-control mb-2"
    Placeholder="Search..." AutoPostBack="true"
    OnTextChanged="txtSearch_TextChanged"/>

<asp:Repeater ID="rptKPI" runat="server">
    <HeaderTemplate>
        <table class="table table-striped">
        <thead><tr>
            <th>Employee</th><th>Month</th><th>Score</th><th>Action</th>
        </tr></thead><tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("EmployeeName") %></td>
            <td><%# Eval("MonthName") %></td>
            <td><%# Eval("FinalScore") %></td>
            <td>
                <asp:LinkButton runat="server" Text="Delete"
                    CommandArgument='<%# Eval("KPIID") %>'
                    OnCommand="DeleteKPI"
                    OnClientClick="return confirm('Delete KPI?')"/>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate></tbody></table></FooterTemplate>
</asp:Repeater>

</div>

</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
