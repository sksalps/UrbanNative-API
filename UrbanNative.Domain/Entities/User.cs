using System;
using System.Collections.Generic;
using System.Text;

namespace UrbanNative.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string? Email { get; set; }
        public string? ReferralCode { get; set; }
        public decimal WalletBalance { get; set; }
        public string? ReferredByCode { get; set; }
    }
}