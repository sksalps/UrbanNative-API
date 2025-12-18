namespace UrbanNative.Application.DTOs
{
    public class ProductImageDto
    {
        public int ProductImageID { get; set; }
        public int ProductID { get; set; }
        public int? ProductVariantID { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
    }
}
