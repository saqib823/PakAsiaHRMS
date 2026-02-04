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
            context.Response.ContentType = "application/json";

            int pageIndex = int.TryParse(context.Request.Form["PageIndex"], out int pi) ? pi : 1;
            int pageSize = int.TryParse(context.Request.Form["PageSize"], out int ps) ? ps : 10;

            string search = context.Request.Form["Search"];

            int? branchID = int.TryParse(context.Request.Form["BranchID"], out int b) ? b : (int?)null;
            int? departmentID = int.TryParse(context.Request.Form["DepartmentID"], out int d) ? d : (int?)null;

            DateTime? startDate = DateTime.TryParse(context.Request.Form["StartDate"], out DateTime sd)
                ? sd
                : (DateTime?)null;

            DateTime? endDate = DateTime.TryParse(context.Request.Form["EndDate"], out DateTime ed)
                ? ed
                : (DateTime?)null;

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

            var jsonList = dt.AsEnumerable().Select(r => new
            {
                EmpNo = r["EmpNo"].ToString(),
                FullName = r["FullName"].ToString(),

                PunchDate = r.Field<DateTime>("PunchDate")
           .ToString("dd-MMM-yyyy"),

                PunchTime =
           r["PunchDateTime"] is DateTime dtValue
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
