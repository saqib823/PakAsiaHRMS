CREATE PROCEDURE [dbo].[SP_Loans_Select_Paged]
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @SearchText NVARCHAR(100) = '',
    @EmployeeID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize
    DECLARE @TotalRecords INT
    
    -- Create a temporary table to store the filtered results
    CREATE TABLE #FilteredLoans (
        LoanID INT,
        EmployeeID INT,
        EmployeeName NVARCHAR(200),
        EmployeeNo NVARCHAR(50),
        DepartmentName NVARCHAR(100),
        DesignationName NVARCHAR(100),
        LoanType NVARCHAR(100),
        LoanAmount DECIMAL(18,2),
        DurationMonths INT,
        MonthlyDeduction DECIMAL(18,2),
        StartDate DATE,
        Status NVARCHAR(50),
        CreatedDate DATETIME
    )
    
    -- Insert filtered data into temporary table
    INSERT INTO #FilteredLoans (
        LoanID, EmployeeID, EmployeeName, EmployeeNo, DepartmentName, DesignationName,
        LoanType, LoanAmount, DurationMonths, MonthlyDeduction, StartDate, Status, CreatedDate
    )
    SELECT 
        l.LoanID,
        l.EmployeeID,
        e.EmployeeName,
        e.EmployeeNo,
        d.DepartmentName,
        des.DesignationName,
        l.LoanType,
        l.LoanAmount,
        l.DurationMonths,
        l.MonthlyDeduction,
        l.StartDate,
        l.Status,
        l.CreatedDate
    FROM Loans l
    INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
    LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID
    LEFT JOIN Designations des ON e.DesignationID = des.DesignationID
    WHERE 
        (@EmployeeID IS NULL OR l.EmployeeID = @EmployeeID)
        AND (
            @SearchText = '' OR
            e.EmployeeName LIKE '%' + @SearchText + '%' OR
            e.EmployeeNo LIKE '%' + @SearchText + '%' OR
            l.LoanType LIKE '%' + @SearchText + '%' OR
            l.Status LIKE '%' + @SearchText + '%'
        )
    ORDER BY l.CreatedDate DESC
    
    -- Get total records
    SELECT @TotalRecords = COUNT(*) FROM #FilteredLoans
    
    -- Return paginated results with TotalRecords as additional column
    SELECT 
        *,
        @TotalRecords AS TotalRecords
    FROM #FilteredLoans
    ORDER BY CreatedDate DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
    
    -- Clean up
    DROP TABLE #FilteredLoans
END
