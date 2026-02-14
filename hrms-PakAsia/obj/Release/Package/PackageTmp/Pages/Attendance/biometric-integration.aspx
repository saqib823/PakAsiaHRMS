<%@ Page Title="Biometric Integration"
    Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="biometric-integration.aspx.cs"
    Inherits="hrms_PakAsia.Pages.Attendance.biometric_integration" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- jQuery -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <script type="text/javascript">
        $(function () {

            var pageIndex = 1;
            var pageSize = 50;

            function fetchAttendance() {

                $.ajax({
                    type: "POST",
                    url: "/Handler/GetLiveAttendance.ashx",
                    data: {
                        PageIndex: pageIndex,
                        PageSize: pageSize,
                        BranchID: $("#<%= ddlBranch.ClientID %>").val(),
                        DepartmentID: $("#<%= ddlDepartment.ClientID %>").val(),
                        StartDate: $("#txtStartDate").val(),
                        EndDate: $("#txtEndDate").val()
                    },
                    success: function (response) {

                        if (!response || !response.Data) return;

                        var html = "";
                        $.each(response.Data, function (i, row) {
                            html += "<tr>";
                            html += "<td>" + row.FullName + "</td>";
                            html += "<td>" + row.VerifyModeName + "</td>";
                            html += "<td>" + row.PunchType + "</td>";
                            html += "<td>" + row.PunchDate + "</td>";
                            html += "<td>" + row.PunchTime + "</td>";
                            html += "</tr>";
                        });

                        $("#attendanceBody").html(html);
                        $("#lblTotal").text(
                            "Total Records: " + response.TotalRecords + " | Page: " + pageIndex
                        );
                    },
                    error: function () {
                        console.error("Failed to load attendance data");
                    }
                });
            }

            // Initial load
            fetchAttendance();

            // 🔁 Live refresh every 1 second
            setInterval(fetchAttendance, 1000);

            // Pagination
            $("#btnPrev").click(function () {
                if (pageIndex > 1) {
                    pageIndex--;
                    fetchAttendance();
                }
            });

            $("#btnNext").click(function () {
                pageIndex++;
                fetchAttendance();
            });

            // Reset page when filters change
            $("#<%= ddlBranch.ClientID %>, #<%= ddlDepartment.ClientID %>, #txtStartDate, #txtEndDate")
                .change(function () {
                    pageIndex = 1;
                    fetchAttendance();
                });

            // Export
            $("#btnExport").click(function () {
                window.location =
                    "/Handler/ExportAttendance.ashx?BranchID=" +
                    $("#<%= ddlBranch.ClientID %>").val() +
                    "&DepartmentID=" +
                    $("#<%= ddlDepartment.ClientID %>").val();
            });
        });
    </script>

    <!-- Filters -->
     <main class="main mt-10" id="top">
        <div class="container">
    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-2">

                <div class="col-md-3">
                    <asp:DropDownList ID="ddlBranch" AutoPostBack="true" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                        <asp:ListItem Text="All Branches" Value="" />
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select">
                        <asp:ListItem Text="All Departments" Value="" />
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <input type="date" id="txtStartDate" class="form-control" />
                </div>

                <div class="col-md-3">
                    <input type="date" id="txtEndDate" class="form-control" />
                </div>
                <div class="col-md-3">
<asp:LinkButton 
            ID="btnRefresh" 
            runat="server" 
            CssClass="btn btn-outline-secondary" 
            OnClick="btnRefresh_Click">
            <i class="fa fa-refresh"></i>
        </asp:LinkButton>

                </div>

            </div>
        </div>
    </div>

    <!-- Attendance Logs -->
    <div class="card">
        <div class="card-header fw-bold">Live Attendance</div>
        <div class="card-body">

            <div class="table-responsive">
                <table class="table table-striped table-sm">
                    <thead>
                        <tr>
                            <th>Full Name</th>
                            <th>Mode</th>
                            <th>Punch Type</th>
                            <th>Date</th>
                            <th>Time</th>
                        </tr>
                    </thead>
                    <tbody id="attendanceBody"></tbody>
                </table>
            </div>

            <!-- Pagination & Export -->
            <div class="d-flex justify-content-between mt-2">
                <div id="lblTotal" class="fw-bold"></div>
                <div>
                    <button type="button" id="btnPrev" class="btn btn-secondary btn-sm">Prev</button>
                    <button type="button" id="btnNext" class="btn btn-secondary btn-sm">Next</button>
                    <button type="button" id="btnExport" class="btn btn-success btn-sm">Export Excel</button>
                </div>
            </div>

        </div>
    </div>
    </div>
    </main>

</asp:Content>
