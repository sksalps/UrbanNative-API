using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Application.Interfaces.UseCases.Wallet;
using UrbanNative.Infrastructure.ExcelDownload;
using UrbanNative.Infrastructure.PDFdownload;


namespace UrbanNative.Api.Controllers.Vendors
{
        [Authorize]
        [ApiController]
        [Route("api/vendors/wallet")]
        public class VendorWalletController : ControllerBase
        {
            private readonly IVendorWalletUseCase _useCase;

            public VendorWalletController(IVendorWalletUseCase useCase)
            {
                _useCase = useCase;
            }

        [HttpPost("ledger")]
        public async Task<IActionResult> GetLedger([FromBody] WalletLedgerFilterDto filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            filter.VendorId = GetVendorId(); // injected

            var result = await _useCase.ExecuteAsync(filter);
            return Ok(result);
        }


        [HttpGet("account-heads")]
            public async Task<IActionResult> GetAccountHeads()
            {
                int VendorId = GetVendorId(); // extension method
            var result = await _useCase.GetAccountHeadsAsync(VendorId);
                return Ok(result);
            }
        [HttpGet("ledgersummary")]
        public async Task<IActionResult> GetSummary(DateTime fromDate, DateTime toDate)
        {
            var vendorId = GetVendorId(); // secure injection

            var result = await _useCase.ExecuteAsync(vendorId, fromDate, toDate);
            return Ok(result);
        }
        
        [HttpGet("smart-search")]
        public async Task<IActionResult> SmartSearch(string term)
        {
            var vendorId = GetVendorId();
            var result = await _useCase.SmartSearchAsync(vendorId, term);
            return Ok(result);
        }

        // <Wallet summary Report> 
        [HttpGet("summary-report")]
        public async Task<IActionResult> GetSummaryReport(DateTime fromDate, DateTime toDate, int? walletTypeId)
        {
            var vendorId = GetVendorId();
            var result = await _useCase.ExecuteSummaryAsync(vendorId, fromDate, toDate, walletTypeId);
            return Ok(result);
        }


        [HttpGet("summary-report/pdf")]
        public async Task<IActionResult> ExportSummaryPdf(DateTime fromDate, DateTime toDate, int? walletTypeId)
        {
            var vendorId = GetVendorId();

            var report = await _useCase.ExecuteSummaryAsync(vendorId, fromDate, toDate, walletTypeId);

            var dateRange = $"{fromDate:dd MMM yyyy} - {toDate:dd MMM yyyy}";
            var pdfBytes = VendorWalletSummaryPdf.Generate(report, dateRange);

            return File(pdfBytes, "application/pdf", $"WalletSummary_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("summary-report/excel")]
            public async Task<IActionResult> ExportSummaryExcel(DateTime fromDate, DateTime toDate, int? walletTypeId)
            {
                var vendorId = GetVendorId();

                var report = await _useCase.ExecuteSummaryAsync(vendorId, fromDate, toDate, walletTypeId);

                var dateRange = $"{fromDate:dd MMM yyyy} - {toDate:dd MMM yyyy}";
                var bytes = VendorWalletSummaryExcel.Generate(report, dateRange);

                return File(bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"WalletSummary_{DateTime.Now:yyyyMMdd}.xlsx");
            }


        [HttpGet("wallet-types")]
        public async Task<IActionResult> GetWalletTypes()
        {
            var result = await _useCase.ExecuteWalletTypeAsync();
            return Ok(result);
        }

        
        /* ================= VENDOR CONTEXT ================= */

        private int GetVendorId()
        {
            var vendorIdClaim = User.FindFirst("VendorId")?.Value;

            if (string.IsNullOrWhiteSpace(vendorIdClaim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return int.Parse(vendorIdClaim);
        }
    }
}
