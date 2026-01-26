using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Products
{
    public class VendorSkuGridDto
    {
        public int SKUId { get; set; }
        public string SKUCode { get; set; } = "";

        public decimal? Price { get; set; }
        public decimal? DP { get; set; } //Discounted Price
        public int Stock { get; set; }          // cached balance
        public bool IsActive { get; set; }

        public int ImageCount { get; set; }
        public string VariantDisplay { get; set; } = "";

        public bool IsStockLocked { get; set; }

        // 🔒 NEW (read-only)
        public int? InitStock { get; set; }
        public int? CurrentStock { get; set; }
    }


    public class VendorSkuSaveDto
    {
        public int SKUId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DP { get; set; } //Discounted Price
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public int ImageCount { get; set; }
    } 

    public class VendorSkuHeaderDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        //public string CategoryName { get; set; } = ""; CategoryName
        public string CategoryPath { get; set; } = "";
        public string VariantSetName { get; set; } = "";
        public int TotalPossibleSkus { get; set; }
    }
}
