using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors
{
    public class VendorLoginRequestDto1
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class VendorLoginResultDto1
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string? Role { get; set; }

        public string ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
    }

    public class VendorLoginResponseDto1
    {
        public string Token { get; set; } = string.Empty;
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string BusinessName { get; set; }
        public string Mobile { get; set; }
        public string? Role { get; set; }
    }
    
    }

