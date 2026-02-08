using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Application.DTOs.Vendors.Orders
{
   
    public class VendorOrderViewSummaryDto
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }

        public string PaymentMode { get; set; }

        public int VendorNOP { get; set; }
        public decimal VendorOrderValue { get; set; }
    }
    public class VendorOrderDetailsDto
    {
        public VendorOrderViewSummaryDto OrderSummary { get; set; }
        public VendorOrderTimelineDto Timeline { get; set; }
        public VendorOrderAddressDto CustomerAddress { get; set; }

        public List<VendorOrderSkuItemDto> Items { get; set; } = [];
        public List<VendorOrderReturnDto> Returns { get; set; } = [];
        public List<VendorOrderReturnImageCountDto> ReturnImages { get; set; } = [];
    }

    
    public class VendorOrderTimelineDto
    {
        public DateTime OrderedAt { get; set; }
        public DateTime? PickedAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string ShipmentStatus { get; set; }
        public string TrackingNo { get; set; }
        public string LogisticsProvider { get; set; }

        public string ShipCity { get; set; }
        public string ShipState { get; set; }
    }

    public class VendorOrderAddressDto
    {
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        
        // Shipping
        public string ShipAddressLines { get; set; }
        public string ShipLandmark { get; set; }
        public string ShipCity { get; set; }
        public string ShipState { get; set; }
        public string ShipCountry { get; set; }
        public string ShipPinCode { get; set; }

        // Billing
        public string BillAddressLines { get; set; }
        public string BillLandmark { get; set; }
        public string BillCity { get; set; }
        public string BillState { get; set; }
        public string BillCountry { get; set; }
        public string BillPinCode { get; set; }
    }

    public class VendorOrderSkuItemDto
    {
        public int OrderItemID { get; set; }
        public string ProductName { get; set; }
        public string SKUCode { get; set; }
        public string VariantDisplay { get; set; }

        public int Quantity { get; set; }

        public decimal DP { get; set; }
        public decimal VendorShare { get; set; }

        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal SGSTPercent { get; set; }

        public decimal SKUTotal { get; set; }

        public int TargetDeliveryDays { get; set; }
        public int ShippedQty { get; set; }
        public int PendingQty { get; set; }

        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string TrackingNo { get; set; }
        public string Status { get; set; }
    }

    public class VendorOrderReturnDto
    {
        public int ReturnId { get; set; }

        public string SKUCode { get; set; }
        public string VariantDisplay { get; set; }

        public string ReturnType { get; set; }
        public string ReturnStatus { get; set; }

        public string CustomerReason { get; set; }

        public string TrackingNumber { get; set; }
        public string ShipmentStatus { get; set; }

        public DateTime? PickedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string PickFrom { get; set; }
        public string DropTo { get; set; }
    }

    public class VendorOrderReturnImageCountDto
    {
        public int ReturnId { get; set; }
        public int ImageCount { get; set; }
    }
    public class VendorOrderDetailsRequestDto
    {
        public int? OrderId { get; set; }
        public int? ShipmentId { get; set; }
        public int? ReturnId { get; set; }
    }


}
