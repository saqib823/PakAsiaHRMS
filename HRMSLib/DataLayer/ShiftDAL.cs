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
    public static class ShiftDAL
    {
        private static Database db =>
                  new DatabaseProviderFactory().Create("defaultDB");
        public static void InsertShift(Shift shift)
        {
            DbCommand cmd = db.GetStoredProcCommand("SP_InsertShift");

            db.AddInParameter(cmd, "@ShiftName", DbType.String, shift.ShiftName);
            db.AddInParameter(cmd, "@ShiftTypeID", DbType.Int32, shift.ShiftTypeID);

            // ✔ Convert TimeSpan → DateTime
            db.AddInParameter(cmd, "@StartTime", DbType.DateTime,
                shift.StartTime.HasValue
                    ? (object)DateTime.Today.Add(shift.StartTime.Value)
                    : DBNull.Value);

            db.AddInParameter(cmd, "@EndTime", DbType.DateTime,
                shift.EndTime.HasValue
                    ? (object)DateTime.Today.Add(shift.EndTime.Value)
                    : DBNull.Value);

            db.AddInParameter(cmd, "@GraceMinutes", DbType.Int32, shift.GraceMinutes);
            db.AddInParameter(cmd, "@MinWorkMinutes", DbType.Int32, shift.MinWorkMinutes);
            db.AddInParameter(cmd, "@IsCrossMidnight", DbType.Boolean, shift.IsCrossMidnight);

            db.ExecuteNonQuery(cmd);
        }

    
        public static void AssignDepartmentShift(int deptId, int shiftId, bool isDefault)
        {
            DbCommand cmd = db.GetSqlStringCommand(
             "INSERT INTO DepartmentShifts VALUES(@D,@S,@Def)");

            db.AddInParameter(cmd, "@D", DbType.Int32, deptId);
            db.AddInParameter(cmd, "@S", DbType.Int32, shiftId);
            db.AddInParameter(cmd, "@Def", DbType.Boolean, isDefault);

            db.ExecuteNonQuery(cmd);
        }
        public static void InsertRotation(string name, string type, List<int> shifts)
        {
             
            DbCommand cmd = db.GetStoredProcCommand("SP_InsertRotation");
            db.AddInParameter(cmd, "@Name", DbType.String, name);
            db.AddInParameter(cmd, "@Type", DbType.String, type);
            db.ExecuteNonQuery(cmd);

            int rotationId = Convert.ToInt32(db.ExecuteScalar(cmd));

            int day = 1;
            foreach (var shift in shifts)
            {
                DbCommand d = db.GetSqlStringCommand(
                 "INSERT INTO ShiftRotationDetails VALUES(@R,@D,@S)");
                db.AddInParameter(d, "@R", DbType.Int32, rotationId);
                db.AddInParameter(d, "@D", DbType.Int32, day++);
                db.AddInParameter(d, "@S", DbType.Int32, shift);
                db.ExecuteNonQuery(d);
            }
        }
        public static void AssignEmployeeShift(int empId, int shiftId, DateTime from)
        {
             
            DbCommand cmd = db.GetSqlStringCommand(
             "INSERT INTO EmployeeShifts VALUES(@E,@S,@F,NULL)");
            db.AddInParameter(cmd, "@E", DbType.Int32, empId);
            db.AddInParameter(cmd, "@S", DbType.Int32, shiftId);
            db.AddInParameter(cmd, "@F", DbType.Date, from);
            db.ExecuteNonQuery(cmd);
        }
        public static DataTable GetShiftsPaged(int pageNumber, int pageSize, out int totalRecords)
        {
            totalRecords = 0;

            DbCommand cmd = db.GetSqlStringCommand(@"
        ;WITH CTE AS
        (
            SELECT 
                S.ShiftID,
                S.ShiftName,
                ST.ShiftTypeName,
                S.StartTime,
                S.EndTime,
                S.GraceMinutes,
                S.MinWorkMinutes,
                S.IsCrossMidnight,
                ROW_NUMBER() OVER (ORDER BY S.ShiftName) AS RowNum
            FROM Shifts S
            INNER JOIN ShiftTypes ST ON ST.ShiftTypeID = S.ShiftTypeID
        )
        SELECT *
        FROM CTE
        WHERE RowNum BETWEEN @StartRow AND @EndRow;

        SELECT COUNT(*) FROM Shifts;
    ");

            int startRow = ((pageNumber - 1) * pageSize) + 1;
            int endRow = pageNumber * pageSize;

            db.AddInParameter(cmd, "@StartRow", DbType.Int32, startRow);
            db.AddInParameter(cmd, "@EndRow", DbType.Int32, endRow);

            DataSet ds = db.ExecuteDataSet(cmd);
            totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

            return ds.Tables[0];
        }
        public static void DeleteShift(int shiftId)
        {
            DbCommand cmd = db.GetSqlStringCommand(
                "DELETE FROM Shifts WHERE ShiftID = @ShiftID");

            db.AddInParameter(cmd, "@ShiftID", DbType.Int32, shiftId);
            db.ExecuteNonQuery(cmd);
        }
        public static DataTable GetShiftTypes()
        {

            DbCommand cmd = db.GetSqlStringCommand(
                "SELECT ShiftTypeID AS ID, ShiftTypeName AS Name FROM ShiftTypes");

            return db.ExecuteDataSet(cmd).Tables[0];
        }
        public static DataRow GetShiftByID(int shiftId)
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
        SELECT *
        FROM Shifts
        WHERE ShiftID = @ShiftID
    ");

            db.AddInParameter(cmd, "@ShiftID", DbType.Int32, shiftId);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        public static void UpdateShift(Shift shift)
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
        UPDATE Shifts
        SET
            ShiftName = @ShiftName,
            ShiftTypeID = @ShiftTypeID,
            StartTime = @StartTime,
            EndTime = @EndTime,
            GraceMinutes = @GraceMinutes,
            MinWorkMinutes = @MinWorkMinutes,
            IsCrossMidnight = @IsCrossMidnight
        WHERE ShiftID = @ShiftID
    ");

            db.AddInParameter(cmd, "@ShiftID", DbType.Int32, shift.ShiftID);
            db.AddInParameter(cmd, "@ShiftName", DbType.String, shift.ShiftName);
            db.AddInParameter(cmd, "@ShiftTypeID", DbType.Int32, shift.ShiftTypeID);

            // ✅ FIX: TimeSpan → DateTime OR DBNull
            db.AddInParameter(cmd, "@StartTime", DbType.DateTime,
                shift.StartTime.HasValue
                    ? (object)DateTime.Today.Add(shift.StartTime.Value)
                    : DBNull.Value);

            db.AddInParameter(cmd, "@EndTime", DbType.DateTime,
                shift.EndTime.HasValue
                    ? (object)DateTime.Today.Add(shift.EndTime.Value)
                    : DBNull.Value);

            db.AddInParameter(cmd, "@GraceMinutes", DbType.Int32, shift.GraceMinutes);
            db.AddInParameter(cmd, "@MinWorkMinutes", DbType.Int32, shift.MinWorkMinutes);

            // ✅ Boolean is OK
            db.AddInParameter(cmd, "@IsCrossMidnight", DbType.Boolean, shift.IsCrossMidnight);

            db.ExecuteNonQuery(cmd);
        }
        // ---------- Split Shift ----------

        public static DataTable GetSplitTypeShifts()
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
        SELECT ShiftID, ShiftName
        FROM Shifts
        WHERE ShiftTypeID = (
            SELECT ShiftTypeID
            FROM ShiftTypes
            WHERE ShiftTypeName LIKE '%Split%'
        )
        ORDER BY ShiftName");

            return db.ExecuteDataSet(cmd).Tables[0];
        }

        public static DataTable GetSplitShiftDetailsPaged(
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            totalRecords = Convert.ToInt32(
                db.ExecuteScalar(
                    db.GetSqlStringCommand("SELECT COUNT(*) FROM SplitShiftDetails")
                )
            );

            DbCommand cmd = db.GetSqlStringCommand(@"
        SELECT
            d.SplitShiftID,
            s.ShiftName,
            d.PartNo,
            CONVERT(VARCHAR(5), d.StartTime, 108) AS StartTime,
            CONVERT(VARCHAR(5), d.EndTime, 108) AS EndTime
        FROM SplitShiftDetails d
        INNER JOIN Shifts s ON s.ShiftID = d.ShiftID
        ORDER BY d.SplitShiftID DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

            db.AddInParameter(cmd, "@Offset", DbType.Int32, pageIndex * pageSize);
            db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

            return db.ExecuteDataSet(cmd).Tables[0];
        }

        public static void InsertSplitShift(
    int shiftId,
    TimeSpan? s1, TimeSpan? e1,
    TimeSpan? s2, TimeSpan? e2)
        {
            // Only insert Part 1 if times are provided
            if ((s1.HasValue && s1.Value != TimeSpan.Zero) || (e1.HasValue && e1.Value != TimeSpan.Zero))
            {
                InsertSplitShiftPart(shiftId, 1, s1, e1);
            }

            // Only insert Part 2 if times are provided
            if ((s2.HasValue && s2.Value != TimeSpan.Zero) || (e2.HasValue && e2.Value != TimeSpan.Zero))
            {
                InsertSplitShiftPart(shiftId, 2, s2, e2);
            }
        }
        private static void InsertSplitShiftPart(
            int shiftId, int partNo, TimeSpan? start, TimeSpan? end)
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
        INSERT INTO SplitShiftDetails
        (ShiftID, PartNo, StartTime, EndTime)
        VALUES (@ShiftID, @PartNo, @StartTime, @EndTime)");

            db.AddInParameter(cmd, "@ShiftID", DbType.Int32, shiftId);
            db.AddInParameter(cmd, "@PartNo", DbType.Int32, partNo);

            db.AddInParameter(cmd, "@StartTime", DbType.DateTime,
                start.HasValue ? (object)DateTime.Today.Add(start.Value) : DBNull.Value);
            db.AddInParameter(cmd, "@EndTime", DbType.DateTime,
                end.HasValue ? (object)DateTime.Today.Add(end.Value) : DBNull.Value);

            db.ExecuteNonQuery(cmd);
        }

        public static void DeleteSplitShift(int splitShiftId)
        {
            DbCommand cmd = db.GetSqlStringCommand(
                "DELETE FROM SplitShiftDetails WHERE SplitShiftID = @ID");

            db.AddInParameter(cmd, "@ID", DbType.Int32, splitShiftId);
            db.ExecuteNonQuery(cmd);
        }

        public static void UpdateSplitShift(int splitShiftId, TimeSpan? start, TimeSpan? end, int partNo)
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
        UPDATE SplitShiftDetails
        SET StartTime = @StartTime,
            EndTime = @EndTime
        WHERE SplitShiftID = @ID AND PartNo = @PartNo
    ");

            db.AddInParameter(cmd, "@ID", DbType.Int32, splitShiftId);
            db.AddInParameter(cmd, "@PartNo", DbType.Int32, partNo);
            db.AddInParameter(cmd, "@StartTime", DbType.DateTime, start.HasValue ? (object)DateTime.Today.Add(start.Value) : DBNull.Value);
            db.AddInParameter(cmd, "@EndTime", DbType.DateTime, end.HasValue ? (object)DateTime.Today.Add(end.Value) : DBNull.Value);

            db.ExecuteNonQuery(cmd);
        }


        public static DataTable GetSplitShiftById(int splitShiftId)
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
        SELECT *
        FROM SplitShiftDetails
        WHERE SplitShiftID = @ID
    ");

            db.AddInParameter(cmd, "@ID", DbType.Int32, splitShiftId);

            // Return the full DataTable, not a single DataRow
            return db.ExecuteDataSet(cmd).Tables[0];
        }


    }

}
