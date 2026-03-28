
namespace UrbanNative.Shared.SharedDTOs
{
    public class BankFormModel
    {
        public string AccountHolderName { get; set; }
        public string AccountNo { get; set; }
        public string? AccountType { get; set; }
        public string IFSCCode { get; set; }
        public string CityName { get; set; }

        // Optional
        public string? FileName { get; set; }
        public string? ApiBaseUrl { get; set; } = "";
        public string? FileURL { get; set; }
        public bool IsPrimary { get; set; } = false;
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public string? BankName { get; set; }
        public int ? BankID { get; set; }
        public string? BranchName { get; set; }
        public string? UPIId { get; set; }
        public string? Pincode { get; set; }
        public int ComplianceId { get; set; }
        public string? ComplianceName { get; set; }
        
    }
}
