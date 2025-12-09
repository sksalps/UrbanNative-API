namespace UrbanNative.Domain.Entities
{
    public class VariantValue
    {
        public int ValueID { get; set; }
        public int VariantID { get; set; }
        public string ValueName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}