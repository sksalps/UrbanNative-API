using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Wallet
{
    public class WalletLedgerFilterDto
    {
        public int VendorId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TxnType { get; set; }
        public string SourceType { get; set; }
        public int? AccountHeadId { get; set; }
        public string OrderNo { get; set; }
    }

    public class WalletSummaryDto
    {
        public decimal AvailableBalance { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal PendingEarnings { get; set; }
        public List<AccountHeadDto> RecentTransactions { get; set; }
    }
    public class AccountHeadDto
    {
        public int AccountHeadId { get; set; }
        public string HeadName { get; set; }
    }

    public class WalletLedgerRowDto
    {
        public DateTime Date { get; set; }
        public string TxnType { get; set; }
        public string SourceType { get; set; }
        public string AccountHead { get; set; }
        public string OrderNo { get; set; }
        public string SKUCode { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal RunningBalance { get; set; }
        public string Narration { get; set; }
    }


}
