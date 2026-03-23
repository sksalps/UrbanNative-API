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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        await _bankService.SaveFullAsync(null, Bank);

        TempData["Success"] = "Bank saved successfully";

        return RedirectToPage("/Business/Index");
    }
}