using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Customers.AuthLogin
{
    public class SendOtpResponseDto
    {
        public string Status { get; set; }
        public int OTP { get; set; }
        public DateTime ExpiryAt { get; set; }
    }

    public class OtpRecordDto
    {
        public int OTPID { get; set; }
        public byte[] OTPHash { get; set; }
        public byte[] OTPSalt { get; set; }
        public DateTime ExpiryAt { get; set; }
        public bool IsVerified { get; set; }
    }
    public class VerifyOtpRequestDto
    {
        public string Identifier { get; set; }
        public int OTP { get; set; }
    }
 
    public class VerifyOtpResponseDto
    {
        public string Status { get; set; }

        public int? UserID { get; set; }

        public int? TempID { get; set; }
        public string TempToken { get; set; }

        public string Token { get; set; } // JWT (only for existing user)
    }

    public class CustomerPasswordLoginRequestDto
    {
        /// <summary>
        /// Can be:
        /// Mobile / Email / UserRandomID / UserNickName
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Plain password (will be verified using PBKDF2 hash)
        /// </summary>
        public string Password { get; set; }
    }

    public class SendOtpRequestDto
    {
        public string Identifier { get; set; }
    }
}

