
namespace UrbanNative.Shared.SharedDTOs
{
    public class BankFormModel
    {
        public string AccountHolderName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string CityName { get; set; }

        // Optional
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? UPIId { get; set; }
        public string? StateName { get; set; }
        public string? Pincode { get; set; }
    }
}
