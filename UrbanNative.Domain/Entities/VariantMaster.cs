namespace UrbanNative.Domain.Entities
{
    public class VariantMaster
    {
        public int VariantID { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}