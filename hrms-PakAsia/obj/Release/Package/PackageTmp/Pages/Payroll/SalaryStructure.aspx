<%@ Page Title="Payroll Automation" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="SalaryStructure.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Payroll.SalaryStructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- ================= SALARY STRUCTURE FORM ================= -->
        <div class="row mt-10">
            <div class="col-12">
                <div class="card shadow-sm mb-4">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Salary Structure Master</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3">
                            <!-- Employee -->
                            <div class="col-md-4">
                                <label class="form-label">Employee <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-select" required>
                                </asp:DropDownList>
                            </div>
                            <!-- Effective From -->
                            <div class="col-md-4">
                                <label class="form-label">Effective From <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>
                            <!-- Status -->
                            <div class="col-md-4">
                                <label class="form-label">Status</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Active" Value="1" Selected="True" />
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
                                <asp:TextBox ID="txtBasic" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">House Rent</label>
                                <asp:TextBox ID="txtHouseRent" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Utilities</label>
                                <asp:TextBox ID="txtUtilities" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Medical</label>
                                <asp:TextBox ID="txtMedical" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Fuel</label>
                                <asp:TextBox ID="txtFuel" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Transport</label>
                                <asp:TextBox ID="txtTransport" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Mobile</label>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Bonus</label>
                                <asp:TextBox ID="txtBonus" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Commission</label>
                                <asp:TextBox ID="txtCommission" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Incentives</label>
                                <asp:TextBox ID="txtIncentives" runat="server" CssClass="form-control" TextMode="Number" step="0.01" value="0" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Custom Allowances</label>
                                <asp:TextBox ID="txtCustomAllowances" runat="server" CssClass="form-control" placeholder="Other allowances" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Gross Salary</label>
                                <asp:TextBox ID="txtGrossSalary" runat="server" CssClass="form-control bg-light" ReadOnly="true" Text="0.00" />
                            </div>
                        </div>

                        <hr />

                        <!-- ================= DEDUCTIONS ================= -->
                        <h6 class="fw-bold text-danger mb-3">Deductions</h6>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label">Custom Deductions</label>
                                <asp:TextBox ID="txtDeductions" runat="server" CssClass="form-control" placeholder="Tax, Loan, etc." />
                            </div>
                        </div>

                        <!-- Buttons -->
                        <div class="mt-4 text-end">
                            <asp:Button ID="btnCalculate" runat="server" CssClass="btn btn-info px-4" Text="Calculate" OnClick="btnCalculate_Click" />
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4 ms-2" Text="Save Salary Structure" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- ================= PAYROLL PROCESSING SECTION ================= -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm mb-4">
                    <div class="card-header bg-success text-white">
                        <h5 class="mb-0">Payroll Processing</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3">
                            <div class="col-md-4">
                                <label class="form-label">Payroll Period</label>
                                <asp:DropDownList ID="ddlPayrollPeriod" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">&nbsp;</label>
                                <asp:Button ID="btnProcessPayroll" runat="server" CssClass="btn btn-warning w-100" Text="Process Payroll" 
                                    OnClick="btnProcessPayroll_Click" OnClientClick="return confirm('Process payroll for selected period?');" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">&nbsp;</label>
                                <asp:Button ID="btnGenerateReports" runat="server" CssClass="btn btn-info w-100" Text="Generate Reports" OnClick="btnGenerateReports_Click" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">&nbsp;</label>
                                <asp:Button ID="btnBankFile" runat="server" CssClass="btn btn-primary w-100" Text="Bank File" OnClick="btnBankFile_Click" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">&nbsp;</label>
                                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-success w-100" Text="Export Excel" OnClick="btnExportExcel_Click" />
                            </div>
                        </div>

                        <!-- Reports Selection Panel -->
                        <div class="row mt-3" id="divReports" runat="server" visible="false">
                            <div class="col-md-4">
                                <label class="form-label">Report Type</label>
                                <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Monthly Payroll Sheet" Value="MonthlySheet" />
                                    <asp:ListItem Text="Salary Register" Value="SalaryRegister" />
                                    <asp:ListItem Text="Department-wise Cost" Value="DepartmentCost" />
                                    <asp:ListItem Text="Year-to-Date" Value="YTD" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Start Date</label>
                                <asp:TextBox ID="txtReportStartDate" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">End Date</label>
                                <asp:TextBox ID="txtReportEndDate" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">&nbsp;</label>
                                <asp:Button ID="btnGenerateReport" runat="server" CssClass="btn btn-primary w-100" Text="Generate" OnClick="btnGenerateReport_Click" />
                            </div>
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
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search employee, designation or department..." />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                            </div>
                            <div class="col-md-6 text-end">
                                <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-bold" Visible="false"></asp:Label>
                            </div>
                        </div>

                        <!-- GridView instead of Repeater for better functionality -->
                        <asp:GridView ID="gvSalary" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover align-middle"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="gvSalary_PageIndexChanging"
                            OnRowCommand="gvSalary_RowCommand" DataKeyNames="SalaryID" OnRowDataBound="gvSalary_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="FullName" HeaderText="Employee" />
                                <asp:BoundField DataField="EmployeeNo" HeaderText="Emp No" />
                                <asp:BoundField DataField="DesignationID" HeaderText="Designation" />
                                <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                                <asp:BoundField DataField="Basic" HeaderText="Basic" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="GrossSalary" HeaderText="Gross Salary" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="EffectiveFrom" HeaderText="Effective From" DataFormatString="{0:dd-MMM-yyyy}" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-secondary" %>'>
                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="text-primary me-2" 
                                            CommandName="EditRecord" CommandArgument='<%# Eval("SalaryID") %>'
                                            ToolTip="Edit Salary Structure">
                                            <i class="uil uil-edit"></i>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="text-danger me-2"
                                            CommandName="DeleteRecord" CommandArgument='<%# Eval("SalaryID") %>'
                                            OnClientClick="return confirm('Delete this salary structure?');"
                                            ToolTip="Delete Salary Structure">
                                            <i class="uil uil-trash-alt"></i>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton ID="btnViewDetails" runat="server" CssClass="text-info me-2"
                                            CommandName="ViewDetails" CommandArgument='<%# Eval("SalaryID") %>'
                                            ToolTip="View Salary Details">
                                            <i class="uil uil-eye"></i>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton ID="btnExport" runat="server" CssClass="text-success"
                                            CommandName="Export" CommandArgument='<%# Eval("SalaryID") %>'
                                            ToolTip="Export to Excel">
                                            <i class="uil uil-file-download-alt"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="pagination justify-content-center" />
                        </asp:GridView>

                        <!-- Pagination Controls -->
                        <div class="d-flex justify-content-between align-items-center mt-3">
                            <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-outline-secondary btn-sm" 
                                OnClick="btnPrev_Click" Enabled="false">Previous</asp:LinkButton>

                            <asp:Label ID="lblPageInfo" runat="server" CssClass="fw-bold">Page 1 of 1</asp:Label>

                            <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-outline-secondary btn-sm" 
                                OnClick="btnNext_Click" Enabled="false">Next</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hidden Fields -->
        <asp:HiddenField ID="hfSalaryID" runat="server" Value="0" />
        
        <!-- Success Modal -->
        <div class="modal fade" id="successModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title">Success</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="text-center">
                            <i class="uil uil-check-circle display-1 text-success"></i>
                            <h4 class="mt-3" id="modalMessage">Operation completed successfully!</h4>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" data-bs-dismiss="modal">OK</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Error Modal -->
        <div class="modal fade" id="errorModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h5 class="modal-title">Error</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="text-center">
                            <i class="uil uil-times-circle display-1 text-danger"></i>
                            <h4 class="mt-3" id="errorMessage">An error occurred!</h4>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <script type="text/javascript">
        function showModal(message, type) {
            if (type === 'success') {
                document.getElementById('modalMessage').innerText = message;
                var modal = new bootstrap.Modal(document.getElementById('successModal'));
                modal.show();
            } else {
                document.getElementById('errorMessage').innerText = message;
                var modal = new bootstrap.Modal(document.getElementById('errorModal'));
                modal.show();
            }
        }

        function calculateGrossSalary() {
            var basic = parseFloat(document.getElementById('<%= txtBasic.ClientID %>').value) || 0;
            var houseRent = parseFloat(document.getElementById('<%= txtHouseRent.ClientID %>').value) || 0;
            var utilities = parseFloat(document.getElementById('<%= txtUtilities.ClientID %>').value) || 0;
            var medical = parseFloat(document.getElementById('<%= txtMedical.ClientID %>').value) || 0;
            var fuel = parseFloat(document.getElementById('<%= txtFuel.ClientID %>').value) || 0;
            var transport = parseFloat(document.getElementById('<%= txtTransport.ClientID %>').value) || 0;
            var mobile = parseFloat(document.getElementById('<%= txtMobile.ClientID %>').value) || 0;
            var bonus = parseFloat(document.getElementById('<%= txtBonus.ClientID %>').value) || 0;
            var commission = parseFloat(document.getElementById('<%= txtCommission.ClientID %>').value) || 0;
            var incentives = parseFloat(document.getElementById('<%= txtIncentives.ClientID %>').value) || 0;

            var grossSalary = basic + houseRent + utilities + medical + 
                            fuel + transport + mobile + bonus + 
                            commission + incentives;
            
            document.getElementById('<%= txtGrossSalary.ClientID %>').value = grossSalary.toFixed(2);
        }

        // Attach event listeners for auto-calculation
        window.onload = function () {
            var inputs = ['<%= txtBasic.ClientID %>', '<%= txtHouseRent.ClientID %>', 
                         '<%= txtUtilities.ClientID %>', '<%= txtMedical.ClientID %>',
                         '<%= txtFuel.ClientID %>', '<%= txtTransport.ClientID %>',
                         '<%= txtMobile.ClientID %>', '<%= txtBonus.ClientID %>',
                         '<%= txtCommission.ClientID %>', '<%= txtIncentives.ClientID %>'];
            
            inputs.forEach(function(inputId) {
                var input = document.getElementById(inputId);
                if (input) {
                    input.addEventListener('input', calculateGrossSalary);
                }
            });
        };
    </script>
</asp:Content>