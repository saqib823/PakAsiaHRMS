<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLogs.aspx.cs" Inherits="hrms_PakAsia.Pages.Organization.AuditLogs" MasterPageFile="~/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <div class="card mb-3">
            <div class="card-header">
                <h5 class="mb-0">Audit Logs</h5>
            </div>
            <div class="card-body">
                <div class="row g-2 mb-3">
                    <div class="col-md-3">
                        <label class="form-label">User</label>
                        <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Module</label>
                        <asp:TextBox ID="txtModule" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Action</label>
                        <asp:TextBox ID="txtAction" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">IP Address</label>
                        <asp:TextBox ID="txtIP" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="row g-2 mb-3">
                    <div class="col-md-6">
                        <label class="form-label">Search in remarks / data</label>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnFilter" runat="server" Text="Apply Filters" CssClass="btn btn-primary w-100"
                            OnClick="btnFilter_Click" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary w-100"
                            OnClick="btnClear_Click" />
                    </div>
                </div>
                <asp:GridView ID="gvLogs" runat="server" CssClass="table table-sm table-striped table-hover"
                    AutoGenerateColumns="False" AllowPaging="True" PageSize="20"
                    OnPageIndexChanging="gvLogs_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="User" />
                        <asp:BoundField DataField="ModuleName" HeaderText="Module" />
                        <asp:BoundField DataField="ActionType" HeaderText="Action" />
                        <asp:BoundField DataField="OldData" HeaderText="Old Data" />
                        <asp:BoundField DataField="NewData" HeaderText="New Data" />
                        <asp:BoundField DataField="RecordID" HeaderText="Record" />
                        <asp:BoundField DataField="IPAddress" HeaderText="IP" />
                        <asp:BoundField DataField="MachineName" HeaderText="Machine" />
                        <asp:BoundField DataField="BrowserInfo" HeaderText="Browser" />
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                    </Columns>
                    <PagerStyle CssClass="pagination" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

