<%@ Page Title="Process Payslip" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="ProcessPayslip.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Payroll.ProcessPayslip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid mt-10">

        <!-- ================= SALARY STRUCTURE FORM ================= -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0">Process Payslip</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3">
                            <!-- Employee -->
                            <div class="col-md-12">
                                <label class="form-label">Employee <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-select"  data-choices="data-choices" data-options='{"removeItemButton":true,"placeholder":true}' required />
                            </div>
                           
                        </div>

                        <div class="mt-4 text-end">
                            <asp:Button ID="btnCalculate" runat="server" CssClass="btn btn-info px-4" Text="Generate" OnClick="btnCalculate_Click" />
                        </div>
                    </div>
                </div>
                 <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0">Process Clearance</h5>
                    </div>
                    <div class="card-body">
                       <div class="row g-3">
                            <!-- Employee -->
                            <div class="col-md-4">
                                <label class="form-label">Employee <span class="text-danger">*</span></label>
<asp:DropDownList ID="ddlEmplyeeClearance" runat="server" CssClass="form-select"
    data-choices="data-choices" data-options='{"removeItemButton":true,"placeholder":true}' required>
</asp:DropDownList>                            </div>
                            <!-- From Date -->
                            <div class="col-md-4">
                                <label class="form-label">From <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>
                            <!-- To Date -->
                            <div class="col-md-4">
                                <label class="form-label">To <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEffectiveTo" runat="server" CssClass="form-control" TextMode="Date" required />
                            </div>
                        </div>

                        <div class="mt-4 text-end">
                            <asp:Button ID="btnProcessClearance" runat="server" CssClass="btn btn-info px-4" Text="Generate" OnClick="btnProcessClearance_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>


   
    </div>

</asp:Content>
