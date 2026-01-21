using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors
{
    public class VendorLoginRequestDto
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }


    
    public class VendorLoginResultDto
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }

        // These MUST be strings because DB stores Base64
        //public string PasswordHash { get; set; }
        //public string PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
    }

    public class VendorLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string BusinessName { get; set; }
        public string Mobile { get; set; }
    }
    
}

