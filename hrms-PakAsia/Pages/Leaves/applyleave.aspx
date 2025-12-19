<%@ Page Title="Apply Leave" Language="C#" MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="applyleave.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Leaves.applyleave" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <main class="main mt-10">
        <div class="container">
            <div class="card">
                <div class="card-header">
                    <h5>Apply Leave</h5>
                </div>
                <div class="card-body">

                    <asp:PlaceHolder ID="phAlert" runat="server" />

                    <div class="row g-3">
                        <div class="col-md-6">
                            <label>Leave Type</label>
                            <asp:DropDownList ID="ddlLeaveType" runat="server"
                                CssClass="form-select" />
                        </div>

                        <div class="col-md-3">
                            <label>Start Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server"
                                TextMode="Date" CssClass="form-control" />
                        </div>

                        <div class="col-md-3">
                            <label>End Date</label>
                            <asp:TextBox ID="txtEndDate" runat="server"
                                TextMode="Date" CssClass="form-control" />
                        </div>

                        <div class="col-12">
                            <label>Reason</label>
                            <asp:TextBox ID="txtReason" runat="server"
                                TextMode="MultiLine" Rows="3"
                                CssClass="form-control" />
                        </div>
                    </div>

                    <div class="mt-4 text-end">
                        <asp:Button ID="btnSubmit" runat="server"
                            Text="Apply Leave"
                            CssClass="btn btn-primary"
                            OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server"
                            Text="Cancel"
                            CssClass="btn btn-secondary"
                            PostBackUrl="leavemanagement.aspx" />
                    </div>

                </div>
            </div>
        </div>
    </main>
</asp:Content>
