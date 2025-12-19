using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSLib.DataLayer
{
    public static class BiometricDeviceDAL
    {
        private static Database db =>
                  new DatabaseProviderFactory().Create("defaultDB");
        public static void SaveDevice(
            string name,
            string type,
            string ip,
            int port,
            int branchId,
            bool isActive,
            bool allowIn,
            bool allowOut,
            bool breakIn,
            bool breakOut,
            bool manualPunch)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_SaveBiometricDevice");

            db.AddInParameter(cmd, "@DeviceName", DbType.String, name);
            db.AddInParameter(cmd, "@DeviceType", DbType.String, type);
            db.AddInParameter(cmd, "@IPAddress", DbType.String, ip);
            db.AddInParameter(cmd, "@Port", DbType.Int32, port);
            db.AddInParameter(cmd, "@BranchID", DbType.Int32, branchId);
            db.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);
            db.AddInParameter(cmd, "@AllowIn", DbType.Boolean, allowIn);
            db.AddInParameter(cmd, "@AllowOut", DbType.Boolean, allowOut);
            db.AddInParameter(cmd, "@BreakIn", DbType.Boolean, breakIn);
            db.AddInParameter(cmd, "@BreakOut", DbType.Boolean, breakOut);
            db.AddInParameter(cmd, "@ManualPunch", DbType.Boolean, manualPunch);

            db.ExecuteNonQuery(cmd);
        }
    }
}
