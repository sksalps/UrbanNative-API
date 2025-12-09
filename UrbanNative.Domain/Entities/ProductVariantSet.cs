namespace UrbanNative.Domain.Entities
{
    public class ProductVariantSet
    {
        public int VariantSetID { get; set; }
        public int ProductID { get; set; }

        // SKU details
        public string? SKU { get; set; }
        public decimal VariantPrice { get; set; }
        public int Stock { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}