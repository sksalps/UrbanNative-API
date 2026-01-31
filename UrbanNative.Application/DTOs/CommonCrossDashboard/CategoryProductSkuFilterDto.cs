namespace UrbanNative.Application.DTOs.CommonCrossDashboard
{
    public class CategoryLookupDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class SkuCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class SkuProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
    public class SkuProductBasicDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int VendorId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int VendorWarehouseAddressId { get; set; }
    }


    public class SkuLookupDto
    {
        public int SKUId { get; set; }
        public string SKUCode { get; set; } = string.Empty;
        public string VariantText { get; set; } = string.Empty;
    }

    public class SkuContextDto
    {
        public int SKUId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string SKUCode { get; set; } = string.Empty;
        public string VariantText { get; set; } = string.Empty;
    }



    public class HsnLookupDto
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; } = string.Empty;
        public decimal GSTPercentage { get; set; }
        public decimal SharedMargin { get; set; }
    }

    public class CommonWarehouseDto
    {
        public int VendorWarehouseAddressID { get; set; }

        public string AddressName { get; set; } = string.Empty;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? CityName { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public string? Pincode { get; set; }
        public bool IsPrimary { get; set; }   // <-- ADD THIS
        public bool IsActive { get; set; }

    }
    public class WarehousePreviewDto
    {
        public int AddressID { get; set; }
        public int VendorId { get; set; }

        public string AddressName { get; set; } = string.Empty;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public string? CityName { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public string? Pincode { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
    }
}
