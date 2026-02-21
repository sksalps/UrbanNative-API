using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Infrastructure.PDFdownload
{
    public class VendorWalletSummaryPdf
    {
        public static byte[] Generate(VendorWalletSummaryReportDto report, string dateRange)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    // ================= HEADER =================
                    page.Header().Text("Wallet Summary")
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();

                    // ================= CONTENT =================
                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        // 🔹 Vendor Details
                        col.Item().Column(vendor =>
                        {
                            vendor.Item().Text($"Vendor: {report.VendorName}")
                                .FontSize(12).Bold();

                            if (!string.IsNullOrWhiteSpace(report.GSTIN))
                                vendor.Item().Text($"GSTIN: {report.GSTIN}")
                                    .FontSize(10);

                            if (!string.IsNullOrWhiteSpace(report.Address))
                                vendor.Item().Text($"Address: {report.Address}")
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken1);
                        });

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // 🔹 Date Range
                        col.Item().Text(dateRange)
                            .FontSize(12)
                            .Bold();

                        // 🔹 Report Date
                        col.Item().Text($"Report Date: {DateTime.Now:dd MMM yyyy}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        // ================= SUMMARY =================
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Credit: {report.Totals.TotalCredit:N2}");
                            row.RelativeItem().Text($"Debit: {report.Totals.TotalDebit:N2}");
                            row.RelativeItem().Text($"Locked: {report.Totals.LockedAmount:N2}");
                            row.RelativeItem().Text($"Available: {report.Totals.AvailableBalance:N2}");
                            row.RelativeItem().Text($"Net: {report.Totals.NetBalance:N2}");
                        });

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // ================= TABLE =================
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Account Head
                                columns.RelativeColumn(3); // Narration
                                columns.RelativeColumn(1); // Credit
                                columns.RelativeColumn(1); // Debit
                                columns.RelativeColumn(1); // Locked
                                columns.RelativeColumn(1); // Net
                            });

                            // 🔹 Table Header
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("Account Head").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Narration").Bold();
                                header.Cell().Element(HeaderCellStyle).AlignRight().Text("Credit").Bold();
                                header.Cell().Element(HeaderCellStyle).AlignRight().Text("Debit").Bold();
                                header.Cell().Element(HeaderCellStyle).AlignCenter().Text("Locked").Bold();
                                header.Cell().Element(HeaderCellStyle).AlignRight().Text("Net").Bold();
                            });


                            // 🔹 Table Rows
                            foreach (var row in report.Rows)
                            {
                                table.Cell().Element(CellStyle).Text(row.AccountHead);
                                table.Cell().Element(CellStyle).Text(row.Narration);
                                table.Cell().Element(CellStyle).AlignRight().Text(row.Credit.ToString("N2"));
                                table.Cell().Element(CellStyle).AlignRight().Text(row.Debit.ToString("N2"));
                                table.Cell().Element(CellStyle).AlignCenter().Text(row.IsLocked ? "Yes" : "No");
                                table.Cell().Element(CellStyle).AlignRight().Text(row.NetAmount.ToString("N2"));
                            }
                        });
                    });

                    // ================= FOOTER =================
                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("UrbanNative Vendor Statement • ").FontSize(9);
                            text.Span(DateTime.Now.ToString("dd MMM yyyy HH:mm"));
                        });
                });
            }).GeneratePdf();
        }

        // ================= CELL STYLES =================

        private static IContainer HeaderCellStyle(IContainer container)
        {
            return container
                .PaddingVertical(5)
                .PaddingHorizontal(3)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Darken1)
                .Background(Colors.Grey.Lighten3);
        }


        private static IContainer CellStyle(IContainer container)
        {
            return container
                .PaddingVertical(4)
                .PaddingHorizontal(3)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2);
        }
    }
}
