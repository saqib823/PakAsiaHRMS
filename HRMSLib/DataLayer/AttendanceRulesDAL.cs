using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public class AttendanceRulesDAL
    {
        private static Database db => new DatabaseProviderFactory().Create("defaultDB");

        // =================== GET RULES WITH PAGINATION ===================
        public DataTable GetRules(string searchText, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            DataTable dt = new DataTable();

            try
            {
                string sql = @"
                    SELECT * FROM AttendanceRules
                    WHERE RuleName LIKE @Search OR RuleType LIKE @Search
                    ORDER BY RuleID
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*) AS TotalCount FROM AttendanceRules
                    WHERE RuleName LIKE @Search OR RuleType LIKE @Search;
                ";

                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@Search", DbType.String, "%" + searchText + "%");
                db.AddInParameter(cmd, "@Offset", DbType.Int32, (pageIndex - 1) * pageSize);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    DataTable dtRules = new DataTable();
                    dtRules.Load(dr);
                    dt = dtRules;

                    if (dr.NextResult() && dr.Read())
                        totalRecords = Convert.ToInt32(dr["TotalCount"]);
                }
            }
            catch
            {
                throw;
            }

            return dt;
        }

        // =================== SAVE RULE ===================
        public void SaveRule(
            int ruleID,
            string ruleType,
            string ruleName,
            int? graceMinutes,
            int? lateAllowance,
            int? halfDayThreshold,
            int? absentThreshold,
            int? allowedEarlyLeaves,
            int? preShiftOT,
            int? postShiftOT,
            int? maxOTHours,
            bool weekendOT,
            bool holidayOT,
            string weeklyOffPattern,
            int? branchHolidayID,
            string user)
        {
            try
            {
                DbCommand cmd;

                if (ruleID > 0)
                {
                    // Update
                    string sqlUpdate = @"
                        UPDATE AttendanceRules SET
                            RuleType=@Type,
                            RuleName=@Name,
                            GraceMinutes=@Grace,
                            LateAllowance=@LateAllowance,
                            HalfDayThreshold=@HalfDay,
                            AbsentThreshold=@Absent,
                            AllowedEarlyLeaves=@AllowedEarly,
                            PreShiftOT=@PreOT,
                            PostShiftOT=@PostOT,
                            MaxOTHours=@MaxOT,
                            WeekendOT=@WeekendOT,
                            HolidayOT=@HolidayOT,
                            WeeklyOffPattern=@WeeklyPattern,
                            BranchHolidayID=@BranchID,
                            ModifiedBy=@ModifiedBy,
                            ModifiedDate=GETDATE()
                        WHERE RuleID=@ID
                    ";

                    cmd = db.GetSqlStringCommand(sqlUpdate);
                    db.AddInParameter(cmd, "@ID", DbType.Int32, ruleID);
                    db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, 1);
                }
                else
                {
                    // Insert
                    string sqlInsert = @"
                        INSERT INTO AttendanceRules
                        (RuleType, RuleName, GraceMinutes, LateAllowance, HalfDayThreshold, AbsentThreshold,
                        AllowedEarlyLeaves, PreShiftOT, PostShiftOT, MaxOTHours, WeekendOT, HolidayOT,
                        WeeklyOffPattern, BranchHolidayID, CreatedBy)
                        VALUES
                        (@Type,@Name,@Grace,@LateAllowance,@HalfDay,@Absent,@AllowedEarly,@PreOT,@PostOT,@MaxOT,
                        @WeekendOT,@HolidayOT,@WeeklyPattern,@BranchID,@User)
                    ";

                    cmd = db.GetSqlStringCommand(sqlInsert);
                    db.AddInParameter(cmd, "@User", DbType.String, user);
                }

                db.AddInParameter(cmd, "@Type", DbType.String, ruleType);
                db.AddInParameter(cmd, "@Name", DbType.String, ruleName);
                db.AddInParameter(cmd, "@Grace", DbType.Int32, (object)graceMinutes ?? DBNull.Value);
                db.AddInParameter(cmd, "@LateAllowance", DbType.Int32, (object)lateAllowance ?? DBNull.Value);
                db.AddInParameter(cmd, "@HalfDay", DbType.Int32, (object)halfDayThreshold ?? DBNull.Value);
                db.AddInParameter(cmd, "@Absent", DbType.Int32, (object)absentThreshold ?? DBNull.Value);
                db.AddInParameter(cmd, "@AllowedEarly", DbType.Int32, (object)allowedEarlyLeaves ?? DBNull.Value);
                db.AddInParameter(cmd, "@PreOT", DbType.Int32, (object)preShiftOT ?? DBNull.Value);
                db.AddInParameter(cmd, "@PostOT", DbType.Int32, (object)postShiftOT ?? DBNull.Value);
                db.AddInParameter(cmd, "@MaxOT", DbType.Int32, (object)maxOTHours ?? DBNull.Value);
                db.AddInParameter(cmd, "@WeekendOT", DbType.Boolean, weekendOT);
                db.AddInParameter(cmd, "@HolidayOT", DbType.Boolean, holidayOT);
                db.AddInParameter(cmd, "@WeeklyPattern", DbType.String, (object)weeklyOffPattern ?? DBNull.Value);
                db.AddInParameter(cmd, "@BranchID", DbType.Int32, (object)branchHolidayID ?? DBNull.Value);

                db.ExecuteNonQuery(cmd);
            }
            catch
            {
                throw;
            }
        }

        // =================== GET RULE BY ID ===================
        public DataRow GetRuleByID(int ruleID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM AttendanceRules WHERE RuleID=@ID";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@ID", DbType.Int32, ruleID);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    dt.Load(dr);
                }
            }
            catch
            {
                throw;
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        // =================== DELETE RULE ===================
        public void DeleteRule(int ruleID)
        {
            try
            {
                string sql = "DELETE FROM AttendanceRules WHERE RuleID=@ID";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@ID", DbType.Int32, ruleID);
                db.ExecuteNonQuery(cmd);
            }
            catch
            {
                throw;
            }
        }
    }
}
