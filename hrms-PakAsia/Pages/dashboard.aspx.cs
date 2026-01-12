using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace hrms_PakAsia.Pages
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected List<EmployeePerformance> EmployeePerformanceList = new List<EmployeePerformance>();
        LoggedInUser currentUser = null;
        // Add these public properties
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LateCount { get; set; }
        public int LeaveCount { get; set; }

        // Add these properties for line chart
        public string ExpectedJson { get; set; }
        public string LateJson { get; set; }
        public string PunctualJson { get; set; }
        public string AbsentJson { get; set; }
        public string AttendanceJson { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSession();
            currentUser = GetSessionData();

            if (!IsPostBack)
            {
                PopulateDashboardData();
            }
        }

        public LoggedInUser GetSessionData()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
            return currentUser;
        }

        public void CheckSession()
        {
            LoggedInUser currentUser = HttpContext.Current.Session["LoggedInUser"] as LoggedInUser;
            if (currentUser == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        private void PopulateDashboardData()
        {
            try
            {
                DataSet ds = DashboardDAL.GetDashboardData();

                if (ds != null && ds.Tables.Count > 0)
                {
                    // Table 1: Today's Attendance Summary & Counters
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        // Set KPI counters
                        litTotalEmployees.Text = row["TotalEmployees"].ToString();
                        litUsers.Text = row["Users"].ToString();
                        litDepartments.Text = row["Departments"].ToString();
                        litPresent.Text = row["Present"].ToString();
                        litAbsent.Text = row["Absent"].ToString();
                        litLate.Text = row["Late"].ToString();
                        litOnLeave.Text = row["OnLeave"].ToString();
                        litAssets.Text = row["TotalAssets"].ToString();
                        litIssuedAssets.Text = row["IssuedAssets"].ToString();
                        litReturnedAssets.Text = row["ReturnedAssets"].ToString();

                        // For Attendance Statistics section
                        typeTotalEmployees.Text = row["TotalEmployees"].ToString();
                        typelitPresent.Text = row["Present"].ToString();
                        typelitAbsent.Text = row["Absent"].ToString();
                        typelitLate.Text = row["Late"].ToString();
                        typelitOnLeave.Text = row["OnLeave"].ToString();

                        // Set properties for pie chart
                        PresentCount = Convert.ToInt32(row["Present"]);
                        AbsentCount = Convert.ToInt32(row["Absent"]);
                        LateCount = Convert.ToInt32(row["Late"]);
                        LeaveCount = Convert.ToInt32(row["OnLeave"]);
                    }

                    // Table 2: Attendance Trend (for line chart)
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<string> dates = new List<string>();
                        List<int> expectedList = new List<int>();
                        List<int> lateList = new List<int>();
                        List<int> punctualList = new List<int>();
                        List<int> absentList = new List<int>();
                        List<int> attendanceList = new List<int>();

                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            int totalEmployees = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalEmployees"]);
                            expectedList.Add(totalEmployees);
                            lateList.Add(Convert.ToInt32(row["Late"]));
                            punctualList.Add(Convert.ToInt32(row["Punctual"]));
                            absentList.Add(Convert.ToInt32(row["Absent"]));
                            int present = Convert.ToInt32(row["Present"]);
                            attendanceList.Add(present);
                        }

                        // Set properties for line chart
                        ExpectedJson = "[" + string.Join(",", expectedList) + "]";
                        LateJson = "[" + string.Join(",", lateList) + "]";
                        PunctualJson = "[" + string.Join(",", punctualList) + "]";
                        AbsentJson = "[" + string.Join(",", absentList) + "]";
                        AttendanceJson = "[" + string.Join(",", attendanceList) + "]";
                    }

                    // Rest of your existing code for Tables 3-6...
                    // Table 3: Assets Activity
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        rptAssets.DataSource = ds.Tables[2];
                        rptAssets.DataBind();
                    }

                    // Table 4: Recent Activity
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        rptRecentActivity.DataSource = ds.Tables[3];
                        rptRecentActivity.DataBind();
                    }

                    // Table 5: Quick Stats
                    if (ds.Tables[4].Rows.Count > 0 && ds.Tables[4].Rows[0] != null)
                    {
                        DataRow statsRow = ds.Tables[4].Rows[0];
                        lblEarlyToday.Text = statsRow["EarlyToday"].ToString();
                        lblLateToday.Text = statsRow["LateToday"].ToString();
                        lblOvertime.Text = statsRow["OvertimeToday"].ToString();
                        lblOnTime.Text = statsRow["OnTime"].ToString();
                    }

                    // Table 6: Employee Performance
                    if (ds != null && ds.Tables.Count > 5 && ds.Tables[5] != null)
                    {
                        DataTable dtEmpPerf = ds.Tables[5];

                        EmployeePerformanceList = dtEmpPerf.AsEnumerable().Select(r => new EmployeePerformance
                        {
                            EmployeeID = Convert.ToInt32(r["EmployeeID"]),
                            EmployeeName = r["EmployeeName"].ToString(),
                            DesignationName = r["DesignationName"].ToString(),
                            WorkLocation = r["WorkLocation"].ToString(),
                            AttendancePct = r["AttendancePct"] != DBNull.Value ? Convert.ToDecimal(r["AttendancePct"]) : 0,
                            PunctualityPct = r["PunctualityPct"] != DBNull.Value ? Convert.ToDecimal(r["PunctualityPct"]) : 0,
                            OvertimeHours = r["OvertimeHours"] != DBNull.Value ? Convert.ToDecimal(r["OvertimeHours"]) : 0,
                            KPIScore = r["KPIScore"] != DBNull.Value ? Convert.ToDecimal(r["KPIScore"]) : 0,
                            Grade = r["Grade"]?.ToString() ?? "-",

                            //TaskCompletionPct = r["TaskCompletionPct"] != DBNull.Value ? Convert.ToDecimal(r["TaskCompletionPct"]) : 0,

                            JoiningDate = r["JoiningDate"] != DBNull.Value
                                ? (DateTime?)Convert.ToDateTime(r["JoiningDate"])
                                : null,

                                                        ContractEndDate = r["ContractEndDate"] != DBNull.Value
                                ? (DateTime?)Convert.ToDateTime(r["ContractEndDate"])
                                : null,
                            ProfileImageUrl = !string.IsNullOrEmpty(r["PhotographPath"]?.ToString()) ? r["PhotographPath"].ToString() : "../assets/img/default-user.png"
                        }).ToList();

                        // Bind repeater
                        rptEmployeePerformance.DataSource = EmployeePerformanceList;
                        rptEmployeePerformance.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                // Log error
                string errorMessage = $"Error loading dashboard data: {ex.Message}";
                ScriptManager.RegisterStartupScript(this, GetType(), "DashboardError",
                    $"alert('{errorMessage}');", true);
            }
        }

        // Helper method to safely get image URL
        protected string GetProfileImageUrl(object imagePath)
        {
            if (imagePath != null && !string.IsNullOrEmpty(imagePath.ToString()))
            {
                // Resolve the URL relative to the application root
                return ResolveUrl("~/" + imagePath.ToString());
            }
            return ResolveUrl("~/assets/img/default-user.png");
        }
        public class EmployeePerformance
        {
            public int EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public string DesignationName { get; set; }
            public string WorkLocation { get; set; }
            public decimal AttendancePct { get; set; }
            public decimal PunctualityPct { get; set; }
            public decimal TaskCompletionPct { get; set; }
            public decimal OvertimeHours { get; set; }
            public decimal KPIScore { get; set; }
            public string Grade { get; set; }
            public DateTime? JoiningDate { get; set; }
            public DateTime? ContractEndDate { get; set; }
            public string ProfileImageUrl { get; set; }

        }
    }

}