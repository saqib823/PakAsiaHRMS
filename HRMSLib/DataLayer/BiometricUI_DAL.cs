using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public class BiometricUI_DAL
    {
        private static Database db =>
            new DatabaseProviderFactory().Create("defaultDB");

        public static DataTable GetDevices()
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
                SELECT D.DeviceName,
                       D.DeviceSerialNo,
                       D.IPAddress,
                       D.Port,
                       B.Name AS BranchName,
                       D.IsActive
                FROM BiometricDevice D
                INNER JOIN Branch B ON B.ID = D.BranchID
            ");

            return db.ExecuteDataSet(cmd).Tables[0];
        }

        public static DataTable GetAttendanceLogs(string empCode)
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
                SELECT
                    E.EmployeeCode,
                    A.PunchType,
                    CONVERT(date, A.PunchDateTime) AS PunchDate,
                    CONVERT(varchar(8), A.PunchDateTime, 108) AS PunchTime,
                    A.DeviceSerialNo
                FROM Attendance A
                INNER JOIN Employee E ON E.EmployeeID = A.EmployeeID
                WHERE (@EmpCode IS NULL OR E.EmployeeCode LIKE '%' + @EmpCode + '%')
                ORDER BY A.PunchDateTime DESC
            ");

            db.AddInParameter(cmd, "@EmpCode", DbType.String,
                string.IsNullOrEmpty(empCode) ? null : empCode);

            return db.ExecuteDataSet(cmd).Tables[0];
        }
    }
}
