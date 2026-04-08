using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Shared.SharedDTOs;
using UrbanNative.Vendors.Services.Interfaces;

public class AddBankModel : PageModel
{
    public string ApiBaseUrl { get; private set; } = "";
    private readonly IBankService _bankService;
    private readonly IConfiguration _config;

    public AddBankModel(IBankService bankService, IConfiguration config)
    {
        _bankService = bankService;
        _config = config;
        ApiBaseUrl = _config["ApiSettings:BaseUrl"] ?? "";
    }

    [BindProperty]
    public BankFormModel Bank { get; set; } = new();
    public bool IsEditMode => Bank?.BankID > 0;
    [BindProperty]
    public string? ComplianceName { get; set; }

    [BindProperty]
    public string? AllowedFileTypes { get; set; }
    [BindProperty]
    public IFormFile? ChequeFile { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string? ComplianceName, string? fileType, int? bankId)
    {
        //Bank.ApiBaseUrl = ApiBaseUrl ?? "";
        // 🔷 Edit Mode
        if (bankId.HasValue)
        {
            //Bank.BankID = bankId;
            
            var result = await _bankService.GetBankByIdAsync(bankId.Value);

            if (result == null)
            {
                TempData["Error"] = "Bank not found.";
                return RedirectToPage("/Business/BankList");
            }

            // 🔒 Block Verified Edit
            if (result.IsVerified)
            {
                TempData["Error"] = "Verified bank cannot be edited.";
                return RedirectToPage("/Business/BankList");
            }

            Bank = new BankFormModel
            {
                BankID = result.BankID,
                BankName = result.BankName,
                AccountHolderName = result.AccountHolderName,
                AccountNo = result.AccountNo,
                AccountType = result.AccountType,
                CityName=result.CityName,

                IFSCCode = result.IFSCCode,
                UPIId = result.UPIId,
                IsPrimary = result.IsPrimary,
                StateName = result.StateName,
                CountryName = result.CountryName,
                BranchName = result.BranchName,
                Pincode = result.Pincode,
                ComplianceId=result.ComplianceId,
                FileName=result.FileName,
                FileURL=result.FileURL,
                ApiBaseUrl = ApiBaseUrl,
            };
        }
        

        // 🔷 Common Params
        this.ComplianceName = string.IsNullOrWhiteSpace(ComplianceName)
            ? "BankDetails"
            : ComplianceName;

        this.AllowedFileTypes = string.IsNullOrWhiteSpace(fileType)
            ? "jpg,jpeg,png"
            : fileType;

        return Page();
    }

    
    public async Task<IActionResult> OnPostAsync()
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
        {
            if (Bank.BankID > 0)
            {
                var existing = await _bankService.GetBankByIdAsync(Bank.BankID);
                Bank.FileURL = existing?.FileURL; // 🔥 restore
                Bank.ApiBaseUrl = ApiBaseUrl;
            }
            return Page(); 
        }


        // 🔹 MAP DTO
        var dto = new BankSaveRequestDto
        {
            BankID = Bank.BankID,
            AccountHolderName = Bank.AccountHolderName,
            AccountNo = Bank.AccountNo,
            IFSCCode = Bank.IFSCCode,
            CityName = Bank.CityName,

            BankName = Bank.BankName,
            AccountType = Bank.AccountType,
            IsPrimary = Bank.IsPrimary,
            BranchName = Bank.BranchName,
            UPIId = Bank.UPIId,
            CountryName=Bank.CountryName,
            StateName = Bank.StateName,
            Pincode = Bank.Pincode,
            FileName = Bank.FileName,
            FileURL = Bank.FileURL,
            ApiBaseUrl=ApiBaseUrl,
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
    
    public async Task<IActionResult> OnPostDeleteAsync(int bankId)
    {
        try
        {
            var result = await _bankService.DeleteBankAsync(bankId);

            return new JsonResult(new
            {
                success = result.IsSuccess,
                message = result.Message
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}