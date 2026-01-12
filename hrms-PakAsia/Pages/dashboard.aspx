<%@ Page Title="Dasboard" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="hrms_PakAsia.Pages.dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   
       <div class="content">
        <div class="row gy-3 mb-6 justify-content-between">
          <div class="col-md-9 col-auto">
            <h2 class="mb-2 text-body-emphasis">Admin Dashboard</h2>
            <h5 class="text-body-tertiary fw-semibold">Here’s what’s going on at your business right now</h5>
          </div>
         
        </div>
        <div class="row mb-3 gy-4">

               <!-- USERS -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-user text-primary-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litUsers" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Users</p>
                       </div>
                   </div>
               </div>

               <!-- DEPARTMENTS -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-building text-success-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litDepartments" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Departments</p>
                       </div>
                   </div>
               </div>

               <%--<!-- BRANCHES -->
  <div class="col-12 col-sm-6 col-xl-3">
    <div class="d-flex align-items-center">
      <span class="fs-4 lh-1 uil uil-map-marker text-warning-dark"></span>
      <div class="ms-2">
        <h3 class="mb-0">8</h3>
        <p class="fs-9 mb-0 text-body-secondary">Branches</p>
      </div>
    </div>
  </div>

  <!-- DESIGNATIONS -->
  <div class="col-12 col-sm-6 col-xl-3">
    <div class="d-flex align-items-center">
      <span class="fs-4 lh-1 uil uil-briefcase text-info-dark"></span>
      <div class="ms-2">
        <h3 class="mb-0">22</h3>
        <p class="fs-9 mb-0 text-body-secondary">Designations</p>
      </div>
    </div>
  </div>--%>

               <!-- EMPLOYEES -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-users-alt text-primary-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litTotalEmployees" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Employees</p>
                       </div>
                   </div>
               </div>

               <!-- PRESENT -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-check-circle text-success-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litPresent" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Present</p>
                       </div>
                   </div>
               </div>

               <!-- ABSENT -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-times-circle text-danger-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litAbsent" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Absent</p>
                       </div>
                   </div>
               </div>

               <!-- LATE -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-clock text-warning-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litLate" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Late</p>
                       </div>
                   </div>
               </div>

               <!-- ON LEAVE -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-plane-departure text-info-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litOnLeave" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">On Leave</p>
                       </div>
                   </div>
               </div>

               <!-- ASSETS -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-box text-primary-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litAssets" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Assets</p>
                       </div>
                   </div>
               </div>

               <!-- ISSUED -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-arrow-up text-success-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litIssuedAssets" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Issued</p>
                       </div>
                   </div>
               </div>

               <!-- RETURNED -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-arrow-down text-danger-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0"><asp:Literal ID="litReturnedAssets" runat="server" /></h3>
                           <p class="fs-9 mb-0 text-body-secondary">Returned</p>
                       </div>
                   </div>
               </div>

               <!-- CANDIDATES -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-file-alt text-warning-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0">40</h3>
                           <p class="fs-9 mb-0 text-body-secondary">Candidates</p>
                       </div>
                   </div>
               </div>

               <!-- ONBOARDINGS -->
               <div class="col-12 col-sm-6 col-xl-3">
                   <div class="d-flex align-items-center">
                       <span class="fs-4 lh-1 uil uil-user-plus text-success-dark"></span>
                       <div class="ms-2">
                           <h3 class="mb-0">12</h3>
                           <p class="fs-9 mb-0 text-body-secondary">On Boardings</p>
                       </div>
                   </div>
               </div>

           </div>

        <div class="mx-n4 px-4 mx-lg-n6 px-lg-6 bg-body-emphasis pt-7 pb-3 border-y">
          <div class="row">
            <div class="col-12 col-xl-7 col-xxl-6">
              <div class="row g-3 mb-3">
                <div class="col-12 col-md-6">
                  <h3 class="text-body-emphasis text-nowrap">Attendance Statistics</h3>
                  <p class="text-body-tertiary mb-md-7">Daily attendence stats here</p>
                  <div class="d-flex align-items-center justify-content-between">
                    <p class="mb-0 fw-bold">Types</p>
                    <p class="mb-0 fs-9">Total count <span class="fw-bold"><asp:Literal ID="typeTotalEmployees" runat="server" /></span></p>
                  </div>
                  <hr class="bg-body-secondary mb-2 mt-2" />
                  <div class="d-flex align-items-center mb-1"><span class="d-inline-block bg-info-light bullet-item me-2"></span>
                    <p class="mb-0 fw-semibold text-body lh-sm flex-1">Present</p>
                    <h5 class="mb-0 text-body"><asp:Literal ID="typelitPresent" runat="server" /></h5>
                  </div>
                  <div class="d-flex align-items-center mb-1"><span class="d-inline-block bg-warning-light bullet-item me-2"></span>
                    <p class="mb-0 fw-semibold text-body lh-sm flex-1">Absent</p>
                    <h5 class="mb-0 text-body"><asp:Literal ID="typelitAbsent" runat="server" /></h5>
                  </div>
                  <div class="d-flex align-items-center mb-1"><span class="d-inline-block bg-danger-light bullet-item me-2"></span>
                    <p class="mb-0 fw-semibold text-body lh-sm flex-1">Late</p>
                    <h5 class="mb-0 text-body"><asp:Literal ID="typelitLate" runat="server" /></h5>
                  </div>
                  <div class="d-flex align-items-center mb-1"><span class="d-inline-block bg-success-light bullet-item me-2"></span>
                    <p class="mb-0 fw-semibold text-body lh-sm flex-1">On Leave</p>
                    <h5 class="mb-0 text-body"><asp:Literal ID="typelitOnLeave" runat="server" /></h5>
                  </div>
                  <a href="~/Pages/Attendance/biometric-integration.aspx" runat="server" class="btn btn-outline-primary mt-5">See Details<span class="fas fa-angle-right ms-2 fs-10 text-center"></span></a>
                </div>
                <div class="col-12 col-md-6">
                  <div class="position-relative mb-sm-4 mb-xl-0">
                   <div class="echart-issue-chart"
                data-echarts='{
                               "series": [{
                                 "name": "Employee Attendance",
                                 "type": "pie",
                                 "radius": ["48%", "90%"],
                                 "startAngle": 30,
                                 "label": { "show": false },
                                 "labelLine": { "show": false },
                                 "data": [
                                    { "value": <%= PresentCount %>, "name": "Present" },
                                   { "value": <%= AbsentCount %>, "name": "Absent" },
                                   { "value": <%= LateCount %>, "name": "Late" },
                                   { "value": <%= LeaveCount %>, "name": "On Leave" }
                                 ]
                               }]
                             }'
                style="min-height: 390px; width: 100%;">
            </div>

                  </div>
                </div>
              </div>
            </div>
            <div class="col-12 col-xl-5 col-xxl-6">
  <h3>Punctuality Statistics</h3>
  <p class="text-body-tertiary mb-0 mb-xl-3">
    Employee attendance & on-time performance
  </p>

  <div
    class="echart-zero-burnout-chart"
    style="min-height:320px;width:100%"
    data-echarts='{
      "legend": {
        "data": [
          { "name": "Expected", "data": <%= ExpectedJson %> },
         { "name": "Late", "data": <%= LateJson %> },
         { "name": "Punctual", "data": <%= PunctualJson %> },
         { "name": "Absent", "data": <%= AbsentJson %> },
         { "name": "Attendance", "data": <%= AttendanceJson %> }
        ]
      },
      "series": [
        {
          "name": "Expected",
          "data": <%= ExpectedJson %>
        },
        {
          "name": "Late",
          "data": <%= LateJson %>
        },
        {
          "name": "Punctual",
          "data": <%= PunctualJson %> 
        },
        {
          "name": "Absent",
          "data": <%= AbsentJson %> 
        },
        {
          "name": "Attendance",
          "data": <%= AttendanceJson %> 
        }
      ]
    }'>
  </div>
</div>

          </div>
        </div>
        <div class="mx-lg-n4 mt-3">
          <div class="row g-3">
            <div class="col-12 col-xl-6 col-xxl-7">
  <div class="card todo-list h-100">
    <div class="card-header border-bottom-0 pb-0">
      <div class="row justify-content-between align-items-center mb-4">
        <div class="col-auto">
          <h3 class="text-body-emphasis">Assets</h3>
          <p class="mb-2 mb-md-0 mb-lg-2 text-body-tertiary">
            Issued, returned & asset condition tracking
          </p>
        </div>
        
      </div>
    </div>

    <div class="card-body py-0 scrollbar to-do-list-body">

     <asp:Repeater ID="rptAssets" runat="server">
<ItemTemplate>

<div class="d-flex hover-actions-trigger py-3 border-top border-translucent">

  <div class="row gx-0 flex-1 cursor-pointer">
    <div class="col">
      <div class="d-flex align-items-center lh-1">

        <label class="fs-8 me-2 line-clamp-1">
          <%# Eval("AssetName") %>
          <%# Eval("EmployeeID") != DBNull.Value ? 
              " issued to EMP-" + Eval("EmployeeID")+ "-" + Eval("FullName") : "" %>
        </label>

        <span class='badge badge-phoenix ms-auto fs-10
          <%# 
            Eval("AssetStatus").ToString() == "ISSUED" ? "badge-phoenix-primary" :
            Eval("AssetStatus").ToString() == "RETURNED" ? "badge-phoenix-success" :
            Eval("AssetStatus").ToString() == "DAMAGED" ? "badge-phoenix-danger" :
            Eval("AssetStatus").ToString() == "LOST" ? "badge-phoenix-secondary" :
            Eval("AssetStatus").ToString() == "REPLACED" ? "badge-phoenix-warning" :
            "badge-phoenix-info"
          %>'>
          <%# Eval("AssetStatus") %>
        </span>

      </div>
    </div>
  </div>
</div>

</ItemTemplate>
</asp:Repeater>

    </div>

    <div class="card-footer border-0">
      <a class="fw-bold fs-9 mt-4" runat="server" href="~/Pages/Asset/AssetManagement.aspx">
        <span class="fas fa-eye me-1"></span>See Details
      </a>
    </div>
  </div>
</div>

            <div class="col-12 col-xl-6 col-xxl-5">
  <div class="card h-100">
    <div class="card-body">
      <div class="card-title d-flex justify-content-between align-items-center mb-1">
        <h3 class="text-body-emphasis">Recent Activity</h3>
       
      </div>
      <p class="text-body-tertiary mb-4">Recent employee check-ins and check-outs</p>
      
      <div class="timeline-vertical timeline-with-details">
      <asp:Repeater ID="rptRecentActivity" runat="server">
<ItemTemplate>

<div class="timeline-item position-relative">
  <div class="row g-md-3">

    <div class="col-12 col-md-auto d-flex">
      <div class="timeline-item-date order-1 order-md-0 me-md-4">
        <p class="fs-10 fw-semibold text-body-tertiary text-opacity-85 text-end">
          <%# Eval("PunchTime") %><br class="d-none d-md-block" />
          <%# Eval("DayLabel") %>
        </p>
      </div>

      <div class="timeline-item-bar position-md-relative me-3 me-md-0">
        <div class='icon-item icon-item-sm rounded-7 shadow-none <%# Eval("IconBgClass") %>'>
          <span class='fa-solid <%# Eval("IconClass") %> <%# Eval("IconTextClass") %> fs-10'></span>
        </div>
        <span class="timeline-bar border-end border-dashed"></span>
      </div>
    </div>

    <div class="col">
      <div class="timeline-item-content ps-6 ps-md-3">

        <div class="d-flex justify-content-between align-items-start mb-1">
          <h5 class="fs-9 lh-sm mb-0">
            <%# Eval("FullName") %> - Checked <%# Eval("PunchType") %>
          </h5>

          <span class='badge fs-10 <%# Eval("BadgeClass") %>'>
            <%# Eval("StatusText") %>
          </span>
        </div>

        <p class="fs-9 mb-1">
          <%# Eval("DesignationName") %>
        </p>

        <div class="d-flex align-items-center fs-9 text-body-secondary">
          <span class="fa-solid fa-map-marker-alt me-1"></span>
          <span><%# Eval("WorkLocation") %></span>
        </div>

      </div>
    </div>

  </div>
</div>

</ItemTemplate>
</asp:Repeater>

      </div>

      <!-- Quick Stats Summary -->
      <div class="mt-4 pt-3 border-top">
        <div class="row g-3">
          <div class="col-6 col-md-3">
            <div class="text-center">
              <h4 class="text-warning mb-1"><asp:Label ID="lblEarlyToday" runat="server" /></h4>
<p class="fs-9 text-body-tertiary mb-0">Early Today</p>
    
            </div>
          </div>
          <div class="col-6 col-md-3">
            <div class="text-center">
              <h4 class="text-danger mb-1"><asp:Label ID="lblLateToday" runat="server" /></h4>
<p class="fs-9 text-body-tertiary mb-0">Late Today</p>

            </div>
          </div>
          <div class="col-6 col-md-3">
            <div class="text-center">
              <h4 class="text-info mb-1"><asp:Label ID="lblOvertime" runat="server" /></h4>
<p class="fs-9 text-body-tertiary mb-0">Overtime</p>

            </div>
          </div>
            <div class="col-6 col-md-3">
            <div class="text-center">
              <h4 class="text-success mb-1"><asp:Label ID="lblOnTime" runat="server" /></h4>
<p class="fs-9 text-body-tertiary mb-0">On Time</p>

            </div>
          </div>
         
        </div>
      </div>
    </div>
  </div>
</div>
          </div>
        </div>
        <div class="row mt-3">
  <div class="col-12">
    <div class="mx-n4 px-4 mx-lg-n6 px-lg-6 bg-body-emphasis pt-6 border-top">
    <div id="employeePerformance" data-list='{"valueNames":["employee","kpiscore","joining","contract","overtime","punctuality","status","action"],"page":6,"pagination":true}'>
        <div class="row align-items-end justify-content-between pb-4 g-3">
            <div class="col-auto">
                <h3>Employee Performance</h3>
                <p class="text-body-tertiary lh-sm mb-0">Employee performance metrics and KPIs</p>
            </div>
        </div>
        <div class="table-responsive ms-n1 ps-1 scrollbar">
            <table class="table fs-9 mb-0 border-top border-translucent">
                <thead>
                    <tr>
                        <th class="sort white-space-nowrap align-middle ps-0" scope="col" data-sort="employee" style="width:30%;">EMPLOYEE NAME</th>
                        <th class="sort align-middle ps-3" scope="col" data-sort="kpiscore" style="width:10%;">KPI SCORE</th>
                        <th class="sort align-middle ps-3" scope="col" data-sort="joining" style="width:10%;">JOINING DATE</th>
                        <th class="sort align-middle ps-3" scope="col" data-sort="contract" style="width:15%;">CONTRACT END</th>
                        <th class="sort align-middle ps-3" scope="col" data-sort="overtime" style="width:12%;">OVERTIME</th>
                        <th class="sort align-middle ps-3" scope="col" data-sort="punctuality" style="width:5%;">PERFORMANCE</th>
                        <th class="align-middle ps-8" scope="col" data-sort="status" style="width:10%;">STATUS</th>
                        <th class="sort align-middle text-end" scope="col" style="width:10%;"></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptEmployeePerformance" runat="server">
    <ItemTemplate>
        <tr class="position-static">
            <td class="align-middle time white-space-nowrap ps-0 employee">
    <div class="d-flex align-items-center">
        <!-- Avatar -->
        <div class="flex-shrink-0 me-2">
            <asp:Image ID="imgProfile" runat="server"
                       CssClass="rounded-circle border"
                       Width="36" Height="36"
                       ImageUrl='<%# !string.IsNullOrEmpty(Eval("ProfileImageUrl").ToString()) ? ResolveUrl(Eval("ProfileImageUrl").ToString()) : ResolveUrl("~/assets/img/default-user.png") %>'
                       AlternateText="Profile Picture" />
        </div>

        <!-- Employee Name & Designation -->
        <div class="flex-grow-1">
            <a class="fw-bold fs-8 d-block mb-0" href="#"><%# Eval("EmployeeName") %></a>
            <span class="text-body-tertiary fs-9"><%# Eval("DesignationName") %></span>
        </div>
    </div>
</td>
    

            <!-- KPI Score -->
            <td class="align-middle white-space-nowrap kpiscore ps-3">
                <div class="text-center">
                    <h5 class="mb-0 <%# Convert.ToDecimal(Eval("KPIScore")) >= 90 ? "text-success" : Convert.ToDecimal(Eval("KPIScore")) >= 75 ? "text-warning" : "text-danger" %>">
                        <%# Eval("KPIScore") %>
                    </h5>
                    <p class="mb-0 fs-9 text-body"><%# Eval("Grade") %></p>
                </div>
            </td>

            <!-- Joining Date -->
            <td class="align-middle white-space-nowrap joining ps-3">
                <p class="mb-0 fs-9 text-body">
                    <%# Eval("JoiningDate") != DBNull.Value ? Convert.ToDateTime(Eval("JoiningDate")).ToString("MMM dd, yyyy") : "-" %>
                </p>
            </td>

            <!-- Contract End -->
            <td class="align-middle white-space-nowrap contract ps-3">
                <p class="mb-0 fs-9 text-body">
                    <%# Eval("ContractEndDate") != DBNull.Value ? Convert.ToDateTime(Eval("ContractEndDate")).ToString("MMM dd, yyyy") : "-" %>
                </p>
            </td>

            <!-- Overtime -->
            <td class="align-middle white-space-nowrap overtime ps-3">
                <p class="fw-bold text-body-emphasis fs-9 mb-0"><%# Eval("OvertimeHours") %> hrs</p>
                <p class="fw-semibold fs-10 text-body-tertiary mb-0">This Month</p>
            </td>

            <!-- Performance / Progress Bars -->
            <td class="align-middle white-space-nowrap ps-3 punctuality">
                <p class="text-body-secondary fs-10 mb-0"><%# Eval("PunctualityPct") %>% / 100%</p>
                <div class="progress" style="height:3px;">
                    <div class="progress-bar bg-success" style='width:<%# Eval("PunctualityPct") %>%;' role="progressbar"></div>
                </div>
            </td>

            <td class="align-middle white-space-nowrap ps-8">
                <div class="progress progress-stack mt-3" style="height:3px;">
                    <div class="progress-bar bg-primary" style='width:<%# Eval("PunctualityPct") %>%' role="progressbar" title="Punctuality"></div>
                    <div class="progress-bar bg-info" style='width:<%# Eval("AttendancePct") %>%' role="progressbar" title="Attendance"></div>
                    <div class="progress-bar bg-warning" style='width:<%# Eval("TaskCompletionPct") %>%' role="progressbar" title="Task Completion"></div>
                    <div class="progress-bar bg-success" style='width:<%# Eval("OvertimeHours") %>%' role="progressbar" title="Overtime"></div>
                </div>
            </td>

            <!-- Action -->
            <td class="align-middle text-end white-space-nowrap pe-0 action">
                <div class="btn-reveal-trigger position-static">
                    <button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs-10" type="button" data-bs-toggle="dropdown">
                        <span class="fas fa-ellipsis-h fs-10"></span>
                    </button>
                    <div class="dropdown-menu dropdown-menu-end py-2">
                        <a class="dropdown-item" href="#!">View Details</a>
                        <a class="dropdown-item" href="#!">Performance Report</a>
                        <div class="dropdown-divider"></div>
                        <a class="dropdown-item text-danger" href="#!">Flag Issue</a>
                    </div>
                </div>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>

                </tbody>
            </table>
        </div>
    </div>
</div>


  </div>
</div>
     
      </div>
    
     
</asp:Content>
