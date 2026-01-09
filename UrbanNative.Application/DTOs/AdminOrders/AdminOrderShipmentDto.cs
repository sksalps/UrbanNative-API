
namespace UrbanNative.Application.DTOs.AdminOrders
{
    public class AdminOrderShipmentHeaderDto
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PayableAmount { get; set; }

        public string ShipAddress { get; set; }
    }
    public class AdminOrderShipmentListDto
    {
        public int ShipmentID { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }

        public int LogisticsProviderID { get; set; }
        public string LogisticsProviderName { get; set; }

        public string TrackingNo { get; set; }
        public string ShipmentType { get; set; }
        public string ShipmentStatus { get; set; }
        public string ReceivedBy { get; set; }

        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
    public class AdminOrderShipmentDetailsDto
    {
        public AdminOrderShipmentHeaderDto Header { get; set; }
        public List<AdminOrderShipmentListDto> Shipments { get; set; } = new();
    }


}
