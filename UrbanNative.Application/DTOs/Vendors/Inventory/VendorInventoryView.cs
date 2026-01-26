using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Inventory
{
    public class VendorInventorySummaryDto
    {
        public int OpeningStock { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int ClosingStock { get; set; }
        public int CurrentStock { get; set; }
    }
    public class VendorInventoryLogDto
    {
        public int LogID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ChangeType { get; set; }
        public int Quantity { get; set; }
        public int OldStock { get; set; }
        public int NewStock { get; set; }
        public string? ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public string? Remarks { get; set; }
    }

}
