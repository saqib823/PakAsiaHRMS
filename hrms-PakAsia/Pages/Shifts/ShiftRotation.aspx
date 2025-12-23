<%@ Page Title="Shift Rotation" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="ShiftRotation.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Shifts.ShiftRotation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container-fluid">

    <!-- ROTATION FORM -->
    <div class="row mt-10">
        <div class="col-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header">
                    <h5 class="mb-0" id="lblFormTitle">Add Rotation</h5>
                </div>
                <div class="card-body">
                    <div class="row g-3">
                        <div class="col-md-4">
                            <label class="form-label">Employee</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Shift</label>
                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Date</label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="mt-4 text-end">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" Text="Save Rotation" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary px-4 ms-2" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- ROTATION GRID -->
    <div class="row">
        <div class="col-12">
            <div class="card shadow-sm">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h6 class="mb-0">Existing Shift Rotations</h6>
                </div>
                <div class="card-body table-responsive">
                    <asp:Repeater ID="rptRotations" runat="server" OnItemCommand="rptRotations_ItemCommand">
                        <HeaderTemplate>
                            <table class="table table-striped table-hover align-middle">
                                <thead>
                                    <tr>
                                        <th>Employee</th>
                                        <th>Shift</th>
                                        <th>Date</th>
                                        <th style="width:120px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("EmployeeName") %></td>
                                <td><%# Eval("ShiftName") %></td>
                                <td><%# Convert.ToDateTime(Eval("RotationDate")).ToString("yyyy-MM-dd") %></td>
                                <td>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="text-primary me-2"
                                        CommandName="Edit" CommandArgument='<%# Eval("RotationID") %>'>
                                        <i class="uil uil-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="text-danger"
                                        CommandName="Delete" CommandArgument='<%# Eval("RotationID") %>'
                                        OnClientClick="return confirm('Are you sure you want to delete this rotation?');">
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

                    <!-- PAGINATION -->
                    <div class="d-flex justify-content-between align-items-center mt-3">
                        <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnPrev_Click">Previous</asp:LinkButton>
                        <asp:Label ID="lblPageInfo" runat="server" CssClass="fw-bold"></asp:Label>
                        <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnNext_Click">Next</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfRotationID" runat="server" Value="0" />

</div>
</asp:Content>
