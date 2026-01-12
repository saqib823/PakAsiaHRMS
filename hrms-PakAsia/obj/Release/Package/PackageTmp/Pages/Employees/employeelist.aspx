<%@ Page Async="true" Title="Employee List" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="employeelist.aspx.cs" Inherits="hrms_PakAsia.Pages.Employees.employeelist" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upEmployee" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <main class="main mt-10" id="top">
                <div class="container">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Employee Management</h5>
                        </div>
                        <div class="card-body">
                            <div id="tableExample3">
                                <div class="search-box mb-3">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtSearchEmployee" runat="server"
                                            CssClass="form-control"
                                            Placeholder="Search employees by name, ID, or department..."
                                            AutoPostBack="true"
                                            OnTextChanged="txtSearchEmployee_TextChanged" />
                                        <span class="input-group-text">
                                            <i class="uil uil-search"></i>
                                        </span>
                                    </div>
                                </div>

                                <!-- Loading Indicator -->
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upEmployee">
                                    <ProgressTemplate>
                                        <div class="text-center py-3">
                                            <div class="spinner-border text-primary" role="status">
                                                <span class="visually-hidden">Loading...</span>
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                 <asp:PlaceHolder ID="phAlert" runat="server"></asp:PlaceHolder>

                                <div class="table-responsive">
                                    <asp:Repeater ID="rptEmployees" runat="server" OnItemCommand="rptEmployees_ItemCommand" 
                                        OnItemDataBound="rptEmployees_ItemDataBound">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-striped table-sm">
                                                <thead>
                                                    <tr>
                                                        <th>Emp ID</th>
                                                        <th>Name</th>
                                                        <th>Department</th>
                                                        <th>Designation</th>
                                                        <th class="text-center">Actions</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("EmployeeID") %></td>
                                                <td><strong><%# Eval("FullName") %></strong></td>
                                                <td><span class="badge bg-info"><%# Eval("Department") %></span></td>
                                                <td><%# Eval("Designation") %></td>
                                                <td class="text-center">
                                                    <div class="btn-group btn-group-sm" role="group">
                                                        <asp:LinkButton
                                                            ID="btnView"
                                                            runat="server"
                                                            CssClass="btn btn-outline-info"
                                                            CommandName="ViewEmployee"
                                                            CommandArgument='<%# Eval("EmployeeID") %>'
                                                            ToolTip="View Details">
                                                            <i class="uil uil-eye"></i>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton
                                                            ID="btnEdit"
                                                            runat="server"
                                                            CssClass="btn btn-outline-primary"
                                                            CommandName="EditEmployee"
                                                            CommandArgument='<%# Eval("EmployeeID") %>'
                                                            ToolTip="Edit Employee">
                                                            <i class="uil uil-edit"></i>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton
                                                            ID="btnDelete"
                                                            runat="server"
                                                            CssClass="btn btn-outline-danger"
                                                            CommandName="DeleteEmployee"
                                                            CommandArgument='<%# Eval("EmployeeID") %>'
                                                            ToolTip="Delete Employee"
                                                            OnClientClick="return confirmDelete(this);">
                                                            <i class="uil uil-trash-alt"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            </tbody>
                                            </table>
                                            <%# ShowEmptyMessage() %>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>

                                <!-- Pagination -->
                                <div class="d-flex justify-content-between align-items-center mt-3">
                                    <div>
                                        <asp:Label ID="lblPageInfo" runat="server" CssClass="text-muted"></asp:Label>
                                    </div>
                                    <div class="d-flex">
                                        <asp:LinkButton ID="btnPrev" runat="server"
                                            CssClass="btn btn-outline-secondary btn-sm me-1"
                                            OnClick="btnPrev_Click"
                                            Enabled="false">
                                            <i class="uil uil-angle-left"></i>
                                        </asp:LinkButton>

                                        <div class="btn-group me-1">
                                            <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server"
                                                        CommandName="Page"
                                                        CommandArgument='<%# Eval("PageNumber") %>'
                                                        CssClass='<%# (bool)Eval("IsCurrent") 
                                                                ? "btn btn-primary btn-sm" 
                                                                : "btn btn-outline-secondary btn-sm" %>'>
                                                        <%# Eval("PageNumber") %>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>

                                        <asp:LinkButton ID="btnNext" runat="server"
                                            CssClass="btn btn-outline-secondary btn-sm"
                                            OnClick="btnNext_Click"
                                            Enabled="false">
                                            <i class="uil uil-angle-right"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
            
            <script type="text/javascript">
                function confirmDelete(button) {
                    return confirm('Are you sure you want to delete this employee?\nThis action cannot be undone.');
                }
                
                // Reset search on escape key
                document.addEventListener('keydown', function(e) {
                    if (e.key === 'Escape') {
                        var searchBox = document.getElementById('<%= txtSearchEmployee.ClientID %>');
                        if (searchBox && searchBox.value) {
                            searchBox.value = '';
                            __doPostBack('<%= txtSearchEmployee.UniqueID %>', '');
                        }
                    }
                });
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtSearchEmployee" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="rptPager" />
            <asp:AsyncPostBackTrigger ControlID="btnPrev" />
            <asp:AsyncPostBackTrigger ControlID="btnNext" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>