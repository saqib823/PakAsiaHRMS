<%@ Page Title="Manual Attendance" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="manualAttenndence.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Attendance.manualAttenndence" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<asp:UpdatePanel ID="upAttendance" runat="server">
<ContentTemplate>
<main class="main mt-10">
<div class="container">

<div class="text-center mb-4">
    <h3>Manual Attendance</h3>
    <p class="text-body-tertiary">Add or edit manual punches</p>
</div>

<div class="row">

    <div class="col-sm-3 mt-3">
        <label>Emp No</label>
        <asp:DropDownList ID="ddlEmployees" DataTextField="Name" DataValueField="ID" runat="server" CssClass="form-select"
            data-choices="data-choices" data-options='{"removeItemButton":true,"placeholder":true}'></asp:DropDownList>
    </div>

    <div class="col-sm-3 mt-3">
        <label>Punch Date</label>
        <asp:TextBox ID="txtPunchDate" runat="server" TextMode="Date" CssClass="form-control" />
    </div>

    <div class="col-sm-3 mt-3">
        <label>Punch Time</label>
        <asp:TextBox ID="txtPunchTime" runat="server" TextMode="Time" CssClass="form-control" />
    </div>

    <div class="col-sm-3 mt-3">
        <label>Punch Type</label>
        <asp:DropDownList ID="ddlPunchType" runat="server" CssClass="form-select">
            <asp:ListItem Text="IN" Value="IN" />
            <asp:ListItem Text="OUT" Value="OUT" />
        </asp:DropDownList>
    </div>

    <div class="col-sm-12 text-end mt-4">
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary me-2" OnClick="btnClear_Click" />
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>

</div>

<asp:PlaceHolder ID="phAlert" runat="server" />

<hr />

<div class="search-box mb-3">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" Placeholder="Search attendance..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
 
    <div class="input-group-append">
        <asp:LinkButton 
            ID="btnRefresh" 
            runat="server" 
            CssClass="btn btn-outline-secondary" 
            OnClick="btnRefresh_Click">
            <i class="fa fa-refresh"></i>
        </asp:LinkButton>
    </div>
</div>

<div class="table-responsive">
<asp:Repeater ID="rptAttendance" runat="server" OnItemCommand="rptAttendance_ItemCommand">
<HeaderTemplate>
<table class="table table-striped table-sm">
<thead>
<tr>
    <th>Emp No</th>
    <th>Name</th>
    <th>Punch Date</th>
    <th>Punch Time</th>
    <th>Type</th>
    <th>Action</th>
</tr>
</thead>
<tbody>
</HeaderTemplate>

<ItemTemplate>
<tr>
<td><%# Eval("EmpNo") %></td>
<td><%# Eval("FullName") %></td>
<td><%# Eval("PunchDate", "{0:yyyy-MM-dd}") %></td>
<td><%# DateTime.Today.Add((TimeSpan)Eval("PunchDateTime")).ToString("hh:mm tt") %></td>
<td><%# Eval("PunchType") %></td>
<td>
    <asp:LinkButton runat="server" CommandName="Edit" CommandArgument='<%# Eval("AttendanceLogID") %>' CssClass="text-primary me-2">Edit</asp:LinkButton>
    <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("AttendanceLogID") %>' CssClass="text-danger" OnClientClick="return confirm('Delete this record?');">Delete</asp:LinkButton>
</td>
</tr>
</ItemTemplate>

<FooterTemplate>
</tbody></table>
</FooterTemplate>
</asp:Repeater>
</div>

<div class="d-flex justify-content-between mt-3">
    <asp:Label ID="lblPageInfo" runat="server" CssClass="text-muted" />
    <div>
        <asp:LinkButton ID="btnPrev" runat="server" Text="«" CssClass="btn btn-outline-secondary btn-sm me-1" OnClick="btnPrev_Click" />
        <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="Page" CommandArgument='<%# Eval("PageNumber") %>' CssClass='<%# (bool)Eval("IsCurrent") ? "btn btn-primary btn-sm me-1" : "btn btn-outline-secondary btn-sm me-1" %>'>
                    <%# Eval("PageNumber") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
        <asp:LinkButton ID="btnNext" runat="server" Text="»" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnNext_Click" />
    </div>
</div>

</div>
</main>
</ContentTemplate>
</asp:UpdatePanel>


</asp:Content>
