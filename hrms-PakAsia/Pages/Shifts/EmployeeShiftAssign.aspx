<%@ Page Title="Employee Shift Assignment"
    Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="EmployeeShiftAssign.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Shifts.EmployeeShiftAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid">

<!-- ========== ASSIGN SHIFT FORM ========== -->
<div class="card shadow-sm mb-4">
    <div class="card-header">
        <h5 class="mb-0">Assign Shift to Employee</h5>
    </div>
    <div class="card-body">
        <div class="row g-3">

            <!-- Employee -->
            <div class="col-md-4">
                <label class="form-label">Employee</label>
                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-select" />
            </div>

            <!-- Shift -->
            <div class="col-md-4">
                <label class="form-label">Shift</label>
                <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-select" />
            </div>

            <!-- Effective From -->
            <div class="col-md-2">
                <label class="form-label">Effective From</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <!-- Effective To -->
            <div class="col-md-2">
                <label class="form-label">Effective To</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

        </div>

        <div class="mt-4 text-end">
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" Text="Save" OnClick="btnSave_Click" />
        </div>
    </div>
</div>

<!-- ========== ASSIGNED SHIFTS GRID ========== -->
<div class="card shadow-sm">
    <div class="card-header">
        <h6 class="mb-0">Assigned Shifts</h6>
    </div>
    <div class="card-body table-responsive">
        <asp:Repeater ID="rptShifts" runat="server" OnItemCommand="rptShifts_ItemCommand">
            <HeaderTemplate>
                <table class="table table-striped align-middle">
                    <thead>
                        <tr>
                            <th>Employee</th>
                            <th>Shift</th>
                            <th>Effective From</th>
                            <th>Effective To</th>
                            <th width="120">Action</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>

            <ItemTemplate>
                <tr>
                    <td><%# Eval("FullName") %></td>
                    <td><%# Eval("ShiftName") %></td>
                    <td><%# Convert.ToDateTime(Eval("EffectiveFrom")).ToString("yyyy-MM-dd") %></td>
                    <td><%# Eval("EffectiveTo") == DBNull.Value ? "" : Convert.ToDateTime(Eval("EffectiveTo")).ToString("yyyy-MM-dd") %></td>
                    <td>
                        <asp:LinkButton runat="server"
                            CommandName="Edit"
                            CommandArgument='<%# Eval("EmployeeShiftID") %>'
                            CssClass="text-primary me-2">
                            <i class="uil uil-edit"></i>
                        </asp:LinkButton>

                        <asp:LinkButton runat="server"
                            CommandName="Delete"
                            CommandArgument='<%# Eval("EmployeeShiftID") %>'
                            CssClass="text-danger"
                            OnClientClick="return confirm('Delete this shift assignment?');">
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

        <!-- Pagination -->
        <div class="d-flex justify-content-between mt-3">
            <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnPrev_Click">Previous</asp:LinkButton>
            <asp:Label ID="lblPage" runat="server" CssClass="fw-bold"></asp:Label>
            <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnNext_Click">Next</asp:LinkButton>
        </div>

    </div>
</div>

<asp:HiddenField ID="hfShiftID" runat="server" Value="0" />

</div>
</asp:Content>

