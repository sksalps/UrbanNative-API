using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.AdminOrders
{
    public class AdminOrderListDto
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime CreatedAt { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PayableAmount { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public int ShipmentCount { get; set; }
    }

    public class AdminOrderItemDto
    {
        public string ProductName { get; set; }
        public string SKUCode { get; set; }
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal GSTAmount { get; set; }

        public string ItemStatus { get; set; }
    }

    public class AdminOrderShipmentDto
    {
        public int ShipmentID { get; set; }
        public int VendorID { get; set; }

        public int LogisticsProviderID { get; set; }
        public string TrackingNo { get; set; }

        public string ShipmentType { get; set; }
        public string ShipmentStatus { get; set; }

        public string ReceivedBy { get; set; }

        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

    public class AdminOrderDetailsDto
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PayableAmount { get; set; }

        public decimal PlatformCommission { get; set; }
        public decimal VendorSharedMargin { get; set; }

        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public List<AdminOrderItemDto> Items { get; set; }
        public List<AdminOrderShipmentDto> Shipments { get; set; }
    }
}
