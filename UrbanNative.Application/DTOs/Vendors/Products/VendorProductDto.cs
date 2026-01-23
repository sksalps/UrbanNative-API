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

    public class VendorProductCreateDto
    {
        public int CategoryID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal MRP { get; set; }
        public decimal? DiscountPrice { get; set; }

        public bool HasVariants { get; set; }
        public int VendorWarehouseAddressID { get; set; }

        public int? ReturnPolicyID { get; set; }
        public decimal? SharedMargin { get; set; }
    }

    public class VendorProductUpdateDto
    {
        public int ProductID { get; set; }
        public int? ReturnPolicyID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal MRP { get; set; }
        public decimal? DiscountPrice { get; set; }

        public int CategoryID { get; set; }
        public bool HasVariants { get; set; }

        public decimal VendorSharedMargin { get; set; }
        public int VendorWarehouseAddressID { get; set; }
    }

    public class VendorProductEditDto
    {
        public int ProductID { get; set; }
        public int? ReturnPolicyID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal MRP { get; set; }
        public decimal? DiscountPrice { get; set; }

        public int CategoryID { get; set; }
        public bool HasVariants { get; set; }

        public decimal VendorSharedMargin { get; set; }

        public int VendorWarehouseAddressID { get; set; }

        // 🔒 Control flags (UI helpers)
        public string ApprovalStatus { get; set; } = string.Empty;
    }
    public class VendorWarehouseDto
    {
        public int VendorWarehouseAddressID { get; set; }

        public string AddressName { get; set; } = string.Empty;

        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }

        public bool IsActive { get; set; }
    }
    public class ReturnPolicyDto
    {
        public int ReturnPolicyID { get; set; }
        public string PolicyName { get; set; } = string.Empty;
        public int ReturnDays { get; set; }
        public string Descriptions { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
