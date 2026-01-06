using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.AdminSKU
{
    public class VariantSelectionDto
    {
        public int VariantId { get; set; }
        
        public List<int> VariantValueIds { get; set; } = new();
}

public class SkuSignatureDto
    {
        public int SeqNo { get; set; }
        public string ValueSignature { get; set; } = string.Empty;
    }

    
    public class AdminSkuOverviewDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int VendorCount { get; set; }
        public bool HasVariants { get; set; }
        public int TotalSkus { get; set; }

        // UI-only (computed in Razor Page)
        public string Status { get; set; } = "";
        public string StatusCss { get; set; } = "";
    }
    
    public class ProductSkuDetailDto
    {
        public int SKUId { get; set; }
        public string SKUCode { get; set; } = string.Empty;
        public string ValueSignature { get; set; } = string.Empty;
        public string VariantDisplay { get; set; } = "";
        public string VendorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AdminVendorSkuCoverageDto
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; } = string.Empty;

        public int VendorSkuCount { get; set; }
        public int TotalCombinations { get; set; }

        public decimal CoveragePercent { get; set; }

        // UI only
        public string Status { get; set; } = string.Empty;
        public string StatusCss { get; set; } = string.Empty;
    }

    public class ProductSkuCoverageHeaderDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string CategoryPath { get; set; } = "";
        public int TotalCombinations { get; set; }
    }
    public class VendorSkuCoverageDetailHeaderDto
    {
        public string ProductName { get; set; } = "";
        public string VendorName { get; set; } = "";
    }
   

    public class VendorSkuCoverageDetailDto
    {
        public string ValueSignature { get; set; }   // optional (for debug)
        public string DisplayText { get; set; }      // Color: Blue, Size: M
        public bool IsPresent { get; set; }
    }


}
