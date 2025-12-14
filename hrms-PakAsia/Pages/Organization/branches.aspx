<%@ Page Title="Add Branch" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="branches.aspx.cs" Inherits="hrms_PakAsia.Pages.Organization.branches" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upBranch" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

    <main class="main mt-5" id="top">
        <div class="container">

            <div class="row py-5">

                <!-- Page Heading -->
                <div class="text-center mb-4">
                    <h3 class="mt-3">Add Branch</h3>
                    <p class="text-body-tertiary">Fill the required information</p>
                </div>

                <!-- Form Row -->
                <div class="row">

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Branch Name</label>
                        <asp:TextBox CssClass="form-control" ID="BranchName" runat="server" placeholder="Enter Branch Name"></asp:TextBox>
                    </div>
                     <div class="col-sm-3 mt-3">
                        <label class="form-label">Location</label>
                        <asp:TextBox CssClass="form-control" ID="Location" runat="server" placeholder="Enter Branch Location"></asp:TextBox>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Status</label>
                        <asp:DropDownList CssClass="form-control" ID="ddlActive" runat="server">
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Buttons -->
                    <div class="text-end mt-4 col-12">
                        <asp:Button ID="btnClear" OnClick="btnClear_Click" runat="server"
                            Text="Clear" CssClass="btn btn-secondary me-2 mb-3" UseSubmitBehavior="false" />
                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server"
                            Text="Save" CssClass="btn btn-primary mb-3" />
                    </div>
                </div>

                <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

                <!-- TABLE BELOW THE FORM -->
                <div id="tableBranches" class="mt-4">
                    <div class="search-box mb-3 mx-auto">
                        <div class="position-relative">
                            <asp:TextBox ID="txtSearch" runat="server"
                                CssClass="form-control search-input search form-control-sm"
                                Placeholder="Search branches..."
                                AutoPostBack="true"
                                OnTextChanged="txtSearch_TextChanged" />              
                            <svg class="svg-inline--fa fa-magnifying-glass search-box-icon" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="magnifying-glass" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                                <path fill="currentColor" d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352a144 144 0 1 0 0-288 144 144 0 1 0 0 288z"></path>
                            </svg>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:Repeater ID="rptBranches" runat="server" OnItemCommand="rptBranches_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-striped table-sm fs-9 mb-0">
                                    <thead>
                                        <tr>
                                            <th>Branch ID</th>
                                            <th>Branch Name</th>
                                            <th>Location</th>
                                            <th>Status</th>
                                            <th>Created Date</th>
                                            <th>Created By</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("BranchID") %></td>
                                    <td><%# Eval("BranchName") %></td>
                                    <td><%# Eval("Location") %></td>
                                    <td><%# Eval("StatusText") %></td>
                                    <td><%# Eval("CreatedDate") %></td>
                                    <td><%# Eval("CreatedBy") %></td>
                                    <td class="text-center">
                                        <asp:LinkButton
                                            ID="btnEdit"
                                            runat="server"
                                            CssClass="text-primary me-2"
                                            CommandName="EditBranch"
                                            CommandArgument='<%# Eval("BranchID") %>'
                                            ToolTip="Edit Branch">
                                            <i class="uil uil-edit"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton
                                            ID="btnDelete"
                                            runat="server"
                                            CssClass="text-danger"
                                            CommandName="DeleteBranch"
                                            CommandArgument='<%# Eval("BranchID") %>'
                                            ToolTip="Delete Branch"
                                            OnClientClick="return confirm('Are you sure you want to delete this branch?');">
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

                    <!-- Pagination -->
                    <div class="d-flex justify-content-between mt-3">
                        <span class="d-none d-sm-inline-block" data-list-info="data-list-info"><asp:Label ID="lblPageInfo" runat="server" CssClass="text-muted"></asp:Label></span>
                        <div class="d-flex justify-content-center mt-3">
                            <asp:LinkButton ID="btnPrev" runat="server"
                                CssClass="btn btn-outline-secondary btn-sm me-1"
                                OnClick="btnPrev_Click">
                                «
                            </asp:LinkButton>

                            <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server"
                                        CommandName="Page"
                                        CommandArgument='<%# Eval("PageNumber") %>'
                                        CssClass='<%# (bool)Eval("IsCurrent") 
                                                    ? "btn btn-primary btn-sm me-1" 
                                                    : "btn btn-outline-secondary btn-sm me-1" %>'>
                                        <%# Eval("PageNumber") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:LinkButton ID="btnNext" runat="server"
                                CssClass="btn btn-outline-secondary btn-sm"
                                OnClick="btnNext_Click">
                                »
                            </asp:LinkButton>
                        </div>
                    </div>

                </div> <!-- /tableBranches -->

            </div>
        </div>
    </main>
    </ContentTemplate>

   <%-- <Triggers>
        <asp:PostBackTrigger ControlID="btnSave" />
    </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
