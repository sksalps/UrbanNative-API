using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Orders
{
    public class VendorOrderListFilterDto
    {
        public int VendorID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? OrderNo { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ShipmentStatus { get; set; }

        // 👇 user input
        public string? SkuOrProduct { get; set; }

        // 👇 derived (NEW)
        public string? SearchType { get; set; }
        // "SKU" | "PRODUCT" | null

        public string RTOFilter { get; set; } = "ALL";

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
    public class VendorSkuProductSuggestionDto
    {
        public int SKUId { get; set; }
        public string SKUCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
    }

    public class VendorOrderListDto
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; } = null!;
        public DateTime OrderDate { get; set; }

        public int NOP { get; set; }
        public int ItemsCount { get; set; }

        public decimal VendorOrderValue { get; set; }

        public string? ShipmentStatus { get; set; }
        public string PaymentStatus { get; set; } = null!;

        public DateTime? TargetDeliveryBy { get; set; }
    }
    public class VendorOrderSummaryDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalVendorValue { get; set; }
    }


}
