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
    public class CommonDAL
    {
        public static DataSet GetDepartments()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT DepartmentID ID, DepartmentName Name FROM Departments WHERE Status = 1 ORDER BY DepartmentName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetRoles()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT RoleId ID, RoleName Name FROM Roles WHERE Status = 1 ORDER BY RoleName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetBranches()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT BranchID ID, BranchName Name FROM Branches WHERE Status = 1 ORDER BY BranchName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetDesignation()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT DesignationID ID, DesignationName Name FROM Designations WHERE Status = 1 ORDER BY DesignationName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetEmployees()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT EmployeeID ID, FullName Name,  FullName + ' - ' + EmployeeNo NameNumber FROM Employees ORDER BY FullName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetBioMetricEmployees()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT [EmpNo] ID, [EmpName] Name FROM [TaurusMJ].[dbo].[RS_Emp] ORDER BY EmpName";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetTitles()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM Title ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetGender()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM Gender ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetMaritalStatus()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM MaritalStatus ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetBloodGroup()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM BloodGroup ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetShiftTiming()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM ShiftTiming ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetWorkDays()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM WorkDays ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetDays()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM Days ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetAttendanceMethod()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM AttendanceMethod ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetPayrollCycle()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM PayrollCycle ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetPaymentMethod()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM PaymentMethod ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetMonths()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                string query = "SELECT ID, Name FROM Months ORDER BY Name";
                return db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
