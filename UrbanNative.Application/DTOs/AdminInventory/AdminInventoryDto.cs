using UrbanNative.Application.Interfaces;

namespace UrbanNative.Application.DTOs.AdminInventory
{
    public class AdminInventoryListDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public bool HasVariants { get; set; }

        public int TotalSKUs { get; set; }
        public int VendorCount { get; set; }
        public int TotalStock { get; set; }
        public int LowStockSKUCount { get; set; }

        public bool IsActive { get; set; }
    }
    public class AdminInventorySummaryDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public bool HasVariants { get; set; }

        public int TotalSKUs { get; set; }
        public int VendorCount { get; set; }
        public int TotalStock { get; set; }

        public bool IsActive { get; set; }
    }
    public class AdminInventorySkuDto
    {
        public int SKUId { get; set; }
        public string VendorName { get; set; } = null!;
        public int Stock { get; set; }
        public bool IsActive { get; set; }

        public string? VariantSignature { get; set; }
    }
    
}


