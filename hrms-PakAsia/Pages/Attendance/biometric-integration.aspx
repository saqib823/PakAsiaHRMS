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
                    PageSize: pageSize
                },
                success: function (response) {

                    if (!response || !response.Data) return;

                    var html = "";
                    $.each(response.Data, function (i, row) {
                        html += "<tr>";
                        html += "<td>" + row.FullName + "</td>";
                        html += "<td>" + row.VerifyModeName + "</td>";
                        html += "<td>" + row.PunchType + "</td>";
                        html += "<td>" + row.OprtDateFormatted + "</td>";
                        html += "<td>" + row.OprtTimeFormatted + "</td>";
                        html += "<td>Biometric</td>";
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

        // 🔁 HARD REFRESH EVERY 1 SECOND (NO CONDITIONS)
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

        // Export
        $("#btnExport").click(function () {
            window.location = "/Handler/ExportAttendance.ashx";
        });

    });
</script>

    <!-- Attendance Logs -->
    <div class="card">
        <div class="card-header fw-bold">Attendance Logs (Auto Refresh)</div>
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
                            <th>Source</th>
                        </tr>
                    </thead>
                    <tbody id="attendanceBody">
                        <!-- rows injected by JS -->
                    </tbody>
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

</asp:Content>
