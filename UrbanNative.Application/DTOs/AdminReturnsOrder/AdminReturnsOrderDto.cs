namespace UrbanNative.Application.DTOs.AdminReturnsOrder
{
    public class AdminReturnListDto
    {
        public int ReturnId { get; set; }

        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }

        public int OrderItemID { get; set; }
        public string ProductName { get; set; }
        public string SKUCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int VendorID { get; set; }
        public string VendorName { get; set; }

        public string ReturnType { get; set; }     // RETURN / RTO
        public string ReturnStatus { get; set; }   // REQUESTED / APPROVED / etc
        public string CustomerReason { get; set; }
        public DateTime RequestedAt { get; set; }

        public string? CourierName { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShipmentStatus { get; set; }
    }

    public class AdminReturnDetailsDto
    {
        // ===== Return =====
        public int ReturnId { get; set; }
        public string ReturnType { get; set; }
        public string ReturnStatus { get; set; }
        public string CustomerReason { get; set; }
        public string? Remarks { get; set; }
        public string? AdminComment { get; set; }
        public int? AdminId { get; set; }
        public DateTime? AdminActionAt { get; set; }
        public DateTime RequestedAt { get; set; }

        // ===== Order =====
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }

        // ===== Order Item =====
        public int OrderItemID { get; set; }
        public string ProductName { get; set; }
        public string SKUCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GSTAmount { get; set; }
        public string ItemStatus { get; set; }

        public int VendorID { get; set; }
        public string VendorName { get; set; }

        // ===== Return Shipment =====
        public int? ReturnShipmentId { get; set; }
        public string? CourierName { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShipmentStatus { get; set; }
        public DateTime? ShipmentCreatedAt { get; set; }
        public DateTime? PickedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

}
