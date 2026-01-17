using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Settings
{
    [Authorize(Policy = "VendorOnly")]
    public class IndexModel : PageModel
    {
        private readonly IVendorSettingsService _service;

        public IndexModel(IVendorSettingsService service)
        {
            _service = service;
        }

        // =========================
        // 📋 SETTINGS LIST
        // =========================
        public List<VendorSystemSettingDto> Settings { get; set; } = new();

        public async Task OnGetAsync()
        {
            Settings = await _service.GetAsync();
        }

        // =========================
        // ✏ UPDATE SETTING
        // =========================
        [BindProperty]
        public VendorSystemSettingUpdateDto EditModel { get; set; } = new();

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid input";
                return RedirectToPage();
            }

            await _service.UpdateAsync(EditModel);

            TempData["Success"] = "Setting updated successfully";
            return RedirectToPage();
        }

        // =========================
        // 🕒 HISTORY (AJAX)
        // =========================
        public async Task<IActionResult> OnGetHistoryAsync(int settingId)
        {
            // Safety guard
            if (settingId <= 0)
                return BadRequest("Invalid setting id");

            var history = await _service.GetHistoryAsync(settingId);

            return new JsonResult(history);
        }
    }
}
