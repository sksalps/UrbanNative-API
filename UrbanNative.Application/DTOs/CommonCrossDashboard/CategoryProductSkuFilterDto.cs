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
    

}
