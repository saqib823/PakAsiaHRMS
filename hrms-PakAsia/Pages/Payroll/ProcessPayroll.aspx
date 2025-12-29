<%@ Page Title="Process Payroll" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="ProcessPayroll.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Payroll.ProcessPayroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- ================= SALARY STRUCTURE FORM ================= -->
        <div class="row mt-10">
            <div class="col-12">
                <div class="card shadow-sm mb-4">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Process Payroll</h5>
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
                                <label class="form-label">From <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>

                            <div class="col-md-4">
                                <label class="form-label">To <span class="text-danger">*</span></label>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" TextMode="Date" required />
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


        <!-- Hidden Fields -->
        <asp:HiddenField ID="hfSalaryID" runat="server" Value="0" />
      

    </div>

    <script type="text/javascript">
      
    </script>
</asp:Content>