using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.AdminWallet
{

    public class WalletBalanceSummaryDto
    {
        public decimal OpeningBalance { get; set; }
        public decimal PeriodCredit { get; set; }
        public decimal PeriodDebit { get; set; }
        public decimal ClosingBalance { get; set; }
    }

    public class WalletLedgerDto
    {
        public long LedgerId { get; set; }
        public string OwnerType { get; set; }
        public int OwnerId { get; set; }
        public string WalletType { get; set; }

        public string TxnType { get; set; }
        public decimal Amount { get; set; }

        public string AccHead { get; set; }

        public string SourceType { get; set; }
        public int SourceId { get; set; }

        public string Narration { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
