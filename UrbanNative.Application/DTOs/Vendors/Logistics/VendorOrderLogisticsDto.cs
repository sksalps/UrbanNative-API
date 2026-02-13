namespace UrbanNative.Application.DTOs.Vendors.Logistics
{
    /* ============================================================
     * CREATE SHIPMENT Create shipment (Dispatch flow)  
     * ============================================================ */
    public class CreateVendorShipmentDto
    {
        /// <summary>
        /// Order against which shipment is created
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Selected OrderItemIDs to be dispatched in this shipment
        /// </summary>
        //public IReadOnlyList<int> OrderItemIds { get; set; } = new List<int>();
        public List<int> OrderItemIds { get; set; } = new();

        /// <summary>
        /// Selected logistics provider
        /// </summary>
        public int LogisticsProviderID { get; set; }

        /// <summary>
        /// Initial shipment status
        /// READY_TO_SHIP | PICKUP_SCHEDULED | PICKED_UP | IN_TRANSIT | OUT_FOR_DELIVERY | DELIVERED
        /// </summary>
        public string InitialShipmentStatus { get; set; } = string.Empty;

        /// <summary>
        /// AWB / Tracking number (mandatory from PICKED_UP onwards)
        /// </summary>
        public string? TrackingNo { get; set; }
    }

    /* ============================================================
     * GRID-1 : VENDOR ORDERS (LOGISTICS VIEW)
     * ============================================================ */
    public class VendorLogisticsOrderDto
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Delivery SLA in days
        /// </summary>
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CityState { get; set; } = string.Empty;

        /// <summary>
        /// Number of distinct products in the order for this vendor
        /// </summary>
        public int NOP { get; set; }

        /// <summary>
        /// Total item count for this vendor
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Last shipment update date for this vendor
        /// </summary>
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// FAILED | RETURN | PENDING | DISPATCHED
        /// (Derived vendor-level dispatch status)
        /// </summary>
        public string DispatchStatus { get; set; } = string.Empty;
    }

    /* ============================================================
     * GRID-2 : Item selection grid for dispatch + shipment status display
     * ============================================================ */
    public class VendorLogisticsItemDto
    {
        public int ShipmentID { get; set; }
        public int OrderItemID { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public int SKUId { get; set; }
        public string SKUDisplay { get; set; } = string.Empty;

        public int ShipmentQty { get; set; }

        public string ShipmentStatus { get; set; } = string.Empty;

        public int? LogisticsProviderID { get; set; }
        public string? ProviderName { get; set; }

        public string? TrackingNo { get; set; }

        public DateTime? DeliveryByDate { get; set; }

        public bool IsEditable { get; set; }
        public bool IsActive { get; set; }
        public bool IsReturn { get; set; }
    }


    /* ============================================================
     * GRID-3 : SHIPMENT Details + Status update (Dispatch flow)
     * ============================================================ */
    public class VendorLogisticsShipmentDto
    {
        public int ShipmentID { get; set; }
        public int OrderID { get; set; }
        public int SKUId { get; set; }
        public string OrderNo { get; set; }
        public int VendorID { get; set; }
        public string CityState { get; set; }

        /// <summary>
        /// NEW | RETURN | RTO
        /// </summary>
        public string ShipmentType { get; set; } = string.Empty;

        /// <summary>
        /// READY_TO_SHIP → PICKUP_SCHEDULED → PICKED_UP → IN_TRANSIT → OUT_FOR_DELIVERY → DELIVERED
        /// FAILED_HOLD → FAILED → RETURN_TO_ORIGIN
        /// </summary>
        public string ShipmentStatus { get; set; } = string.Empty;

        public int? LogisticsProviderID { get; set; }
        public string? ProviderName { get; set; }

        public string? TrackingNo { get; set; }
        public string? UpdatedBy { get; set; }
        
        public int ItemCount { get; set; }

        /// <summary>
        /// Calculated SLA date (Order.CreatedAt + Handling + Transit)
        /// </summary>
        public DateTime? DeliveryByDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ShippedAt { get; set; }

        /// <summary>
        /// UI helper
        /// </summary>
        public int DelayDays =>
            DeliveryByDate.HasValue && DateTime.UtcNow.Date > DeliveryByDate.Value.Date
                ? (DateTime.UtcNow.Date - DeliveryByDate.Value.Date).Days
                : 0;
        public int OrderDays =>
            OrderDate.HasValue && OrderDate.Value.Date < DateTime.UtcNow.Date
                ? (DateTime.UtcNow.Date - OrderDate.Value.Date).Days           : 0;
    }

    /* ============================================================
     * GRID-3 : UPDATE SHIPMENT STATUS
     * ============================================================ */
    public class UpdateShipmentStatusDto
    {
        public int ShipmentID { get; set; }

        /// <summary>
        /// READY_TO_SHIP → PICKUP_SCHEDULED → PICKED_UP → IN_TRANSIT → OUT_FOR_DELIVERY → DELIVERED
        /// FAILED_HOLD → FAILED → RETURN_TO_ORIGIN
        /// </summary>
        public string NewShipmentStatus { get; set; } = string.Empty;
        public string ShipmentType { get; set; } = string.Empty;

        /// <summary>
        /// Logistics provider (locked after PICKED_UP)
        /// </summary>
        public int? LogisticsProviderID { get; set; }

        /// <summary>
        /// AWB / Tracking number
        /// </summary>
        public string? TrackingNo { get; set; }
    }

    /* ============================================================
     * INTERNAL : SHIPMENT OWNERSHIP SNAPSHOT (ownership, status checks)
     * (Used for validation in UseCase)  
     * ============================================================ */
    public class VendorShipmentSnapshotDto
    {
        public int ShipmentID { get; set; }
        public int VendorID { get; set; }
        public string ShipmentStatus { get; set; } = string.Empty;
        public int? LogisticsProviderID { get; set; }
    }

    
    /* ============================================================
     * FILTER BAR (SEARCH + TOGGLE + actionable/completed)
     * ============================================================ */
    public class VendorLogisticsFilterDto
    {
        public string? SearchText { get; set; }

        /// <summary>
        /// false = Actionable (default)
        /// true  = Completed
        /// </summary>
        public bool ShowCompleted { get; set; } = false;
    }

    /* ============================================================
     * SMART SEARCH AUTOCOMPLETE
     * ============================================================ */
    public class VendorLogisticsSuggestionDto
    {
        /// <summary>
        /// ORDER | SKU | SHIPMENT | AWB | RETURN
        /// </summary>
        public string SuggestionType { get; set; } = string.Empty;

        public string DisplayText { get; set; } = string.Empty;

        /// <summary>
        /// Value to be placed in search box on selection
        /// </summary>
        public string SearchValue { get; set; } = string.Empty;
    }

    
}
