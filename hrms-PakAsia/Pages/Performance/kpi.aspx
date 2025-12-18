<%@ Page Title="Employee KPI" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="kpi.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Performance.kpi" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<asp:UpdatePanel ID="upKPI" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<main class="main mt-10" id="top">
<div class="container">

<div class="row py-5">

    <!-- Heading -->
    <div class="text-center mb-4">
        <h3 class="mt-3">Employee KPI Evaluation</h3>
        <p class="text-body-tertiary">Calculate monthly performance indicators</p>
    </div>

    <!-- KPI INPUT FORM -->
    <div class="row">

        <div class="col-sm-3 mt-3">
            <label class="form-label">Employee</label>
            <asp:DropDownList ID="ddlEmployee" runat="server"
                CssClass="form-control"
                DataTextField="Name"
                DataValueField="ID" />
        </div>

        <div class="col-sm-3 mt-3">
            <label class="form-label">Month</label> 
            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"  DataTextField="Name" DataValueField="ID">
      
            </asp:DropDownList>
        </div>

        <div class="col-sm-3 mt-3">
            <label class="form-label">Attendance (%)</label>
            <asp:TextBox ID="txtAttendance" runat="server"
                CssClass="form-control" placeholder="e.g. 95" />
        </div>

        <div class="col-sm-3 mt-3">
            <label class="form-label">Punctuality (%)</label>
            <asp:TextBox ID="txtPunctuality" runat="server"
                CssClass="form-control" placeholder="e.g. 90" />
        </div>

        <div class="col-sm-3 mt-3">
            <label class="form-label">Task Completion (%)</label>
            <asp:TextBox ID="txtTaskCompletion" runat="server"
                CssClass="form-control" placeholder="e.g. 85" />
        </div>

        <div class="col-sm-3 mt-3">
            <label class="form-label">Overtime Hours</label>
            <asp:TextBox ID="txtOvertime" runat="server"
                CssClass="form-control" placeholder="e.g. 12" />
        </div>

        <div class="col-sm-3 mt-3">
            <label class="form-label">Final KPI Score</label>
            <asp:TextBox ID="txtFinalScore" runat="server"
                CssClass="form-control fw-bold" ReadOnly="true" />
        </div>

    </div>

    <!-- Buttons -->
    <div class="text-end mt-4">
        <asp:Button ID="btnCalculate" runat="server"
            Text="Calculate KPI"
            CssClass="btn btn-primary me-2"
            OnClick="btnCalculate_Click" />

        <asp:Button ID="btnSave" runat="server"
            Text="Save"
            CssClass="btn btn-success"
            OnClick="btnSave_Click" />
    </div>

    <asp:PlaceHolder ID="phAlert" runat="server" />
    <asp:TextBox runat="server" CssClass="form-control" ID="txtSearch" placeholder="Search..."/> 
    <!-- KPI TABLE -->
    <div class="table-responsive mt-4">
        <asp:Repeater ID="rptKPI" runat="server">
            <HeaderTemplate>
                <table class="table table-striped table-sm">
                    <thead>
                        <tr>
                            <th>Employee</th>
                            <th>Month</th>
                            <th>Attendance</th>
                            <th>Punctuality</th>
                            <th>Tasks</th>
                            <th>Overtime</th>
                            <th>Final Score</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>

            <ItemTemplate>
                <tr>
                    <td><%# Eval("EmployeeName") %></td>
                    <td><%# Eval("MonthName") %></td>
                    <td><%# Eval("AttendancePct") %>%</td>
                    <td><%# Eval("PunctualityPct") %>%</td>
                    <td><%# Eval("TaskCompletion") %>%</td>
                    <td><%# Eval("OvertimeHours") %></td>
                    <td class="fw-bold"><%# Eval("FinalScore") %></td>
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
</main>

</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
