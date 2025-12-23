<%@ Page Title="Department Shifts" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="DepartmentShifts.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Shifts.DepartmentShifts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid">

    <!-- ================= FORM ================= -->
    <div class="row mt-10">
        <div class="col-lg-12">

            <div class="card shadow-sm">
                <div class="card-header">
                    <h5 class="mb-0">Assign Shift to Department</h5>
                </div>

                <div class="card-body">

                    <div class="row g-3">

                        <!-- Department -->
                        <div class="col-md-6">
                            <label class="form-label">Department</label>
                            <asp:DropDownList ID="ddlDepartment" runat="server" DataTextField="Name" DataValueField="ID"
                                CssClass="form-select">
                            </asp:DropDownList>
                        </div>

                        <!-- Shift -->
                        <div class="col-md-6">
                            <label class="form-label">Shift</label>
                            <asp:DropDownList ID="ddlShift" runat="server"  DataTextField="Name" DataValueField="ID"
                                CssClass="form-select">
                            </asp:DropDownList>
                        </div>

                        <!-- Default -->
                        <div class="col-md-12">
                            <div class="form-check mt-2">
                                <asp:CheckBox ID="chkDefault" runat="server"
                                    CssClass="form-check-input" />
                                <label class="form-check-label ms-2">
                                    Set as Default Shift for Department
                                </label>
                            </div>
                        </div>

                    </div>

                    <!-- Buttons -->
                    <div class="mt-4 text-end">
                        <asp:Button ID="btnSave" runat="server"
                            CssClass="btn btn-success px-4"
                            Text="Save"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server"
                            CssClass="btn btn-secondary px-4 ms-2"
                            Text="Cancel"
                            CausesValidation="false"
                            OnClick="btnCancel_Click" />
                    </div>

                </div>
            </div>

        </div>
    </div>

    <!-- ================= GRID ================= -->
    <div class="row mt-4">
        <div class="col-12">

            <div class="card shadow-sm">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h6 class="mb-0">Department Shift Mapping</h6>
                </div>

                <div class="card-body table-responsive">

                    <asp:Repeater ID="rptDepartmentShifts" runat="server"
                        OnItemCommand="rptDepartmentShifts_ItemCommand">

                        <HeaderTemplate>
                            <table class="table table-striped table-hover align-middle">
                                <thead>
                                    <tr>
                                        <th>Department</th>
                                        <th>Shift</th>
                                        <th>Default</th>
                                        <th style="width:120px;">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("DepartmentName") %></td>
                                <td><%# Eval("ShiftName") %></td>
                                <td>
                                    <span class='<%# Convert.ToBoolean(Eval("IsDefault")) 
                                        ? "badge bg-success" 
                                        : "badge bg-secondary" %>'>
                                        <%# Convert.ToBoolean(Eval("IsDefault")) ? "Yes" : "No" %>
                                    </span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnEdit" runat="server"
                                        CssClass="text-primary me-2"
                                        CommandName="EditRow"
                                        CommandArgument='<%# Eval("DepartmentShiftID") %>'>
                                        <i class="uil uil-edit"></i>
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnDelete" runat="server"
                                        CssClass="text-danger"
                                        CommandName="DeleteRow"
                                        CommandArgument='<%# Eval("DepartmentShiftID") %>'
                                        OnClientClick="return confirm('Delete this mapping?');">
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

                </div>
            </div>

        </div>
    </div>

    <asp:HiddenField ID="hfDepartmentShiftID" runat="server" Value="0" />

</div>

</asp:Content>
