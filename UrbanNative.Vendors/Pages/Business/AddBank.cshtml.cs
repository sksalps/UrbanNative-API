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
    [BindProperty]
    public IFormFile? ChequeFile { get; set; }

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

    public async Task<IActionResult> OnPostAsync(IFormFile? ChequeFile)
    {
        // 🔹 FIELD LEVEL VALIDATION (INLINE)
        if (string.IsNullOrWhiteSpace(Bank.AccountHolderName))
            ModelState.AddModelError("Bank.AccountHolderName", "Account Holder Name is required");

        if (string.IsNullOrWhiteSpace(Bank.BankName))
            ModelState.AddModelError("Bank.BankName", "Bank Name is required");
        if (string.IsNullOrWhiteSpace(Bank.AccountType))
            ModelState.AddModelError("Bank.AccountType", "Please select account type");

        if (string.IsNullOrWhiteSpace(Bank.AccountNo))
            ModelState.AddModelError("Bank.AccountNo", "Account Number is required");

        if (string.IsNullOrWhiteSpace(Bank.IFSCCode))
            ModelState.AddModelError("Bank.IFSCCode", "IFSC is required");

        if (string.IsNullOrWhiteSpace(Bank.CityName))
            ModelState.AddModelError("Bank.CityName", "City is required");

        // 🔴 STOP IF INVALID
        if (!ModelState.IsValid)
            return Page();

        // 🔹 MAP DTO
        var dto = new BankSaveRequestDto
        {
            AccountHolderName = Bank.AccountHolderName,
            AccountNo = Bank.AccountNo,
            IFSCCode = Bank.IFSCCode,
            CityName = Bank.CityName,

            BankName = Bank.BankName,
            AccountType = Bank.AccountType,
            BranchName = Bank.BranchName,
            UPIId = Bank.UPIId,
            StateName = Bank.StateName,
            Pincode = Bank.Pincode,

            ComplianceName = ComplianceName
        };

        // 🔹 CALL SERVICE
        var result = await _bankService.SaveFullAsync(ChequeFile, dto);

     
        // 🔴 HANDLE VALIDATION ERRORS
        if (!result.IsSuccess && result.Errors != null && result.Errors.Any())
        {
            foreach (var field in result.Errors)
            {
                var key = $"Bank.{field.Key}";
                var message = field.Value.FirstOrDefault();

                ModelState.AddModelError(key, message);
            }

            return Page();
        }

        // 🔴 HANDLE SYSTEM ERROR (VERY IMPORTANT)
        if (!result.IsSuccess && !string.IsNullOrWhiteSpace(result.Message))
        {
            ModelState.AddModelError("", result.Message);
            return Page();
        }

        // 🔥 SUCCESS MESSAGE (USED BY JS TOAST)
        TempData["Success"] = "Bank saved successfully";

        // ❌ DO NOT REDIRECT HERE
        return Page();
    }
    public async Task<IActionResult> OnPostExtractOnlyAsync(IFormFile ChequeFile)
    {
        if (ChequeFile == null || ChequeFile.Length == 0)
        {
            return new JsonResult(new { isValid = false, message = "Invalid file" });
        }
        // 🔥 ALWAYS ensure ComplianceName
        if (string.IsNullOrWhiteSpace(ComplianceName))
            ComplianceName = "BankDetails";

        var result = await _bankService.ExtractOnlyAsync(ChequeFile, ComplianceName);

        return new JsonResult(result);
    }

}