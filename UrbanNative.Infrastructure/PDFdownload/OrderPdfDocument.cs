using QuestPDF.Elements.Table;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Infrastructure.PDFdownload
{
    public class OrderPdfDocument : IDocument
    {
        private readonly AdminOrderDetailsDto _order;

        public OrderPdfDocument(AdminOrderDetailsDto order)
        {
            _order = order;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
            });
        }

        /* ================= HEADER ================= */
        private void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text($"Order Details – {_order.Header.OrderNo}")
                    .FontSize(16).Bold();

                col.Item().Text($"User: {_order.Header.UserName} ({_order.Header.UserID})");
                col.Item().Text($"Order Date: {_order.Header.CreatedAt:dd-MMM-yyyy}");
                col.Item().Text($"Payment Status: {_order.Header.PaymentStatus}");
                col.Item().Text($"Order Status: {_order.Header.OrderStatus}");
                col.Item().Text($"Total Amount: ₹{_order.Header.TotalAmount}");
                col.Item().Text($"Payable Amount: ₹{_order.Header.PayableAmount}");

                col.Item().PaddingTop(5)
                    .Text($"Ship Address: {_order.Header.ShipAddress}");
            });
        }

        /* ================= CONTENT ================= */
        private void ComposeContent(IContainer container)
        {
            container.PaddingTop(15).Column(col =>
            {
                /* ---------- ITEMS ---------- */
                col.Item().Text("Items").Bold().FontSize(12);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(60);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.ConstantColumn(30);
                        columns.ConstantColumn(70);
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(80);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "SKU");
                        HeaderCell(header.Cell(), "Product");
                        HeaderCell(header.Cell(), "Vendor");
                        HeaderCell(header.Cell(), "Qty");
                        HeaderCell(header.Cell(), "Unit");
                        HeaderCell(header.Cell(), "Subtotal");
                        HeaderCell(header.Cell(), "GST");
                        HeaderCell(header.Cell(), "Shipment");
                        HeaderCell(header.Cell(), "Status");
                    });

                    foreach (var i in _order.Items)
                    {
                        BodyCell(table.Cell(), i.SKUCode);
                        BodyCell(table.Cell(), i.ProductName);
                        BodyCell(table.Cell(), i.VendorName);
                        BodyCell(table.Cell(), i.Quantity.ToString());
                        BodyCell(table.Cell(), $"₹{i.UnitPrice}");
                        BodyCell(table.Cell(), $"₹{i.SubTotal}");
                        BodyCell(table.Cell(), $"₹{i.GSTAmount}");
                        BodyCell(table.Cell(), i.ShipmentID > 0 ? i.ShipmentID.ToString() : "-");
                        BodyCell(table.Cell(), i.ItemStatus);
                    }
                });

                /* ---------- SHIPMENTS ---------- */
                col.Item().PaddingTop(20).Text("Shipments").Bold().FontSize(12);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(60);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.ConstantColumn(50);
                        columns.ConstantColumn(40);
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(80);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Shipment");
                        HeaderCell(header.Cell(), "Vendor");
                        HeaderCell(header.Cell(), "Logistics");
                        HeaderCell(header.Cell(), "Tracking");
                        HeaderCell(header.Cell(), "Type");
                        HeaderCell(header.Cell(), "NOP");
                        HeaderCell(header.Cell(), "Status");
                        HeaderCell(header.Cell(), "Shipped At");
                        HeaderCell(header.Cell(), "Delivered At");
                        HeaderCell(header.Cell(), "Received By");
                    });

                    foreach (var s in _order.Shipments)
                    {
                        BodyCell(table.Cell(), s.ShipmentID.ToString());
                        BodyCell(table.Cell(), s.VendorName);
                        BodyCell(table.Cell(), s.LogisticsProviderName ?? "-");
                        BodyCell(table.Cell(), s.TrackingNo ?? "-");
                        BodyCell(table.Cell(), s.ShipmentType);
                        BodyCell(table.Cell(), s.NumberOfPackages.ToString());
                        BodyCell(table.Cell(), s.ShipmentStatus);
                        BodyCell(table.Cell(), s.ShippedAt?.ToString("dd-MMM-yyyy") ?? "-");
                        BodyCell(table.Cell(), s.DeliveredAt?.ToString("dd-MMM-yyyy") ?? "-");
                        BodyCell(table.Cell(), s.ReceivedBy ?? "-");
                    }
                });
            });
        }

        /* ================= HELPERS ================= */
        private static void HeaderCell(ITableCellContainer cell, string text)
        {
            cell.Element(c => c
                .Border(1)
                .Padding(4)
                .AlignMiddle()
                .Text(text).Bold().FontSize(9));
        }

        private static void BodyCell(ITableCellContainer cell, string text)
        {
            cell.Element(c => c
                .BorderBottom(0.5f)
                .Padding(4)
                .AlignMiddle()
                .Text(text).FontSize(9));
        }
    }
}
