using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Infrastructure.PDFdownload
{
    public static class OrderPdfGenerator
    {
        public static byte[] Generate(AdminOrderDetailsDto order)
        {
            var document = new OrderPdfDocument(order);
            return document.GeneratePdf();
        }

    }
}
