using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services;

public class AddBankModel : PageModel
{
    private readonly BankService _bankService;

    public AddBankModel(BankService bankService)
    {
        _bankService = bankService;
    }

    // ================= BIND =================
    [BindProperty]
    public BankSaveRequestDto Bank { get; set; } = new();

    // ================= ON GET =================
    public IActionResult OnGet(int? bankId)
    {
        // 🔹 Set EntityType (context driven)
        //Bank.EntityType = "Vendor";

        // 🔹 Edit Mode (optional)
        if (bankId.HasValue)
        {
            // Optional: load existing bank (future)
        }

        return Page();
    }

    // ================= UPLOAD + OCR =================

    public async Task<IActionResult> OnPostExtractOnlyAsync(IFormFile ChequeFile)
    {
        if (ChequeFile == null || ChequeFile.Length == 0)
        {
            return new JsonResult(new { isValid = false, message = "Invalid file" });
        }

        var result = await _bankService.ExtractOnlyAsync(ChequeFile);

        return new JsonResult(result);
    }
    // ================= SAVE =================
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // 🔥 Step 1: Ensure compliance row exists / updated
            var uploadId = await _bankService.UpsertComplianceAsync(
                Bank.UploadId,     // 0 → insert, >0 → update
                Bank.AccountNo     // DocumentNumber
            );

            Bank.UploadId = uploadId;

            // 🔥 Step 2: Save BankMaster
            await _bankService.SaveAsync(Bank);

            TempData["Success"] = "Bank details saved successfully";

            return RedirectToPage("/Business/Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return Page();
        }
    }
}