namespace UrbanNative.Domain.Entities
{
    public class LowStockResult
    {
        public int VariantSetID { get; set; }
        public int Stock { get; set; }
        public int ReorderLevel { get; set; }
        public bool IsLowStock { get; set; }
    }
}