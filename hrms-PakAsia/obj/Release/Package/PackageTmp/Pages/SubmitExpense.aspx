<%@ Page Title="Submit Expense" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="SubmitExpense.aspx.cs" Inherits="hrms_PakAsia.Pages.SubmitExpense" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <main class="main mt-10" id="top">
                <div class="container">

                    <div class="row py-5">
                        <!-- Page Heading -->
                        <div class="text-center mb-4">
                            <h3 class="mt-3">Submit Expense Claim</h3>
                            <p class="text-body-tertiary">Fill in expense details and attach receipt</p>
                        </div>

                        <!-- Expense Form -->
                        <div class="row">
                            <div class="col-sm-3 mt-3">
                                <label class="form-label">Employee</label>
                                <asp:DropDownList ID="ddlEmployee" CssClass="form-select" runat="server" DataTextField="Name" DataValueField="ID" ></asp:DropDownList>
                            </div>

                            <div class="col-sm-3 mt-3">
                                <label class="form-label">Expense Type</label>
                                <asp:TextBox ID="txtExpenseType" CssClass="form-control" runat="server" placeholder="Expense Type"></asp:TextBox>
                            </div>

                            <div class="col-sm-3 mt-3">
                                <label class="form-label">Amount</label>
                                <asp:TextBox ID="txtAmount" CssClass="form-control" runat="server" placeholder="0.00"></asp:TextBox>
                            </div>

                            <div class="col-sm-3 mt-3">
                                <label class="form-label">Date</label>
                                <asp:TextBox ID="txtDate" CssClass="form-control" runat="server" TextMode="Date"></asp:TextBox>
                            </div>

                            <div class="col-sm-6 mt-3">
                                <label class="form-label">Description</label>
                                <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>

                            <div class="col-sm-6 mt-3">
                                <label class="form-label">Attach Receipt</label>
                                <input class="form-control" id="fileReceipt" type="file" runat="server" />
                            </div>

                            <!-- Buttons -->
                            <div class="text-end mt-4">
                                <asp:Button ID="btnClear" OnClick="btnClear_Click" runat="server" Text="Clear" CssClass="btn btn-secondary me-2 mb-3" UseSubmitBehavior="false" />
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" CssClass="btn btn-primary mb-3" />
                            </div>
                        </div>

                        <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

                        <!-- Expense Table -->
                        <div id="tableExpenses" class="mt-5">
                            <div class="search-box mb-3 mx-auto">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control search-input search form-control-sm" Placeholder="Search expenses..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
                            </div>
                            <div class="table-responsive">
                               <asp:Repeater ID="rptExpenses" runat="server" OnItemCommand="rptExpenses_ItemCommand" OnItemDataBound="rptExpenses_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-striped table-sm fs-9 mb-0">
            <thead>
                <tr>
                    <th>Employee</th>
                    <th>Type</th>
                    <th>Amount</th>
                    <th>Date</th>
                    <th>Description</th>
                    <th>Receipt</th>
                    <th>Status</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("EmployeeName") %></td>
            <td><%# Eval("ExpenseType") %></td>
            <td><%# Eval("Amount", "{0:N2}") %></td>
            <td><%# Eval("ExpenseDate", "{0:yyyy-MM-dd}") %></td>
            <td><%# Eval("Description") %></td>
            <td>
                <asp:HyperLink ID="hlReceipt" runat="server" NavigateUrl='<%# Eval("ReceiptPath") %>' Target="_blank">
                    View
                </asp:HyperLink>
            </td>
            <td>
                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' CssClass='<%# Eval("Status")=="Pending"?"text-warning":Eval("Status")=="Approved"?"text-success":"text-danger" %>'></asp:Label>
            </td>
            <td>
                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditExpense" CommandArgument='<%# Eval("ExpenseID") %>' CssClass="text-primary me-2">
                    <i class="uil uil-edit"></i>
                </asp:LinkButton>

                <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteExpense" CommandArgument='<%# Eval("ExpenseID") %>' CssClass="text-danger me-2" OnClientClick="return confirm('Are you sure?');">
                    <i class="uil uil-trash-alt"></i>
                </asp:LinkButton>

                <asp:LinkButton ID="btnApprove" runat="server" CommandName="ApproveExpense" CommandArgument='<%# Eval("ExpenseID") %>' CssClass="text-success me-2" Visible='<%# Eval("Status").ToString()=="Pending" %>'>
                    <i class="uil uil-check-circle"></i>
                </asp:LinkButton>

                <asp:LinkButton ID="btnDisapprove" runat="server" CommandName="DisapproveExpense" CommandArgument='<%# Eval("ExpenseID") %>' CssClass="text-danger" Visible='<%# Eval("Status").ToString()=="Pending" %>'>
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
