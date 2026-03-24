using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance
{

   public class BankSaveRequestDto
        {
            public int? BankID { get; set; }
            // 🔹 Compliance
            public int ComplianceId { get; set; }
            public string? ComplianceName { get; set; }

            // 🔹 File (optional)
            public string? FileName { get; set; }
            public string? FileURL { get; set; }

            // 🔹 Bank Info
            [Required(ErrorMessage = "Account Holder Name is required")]
            public string AccountHolderName { get; set; }

            [Required(ErrorMessage = "Account Number is required")]
            [RegularExpression(@"^\d{9,18}$",
                ErrorMessage = "Account Number must be 9–18 digits")]
            public string AccountNo { get; set; }

            [Required(ErrorMessage = "IFSC Code is required")]
            [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$",
                ErrorMessage = "Invalid IFSC format")]
            public string IFSCCode { get; set; }
            [Required(ErrorMessage = "Bank Name is required")]
            public string? BankName { get; set; }
            [Required(ErrorMessage = "Select, Account Type is required")]
            public string? AccountType { get; set; }

            public string? BranchName { get; set; }
            
            public string? UPIId { get; set; }

            // 🔹 Location
            [Required(ErrorMessage = "City is required")]
            public string CityName { get; set; }

            public string? StateName { get; set; }
            public string? CountryName { get; set; }
            public string? Pincode { get; set; }

            // 🔹 Flags
            public bool IsPrimary { get; set; } = false;
            public bool IsFromCompliance { get; set; } = false;

            // 🔹 Entity Context (VERY IMPORTANT)
            public string? EntityType { get; set; }   // Vendor / Customer / Affiliate
            public int EntityID { get; set; }
        }

    
    public class BankSaveFormDto
    {
        //[Required(ErrorMessage = "Account Holder Name is required")]
        public string AccountHolderName { get; set; }

        //[Required(ErrorMessage = "Account Number is required")]
        //[RegularExpression(@"^\d{9,18}$",            ErrorMessage = "Account Number must be 9–18 digits")]
        public string AccountNo { get; set; }

       // [Required(ErrorMessage = "IFSC Code is required")]
       // [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$",            ErrorMessage = "Invalid IFSC format")]
        public string IFSCCode { get; set; }

        //[Required(ErrorMessage = "City is required")]
        public string CityName { get; set; }


        // Optional
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? UPIId { get; set; }
        public string? StateName { get; set; }
        public string? Pincode { get; set; }

        
    }
    /*
    public class BankSaveRequestDto
    {
        public int? BankID { get; set; }
        public int? ComplianceId { get; set; }
        public string? ComplianceName { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
        [Required(ErrorMessage = "Account Holder Name is required")]
        public string AccountHolderName { get; set; }

        public string BankName { get; set; }
        public string BranchName { get; set; }
        [Required(ErrorMessage = "Account Number is required")]
        [RegularExpression(@"^\d{9,18}$",   ErrorMessage = "Account Number must be 9–18 digits")]
        
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public string UPIId { get; set; }
        [Required(ErrorMessage = "IFSC Code is required")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$",        ErrorMessage = "IFSC must be like ABCD0123456")]
        
        public string IFSCCode { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string CityName { get; set; }
        public string Pincode { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsFromCompliance { get; set; } = false;
       public string EntityType { get; set; }   // Vendor / Affiliate / Customer
        public int EntityID { get; set; }
    }
    */


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
