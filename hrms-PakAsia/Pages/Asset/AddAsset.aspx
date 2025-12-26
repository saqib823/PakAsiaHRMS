<%@ Page Title="Add Asset" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="AddAsset.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Asset.AddAsset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container-fluid">

    <div class="row mt-4">
        <div class="col-12">

            <div class="card shadow-sm mb-4">
                <div class="card-header text-white bg-primary">
                    <h5 class="mb-0">Add / Edit Asset</h5>
                </div>
                <div class="card-body">

                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label">Asset Name</label>
                            <asp:TextBox ID="txtAssetName" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Active" Value="1" />
                                <asp:ListItem Text="Inactive" Value="0" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 d-flex align-items-end">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click" />
                        </div>
                    </div>

                    <hr />

                    <h6 class="fw-bold">Existing Assets</h6>
                    <asp:Repeater ID="rptAssets" runat="server" OnItemCommand="rptAssets_ItemCommand">
                        <HeaderTemplate>
                            <table class="table table-striped table-hover">
                                <thead>
                                    <tr>
                                        <th>Asset Name</th>
                                        <th>Status</th>
                                        <th style="width:120px;">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("AssetName") %></td>
                                <td><%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %></td>
                                <td>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("AssetID") %>'
                                        CssClass="text-primary me-2"><i class="uil uil-edit"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("AssetID") %>'
                                        CssClass="text-danger" OnClientClick="return confirm('Delete this asset?');">
                                        <i class="uil uil-trash-alt"></i></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                    <asp:HiddenField ID="hfAssetID" runat="server" Value="0" />

                </div>
            </div>

        </div>
    </div>

</div>
</asp:Content>
