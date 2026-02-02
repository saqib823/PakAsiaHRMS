using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public static class RecruitmentDAL
    {
        private static Database db = new DatabaseProviderFactory().Create("defaultDB");

        // INSERT / UPDATE / DELETE JobPosting
        public static bool SaveJobPosting(int mode, long? id, string name, int status, DateTime? createdDate, long? createdBy)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SP_JobPosting");
                db.AddInParameter(cmd, "@Mode", DbType.Int32, mode);
                db.AddInParameter(cmd, "@ID", DbType.Int64, id.HasValue ? id.Value : (object)DBNull.Value);
                db.AddInParameter(cmd, "@Name", DbType.String, string.IsNullOrEmpty(name) ? (object)DBNull.Value : name);
                db.AddInParameter(cmd, "@Status", DbType.Int32, status);
                db.AddInParameter(cmd, "@CreatedDate", DbType.DateTime, createdDate.HasValue ? createdDate.Value : (object)DBNull.Value);
                db.AddInParameter(cmd, "@CreatedBy", DbType.Int64, createdBy.HasValue ? createdBy.Value : (object)DBNull.Value);

                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void UpdateCandidateStatus(long candidateId, int status)
        {

            DbCommand cmd = db.GetStoredProcCommand("SP_UpdateCandidateStatus");
                db.AddInParameter(cmd, "@ID", System.Data.DbType.Int64, candidateId);
                db.AddInParameter(cmd, "@Status", System.Data.DbType.Int32, status);
                db.ExecuteNonQuery(cmd);
            
        }

        // GET JobPosting with Pagination
        public static DataTable GetJobPostingsPaged(int pageNumber, int pageSize, string searchName, int? status, out int totalRecords)
        {
            totalRecords = 0;
            DataTable dt = new DataTable();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SP_GetJobPostingPaged");
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
                db.AddInParameter(cmd, "@SearchName", DbType.String, string.IsNullOrEmpty(searchName) ? (object)DBNull.Value : searchName);
                db.AddInParameter(cmd, "@Status", DbType.Int32, status.HasValue ? status.Value : (object)DBNull.Value);

                dt = db.ExecuteDataSet(cmd).Tables[0];

                if (dt.Rows.Count > 0 && dt.Columns.Contains("TotalRecords"))
                {
                    totalRecords = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);
                }
            }
            catch { }

            return dt;
        }

        // GET JobPosting By ID
        public static DataRow GetJobPostingById(long id)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SP_GetJobPostingPaged");
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, 1);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, 1);
                db.AddInParameter(cmd, "@SearchName", DbType.String, DBNull.Value);
                db.AddInParameter(cmd, "@Status", DbType.Int32, DBNull.Value);
                dt = db.ExecuteDataSet(cmd).Tables[0];

                DataRow row = dt.Select($"ID = {id}")?.Length > 0 ? dt.Select($"ID = {id}")[0] : null;
                return row;
            }
            catch { return null; }
        }

        // DELETE JobPosting
        public static bool DeleteJobPosting(long id)
        {
            return SaveJobPosting(3, id, null, 0, null, null);
        }
        public static bool SaveCandidate(int mode, long? id, long jobId, string name, int status,
            DateTime? createdDate, long? createdBy, string CVPath)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SP_Candidates");
                db.AddInParameter(cmd, "@Mode", DbType.Int32, mode);
                db.AddInParameter(cmd, "@ID", DbType.Int64, id.HasValue ? id.Value : (object)DBNull.Value);
                db.AddInParameter(cmd, "@JobID", DbType.Int64, jobId);
                db.AddInParameter(cmd, "@Name", DbType.String, string.IsNullOrEmpty(name) ? (object)DBNull.Value : name);
                db.AddInParameter(cmd, "@Status", DbType.Int32, status);
                db.AddInParameter(cmd, "@CreatedDate", DbType.DateTime, createdDate.HasValue ? createdDate.Value : (object)DBNull.Value);
                db.AddInParameter(cmd, "@CreatedBy", DbType.Int64, createdBy.HasValue ? createdBy.Value : (object)DBNull.Value);
                db.AddInParameter(cmd, "@CVPath", DbType.String, CVPath);

                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // GET Candidates with pagination
        public static DataTable GetCandidatesPaged(int pageNumber, int pageSize, string searchName, long? jobId, out int totalRecords)
        {
            totalRecords = 0;
            DataTable dt = new DataTable();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SP_GetCandidatesPaged");
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, pageNumber);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
                db.AddInParameter(cmd, "@SearchName", DbType.String, string.IsNullOrEmpty(searchName) ? (object)DBNull.Value : searchName);
                db.AddInParameter(cmd, "@JobID", DbType.Int64, jobId.HasValue ? jobId.Value : (object)DBNull.Value);

                DataSet ds = db.ExecuteDataSet(cmd);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);
            }
            catch { }

            return dt;
        }

        // GET Candidate by ID
        public static DataRow GetCandidateById(long id)
        {
            DataTable dt = new DataTable();
            try
            {
                int total;
                dt = GetCandidatesPaged(1, 1, null, null, out total);
                DataRow row = dt.Select($"ID={id}").Length > 0 ? dt.Select($"ID={id}")[0] : null;
                return row;
            }
            catch { return null; }
        }

        // DELETE Candidate
        public static bool DeleteCandidate(long id)
        {
            return SaveCandidate(3, id, 0, null, 0, null, null,"");
        }
    }
}
