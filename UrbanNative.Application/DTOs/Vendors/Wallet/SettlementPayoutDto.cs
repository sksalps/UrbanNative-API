using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UrbanNative.Application.DTOs.Vendors.Wallet
{
    // DTOs/Vendors/SettlementPayoutSummaryDto.cs
    

    public class SettlementPayoutSummaryDto
    {
        public int TotalSettlements { get; set; }
        public decimal TotalSettlementAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal FailedAmount { get; set; }
    }

    // DTOs/Vendors/SettlementPayoutDto.cs

    public class SettlementPayoutDto
    {
        public long SettlementID { get; set; }
        public DateTime SettlementDate { get; set; }
        public string? PaymentByBankName { get; set; }
        public string PaymentMode { get; set; } = string.Empty;

        public decimal SettlementAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string SettlementStatus { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }

    // DTOs/Vendors/SettlementPaymentDto.cs Grid2 details for a settlement

    public class SettlementPaymentDto
    {
        public long PaymentBatchID { get; set; }
        public long SettlementID { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string BatchStatus { get; set; } = string.Empty;
        public string? BankReferenceNo { get; set; }
        
        public string? PaymentByBankName { get; set; }
        public string? PaymentToBank { get; set; }
        public string? BankAccountNo { get; set; }

        public decimal SettlementAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public int PaymentAttemptCount { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? FailureReason { get; set; }
    }
}
