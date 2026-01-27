
namespace UrbanNative.Application.DTOs
{
    public class CategoryLookupDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class ProductLookupDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }

    public class SKULookupDto
    {
        public int SKUId { get; set; }
        public string SKUCode { get; set; } = string.Empty;
        public string VariantText { get; set; } = string.Empty;
    }

}
