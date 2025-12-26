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

            int totalRecords;

            var dt = AttendanceDAL.GetAttendanceLogs("", null, null, pageIndex, pageSize, out totalRecords);

            var jsonList = dt.AsEnumerable().Select(r => new
            {
                FullName = r["FullName"],
                VerifyModeName = r["VerifyModeName"],
                PunchType = r["PunchType"],
                OprtDateFormatted = Convert.ToDateTime(r["OprtDate"]).ToString("dd-MMM-yyyy"),
                OprtTimeFormatted = Convert.ToDateTime(r["OprtDate"]).ToString("hh:mm tt")
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
