using HRMSLib.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hrms_PakAsia.Handler
{
    /// <summary>
    /// Summary description for BiometricPush
    /// </summary>
    public class BiometricPush : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            string sn = req.Form["SN"];
            string pin = req.Form["PIN"];
            string status = req.Form["Status"];
            string verify = req.Form["Verify"];

            if (string.IsNullOrEmpty(sn) || string.IsNullOrEmpty(pin))
            {
                context.Response.Write("INVALID");
                return;
            }

            DateTime punchDateTime = DateTime.Now;

            BiometricDAL.InsertRawLog(
                sn,
                pin,
                punchDateTime,
                Convert.ToInt32(status),
                Convert.ToInt32(verify)
            );

            BiometricDAL.ProcessAttendance(
                pin,
                punchDateTime,
                Convert.ToInt32(status),
                sn
            );

            context.Response.Write("OK");
        }

        public bool IsReusable => false;
    }
}