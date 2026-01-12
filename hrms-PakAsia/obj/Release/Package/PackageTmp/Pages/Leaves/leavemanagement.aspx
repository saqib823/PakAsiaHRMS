<%@ Page Async="true" Title="Leave Management" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="leavemanagement.aspx.cs" Inherits="hrms_PakAsia.Pages.Leaves.leavemanagement" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upLeave" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <main class="main mt-10" id="top">
                <div class="container">
                    <div class="card">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <h5 class="mb-0">Leave Management</h5>
                            <asp:LinkButton ID="btnApplyLeave" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnApplyLeave_Click">
                                <i class="uil uil-plus"></i> Apply Leave
                            </asp:LinkButton>
                        </div>
                        <div class="card-body">
                            <div id="tableLeave">
                                <div class="search-box mb-3">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtSearchLeave" runat="server"
                                            CssClass="form-control"
                                            Placeholder="Search by employee or leave type..."
                                            AutoPostBack="true"
                                            OnTextChanged="txtSearchLeave_TextChanged" />
                                        <span class="input-group-text">
                                            <i class="uil uil-search"></i>
                                        </span>
                                    </div>
                                </div>

                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upLeave">
                                    <ProgressTemplate>
                                        <div class="text-center py-3">
                                            <div class="spinner-border text-primary" role="status">
                                                <span class="visually-hidden">Loading...</span>
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:PlaceHolder ID="phAlertLeave" runat="server"></asp:PlaceHolder>

                                <div class="table-responsive">
                                    <asp:Repeater ID="rptLeaves" runat="server" OnItemCommand="rptLeaves_ItemCommand" OnItemDataBound="rptLeaves_ItemDataBound">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-striped table-sm">
                                                <thead>
                                                    <tr>
                                                        <th>Emp ID</th>
                                                        <th>Employee Name</th>
                                                        <th>Leave Type</th>
                                                        <th>Total Days</th>
                                                        <th>Used</th>
                                                        <th>Remaining</th>
                                                        <th>Status</th>
                                                        <th class="text-center">Actions</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("EmployeeID") %></td>
                                                <td><strong><%# Eval("FullName") %></strong></td>
                                                <td><%# Eval("LeaveName") %></td>
                                                <td><%# Eval("TotalAllocated") %></td>
                                                <td><%# Eval("Used") %></td>
                                                <td><%# Eval("Remaining") %></td>
                                                <td>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                        CssClass='<%# Eval("Status").ToString() == "Approved" ? "badge bg-success" : Eval("Status").ToString() == "Pending" ? "badge bg-warning" : "badge bg-danger" %>'>
                                                    </asp:Label>
                                                </td>
                                                <td class="text-center">
                                                    <div class="btn-group btn-group-sm" role="group">
                                                        <asp:LinkButton ID="btnApprove" runat="server" CssClass="btn btn-outline-success"
                                                            CommandName="ApproveLeave" CommandArgument='<%# Eval("EmployeeLeaveID") %>' ToolTip="Approve Leave">
                                                            <i class="uil uil-check"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnReject" runat="server" CssClass="btn btn-outline-danger"
                                                            CommandName="RejectLeave" CommandArgument='<%# Eval("EmployeeLeaveID") %>' ToolTip="Reject Leave">
                                                            <i class="uil uil-times"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnEncash" runat="server" CssClass="btn btn-outline-primary"
                                                            CommandName="EncashLeave" CommandArgument='<%# Eval("EmployeeLeaveID") %>' ToolTip="Encash Leave">
                                                            <i class="uil uil-money-insert"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                                </tbody>
                                            </table>
                                            <%# ShowEmptyMessageLeave() %>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>

                                <!-- Pagination -->
                                <div class="d-flex justify-content-between align-items-center mt-3">
                                    <div>
                                        <asp:Label ID="lblPageInfoLeave" runat="server" CssClass="text-muted"></asp:Label>
                                    </div>
                                    <div class="d-flex">
                                        <asp:LinkButton ID="btnPrevLeave" runat="server" CssClass="btn btn-outline-secondary btn-sm me-1" OnClick="btnPrevLeave_Click" Enabled="false">
                                            <i class="uil uil-angle-left"></i>
                                        </asp:LinkButton>

                                        <div class="btn-group me-1">
                                            <asp:Repeater ID="rptPagerLeave" runat="server" OnItemCommand="rptPagerLeave_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CommandName="Page" CommandArgument='<%# Eval("PageNumber") %>'
                                                        CssClass='<%# (bool)Eval("IsCurrent") ? "btn btn-primary btn-sm" : "btn btn-outline-secondary btn-sm" %>'>
                                                        <%# Eval("PageNumber") %>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>

                                        <asp:LinkButton ID="btnNextLeave" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnNextLeave_Click" Enabled="false">
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
                function confirmAction(message) {
                    return confirm(message);
                }

                // Reset search on escape key
                document.addEventListener('keydown', function (e) {
                    if (e.key === 'Escape') {
                        var searchBox = document.getElementById('<%= txtSearchLeave.ClientID %>');
                        if (searchBox && searchBox.value) {
                            searchBox.value = '';
                            __doPostBack('<%= txtSearchLeave.UniqueID %>', '');
                        }
                    }
                });
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtSearchLeave" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="rptPagerLeave" />
            <asp:AsyncPostBackTrigger ControlID="btnPrevLeave" />
            <asp:AsyncPostBackTrigger ControlID="btnNextLeave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
