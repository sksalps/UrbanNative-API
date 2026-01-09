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

        public int UserID { get; set; }
        public string UserName { get; set; }   // ✅ ADD THIS

        public decimal TotalAmount { get; set; }
        public decimal PayableAmount { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public int ShipmentCount { get; set; }
    }


    public class AdminOrderPagedResultDto
    {
        public List<AdminOrderListDto> Orders { get; set; } = new();
        public int TotalRecords { get; set; }
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

    /* public class AdminOrderDetailsDto  
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
 **/
    //**********Order Details DTO***********//
    public class AdminOrderDetailsHeaderDto
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

    public class AdminOrderDetailsItemDto
    {
        public int OrderItemID { get; set; }

        public string SKUCode { get; set; }
        public string ProductName { get; set; }

        public int VendorID { get; set; }
        public string VendorName { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal GSTAmount { get; set; }

        public decimal VendorSharedMargin { get; set; }

        public int ShipmentID { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string ItemStatus { get; set; } // READY / IN_TRANSIT / DELIVERED
    }
    public class AdminOrderDetailsShipmentDto //On Order Details Page
    {
        public int ShipmentID { get; set; }

        public int VendorID { get; set; }
        public string VendorName { get; set; }

        public int LogisticsProviderID { get; set; }
        public string LogisticsProviderName { get; set; }

        public string TrackingNo { get; set; }
        public string ShipmentType { get; set; }

        public int NumberOfPackages { get; set; }

        public string ShipmentStatus { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string ReceivedBy { get; set; }
    }

    public class AdminOrderDetailsDto
    {
        public AdminOrderDetailsHeaderDto Header { get; set; }  
        public List<AdminOrderDetailsItemDto> Items { get; set; } = new();
        public List<AdminOrderDetailsShipmentDto> Shipments { get; set; } = new();
    }

    //*******************Order Details DTO End******************//

}
