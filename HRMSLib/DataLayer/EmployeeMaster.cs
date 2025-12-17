using HRMSLib.BusinessLogic;
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
    [Serializable]
    public class EmployeeMaster
    {
        public async Task<long> EmployeeBasicInfoAsync(
            string empID, string empTitle, string empFullName, string empGuardian,
            DateTime empDoB, string empGender, string empCnic, DateTime empCNICExpiry,
            string empMaritalStatus, string Nationality, string Religion, string empImagePath,
            string empBloodGroup, string empCreatedBy)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("SP_Employee");

                // Input parameters
                db.AddInParameter(cmd, "@Mode", DbType.Int32, 1);
                db.AddInParameter(cmd, "@empID", DbType.String, empID);
                db.AddInParameter(cmd, "@empTitle", DbType.String, empTitle);
                db.AddInParameter(cmd, "@empFullName", DbType.String, empFullName);
                db.AddInParameter(cmd, "@empGuardian", DbType.String, empGuardian);
                db.AddInParameter(cmd, "@empDoB", DbType.Date, empDoB);
                db.AddInParameter(cmd, "@empGender", DbType.String, empGender);
                db.AddInParameter(cmd, "@empCnic", DbType.String, empCnic);
                db.AddInParameter(cmd, "@empCNICExpiry", DbType.Date, empCNICExpiry);
                db.AddInParameter(cmd, "@empMaritalStatus", DbType.String, empMaritalStatus);
                db.AddInParameter(cmd, "@Nationality", DbType.String, Nationality);
                db.AddInParameter(cmd, "@Religion", DbType.String, Religion);
                db.AddInParameter(cmd, "@empImagePath", DbType.String, empImagePath);
                db.AddInParameter(cmd, "@BloodGroup", DbType.String, empBloodGroup);
                db.AddInParameter(cmd, "@empCreatedBy", DbType.String, empCreatedBy);

                // Output parameter to get inserted EmployeeID
                db.AddOutParameter(cmd, "@empID_New", DbType.Int64, sizeof(long));

                // Execute
                await Task.Run(() => db.ExecuteNonQuery(cmd));

                // Read the output value
                long newEmployeeID = Convert.ToInt64(db.GetParameterValue(cmd, "@empID_New"));
                return newEmployeeID;
            }
            catch (Exception)
            {
                return 0; // return 0 if failed
            }
        }
        public async Task<long> EmployeeContactsAsync(
             string EmployeeID, string empPersonalEmail, string empOfficialEmail,
             string empPhone, string empAlternatePhone, string empEmergencyContact, string empEmergencyName,
             string empEmergencyRelationship, string empPermanentAddress1, string empPermanentAddress2,
             string empPermCity, string empPermState, string empPermPostalCode, string empCurrentAddress1,
             string empCurrentAddress2, string CurrentState)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("SP_Employee");

                // Input parameters
                db.AddInParameter(cmd, "@Mode", DbType.Int32, 2);
                db.AddInParameter(cmd, "@EmployeeID", DbType.Int64, Convert.ToInt64(EmployeeID));
                db.AddInParameter(cmd, "@MobileNumber", DbType.String, empPhone);
                db.AddInParameter(cmd, "@AlternateMobileNumber", DbType.String, empAlternatePhone);
                db.AddInParameter(cmd, "@PersonalEmail", DbType.String, empPersonalEmail);
                db.AddInParameter(cmd, "@PermanentAddress", DbType.String, string.IsNullOrEmpty(empPermanentAddress2) ? empPermanentAddress1 : empPermanentAddress2);
                db.AddInParameter(cmd, "@CurrentAddress", DbType.String, string.IsNullOrEmpty(empCurrentAddress2) ? empCurrentAddress1 : empCurrentAddress2);
                db.AddInParameter(cmd, "@City", DbType.String, empPermCity);
                db.AddInParameter(cmd, "@Province", DbType.String, CurrentState);
                db.AddInParameter(cmd, "@EmergencyContactName", DbType.String, empEmergencyName);
                db.AddInParameter(cmd, "@EmergencyContactRelation", DbType.String, empEmergencyRelationship);
                db.AddInParameter(cmd, "@EmergencyContactNumber", DbType.String, empEmergencyContact);
                db.AddInParameter(cmd, "@OfficialEmail", DbType.String, empOfficialEmail);
                db.AddInParameter(cmd, "@PostalCode", DbType.String, empPermPostalCode);

                db.AddOutParameter(cmd, "@empID_New", DbType.Int64, sizeof(long));

                await Task.Run(() => db.ExecuteNonQuery(cmd));

                long newEmployeeID = Convert.ToInt64(db.GetParameterValue(cmd, "@empID_New"));
                return newEmployeeID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<long> EmployeeEmploymentAsync(
            long EmployeeID, int DepartmentID, int DesignationID, int ReportingManagerID,
            string EmploymentType, string EmploymentStatus, DateTime JoiningDate,
            DateTime? ConfirmationDate, DateTime? ContractEndDate, DateTime? ProbationEndDate,
            string WorkLocation, string EmployeeCategory)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("SP_Employee");

                db.AddInParameter(cmd, "@Mode", DbType.Int32, 3);
                db.AddInParameter(cmd, "@EmployeeID", DbType.Int64, EmployeeID);
                db.AddInParameter(cmd, "@DepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(cmd, "@DesignationID", DbType.Int32, DesignationID);
                db.AddInParameter(cmd, "@ReportingManagerID", DbType.Int64, ReportingManagerID);
                db.AddInParameter(cmd, "@EmploymentType", DbType.String, EmploymentType);
                db.AddInParameter(cmd, "@EmploymentStatus", DbType.String, EmploymentStatus);
                db.AddInParameter(cmd, "@JoiningDate", DbType.Date, JoiningDate);
                db.AddInParameter(cmd, "@ConfirmationDate", DbType.Date, (object)ConfirmationDate ?? DBNull.Value);
                db.AddInParameter(cmd, "@ContractEndDate", DbType.Date, (object)ContractEndDate ?? DBNull.Value);
                db.AddInParameter(cmd, "@ProbationEndDate", DbType.DateTime, (object)ProbationEndDate ?? DBNull.Value);
                db.AddInParameter(cmd, "@WorkLocation", DbType.String, WorkLocation);
                db.AddInParameter(cmd, "@EmployeeCategory", DbType.String, EmployeeCategory);
                db.AddOutParameter(cmd, "@empID_New", DbType.Int64, sizeof(long));

                await Task.Run(() => db.ExecuteNonQuery(cmd));

                return EmployeeID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<long> EmployeeAttendanceAsync(long EmployeeID, int ShiftID, List<string> WeeklyOffDays,
    int AttendancePolicyID, int AllowedLateCount, int AllowedEarlyLeaveCount, string BiometricMachineUserID)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("SP_Employee");

                db.AddInParameter(cmd, "@Mode", DbType.Int32, 4);
                db.AddInParameter(cmd, "@EmployeeID", DbType.Int64, EmployeeID);
                db.AddInParameter(cmd, "@ShiftID", DbType.Int32, ShiftID);

                // Join multiple weekly off days into a comma-separated string
                string weeklyOff = WeeklyOffDays != null && WeeklyOffDays.Count > 0
                                   ? string.Join(",", WeeklyOffDays)
                                   : null;
                db.AddInParameter(cmd, "@WeeklyOffDay", DbType.String, weeklyOff);

                db.AddInParameter(cmd, "@AttendancePolicyID", DbType.Int32, AttendancePolicyID);
                db.AddInParameter(cmd, "@AllowedLateCount", DbType.Int32, AllowedLateCount);
                db.AddInParameter(cmd, "@AllowedEarlyLeaveCount", DbType.Int32, AllowedEarlyLeaveCount);
                db.AddInParameter(cmd, "@BiometricMachineUserID", DbType.String, BiometricMachineUserID);
                db.AddOutParameter(cmd, "@empID_New", DbType.Int64, sizeof(long));

                await Task.Run(() => db.ExecuteNonQuery(cmd));

                return EmployeeID;
            }
            catch
            {
                return 0;
            }
        }
        public async Task<long> EmployeePayrollAsync(long employeeID, int salaryType, decimal basicSalary,
  decimal houseRent, decimal medicalAllowance, decimal transportAllowance, decimal otherAllowances, decimal grossSalary,
  decimal overtimeRate, int payrollCycle, string paymentMethod, string bankName, string bankAccount, string taxStatusOrNTN,
  string eobiNumber, string socialSecurity)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("SP_Employee");

                db.AddInParameter(cmd, "@Mode", DbType.Int32, 5);
                db.AddInParameter(cmd, "@EmployeeID", DbType.Int64, employeeID);

                db.AddInParameter(cmd, "@SalaryType", DbType.Int32, salaryType);
                db.AddInParameter(cmd, "@BasicSalary", DbType.Decimal, basicSalary);
                db.AddInParameter(cmd, "@HouseRent", DbType.Decimal, houseRent);
                db.AddInParameter(cmd, "@MedicalAllowance", DbType.Decimal, medicalAllowance);
                db.AddInParameter(cmd, "@TransportAllowance", DbType.Decimal, transportAllowance);
                db.AddInParameter(cmd, "@OtherAllowances", DbType.Decimal, otherAllowances);
                db.AddInParameter(cmd, "@GrossSalary", DbType.Decimal, grossSalary);
                db.AddInParameter(cmd, "@OvertimeRate", DbType.Decimal, overtimeRate);
                db.AddInParameter(cmd, "@PayrollCycle", DbType.Int32, payrollCycle);
                db.AddInParameter(cmd, "@SalaryPaymentMethod", DbType.String, paymentMethod);
                db.AddInParameter(cmd, "@BankName", DbType.String, bankName);
                db.AddInParameter(cmd, "@BankAccountOrIBAN", DbType.String, bankAccount);
                db.AddInParameter(cmd, "@TaxStatusOrNTN", DbType.String, taxStatusOrNTN);
                db.AddInParameter(cmd, "@EOBINumber", DbType.String, eobiNumber);
                db.AddInParameter(cmd, "@SocialSecurityNumber", DbType.String, socialSecurity);
                db.AddOutParameter(cmd, "@empID_New", DbType.Int64, sizeof(long));

                await Task.Run(() => db.ExecuteNonQuery(cmd));

                return employeeID;
            }
            catch
            {
                return 0;
            }
        }
        public async Task<long> EmployeeLegalAsync(long employeeID,
    string contractFile, string cnicFront, string cnicBack,
    List<string> educationFiles, List<string> experienceFiles, List<string> otherDocs,
    string ndaStatus, string termsStatus,
    bool appointmentLetterIssued, DateTime? contractStart)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create("defaultDB");
                DbCommand cmd = db.GetStoredProcCommand("SP_Employee");

                db.AddInParameter(cmd, "@Mode", DbType.Int32, 6);
                db.AddInParameter(cmd, "@EmployeeID", DbType.Int64, employeeID);

                db.AddInParameter(cmd, "@ContractFile", DbType.String, contractFile);
                db.AddInParameter(cmd, "@CNICFrontFile", DbType.String, cnicFront);
                db.AddInParameter(cmd, "@CNICBackFile", DbType.String, cnicBack);

                db.AddInParameter(cmd, "@EducationCertificates", DbType.String,
                    educationFiles != null ? string.Join(",", educationFiles) : null);
                db.AddInParameter(cmd, "@ExperienceLetters", DbType.String,
                    experienceFiles != null ? string.Join(",", experienceFiles) : null);
                db.AddInParameter(cmd, "@OtherDocuments", DbType.String,
                    otherDocs != null ? string.Join(",", otherDocs) : null);

                db.AddInParameter(cmd, "@NDASigned", DbType.String, ndaStatus);
                db.AddInParameter(cmd, "@TermsAccepted", DbType.String, termsStatus);

                db.AddInParameter(cmd, "@AppointmentLetterIssued", DbType.Boolean, appointmentLetterIssued);

                db.AddInParameter(cmd, "@ContractStartDate", DbType.Date, contractStart);
                db.AddOutParameter(cmd, "@empID_New", DbType.Int64, sizeof(long));

                await Task.Run(() => db.ExecuteNonQuery(cmd));
                return employeeID;
            }
            catch
            {
                return 0;
            }
        }

        public DataTable GetEmployees(int pageNumber, int pageSize, string searchText, string sortField,
     string sortOrder, out int totalRecords)
        {
            totalRecords = 0;

            Database db = new DatabaseProviderFactory().Create("defaultDB");
            DbCommand cmd = db.GetStoredProcCommand("SP_GetEmployees_Paged");

            db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@SearchText", DbType.String, string.IsNullOrWhiteSpace(searchText) ? null : searchText);
            db.AddInParameter(cmd, "@SortField", DbType.String, sortField ?? "EmployeeID");
            db.AddInParameter(cmd, "@SortOrder", DbType.String, sortOrder ?? "DESC");

            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                totalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecords"]);
                return ds.Tables[0];
            }

            return new DataTable();
        }

        public void DeleteEmployee(long employeeID)
        {
            Database db = new DatabaseProviderFactory().Create("defaultDB");
            string sql = "DELETE FROM [Employees] WHERE EmployeeID = @EmployeeID";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
                db.ExecuteNonQuery(cmd);
            }
            
        }
    }
}