<%@ Page Title="Salary Structure" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="SalaryStructure.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Payroll.SalaryStructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- ================= SALARY STRUCTURE FORM ================= -->
        <div class="row mt-10">
            <div class="col-12">

                <div class="card shadow-sm mb-4">
                    <div class="card-header text-white">
                        <h5 class="mb-0">Salary Structure Master</h5>
                    </div>

                    <div class="card-body">

                        <div class="row g-3">

                            <!-- Employee -->
                            <div class="col-md-4">
                                <label class="form-label">Employee</label>
                                <asp:DropDownList ID="ddlEmployee" runat="server"
                                    CssClass="form-select">
                                </asp:DropDownList>
                            </div>

                            <!-- Effective From -->
                            <div class="col-md-4">
                                <label class="form-label">Effective From</label>
                                <asp:TextBox ID="txtEffectiveFrom" runat="server"
                                    CssClass="form-control"
                                    TextMode="Date" />
                            </div>

                            <!-- Status -->
                            <div class="col-md-4">
                                <label class="form-label">Status</label>
                                <asp:DropDownList ID="ddlStatus" runat="server"
                                    CssClass="form-select">
                                    <asp:ListItem Text="Active" Value="1" />
                                    <asp:ListItem Text="Inactive" Value="0" />
                                </asp:DropDownList>
                            </div>

                        </div>

                        <hr />

                        <!-- ================= EARNINGS ================= -->
                        <h6 class="fw-bold text-primary mb-3">Earnings</h6>

                        <div class="row g-3">

                            <div class="col-md-3">
                                <label class="form-label">Basic</label>
                                <asp:TextBox ID="txtBasic" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">House Rent</label>
                                <asp:TextBox ID="txtHouseRent" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Utilities</label>
                                <asp:TextBox ID="txtUtilities" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Medical</label>
                                <asp:TextBox ID="txtMedical" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Fuel</label>
                                <asp:TextBox ID="txtFuel" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Transport</label>
                                <asp:TextBox ID="txtTransport" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Mobile</label>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Bonus</label>
                                <asp:TextBox ID="txtBonus" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Commission</label>
                                <asp:TextBox ID="txtCommission" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-3">
                                <label class="form-label">Incentives</label>
                                <asp:TextBox ID="txtIncentives" runat="server" CssClass="form-control" TextMode="Number" />
                            </div>

                            <div class="col-md-6">
                                <label class="form-label">Custom Allowances</label>
                                <asp:TextBox ID="txtCustomAllowances" runat="server"
                                    CssClass="form-control"
                                    placeholder="Other allowances (if any)" />
                            </div>

                        </div>

                        <hr />

                        <!-- ================= DEDUCTIONS ================= -->
                        <h6 class="fw-bold text-danger mb-3">Deductions</h6>

                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label">Custom Deductions</label>
                                <asp:TextBox ID="txtDeductions" runat="server"
                                    CssClass="form-control"
                                    placeholder="Tax, Loan, etc." />
                            </div>
                        </div>

                        <!-- Buttons -->
                        <div class="mt-4 text-end">
                            <asp:Button ID="btnSave" runat="server"
                                CssClass="btn btn-success px-4"
                                Text="Save Salary Structure"
                                OnClick="btnSave_Click" />
                        </div>

                    </div>
                </div>

            </div>
        </div>

        <!-- ================= SALARY STRUCTURE GRID ================= -->
        <div class="row">
            <div class="col-12">

                <div class="card shadow-sm">
                    <div class="card-header">
                        <h6 class="mb-0">Existing Salary Structures</h6>
                    </div>

                    <div class="card-body table-responsive">
                        <div class="row mb-3">
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearch" runat="server"
                                    CssClass="form-control"
                                    placeholder="Search employee or salary..." />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSearch" runat="server"
                                    Text="Search"
                                    CssClass="btn btn-primary"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>

                        <asp:Repeater ID="rptSalary" runat="server" OnItemCommand="rptSalary_ItemCommand">

                            <HeaderTemplate>
                                <table class="table table-striped table-hover align-middle">
                                    <thead>
                                        <tr>
                                            <th>Employee</th>
                                            <th>Basic</th>
                                            <th>Gross Salary</th>
                                            <th>Effective From</th>
                                            <th>Status</th>
                                            <th style="width: 120px;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("FullName") %></td>
                                    <td><%# Eval("Basic") %></td>
                                    <td><%# Eval("GrossSalary") %></td>
                                    <td><%# Eval("EffectiveFrom","{0:dd-MMM-yyyy}") %></td>
                                    <td>
                                        <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-secondary" %>'>
                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnEdit" runat="server"
                                            CssClass="text-primary me-2"
                                            CommandName="Edit"
                                            CommandArgument='<%# Eval("SalaryID") %>'>
    <i class="uil uil-edit"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnDelete" runat="server"
                                            CssClass="text-danger  me-2"
                                            CommandName="Delete"
                                            CommandArgument='<%# Eval("SalaryID") %>'
                                            OnClientClick="return confirm('Delete salary structure?');">
    <i class="uil uil-trash-alt"></i>     
                                        </asp:LinkButton>

                                            <asp:LinkButton ID="btnExportPDF" runat="server"
                                            CssClass="text-secondary  me-2"
                                            CommandName="ExportPDF"
                                            CommandArgument='<%# Eval("SalaryID") %>'>
    <i class="uil uil-document-info"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnExportExcelF" runat="server"
                                            CssClass="text-success  me-2"
                                            CommandName="ExportExcel"
                                            CommandArgument='<%# Eval("SalaryID") %>'>
    <i class="uil uil-grid"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>

                            <FooterTemplate>
                                </tbody>
</table>
                            </FooterTemplate>

                        </asp:Repeater>
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

        <asp:HiddenField ID="hfSalaryID" runat="server" Value="0" />

    </div>
</asp:Content>
