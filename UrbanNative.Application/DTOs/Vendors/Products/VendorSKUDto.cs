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
        public string SKUCode { get; set; } = string.Empty;

        public decimal? Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }

        public int ImageCount { get; set; }

        // 🔒 UI Display Only
        public string VariantDisplay { get; set; } = "Default Product";
    }

    public class VendorSkuSaveDto
    {
        public int SKUId { get; set; }
        public decimal? Price { get; set; }
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
