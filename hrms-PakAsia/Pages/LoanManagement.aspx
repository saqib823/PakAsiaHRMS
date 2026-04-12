<%@ Page Title="Loan Management" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="LoanManagement.aspx.cs"
    Inherits="hrms_PakAsia.Pages.LoanManagement" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upLoan" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <main class="main mt-10">
                <div class="container">

                    <!-- Heading -->
                    <div class="text-center mb-4">
                        <h3>Loan Management</h3>
                        <p class="text-body-tertiary">Apply, approve and manage employee loans</p>
                    </div>

                    <!-- Loan Form -->
                    <div class="row">
                        <div class="col-md-3 mt-3">
                            <label>Employee</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-select" />
                        </div>

                        <div class="col-md-3 mt-3">
                            <label>Loan Type</label>
                            <asp:TextBox ID="txtLoanType" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-3 mt-3">
                            <label>Loan Amount</label>
                            <asp:TextBox ID="txtLoanAmount" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-3 mt-3">
                            <label>Duration (Months)</label>
                            <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-3 mt-3">
                            <label>Start Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" CssClass="form-control" />
                        </div>

                        <div class="text-end mt-4">
                            <asp:Button ID="btnClear" runat="server" Text="Clear"
                                CssClass="btn btn-secondary me-2"
                                OnClick="btnClear_Click" UseSubmitBehavior="false" />
                            <asp:Button ID="btnSave" runat="server" Text="Apply Loan"
                                CssClass="btn btn-primary"
                                OnClick="btnSave_Click" />
                        </div>
                    </div>

                    <asp:PlaceHolder ID="phAlert" runat="server" />

                    <!-- Loan Table -->
                    <div class="mt-5">
                        <asp:TextBox ID="txtSearch" runat="server"
                            CssClass="form-control form-control-sm mb-3"
                            Placeholder="Search loans..."
                            AutoPostBack="true"
                            OnTextChanged="txtSearch_TextChanged" />

                        <div class="table-responsive">
                            <asp:Repeater ID="rptLoans" runat="server"
                                OnItemCommand="rptLoans_ItemCommand"
                                OnItemDataBound="rptLoans_ItemDataBound">

                                <HeaderTemplate>
                                    <table class="table table-striped table-sm">
                                        <thead>
                                            <tr>
                                                <th>Employee</th>
                                                <th>Loan Type</th>
                                                <th>Amount</th>
                                                <th>Duration</th>
                                                <th>Monthly</th>
                                                <th>Start</th>
                                                <th>Status</th>
                                                <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("EmployeeName") %></td>
                                        <td><%# Eval("LoanType") %></td>
                                        <td><%# Eval("LoanAmount","{0:N2}") %></td>
                                        <td><%# Eval("DurationMonths") %></td>
                                        <td><%# Eval("MonthlyDeduction","{0:N2}") %></td>
                                        <td><%# Eval("StartDate","{0:yyyy-MM-dd}") %></td>
                                        <td>
                                            <span class='<%# Eval("Status").ToString()=="Pending"?"text-warning":
                                                Eval("Status").ToString()=="Approved"?"text-success":"text-danger" %>'>
                                                <%# Eval("Status") %>
                                            </span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnPrint" runat="server"
                                                CommandName="Print"
                                                CommandArgument='<%# Eval("LoanID") %>'
                                                CssClass="text-primary me-2"
                                                title="Print Loan Details">
                                                <i class="uil uil-print"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnApprove" runat="server"
                                                CommandName="Approve"
                                                CommandArgument='<%# Eval("LoanID") %>'
                                                CssClass="text-success me-2">
                                                <i class="uil uil-check-circle"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnReject" runat="server"
                                                CommandName="Reject"
                                                CommandArgument='<%# Eval("LoanID") %>'
                                                CssClass="text-danger">
                                                <i class="uil uil-times-circle"></i>
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

                        <!-- Pagination Controls -->
                        <div class="d-flex justify-content-between align-items-center mt-3">
                            <div class="text-muted">
                                Showing 
                                <asp:Label ID="lblStartRecord" runat="server" Text="0"></asp:Label> - 
                                <asp:Label ID="lblEndRecord" runat="server" Text="0"></asp:Label> 
                                of <asp:Label ID="lblTotalRecords" runat="server" Text="0"></asp:Label> records
                            </div>
                            <div class="btn-group" role="group">
                                <asp:LinkButton ID="btnFirst" runat="server" 
                                    CssClass="btn btn-outline-primary btn-sm" 
                                    OnClick="btnPagination_Click" 
                                    CommandArgument="First" 
                                    ToolTip="First Page">
                                    <i class="uil uil-angle-double-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnPrevious" runat="server" 
                                    CssClass="btn btn-outline-primary btn-sm" 
                                    OnClick="btnPagination_Click" 
                                    CommandArgument="Previous" 
                                    ToolTip="Previous Page">
                                    <i class="uil uil-angle-left"></i>
                                </asp:LinkButton>
                                <span class="btn btn-outline-secondary btn-sm disabled">
                                    Page <asp:Label ID="lblCurrentPage" runat="server" Text="1"></asp:Label> 
                                    of <asp:Label ID="lblTotalPages" runat="server" Text="1"></asp:Label>
                                </span>
                                <asp:LinkButton ID="btnNext" runat="server" 
                                    CssClass="btn btn-outline-primary btn-sm" 
                                    OnClick="btnPagination_Click" 
                                    CommandArgument="Next" 
                                    ToolTip="Next Page">
                                    <i class="uil uil-angle-right"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnLast" runat="server" 
                                    CssClass="btn btn-outline-primary btn-sm" 
                                    OnClick="btnPagination_Click" 
                                    CommandArgument="Last" 
                                    ToolTip="Last Page">
                                    <i class="uil uil-angle-double-right"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                </div>
            </main>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
