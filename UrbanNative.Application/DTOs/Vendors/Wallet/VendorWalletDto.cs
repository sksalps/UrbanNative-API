using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Wallet
{

    public class VendorWalletTypeDto
    {
        public int WalletAccountId { get; set; }
        public int WalletTypeId { get; set; }
        public string WalletType { get; set; } = "";
        public string WalletTypeName { get; set; } = "";
    }

    public class WalletLedgerFilterDto
    {
        public int? WalletAccountId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TxnType { get; set; }        // nullable
        public string? FinancialEvent { get; set; }     // nullable
        public int? AccountHeadId { get; set; }
        public string? OrderNo { get; set; }

          // 🔥 prevents model binding validation
        public int VendorId { get; set; }
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

        public decimal LedgerBalance => BalanceReal + BalanceLocked + CFBalance;
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

    public class WalletSearchSuggestionDto
    {
        public string OrderNo { get; set; }
        public string SKUCode { get; set; }
        public string Variant { get; set; }
    }

    /* Wallet Summary Report - this is a new report that shows the summary
    of wallet transactions for a given period, grouped by account head.
    It includes totals for credits, debits, locked amounts, and available balance.*/

    public class VendorWalletSummaryReportDto
    {
        public List<AccountHeadSummaryDto> Rows { get; set; } = new();
        public WalletSummaryTotalsDto Totals { get; set; } = new();
        // 🔹 NEW
        public string VendorName { get; set; } = "";
        public string GSTIN { get; set; } = "";
        public string Address { get; set; } = "";
    }
    public class AccountHeadSummaryDto
    {
        public int WalletAccountId { get; set; }
        public int AccountHeadId { get; set; }
        public string AccountHead { get; set; }
        public string Narration { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public bool IsLocked { get; set; }
        public decimal NetAmount { get; set; }
    }
    public class WalletSummaryTotalsDto
    {
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal LockedAmount { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal NetBalance { get; set; }
    }
    
    public class WalletSummaryFilterDto
    {
        /// <summary>
        /// Financial Year selector: "current", "previous", or "custom"
        /// </summary>
        public string FY { get; set; } = "current";

        /// <summary>
        /// Start date for report range
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// End date for report range
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Wallet type filter (null = all)
        /// </summary>
        public int? WalletAccountId { get; set; }
        

    }
    //use with Wallet SummaryReportDto to filter by wallet type (e.g. main wallet, cashback wallet, etc.)
    public class WalletTypeDto1
    {
        public int WalletTypeId { get; set; }
        public string WalletTypeName { get; set; }
    }
    //Use in Wallet Summary Report head
    public class VendorContextDto 
    {
        public int VendorId { get; set; }
        public string BusinessName { get; set; } = "";
        public string GSTNumber { get; set; } = "";
        public string AddressPreview { get; set; } = "";
    }


}
