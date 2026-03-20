using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance
{
    public class BankSaveRequestDto
    {
        public int? BankID { get; set; }
        public int ComplianceId { get; set; }
        public int UploadId { get; set; }

        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public string UPIId { get; set; }
        public string IFSCCode { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Pincode { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsFromCompliance { get; set; } = false;
       public string EntityType { get; set; }   // Vendor / Affiliate / Customer
        public int EntityID { get; set; }
    }

    public class UploadResponseDto
    {
        public int UploadId { get; set; }
    }
    public class BankUpsertRequestDto
    {
        public int UploadId { get; set; }
        public string? DocumentNumber { get; set; }
    }
}
