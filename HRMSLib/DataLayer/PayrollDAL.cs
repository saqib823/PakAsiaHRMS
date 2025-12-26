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
    public class PayrollDAL
    {
        private static Database db =>
                 new DatabaseProviderFactory().Create("defaultDB");



        #region Salary Structure

        public DataSet GetSalaryStructuresPaged(string searchText, int pageIndex, int pageSize)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_GetSalaryStructures_Paged");

            db.AddInParameter(cmd, "@SearchText", DbType.String, searchText);
            db.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

            return db.ExecuteDataSet(cmd);
        }
        public DataRow GetSalaryById(int salaryId)
        {
            string sql = @"
        SELECT 
            S.SalaryID,
            E.FullName,
            S.Basic,
            S.EmployeeID,
            S.HouseRent,
            S.Utilities,
            S.Medical,
            S.Fuel,
            S.Transport,
            S.Mobile,
            S.Bonus,
            S.Commission,
            S.Incentives,
            (S.Basic + S.HouseRent + S.Utilities + S.Medical + 
             S.Fuel + S.Transport + S.Mobile + S.Bonus +
             S.Commission + S.Incentives) AS GrossSalary,
            S.CustomAllowances,
            S.Deductions,
            S.EffectiveFrom,
            S.IsActive
        FROM SalaryStructure S
        INNER JOIN Employees E ON E.EmployeeID = S.EmployeeID
        WHERE S.SalaryID = @SalaryID";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@SalaryID", DbType.Int32, salaryId);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }


        public void SaveSalaryStructure(
            int salaryId,
            int employeeId,
            DateTime effectiveFrom,
            decimal basic,
            decimal houseRent,
            decimal utilities,
            decimal medical,
            decimal fuel,
            decimal transport,
            decimal mobile,
            decimal bonus,
            decimal commission,
            decimal incentives,
            string customAllowances,
            string deductions,
            bool isActive)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_SaveSalaryStructure");

            db.AddInParameter(cmd, "@SalaryID", DbType.Int32, salaryId);
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);
            db.AddInParameter(cmd, "@EffectiveFrom", DbType.Date, effectiveFrom);
            db.AddInParameter(cmd, "@Basic", DbType.Decimal, basic);
            db.AddInParameter(cmd, "@HouseRent", DbType.Decimal, houseRent);
            db.AddInParameter(cmd, "@Utilities", DbType.Decimal, utilities);
            db.AddInParameter(cmd, "@Medical", DbType.Decimal, medical);
            db.AddInParameter(cmd, "@Fuel", DbType.Decimal, fuel);
            db.AddInParameter(cmd, "@Transport", DbType.Decimal, transport);
            db.AddInParameter(cmd, "@Mobile", DbType.Decimal, mobile);
            db.AddInParameter(cmd, "@Bonus", DbType.Decimal, bonus);
            db.AddInParameter(cmd, "@Commission", DbType.Decimal, commission);
            db.AddInParameter(cmd, "@Incentives", DbType.Decimal, incentives);
            db.AddInParameter(cmd, "@CustomAllowances", DbType.String, customAllowances);
            db.AddInParameter(cmd, "@Deductions", DbType.String, deductions);
            db.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);

            db.ExecuteNonQuery(cmd);
        }

        public void DeleteSalary(int salaryId)
        {
            DbCommand cmd = db.GetSqlStringCommand(
                "DELETE FROM SalaryStructure WHERE SalaryID = @SalaryID");

            db.AddInParameter(cmd, "@SalaryID", DbType.Int32, salaryId);

            db.ExecuteNonQuery(cmd);
        }

        #endregion
    }
}
