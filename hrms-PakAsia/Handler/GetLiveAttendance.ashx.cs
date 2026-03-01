using HRMSLib.BusinessLogic;
using HRMSLib.DataLayer;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace hrms_PakAsia.Handler
{
    public class GetLiveAttendance : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string export = context.Request["export"];   // works for both GET & POST
            
            int pageIndex = int.TryParse(context.Request["PageIndex"], out int pi) ? pi : 1;
            int pageSize = int.TryParse(context.Request["PageSize"], out int ps) ? ps : 10;

            string search = context.Request["Search"];

            int? branchID = int.TryParse(context.Request["BranchID"], out int b) ? b : (int?)null;
            int? departmentID = int.TryParse(context.Request["DepartmentID"], out int d) ? d : (int?)null;
            if (export == "1")
            {
                pageIndex = 1;
                pageSize = int.MaxValue;   // get all records
            }
            DateTime? startDate = DateTime.TryParse(context.Request["StartDate"], out DateTime sd)
                ? sd : (DateTime?)null;

            DateTime? endDate = DateTime.TryParse(context.Request["EndDate"], out DateTime ed)
                ? ed : (DateTime?)null;

            int totalRecords;

            var dt = AttendanceDAL.GetAttendanceLogs(
                search,
                startDate,
                endDate,
                branchID,
                departmentID,
                pageIndex,
                pageSize,
                out totalRecords
            );

            // ===========================
            // 🔽 IF EXPORT REQUEST
            // ===========================
            if (export == "1")
            {
                context.Response.Clear();
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.AddHeader("content-disposition",
                    "attachment; filename=Attendance_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");

                byte[] bytes = PayrollPDFHelper.ExportAttendance(dt, "Attendance");
                context.Response.BinaryWrite(bytes);
                context.Response.End();
                return;
            }

            // ===========================
            // 🔽 NORMAL JSON RESPONSE
            // ===========================
            context.Response.ContentType = "application/json";

            var jsonList = dt.AsEnumerable().Select(r => new
            {
                EmpNo = r["EmpNo"].ToString(),
                FullName = r["FullName"].ToString(),
                PunchDate = r.Field<DateTime>("PunchDate").ToString("dd-MMM-yyyy"),
                PunchTime = r["PunchDateTime"] is DateTime dtValue
                    ? dtValue.ToString("HH:mm")
                    : ((TimeSpan)r["PunchDateTime"]).ToString(@"hh\:mm"),
                PunchType = r["PunchType"].ToString(),
                VerifyModeName = r["VerifyModeName"].ToString()
            }).ToList();

            string json = JsonConvert.SerializeObject(new
            {
                Data = jsonList,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            });

            context.Response.Write(json);
        }
        public bool IsReusable => false;
    }
}
