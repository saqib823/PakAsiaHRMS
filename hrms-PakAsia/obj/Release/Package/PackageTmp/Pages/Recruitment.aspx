<%@ Page Title="Job Posting" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="Recruitment.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Recruitment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="main mt-10" id="top">
        <div class="container">

            <!-- ================= JOB POSTING ================= -->
            <div class="row py-5">
                <div class="text-center mb-4">
                    <h3 class="mt-3">Job Posting</h3>
                </div>

                <div class="row">
                    <div class="col-sm-4 mt-3">
                        <label class="form-label">Job Name</label>
                        <asp:TextBox CssClass="form-control" ID="txtJobName" runat="server" Placeholder="Enter job name"></asp:TextBox>
                    </div>

                    <div class="col-sm-4 mt-3">
                        <label class="form-label">&nbsp;</label><br />
                        <asp:Button ID="btnSaveJob" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveJob_Click" />
                        <asp:Button ID="btnClearJob" runat="server" Text="Clear" CssClass="btn btn-secondary ms-2" OnClick="btnClearJob_Click" />
                    </div>
                </div>

                <div class="mt-5">
                    <asp:TextBox ID="txtSearchJob" runat="server" CssClass="form-control mb-3"
                                 Placeholder="Search jobs..." AutoPostBack="true"
                                 OnTextChanged="txtSearchJob_TextChanged"></asp:TextBox>

                    <asp:Repeater ID="rptJobs" runat="server" OnItemCommand="rptJobs_ItemCommand">
                        <HeaderTemplate>
                            <table class="table table-striped table-sm">
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Job Name</th>
                                        <th>Created Date</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ID") %></td>
                                <td><%# Eval("Name") %></td>
                               <td><%# Eval("CreatedDate") %></td>
<td class="text-nowrap">

    <!-- Edit Job -->
    <asp:LinkButton ID="btnEditJob" runat="server"
        CommandName="EditJob"
        CommandArgument='<%# Eval("ID") %>'
        CssClass="text-primary me-2"
        ToolTip="Edit Job">
        <i class="uil uil-edit"></i>
    </asp:LinkButton>

    <!-- Delete Job -->
    <asp:LinkButton ID="btnDeleteJob" runat="server"
        CommandName="DeleteJob"
        CommandArgument='<%# Eval("ID") %>'
        CssClass="text-danger"
        ToolTip="Delete Job"
        OnClientClick="return confirm('Are you sure?');">
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

                    <!-- Job Pagination -->
                    <div class="d-flex justify-content-between mt-3">
                        <span class="text-muted">
                            <asp:Label ID="lblJobPageInfo" runat="server"></asp:Label>
                        </span>
                        <div class="d-flex justify-content-center">
                            <asp:LinkButton ID="btnJobPrev" runat="server" CssClass="btn btn-outline-secondary btn-sm me-1"
                                            OnClick="btnJobPrev_Click">«</asp:LinkButton>
                            <asp:Repeater ID="rptJobPager" runat="server" OnItemCommand="rptJobPager_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server"
                                                    CommandName="Page"
                                                    CommandArgument='<%# Eval("PageNumber") %>'
                                                    CssClass='<%# (bool)Eval("IsCurrent") ? "btn btn-primary btn-sm me-1" : "btn btn-outline-secondary btn-sm me-1" %>'>
                                        <%# Eval("PageNumber") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:LinkButton ID="btnJobNext" runat="server" CssClass="btn btn-outline-secondary btn-sm"
                                            OnClick="btnJobNext_Click">»</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- ================= CANDIDATES ================= -->
            <div class="row py-5">
                <div class="text-center mb-4">
                    <h3 class="mt-3">Candidates</h3>
                </div>

                <div class="row">
                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Candidate Name</label>
                        <asp:TextBox ID="txtCandidateName" runat="server" CssClass="form-control" Placeholder="Enter name"></asp:TextBox>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Job</label>
                        <asp:DropDownList ID="ddlJob" runat="server" CssClass="form-select" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
                    </div>

                    <div class="col-sm-3 mt-3">
                        <label class="form-label">Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Select" Value="0" />
                            <asp:ListItem Text="Pass" Value="1" />
                            <asp:ListItem Text="Failed" Value="2" />
                            <asp:ListItem Text="Hold" Value="3" />
                        </asp:DropDownList>
                    </div>
          <div class="col-sm-3 mt-3">
    <label class="form-label">Documents</label>
    <asp:FileUpload ID="fuCV" runat="server"
        CssClass="form-control"
        AllowMultiple="true" />
</div>


                    <div class="col-sm-3 mt-3">
                        <label class="form-label">&nbsp;</label><br />
                        <asp:Button ID="btnSaveCandidate" runat="server" Text="Save" CssClass="btn btn-primary"
                                    OnClick="btnSaveCandidate_Click" />
                        <asp:Button ID="btnClearCandidate" runat="server" Text="Clear" CssClass="btn btn-secondary ms-2"
                                    OnClick="btnClearCandidate_Click" />
                    </div>
                </div>

                <div class="mt-5">
                    <asp:TextBox ID="txtSearchCandidate" runat="server" CssClass="form-control mb-3"
                                 Placeholder="Search..." AutoPostBack="true"
                                 OnTextChanged="txtSearchCandidate_TextChanged"></asp:TextBox>

                    <asp:Repeater ID="rptCandidates" runat="server" OnItemCommand="rptCandidates_ItemCommand" OnItemDataBound="rptCandidates_ItemDataBound">
                        <HeaderTemplate>
                            <table class="table table-striped table-sm">
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Name</th>
                                        <th>Job</th>
                                        <th>Status</th>
                                        <th>Created Date</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ID") %></td>
                                <td><%# Eval("Name") %></td>
                                <td><%# Eval("JobName") %></td>
                               <td>
    <asp:LinkButton ID="btnPass" runat="server" CommandName="ChangeStatus" CommandArgument='<%# Eval("ID") + ",1" %>'
                    CssClass="btn btn-success btn-sm me-1">Pass</asp:LinkButton>
    <asp:LinkButton ID="btnFailed" runat="server" CommandName="ChangeStatus" CommandArgument='<%# Eval("ID") + ",2" %>'
                    CssClass="btn btn-danger btn-sm me-1">Failed</asp:LinkButton>
    <asp:LinkButton ID="btnHold" runat="server" CommandName="ChangeStatus" CommandArgument='<%# Eval("ID") + ",3" %>'
                    CssClass="btn btn-warning btn-sm">Hold</asp:LinkButton>
</td>

                                <td><%# Eval("CreatedDate") %></td>
                                <td class="text-nowrap">

    <!-- Edit -->
    <asp:LinkButton ID="btnEditCandidate" runat="server"
        CommandName="EditCandidate"
        CommandArgument='<%# Eval("ID") %>'
        CssClass="text-primary me-2"
        ToolTip="Edit Candidate">
        <i class="uil uil-edit"></i>
    </asp:LinkButton>

    <!-- Delete -->
    <asp:LinkButton ID="btnDeleteCandidate" runat="server"
        CommandName="DeleteCandidate"
        CommandArgument='<%# Eval("ID") %>'
        CssClass="text-danger me-2"
        ToolTip="Delete Candidate"
        OnClientClick="return confirm('Are you sure?');">
        <i class="uil uil-trash-alt"></i>
    </asp:LinkButton>

<!-- CV -->
<asp:HyperLink ID="lnkShowCv" runat="server"
    CssClass="text-success open-docs"
    ToolTip="View CV"
    Target="_blank"
    data-paths='<%# Eval("CVPath") %>'
    Visible='<%# !string.IsNullOrEmpty(Eval("CVPath").ToString()) %>'>
    <i class="uil uil-file-alt"></i>
</asp:HyperLink>

</td>

                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                    <!-- Candidate Pagination -->
                    <div class="d-flex justify-content-between mt-3">
                        <span class="text-muted">
                            <asp:Label ID="lblCandidatePageInfo" runat="server"></asp:Label>
                        </span>
                        <div class="d-flex justify-content-center">
                            <asp:LinkButton ID="btnCandidatePrev" runat="server" CssClass="btn btn-outline-secondary btn-sm me-1"
                                            OnClick="btnCandidatePrev_Click">«</asp:LinkButton>
                            <asp:Repeater ID="rptCandidatePager" runat="server" OnItemCommand="rptCandidatePager_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server"
                                                    CommandName="Page"
                                                    CommandArgument='<%# Eval("PageNumber") %>'
                                                    CssClass='<%# (bool)Eval("IsCurrent") ? "btn btn-primary btn-sm me-1" : "btn btn-outline-secondary btn-sm me-1" %>'>
                                        <%# Eval("PageNumber") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:LinkButton ID="btnCandidateNext" runat="server" CssClass="btn btn-outline-secondary btn-sm"
                                            OnClick="btnCandidateNext_Click">»</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <script>
document.addEventListener('DOMContentLoaded', function () {
    const links = document.querySelectorAll('.open-docs');

    links.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault(); // prevent default navigation

            let paths = this.dataset.paths;
            if (!paths) return;

            // Split comma-separated paths
            let files = paths.split(',');

            files.forEach(function(path) {
                path = path.trim();
                if (!path) return;

                // Remove ~/
                path = path.replace(/^~\//, '');

                // Prepend origin
                let url = window.location.origin + '/' + path;

                // Encode spaces and special characters
                url = encodeURI(url);

                // Open each file in a new tab
                window.open(url, '_blank');
            });
        });
    });
});
</script>

</asp:Content>

