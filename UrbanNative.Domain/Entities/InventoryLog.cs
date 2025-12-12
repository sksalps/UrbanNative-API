namespace UrbanNative.Domain.Entities
{
    public class InventoryLog
    {
        public int LogID { get; set; }
        public int VariantSetID { get; set; }
        public string ChangeType { get; set; } = string.Empty; // IN or OUT
        public int Quantity { get; set; }
        public int OldStock { get; set; }
        public int NewStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}