using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Shared.SharedDTOs;
using UrbanNative.Vendors.Services.Interfaces;

public class AddBankModel : PageModel
{
    private readonly IBankService _bankService;

    public AddBankModel(IBankService bankService)
    {
        _bankService = bankService;
    }

    [BindProperty]
    public BankFormModel Bank { get; set; } = new();

    [BindProperty]
    public string ComplianceName { get; set; }

    [BindProperty]
    public string AllowedFileTypes { get; set; }

    public IActionResult OnGet(string? ComplianceName, string? fileType)
    {
        this.ComplianceName = string.IsNullOrWhiteSpace(ComplianceName)
            ? "BankDetails"
            : ComplianceName;

        this.AllowedFileTypes = string.IsNullOrWhiteSpace(fileType)
            ? "jpg,jpeg,png"
            : fileType;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(IFormFile ChequeFile)
    {
        if (!ModelState.IsValid)
            return Page();

        var dto = new BankSaveRequestDto
        {
            AccountHolderName = Bank.AccountHolderName,
            AccountNo = Bank.AccountNo,
            IFSCCode = Bank.IFSCCode,
            CityName = Bank.CityName,

            BankName = Bank.BankName,
            BranchName = Bank.BranchName,
            UPIId = Bank.UPIId,
            StateName = Bank.StateName,
            Pincode = Bank.Pincode,

            ComplianceName = ComplianceName
        };

        await _bankService.SaveFullAsync(ChequeFile, dto);

        TempData["Success"] = "Bank saved successfully";

        return RedirectToPage("/Business/Index");
    }

    public async Task<IActionResult> OnPostExtractOnlyAsync(IFormFile ChequeFile)
    {
        if (ChequeFile == null || ChequeFile.Length == 0)
        {
            return new JsonResult(new { isValid = false, message = "Invalid file" });
        }
        // 🔥 ALWAYS ensure ComplianceName
        if (string.IsNullOrWhiteSpace(ComplianceName))
            ComplianceName = "Bank Details";

        var result = await _bankService.ExtractOnlyAsync(ChequeFile, ComplianceName);

        return new JsonResult(result);
    }

}