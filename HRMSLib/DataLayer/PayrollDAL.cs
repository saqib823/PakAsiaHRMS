using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HRMSLib.DataLayer
{
    public class PayrollDAL
    {
        private static Database db => new DatabaseProviderFactory().Create("defaultDB");

        #region Salary Structure Methods

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
                    E.EmployeeNo,
                    EE.DesignationID,
                    DEPT.DepartmentName,
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
                LEFT JOIN EmployeeEmployment EE ON EE.EmployeeID = E.EmployeeID
                LEFT JOIN Departments DEPT ON DEPT.DepartmentID = EE.DepartmentID
                WHERE S.SalaryID = @SalaryID";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@SalaryID", DbType.Int32, salaryId);
            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public int SaveSalaryStructure(SalaryStructureModel model)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_SaveSalaryStructure");

            db.AddInParameter(cmd, "@SalaryID", DbType.Int32, model.SalaryID);
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, model.EmployeeID);
            db.AddInParameter(cmd, "@EffectiveFrom", DbType.Date, model.EffectiveFrom);
            db.AddInParameter(cmd, "@Basic", DbType.Decimal, model.Basic);
            db.AddInParameter(cmd, "@HouseRent", DbType.Decimal, model.HouseRent);
            db.AddInParameter(cmd, "@Utilities", DbType.Decimal, model.Utilities);
            db.AddInParameter(cmd, "@Medical", DbType.Decimal, model.Medical);
            db.AddInParameter(cmd, "@Fuel", DbType.Decimal, model.Fuel);
            db.AddInParameter(cmd, "@Transport", DbType.Decimal, model.Transport);
            db.AddInParameter(cmd, "@Mobile", DbType.Decimal, model.Mobile);
            db.AddInParameter(cmd, "@Bonus", DbType.Decimal, model.Bonus);
            db.AddInParameter(cmd, "@Commission", DbType.Decimal, model.Commission);
            db.AddInParameter(cmd, "@Incentives", DbType.Decimal, model.Incentives);
            db.AddInParameter(cmd, "@CustomAllowances", DbType.String, model.CustomAllowances);
            db.AddInParameter(cmd, "@Deductions", DbType.String, model.Deductions);
            db.AddInParameter(cmd, "@IsActive", DbType.Boolean, model.IsActive);
            db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, model.CreatedBy);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return Convert.ToInt32(dt.Rows[0]["NewSalaryID"]);
        }

        public bool DeleteSalary(int salaryId)
        {
            string sql = "DELETE FROM SalaryStructure WHERE SalaryID = @SalaryID";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@SalaryID", DbType.Int32, salaryId);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        #endregion

        #region Payroll Processing Methods

        public DataSet GetPayrollPeriods(string status = null)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_GetPayrollPeriods");
            db.AddInParameter(cmd, "@Status", DbType.String, string.IsNullOrEmpty(status) ? (object)DBNull.Value : status);
            return db.ExecuteDataSet(cmd);
        }

        public int SavePayrollPeriod(PayrollPeriodModel model)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_SavePayrollPeriod");

            db.AddInParameter(cmd, "@PeriodID", DbType.Int32, model.PeriodID);
            db.AddInParameter(cmd, "@PeriodName", DbType.String, model.PeriodName);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, model.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, model.EndDate);
            db.AddInParameter(cmd, "@Status", DbType.String, model.Status);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return Convert.ToInt32(dt.Rows[0]["NewPeriodID"]);
        }

        public string ProcessPayroll(int periodId, int processedBy)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_ProcessPayroll");
            db.AddInParameter(cmd, "@PeriodID", DbType.Int32, periodId);
            db.AddInParameter(cmd, "@ProcessedBy", DbType.Int32, processedBy);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return dt.Rows[0]["Message"].ToString();
        }

        public DataSet GetPayrollReport(string reportType, int? periodId = null,
            DateTime? startDate = null, DateTime? endDate = null,
            int? departmentId = null)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_GetPayrollReports");

            db.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
            db.AddInParameter(cmd, "@PeriodID", DbType.Int32, periodId ?? 0);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate ?? (object)DBNull.Value);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate ?? (object)DBNull.Value);
            db.AddInParameter(cmd, "@DepartmentID", DbType.Int32, departmentId ?? 0);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GenerateBankFile(int periodId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_GenerateBankFile");
            db.AddInParameter(cmd, "@PeriodID", DbType.Int32, periodId);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetEmployeeAttendanceSummary(int employeeId, DateTime startDate, DateTime endDate)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_GetEmployeeAttendanceSummary");
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);

            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetPayrollProcessingData(int periodId)
        {
            string sql = @"
                SELECT 
                    PP.PeriodName,
                    E.FullName,
                    E.EmployeeNo,
                    EE.DesignationID,
                    DEPT.DepartmentName,
                    PR.*
                FROM PayrollProcessing PR
                INNER JOIN Employees E ON E.EmployeeID = PR.EmployeeID
                INNER JOIN PayrollPeriod PP ON PP.PeriodID = PR.PeriodID
                LEFT JOIN EmployeeEmployment EE ON EE.EmployeeID = E.EmployeeID
                LEFT JOIN Departments DEPT ON DEPT.DepartmentID = EE.DepartmentID
                WHERE PR.PeriodID = @PeriodID
                ORDER BY E.FullName";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@PeriodID", DbType.Int32, periodId);
            return db.ExecuteDataSet(cmd);
        }

        #endregion

        #region Employee Deductions Methods

        public DataSet GetEmployeeDeductions(int? employeeId = null, string deductionType = null)
        {
            string sql = @"
                SELECT ED.*, E.FullName, E.EmployeeNo
                FROM EmployeeDeductions ED
                INNER JOIN Employees E ON E.EmployeeID = ED.EmployeeID
                WHERE (@EmployeeID IS NULL OR ED.EmployeeID = @EmployeeID)
                    AND (@DeductionType IS NULL OR ED.DeductionType = @DeductionType)
                ORDER BY ED.StartDate DESC";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeId ?? (object)DBNull.Value);
            db.AddInParameter(cmd, "@DeductionType", DbType.String, string.IsNullOrEmpty(deductionType) ? (object)DBNull.Value : deductionType);

            return db.ExecuteDataSet(cmd);
        }

        public int SaveEmployeeDeduction(EmployeeDeductionModel model)
        {
            string sql = @"
                IF @DeductionID = 0
                BEGIN
                    INSERT INTO EmployeeDeductions (
                        EmployeeID, DeductionType, Description, Amount, 
                        InstallmentAmount, TotalInstallments, StartDate, EndDate, Status
                    )
                    VALUES (
                        @EmployeeID, @DeductionType, @Description, @Amount,
                        @InstallmentAmount, @TotalInstallments, @StartDate, @EndDate, @Status
                    );
                    
                    SELECT SCOPE_IDENTITY() AS NewDeductionID;
                END
                ELSE
                BEGIN
                    UPDATE EmployeeDeductions SET
                        EmployeeID = @EmployeeID,
                        DeductionType = @DeductionType,
                        Description = @Description,
                        Amount = @Amount,
                        InstallmentAmount = @InstallmentAmount,
                        TotalInstallments = @TotalInstallments,
                        StartDate = @StartDate,
                        EndDate = @EndDate,
                        Status = @Status
                    WHERE DeductionID = @DeductionID;
                    
                    SELECT @DeductionID AS NewDeductionID;
                END";

            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@DeductionID", DbType.Int32, model.DeductionID);
            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, model.EmployeeID);
            db.AddInParameter(cmd, "@DeductionType", DbType.String, model.DeductionType);
            db.AddInParameter(cmd, "@Description", DbType.String, model.Description);
            db.AddInParameter(cmd, "@Amount", DbType.Decimal, model.Amount);
            db.AddInParameter(cmd, "@InstallmentAmount", DbType.Decimal, model.InstallmentAmount);
            db.AddInParameter(cmd, "@TotalInstallments", DbType.Int32, model.TotalInstallments);
            db.AddInParameter(cmd, "@StartDate", DbType.Date, model.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.Date, model.EndDate ?? (object)DBNull.Value);
            db.AddInParameter(cmd, "@Status", DbType.String, model.Status);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return Convert.ToInt32(dt.Rows[0]["NewDeductionID"]);
        }

        public bool DeleteEmployeeDeduction(int deductionId)
        {
            string sql = "DELETE FROM EmployeeDeductions WHERE DeductionID = @DeductionID";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@DeductionID", DbType.Int32, deductionId);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        #endregion

        #region Payroll Settings Methods

        public DataSet GetPayrollSettings()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_GetPayrollSettings");
            return db.ExecuteDataSet(cmd);
        }

        public bool SavePayrollSetting(string settingKey, string settingValue, string description = null)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_SavePayrollSettings");

            db.AddInParameter(cmd, "@SettingKey", DbType.String, settingKey);
            db.AddInParameter(cmd, "@SettingValue", DbType.String, settingValue);
            db.AddInParameter(cmd, "@Description", DbType.String, string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);

            return db.ExecuteNonQuery(cmd) > 0;
        }

        #endregion

        #region Utility Methods

        public DataSet GetEmployeesForPayroll(DateTime startDate, DateTime endDate)
        {
            string sql = @"
                SELECT 
                    E.EmployeeID,
                    E.FullName,
                    E.EmployeeNo,
                    EE.DesignationID,
                    DEPT.DepartmentName,
                    SS.*
                FROM Employees E
                INNER JOIN SalaryStructure SS ON SS.EmployeeID = E.EmployeeID AND SS.IsActive = 1
                LEFT JOIN EmployeeEmployment EE ON EE.EmployeeID = E.EmployeeID
                LEFT JOIN Departments DEPT ON DEPT.DepartmentID = EE.DepartmentID
                WHERE E.IsActive = 1
                ORDER BY E.FullName";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetPayrollSummary(int periodId)
        {
            string sql = @"
                SELECT 
                    COUNT(*) AS TotalEmployees,
                    SUM(BasicSalary) AS TotalBasic,
                    SUM(GrossSalary) AS TotalGross,
                    SUM(TotalDeductions) AS TotalDeductions,
                    SUM(NetSalary) AS TotalNet,
                    AVG(NetSalary) AS AverageSalary
                FROM PayrollProcessing
                WHERE PeriodID = @PeriodID";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@PeriodID", DbType.Int32, periodId);
            return db.ExecuteDataSet(cmd);
        }

        #endregion
    }

    #region Model Classes

    public class SalaryStructureModel
    {
        public int SalaryID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public decimal Basic { get; set; }
        public decimal HouseRent { get; set; }
        public decimal Utilities { get; set; }
        public decimal Medical { get; set; }
        public decimal Fuel { get; set; }
        public decimal Transport { get; set; }
        public decimal Mobile { get; set; }
        public decimal Bonus { get; set; }
        public decimal Commission { get; set; }
        public decimal Incentives { get; set; }
        public string CustomAllowances { get; set; }
        public string Deductions { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class PayrollPeriodModel
    {
        public int PeriodID { get; set; }
        public string PeriodName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Draft";
    }

    public class EmployeeDeductionModel
    {
        public int DeductionID { get; set; }
        public int EmployeeID { get; set; }
        public string DeductionType { get; set; } // Loan, Advance, Other
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int TotalInstallments { get; set; }
        public int PaidInstallments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active";
    }

    public class PayrollProcessingModel
    {
        public int PayrollID { get; set; }
        public int EmployeeID { get; set; }
        public int PeriodID { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public int AttendanceDays { get; set; }
        public int LateMinutes { get; set; }
        public int EarlyLeaveMinutes { get; set; }
        public decimal HalfDays { get; set; }
        public int AbsentDays { get; set; }
        public decimal LateDeduction { get; set; }
        public decimal EarlyLeaveDeduction { get; set; }
        public decimal HalfDayDeduction { get; set; }
        public decimal AbsentDeduction { get; set; }
        public decimal LoanDeduction { get; set; }
        public decimal AdvanceDeduction { get; set; }
        public decimal EOBI { get; set; }
        public decimal ProvidentFund { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal GrossSalary { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime ProcessedDate { get; set; }
        public int ProcessedBy { get; set; }
    }

    #endregion
}