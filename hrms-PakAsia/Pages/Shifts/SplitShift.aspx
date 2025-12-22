<%@ Page Title="Split Shift"
    Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="SplitShift.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Shifts.SplitShift" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid">

    <!-- SPLIT SHIFT FORM -->
    <div class="row mt-10">
        <div class="col-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header text-white">
                    <h5 class="mb-0">Split Shift Setup</h5>
                </div>
                <div class="card-body">

                    <!-- Shift Dropdown -->
                    <div class="row g-3 mb-4">
                        <div class="col-md-6">
                            <label class="form-label">Select Split Shift</label>
                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                    </div>

                    <!-- Part 1 -->
                    <h6 class="text-muted mb-2">Part 1</h6>
                    <div class="row g-3 mb-3">
                        <div class="col-md-3">
                            <label class="form-label">Start Time</label>
                            <asp:TextBox ID="txtPart1Start" runat="server" CssClass="form-control" TextMode="Time" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">End Time</label>
                            <asp:TextBox ID="txtPart1End" runat="server" CssClass="form-control" TextMode="Time" />
                        </div>
                    </div>

                    <!-- Part 2 -->
                    <h6 class="text-muted mb-2">Part 2</h6>
                    <div class="row g-3 mb-4">
                        <div class="col-md-3">
                            <label class="form-label">Start Time</label>
                            <asp:TextBox ID="txtPart2Start" runat="server" CssClass="form-control" TextMode="Time" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">End Time</label>
                            <asp:TextBox ID="txtPart2End" runat="server" CssClass="form-control" TextMode="Time" />
                        </div>
                    </div>

                    <div class="text-end">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" Text="Save Split Shift" OnClick="btnSave_Click" />
                    </div>

                </div>
            </div>
        </div>
    </div>

    <!-- SPLIT SHIFT GRID -->
    <div class="row">
        <div class="col-12">
            <div class="card shadow-sm">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h6 class="mb-0">Existing Split Shifts</h6>
                </div>
                <div class="card-body table-responsive">

                    <asp:Repeater ID="rptSplitShifts" runat="server" OnItemCommand="rptSplitShifts_ItemCommand">
                        <HeaderTemplate>
                            <table class="table table-striped table-hover align-middle">
                                <thead>
                                    <tr>
                                        <th>Shift</th>
                                        <th>Part</th>
                                        <th>Start</th>
                                        <th>End</th>
                                        <th style="width:120px;">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ShiftName") %></td>
                                <td>Part <%# Eval("PartNo") %></td>
                                <td><%# Eval("StartTime") %></td>
                                <td><%# Eval("EndTime") %></td>
                                <td>
                                    <asp:LinkButton runat="server" CssClass="text-primary me-2" CommandName="EditRow" CommandArgument='<%# Eval("SplitShiftID") %>'>
                                        <i class="uil uil-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="text-danger" CommandName="DeleteRow" CommandArgument='<%# Eval("SplitShiftID") %>' OnClientClick="return confirm('Delete this record?');">
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
                    <div class="d-flex justify-content-center mt-3">
                        <asp:LinkButton ID="btnPrev" runat="server" OnClick="btnPrev_Click" CssClass="btn btn-outline-secondary me-2">Previous</asp:LinkButton>
                        <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Page" CommandArgument='<%# Eval("PageIndex") %>' CssClass='<%# (bool)Eval("IsCurrent") ? "btn btn-sm btn-primary mx-1" : "btn btn-sm btn-outline-primary mx-1" %>'>
                                    <%# Eval("Text") %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="btn btn-outline-secondary ms-2">Next</asp:LinkButton>
                    </div>
                    <div class="text-center mt-2">
                        <asp:Label ID="lblPageInfo" runat="server" CssClass="fw-bold"></asp:Label>
                    </div>

                </div>
            </div>
        </div>
    </div>

</div>
</asp:Content>
