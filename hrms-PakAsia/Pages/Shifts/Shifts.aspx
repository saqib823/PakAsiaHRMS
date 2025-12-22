<%@ Page Title="Shifts" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="Shifts.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Shifts.Shifts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid">

    <!-- ================= SHIFT FORM ================= -->
    <div class="row mt-10">
        <div class="col-12">

            <div class="card shadow-sm mb-4">
                <div class="card-header text-white">
                    <h5 class="mb-0">Shift Master</h5>
                </div>

                <div class="card-body">

                    <div class="row g-3">

                        <!-- Shift Name -->
                        <div class="col-md-4">
                            <label class="form-label">Shift Name</label>
                            <asp:TextBox ID="txtShiftName" runat="server"
                                CssClass="form-control"
                                placeholder="Morning / Evening / Night" />
                        </div>

                        <!-- Shift Type -->
                        <div class="col-md-4">
                            <label class="form-label">Shift Type</label>
                            <asp:DropDownList ID="ddlShiftType" runat="server"
                                CssClass="form-select">
                            </asp:DropDownList>
                        </div>

                        <!-- Grace Minutes -->
                        <div class="col-md-4">
                            <label class="form-label">Grace Minutes</label>
                            <asp:TextBox ID="txtGraceMinutes" runat="server"
                                CssClass="form-control"
                                TextMode="Number" />
                        </div>

                        <!-- Start Time -->
                        <div class="col-md-3">
                            <label class="form-label">Start Time</label>
                            <asp:TextBox ID="txtStartTime" runat="server"
                                CssClass="form-control"
                                TextMode="Time" />
                        </div>

                        <!-- End Time -->
                        <div class="col-md-3">
                            <label class="form-label">End Time</label>
                            <asp:TextBox ID="txtEndTime" runat="server"
                                CssClass="form-control"
                                TextMode="Time" />
                        </div>

                        <!-- Minimum Work Minutes -->
                        <div class="col-md-3">
                            <label class="form-label">Min Work Minutes</label>
                            <asp:TextBox ID="txtMinWorkMinutes" runat="server"
                                CssClass="form-control"
                                TextMode="Number" />
                        </div>

                        <!-- Cross Midnight -->
                        <div class="col-md-3 d-flex align-items-end">
                            <div class="form-check">
                                <asp:CheckBox ID="chkCrossMidnight" runat="server"
                                    CssClass="form-check-input" />
                                <label class="form-check-label ms-2">
                                    Cross Midnight (Night Shift)
                                </label>
                            </div>
                        </div>

                    </div>

                    <!-- Buttons -->
                    <div class="mt-4 text-end">
                        <asp:Button ID="btnSave" runat="server"
                            CssClass="btn btn-success px-4"
                            Text="Save Shift"
                            OnClick="btnSave_Click" />
                    </div>

                </div>
            </div>

        </div>
    </div>

  <!-- ================= SHIFT GRID ================= -->
<div class="row">
    <div class="col-12">

        <div class="card shadow-sm">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h6 class="mb-0">Existing Shifts</h6>
            </div>

            <div class="card-body table-responsive">

                <asp:Repeater ID="rptShifts" runat="server" OnItemCommand="rptShifts_ItemCommand">
                    <HeaderTemplate>
                        <table class="table table-striped table-hover align-middle">
                            <thead>
                                <tr>
                                    <th>Shift Name</th>
                                    <th>Type</th>
                                    <th>Start</th>
                                    <th>End</th>
                                    <th>Grace</th>
                                    <th>Min Work</th>
                                    <th>Night Shift</th>
                                    <th style="width:120px;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("ShiftName") %></td>
                            <td><%# Eval("ShiftTypeName") %></td>
                            <td><%# Eval("StartTime") %></td>
                            <td><%# Eval("EndTime") %></td>
                            <td><%# Eval("GraceMinutes") %></td>
                            <td><%# Eval("MinWorkMinutes") %></td>
                            <td>
                                <span class='<%# Convert.ToBoolean(Eval("IsCrossMidnight")) ? "badge bg-warning" : "badge bg-success" %>'>
                                    <%# Convert.ToBoolean(Eval("IsCrossMidnight")) ? "Yes" : "No" %>
                                </span>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CssClass="text-primary me-2"
                                    CommandName="EditShift"
                                    CommandArgument='<%# Eval("ShiftID") %>'>
                                   <i class="uil uil-edit"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CssClass="text-danger"
                                    CommandName="DeleteShift"
                                    CommandArgument='<%# Eval("ShiftID") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this shift?');">
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

    <asp:HiddenField ID="hfShiftID" runat="server" Value="0" />

</div>

<!-- ================= JS (SAFE FOR POSTBACK) ================= -->
<script type="text/javascript">
    function toggleEndTime() {
        var ddl = document.getElementById('<%= ddlShiftType.ClientID %>');
        var endTime = document.getElementById('<%= txtEndTime.ClientID %>');

        if (!ddl || !endTime) return;

        var selectedText = ddl.options[ddl.selectedIndex].text;
        endTime.disabled = (selectedText === 'Flexible');
    }

    document.addEventListener("DOMContentLoaded", function () {
        toggleEndTime();

        var ddl = document.getElementById('<%= ddlShiftType.ClientID %>');
        if (ddl) {
            ddl.addEventListener('change', toggleEndTime);
        }
    });
</script>

</asp:Content>
