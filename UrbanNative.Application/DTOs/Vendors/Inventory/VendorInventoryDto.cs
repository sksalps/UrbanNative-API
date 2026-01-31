using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Inventory
{

    //===============================================
    //Vendor Side Inventory Add DTOs   
    //===============================================
    public class ProductInventoryInRequestDto
    {
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }

        public int? WarehouseId { get; set; }

        public string? Remarks { get; set; }   // NEW – common for all SKUs

        public List<ProductInventoryInItemDto> Items { get; set; } = new();
    }
    public class ProductInventoryInItemDto
    {
        public int SKUId { get; set; }

        public int? Quantity { get; set; }   // IN only, >0
    }
    public class InventoryInResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public int ProductId { get; set; }   // 🔒 non-nullable

        public int TotalSkusProcessed { get; set; }
        public int TotalQuantityAdded { get; set; }
    }

    public class ProductSkuSnapshotDto
    {
        public int SKUId { get; set; }

        public bool IsActive { get; set; }

        public int CurrentStock { get; set; }

        /// <summary>
        /// True if SKU has already been initiated
        /// (i.e., at least one inventory log exists or stock row exists)
        /// </summary>
        public bool IsInitiated { get; set; }
    }

    public class ProductAddInventorySkuGridDto
    {
        public int SKUId { get; set; }
        public string SkuCode { get; set; } = "";

        public string VariantDisplay { get; set; } = "";

        public int OverallStock { get; set; }          // NEW
        public int WarehouseStock { get; set; }         // NEW

        public bool IsActive { get; set; }
        public bool IsInitiated { get; set; }
    }



    //===============================================
    //Vendor Side Inventory view DTOs   
    //===============================================
    public class VendorInventorySummaryDto
    {
        // Context
        public string CategoryName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string SKUCode { get; set; } = string.Empty;
        public string SKUCombination { get; set; } = string.Empty;

        // Numbers
        public int OpeningStock { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int ClosingStock { get; set; }
        public int CurrentStock { get; set; }
    }

    public class VendorInventoryLogDto
    {
        public int LogID { get; set; }
        public string SKUCode { get; set; } = string.Empty;   // NEW COLUMN
        public DateTime CreatedAt { get; set; }
        public int? AddressID { get; set; }
        //public string WarehouseName { get; set; } = string.Empty; 
        public string ChangeType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int OldStock { get; set; }
        public int NewStock { get; set; }
        public string? ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }

}
