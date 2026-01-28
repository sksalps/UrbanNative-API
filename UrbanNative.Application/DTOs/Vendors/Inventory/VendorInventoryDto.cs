using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Inventory
{
   
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
