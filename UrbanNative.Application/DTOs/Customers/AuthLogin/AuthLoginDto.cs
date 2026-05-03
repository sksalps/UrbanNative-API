using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Application.DTOs.Customers.AuthLogin
{
    public class SendOtpResponseDto
    {
        public string Status { get; set; }
        public int OTP { get; set; }
        public DateTime ExpiryAt { get; set; }
        public string referralCode { get; set; } = string.Empty;
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
        public string? SessionID { get; set; } = string.Empty;
        public string? ByReferralCode { get; set; }=string.Empty;
    }
 
    public class VerifyOtpResponseDto
    {
        public bool success { get; set; } //True if OTP is correct and verified, False otherwise
        public string? Status { get; set; } //Login / Register / OTP Expired / Invalid OTP etc.
        public int? TempID { get; set; }
        public string? TempToken { get; set; }
        public int? ReferredByUserID { get; set; }=0;
        public string? ByReferralCode { get; set; }=string.Empty;

        //public string? SessionID { get; set; }= string.Empty;
        public UserLoginResponseDto? userExist { get; set; } = null;
    }

    public class verifiedResponseDto 
    { 
        public string? StatusLogin { get; set; }=string.Empty;
        public int? UserID { get; set; }
        public long? UserRandomID { get; set; }
        public string? ReferralCode { get; set; }
        public string? Token { get; set; } // JWT (only for existing user)

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
        public string? ReferralCode { get; set; }
    }

    public class RegisterRequestDto //for receiving finalregistration details of user after OTP verification
    {
        public int TempID { get; set; }
        public string TempToken { get; set; }
        public int? ReferredByUserID { get; set; }=0;
        public string? ByReferralCode { get; set; }=string.Empty;   
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string? Email { get; set; }=null;
        //public string? Pincode { get; set; }=null;
        public int? AddressID { get; set; } = 0;
    }
    public class RegisterResponseDto //for sending response after registration
    {
        public string? Status { get; set; }
        public string Message { get; set; }=string.Empty;
        public int UserID { get; set; }
        public long UserRandomID { get; set; }
        public string ReferralCode { get; set; }
        public UserLoginResponseDto? userExist { get; set; } = null;

    }
    public class AuthUserDto
    {
        public int UserID { get; set; }
        public long UserRandomID { get; set; }
        public string? UserNickName { get; set; } = string.Empty;
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ReferralCode { get; set; }
        public string SponsorReferralCode { get; set; }=string.Empty;
        public string? SponsorName { get; set; }=string.Empty;  
        public bool IsActive { get; set; }
    }

    public class UserLoginResponseDto
    {
        public string LoginStatus { get; set; } // SUCCESS / FAILED / INACTIVE etc.
        public int UserID { get; set; }
        public string Token { get; set; } = string.Empty; // JWT
        public string Role { get; set; } = string.Empty;
        public string ReferralCode { get; set; } = string.Empty;
        public long UserRandomID { get; set; }
        public string? UserNickName { get; set; } = string.Empty;
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string SponsorReferralCode { get; set; } = string.Empty;
        public string? SponsorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class TempUserDto //for getting temp user details 
    {
        public int TempID { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int? ReferredByUserID { get; set; }
        public string TempToken { get; set; }
        public DateTime ExpiryAt { get; set; }
        public int? UserId { get; set; }
        
    }

    public class CreateTempUserRequestDto //for creating temp user for no existing user found
    {
        public string? Identifier { get; set; }   // Mobile / Email (optional)
        public string? SessionID { get; set; }
        public int? ReferredByUserID { get; set; }

        public string? UserAgent { get; set; }
        public string? IPAddress { get; set; }
    }

    public class CreateTempUserResponseDto
    {
        public int TempID { get; set; }
        public string TempToken { get; set; } = "";
    }
}

