-- =============================================
-- Author: PakAsia HRMS System
-- Description: Gets loan details by LoanID with employee information
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetLoanById]
    @LoanID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        l.LoanID,
        l.EmployeeID,
        l.LoanType,
        l.LoanAmount,
        l.DurationMonths,
        l.MonthlyDeduction,
        l.StartDate,
        l.Status,
        l.ApprovedBy,
        l.ApprovedDate,
        l.CreatedBy,
        l.CreatedDate,
        e.FullName AS EmployeeName,
        e.EmployeeNo,
        d.DepartmentName AS DepartmentName,
        des.DesignationName AS DesignationName
    FROM 
        Loans l
    INNER JOIN 
        Employees e ON l.EmployeeID = e.EmployeeID
 LEFT JOIN 
        EmployeeEmployment ee ON ee.EmployeeID = e.EmployeeID
    LEFT JOIN 
        Departments d ON ee.DepartmentID = d.DepartmentID
    LEFT JOIN 
        Designations des ON ee.DesignationID = des.DesignationID
    WHERE 
        l.LoanID = @LoanID
    
    SELECT 
        @@ROWCOUNT AS RecordCount
END
