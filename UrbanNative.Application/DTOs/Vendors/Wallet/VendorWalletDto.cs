using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Wallet
{


public class WalletLedgerFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TxnType { get; set; }        // nullable
        public string? FinancialEvent { get; set; }     // nullable
        public int? AccountHeadId { get; set; }
        public string? OrderNo { get; set; }

          // 🔥 prevents model binding validation
        public int VendorId { get; set; }
    }
    public class WalletSummaryDto
    {
        public decimal AvailableBalance { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal PendingEarnings { get; set; }
        public List<AccountHeadDto> RecentTransactions { get; set; }
    }
    // This is the summary for the ledger page, which includes opening balance, total credits/debits in the period, and closing balance.
    public class WalletLedgerSummaryDto
    {
        // Opening balance before filter range
        public decimal CFBalance { get; set; }

        // Credits
        public decimal CreditReal { get; set; }
        public decimal CreditLocked { get; set; }

        // Debits
        public decimal DebitReal { get; set; }
        public decimal DebitLocked { get; set; }

        // Computed balances
        public decimal BalanceReal { get; set; }
        public decimal BalanceLocked { get; set; }

        // Derived values
        public decimal AvailableBalance => BalanceReal;

        public decimal LedgerBalance => BalanceReal + BalanceLocked;
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
        public string FinancialEvent { get; set; }
        public string ReferenceNo { get; set; }
        public string AccountHead { get; set; }
        public string OrderNo { get; set; }
        public string SKUCode { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal RunningBalance { get; set; }
        public string Narration { get; set; }
    }


}
