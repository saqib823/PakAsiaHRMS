<%@ Page Title="Asset Management" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="AssetManagement.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Asset.AssetManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid">

    <!-- Asset Issue / Return Form -->
    <div class="row mt-10">
        <div class="col-12">
            <div class="card shadow-sm mb-4">
                <div class="card-header text-white">
                    <h5 class="mb-0">Asset Issue / Return</h5>
                </div>
                <div class="card-body">
                    <div class="row g-3">
                        <div class="col-md-4">
                            <label class="form-label">Employee</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Asset</label>
                            <asp:DropDownList ID="ddlAsset" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Issue Date</label>
                            <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Return Date</label>
                            <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Condition</label>
                            <asp:DropDownList ID="ddlCondition" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Good" Value="Good" />
                                <asp:ListItem Text="Damaged" Value="Damaged" />
                                <asp:ListItem Text="Lost" Value="Lost" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Deduction (if any)</label>
                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="form-control" TextMode="Number" />
                        </div>
                    </div>

                    <div class="mt-4 text-end">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" Text="Save" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Asset Records Grid -->
    <div class="row">
        <div class="col-12">
            <div class="card shadow-sm">
                <div class="card-header">
                    <h6 class="mb-0">Existing Asset Records</h6>
                </div>
                <div class="card-body table-responsive">
                     <div class="d-flex justify-content-between align-items-center mb-3">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control w-50" Placeholder="Search..." />
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary ms-2" Text="Search" OnClick="btnSearch_Click" />
                        
                    </div>
                    <asp:Repeater ID="rptAssets" runat="server" OnItemCommand="rptAssets_ItemCommand">
                        <HeaderTemplate>
                            <table class="table table-striped table-hover align-middle">
                                <thead>
                                    <tr>
                                        <th>Employee</th>
                                        <th>Asset</th>
                                        <th>Issue Date</th>
                                        <th>Return Date</th>
                                        <th>Condition</th>
                                        <th>Deduction</th>
                                        <th style="width:120px;">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("EmployeeName") %></td>
                                <td><%# Eval("AssetName") %></td>
                                <td><%# Convert.ToDateTime(Eval("IssueDate")).ToString("dd-MMM-yyyy") %></td>
                                <td><%# Eval("ReturnDate") != DBNull.Value ? Convert.ToDateTime(Eval("ReturnDate")).ToString("dd-MMM-yyyy") : "" %></td>
                                <td><%# Eval("Condition") %></td>
                                <td><%# Eval("Deduction") %></td>
                                <td>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="text-primary me-2"
                                        CommandName="Edit" CommandArgument='<%# Eval("AssetRecordID") %>'>
                                        <i class="uil uil-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="text-danger"
                                        CommandName="Delete" CommandArgument='<%# Eval("AssetRecordID") %>'
                                        OnClientClick="return confirm('Delete asset record?');">
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
                    <div>
                        <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnPrev_Click">Previous</asp:LinkButton>
                        <asp:Label ID="lblPageInfo" runat="server" CssClass="fw-bold mx-2"></asp:Label>
                        <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btnNext_Click">Next</asp:LinkButton>
                    </div>
                   
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfAssetID" runat="server" Value="0" />

</div>
</asp:Content>
