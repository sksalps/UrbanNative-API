namespace UrbanNative.Domain.Entities
{
    public class Vendor
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = "";
        public string? ContactPerson { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? GSTNumber { get; set; }
        public string? PANNumber { get; set; }
        public string? BusinessAddress { get; set; }
        public string? PickupAddress { get; set; }
        public string? State { get; set; }
        public string? StateCode { get; set; }
        
        public string ApprovalStatus { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    }


}
