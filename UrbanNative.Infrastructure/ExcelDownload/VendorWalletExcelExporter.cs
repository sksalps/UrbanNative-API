using ClosedXML.Excel;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Infrastructure.ExcelDownload
{
    public class VendorWalletSummaryExcel
    {
        public static byte[] Generate(VendorWalletSummaryReportDto report, string dateRange)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Wallet Summary");

            int row = 1;

            // Title
            sheet.Cell(row++, 1).Value = "Vendor Wallet Summary";
            sheet.Range(1, 1, 1, 6).Merge().Style.Font.Bold = true;

            // Vendor Info
            sheet.Cell(row++, 1).Value = $"Vendor: {report.VendorName}";
            sheet.Cell(row++, 1).Value = $"GSTIN: {report.GSTIN}";
            sheet.Cell(row++, 1).Value = $"Address: {report.Address}";

            row++;

            // Date Range & Report Date
            sheet.Cell(row++, 1).Value = dateRange;
            sheet.Cell(row++, 1).Value = $"Report Date: {DateTime.Now:dd MMM yyyy}";

            row++;

            // Summary
            sheet.Cell(row++, 1).Value = $"Credit: {report.Totals.TotalCredit:N2}";
            sheet.Cell(row++, 1).Value = $"Debit: {report.Totals.TotalDebit:N2}";
            sheet.Cell(row++, 1).Value = $"Locked: {report.Totals.LockedAmount:N2}";
            sheet.Cell(row++, 1).Value = $"Available: {report.Totals.AvailableBalance:N2}";
            sheet.Cell(row++, 1).Value = $"Net: {report.Totals.NetBalance:N2}";

            row++;

            // Table Header
            sheet.Cell(row, 1).Value = "Account Head";
            sheet.Cell(row, 2).Value = "Narration";
            sheet.Cell(row, 3).Value = "Credit";
            sheet.Cell(row, 4).Value = "Debit";
            sheet.Cell(row, 5).Value = "Locked";
            sheet.Cell(row, 6).Value = "Net";

            sheet.Range(row, 1, row, 6).Style.Font.Bold = true;
            row++;

            // Rows
            foreach (var r in report.Rows)
            {
                sheet.Cell(row, 1).Value = r.AccountHead;
                sheet.Cell(row, 2).Value = r.Narration;
                sheet.Cell(row, 3).Value = r.Credit;
                sheet.Cell(row, 4).Value = r.Debit;
                sheet.Cell(row, 5).Value = r.IsLocked ? "Yes" : "No";
                sheet.Cell(row, 6).Value = r.NetAmount;
                row++;
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
