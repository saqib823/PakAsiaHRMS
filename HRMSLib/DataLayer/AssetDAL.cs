using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HRMSLib.DataLayer
{
    public class AssetDAL
    {
        private static Database db => new DatabaseProviderFactory().Create("defaultDB");

        public DataTable GetAllAssets()
        {
            DbCommand cmd = db.GetSqlStringCommand("SELECT AssetID, AssetName,IsActive FROM Assets WHERE IsActive=1");
            return db.ExecuteDataSet(cmd).Tables[0];
        }

        public DataTable GetAssetRecords(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string sql = @"
                SELECT COUNT(*) OVER() AS TotalCount,
                       AR.AssetRecordID, AR.EmployeeID, E.FullName AS EmployeeName, A.AssetName,
                       AR.IssueDate, AR.ReturnDate, AR.Condition, AR.Deduction
                FROM AssetRecords AR
                INNER JOIN Employees E ON E.EmployeeID = AR.EmployeeID
                INNER JOIN Assets A ON A.AssetID = AR.AssetID
                WHERE (@SearchTerm = '' OR E.FullName LIKE '%' + @SearchTerm + '%' OR A.AssetName LIKE '%' + @SearchTerm + '%')";

            sql = $@"
                WITH AssetCTE AS ({sql})
                SELECT *
                FROM (
                    SELECT *, ROW_NUMBER() OVER (ORDER BY AssetRecordID DESC) AS RowNum
                    FROM AssetCTE
                ) AS T
                WHERE RowNum BETWEEN @StartRow AND @EndRow
                ORDER BY AssetRecordID DESC";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm ?? string.Empty);

            int startRow = (pageIndex - 1) * pageSize + 1;
            int endRow = pageIndex * pageSize;

            db.AddInParameter(cmd, "@StartRow", DbType.Int32, startRow);
            db.AddInParameter(cmd, "@EndRow", DbType.Int32, endRow);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            totalRecords = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;

            return dt;
        }

        public void SaveAsset(int assetRecordID, int employeeID, int assetID, DateTime issueDate, DateTime? returnDate, string condition, decimal deduction)
        {
            DbCommand cmd;
            if (assetRecordID == 0)
            {
                cmd = db.GetSqlStringCommand(@"
                    INSERT INTO AssetRecords (EmployeeID, AssetID, IssueDate, ReturnDate, Condition, Deduction)
                    VALUES (@EmployeeID, @AssetID, @IssueDate, @ReturnDate, @Condition, @Deduction)");
            }
            else
            {
                cmd = db.GetSqlStringCommand(@"
                    UPDATE AssetRecords
                    SET EmployeeID=@EmployeeID, AssetID=@AssetID, IssueDate=@IssueDate,
                        ReturnDate=@ReturnDate, Condition=@Condition, Deduction=@Deduction
                    WHERE AssetRecordID=@AssetRecordID");
                db.AddInParameter(cmd, "@AssetRecordID", DbType.Int32, assetRecordID);
            }

            db.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
            db.AddInParameter(cmd, "@AssetID", DbType.Int32, assetID);
            db.AddInParameter(cmd, "@IssueDate", DbType.Date, issueDate);
            db.AddInParameter(cmd, "@ReturnDate", DbType.Date, (object)returnDate ?? DBNull.Value);
            db.AddInParameter(cmd, "@Condition", DbType.String, condition);
            db.AddInParameter(cmd, "@Deduction", DbType.Decimal, deduction);

            db.ExecuteNonQuery(cmd);
        }

        public DataRow GetAssetById(int assetRecordID)
        {
            DbCommand cmd = db.GetSqlStringCommand(@"
                SELECT AR.AssetRecordID, AR.EmployeeID, E.FullName AS EmployeeName, A.AssetName,
                       AR.IssueDate, AR.ReturnDate, AR.Condition, AR.Deduction
                FROM AssetRecords AR
                INNER JOIN Employees E ON E.EmployeeID = AR.EmployeeID
                INNER JOIN Assets A ON A.AssetID = AR.AssetID
                WHERE AR.AssetRecordID=@AssetRecordID");

            db.AddInParameter(cmd, "@AssetRecordID", DbType.Int32, assetRecordID);

            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public void DeleteAsset(int assetRecordID)
        {
            DbCommand cmd = db.GetSqlStringCommand("DELETE FROM AssetRecords WHERE AssetRecordID=@AssetRecordID");
            db.AddInParameter(cmd, "@AssetRecordID", DbType.Int32, assetRecordID);
            db.ExecuteNonQuery(cmd);
        }
        public void SaveAssetMaster(int assetID, string assetName, bool isActive)
        {
            DbCommand cmd;
            if (assetID == 0)
            {
                cmd = db.GetSqlStringCommand("INSERT INTO Assets (AssetName, IsActive) VALUES (@AssetName, @IsActive)");
            }
            else
            {
                cmd = db.GetSqlStringCommand("UPDATE Assets SET AssetName=@AssetName, IsActive=@IsActive WHERE AssetID=@AssetID");
                db.AddInParameter(cmd, "@AssetID", DbType.Int32, assetID);
            }

            db.AddInParameter(cmd, "@AssetName", DbType.String, assetName);
            db.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);

            db.ExecuteNonQuery(cmd);
        }

        public DataRow GetAssetMasterById(int assetID)
        {
            DbCommand cmd = db.GetSqlStringCommand("SELECT AssetID, AssetName, IsActive FROM Assets WHERE AssetID=@AssetID");
            db.AddInParameter(cmd, "@AssetID", DbType.Int32, assetID);
            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public void DeleteAssetMaster(int assetID)
        {
            DbCommand cmd = db.GetSqlStringCommand("DELETE FROM Assets WHERE AssetID=@AssetID");
            db.AddInParameter(cmd, "@AssetID", DbType.Int32, assetID);
            db.ExecuteNonQuery(cmd);
        }

    }
}
