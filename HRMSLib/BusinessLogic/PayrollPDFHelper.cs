using System;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

namespace HRMSLib.BusinessLogic
{
    public class PayrollPDFHelper
    {
        public static byte[] GeneratePayslip(DataRow salaryRow)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                // Header
                doc.Add(new Paragraph("Company Name", boldFont));
                doc.Add(new Paragraph("Payslip", boldFont));
                doc.Add(new Paragraph("\n"));

                // Employee info
                doc.Add(new Paragraph($"Employee: {salaryRow["FullName"]}", regularFont));
                doc.Add(new Paragraph($"Effective From: {Convert.ToDateTime(salaryRow["EffectiveFrom"]):dd-MMM-yyyy}", regularFont));
                doc.Add(new Paragraph($"Status: {(Convert.ToBoolean(salaryRow["IsActive"]) ? "Active" : "Inactive")}", regularFont));
                doc.Add(new Paragraph("\n"));

                // Salary table
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;

                void AddRow(string label, string value)
                {
                    table.AddCell(new PdfPCell(new Phrase(label, boldFont)) { Border = Rectangle.NO_BORDER });
                    table.AddCell(new PdfPCell(new Phrase(value, regularFont)) { Border = Rectangle.NO_BORDER });
                }

                AddRow("Basic", salaryRow["Basic"].ToString());
                AddRow("House Rent", salaryRow["HouseRent"].ToString());
                AddRow("Utilities", salaryRow["Utilities"].ToString());
                AddRow("Medical", salaryRow["Medical"].ToString());
                AddRow("Fuel", salaryRow["Fuel"].ToString());
                AddRow("Transport", salaryRow["Transport"].ToString());
                AddRow("Mobile Allowance", salaryRow["Mobile"].ToString());
                AddRow("Bonus", salaryRow["Bonus"].ToString());
                AddRow("Commission", salaryRow["Commission"].ToString());
                AddRow("Incentives", salaryRow["Incentives"].ToString());
                AddRow("Custom Allowances", salaryRow["CustomAllowances"].ToString());
                AddRow("Deductions", salaryRow["Deductions"].ToString());

                decimal gross = Convert.ToDecimal(salaryRow["GrossSalary"]);
                decimal deductions = salaryRow["Deductions"] != DBNull.Value ? Convert.ToDecimal(salaryRow["Deductions"]) : 0;
                decimal net = gross - deductions;

                AddRow("Gross Salary", gross.ToString("N2"));
                AddRow("Net Pay", net.ToString("N2"));

                doc.Add(table);
                doc.Close();

                return ms.ToArray();
            }
           
        }

        public static byte[] ExportToExcel(DataRow row, string sheetName = "Payslip")
        {
            // Non-commercial license context
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(sheetName);

                // Write column headers
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    ws.Cells[1, i + 1].Value = row.Table.Columns[i].ColumnName;
                }

                // Write row values
                for (int c = 0; c < row.Table.Columns.Count; c++)
                {
                    ws.Cells[2, c + 1].Value = row[c];
                }

                // Auto-fit columns
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
        public static byte[] ExportAttendance(DataTable dt, string sheetName)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(sheetName);

                // Write column headers
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    ws.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                    ws.Cells[1, col + 1].Style.Font.Bold = true;
                }

                // Write all rows
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        ws.Cells[row + 2, col + 1].Value = dt.Rows[row][col];
                    }
                }

                // Auto-fit columns
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
    }
    
}
