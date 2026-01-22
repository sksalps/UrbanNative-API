using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Products
{
    public class VendorProductListDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public bool HasVariants { get; set; }

        public string ApprovalStatus { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // 🔑 SKU Engine derived
        public string SkuStatus { get; set; } = string.Empty;

        // 🔍 (Future ready – UI columns later)
        public int? LiveOrders { get; set; }
        public int? TotalOrders { get; set; }

        public string? HSNCode { get; set; }
        public decimal? GSTPercentage { get; set; }
    }
    public class CategoryLookupDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
    public class HsnLookupDto
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; } = string.Empty;
        public decimal GSTPercentage { get; set; }
    }


    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
