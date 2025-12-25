using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminVariant;

namespace UrbanNative.Admin.Pages.Variants
{
    public class VariantDetailsModel : PageModel
    {
        private readonly IAdminVariantService _service;

        public VariantDetailsModel(IAdminVariantService service)
        {
            _service = service;
        }

        [BindProperty]
        public string NewValueName { get; set; } = string.Empty;

        public AdminVariantDetailsDto Variant { get; set; } = null!;
        public IEnumerable<AdminVariantValueDto> Values { get; set; }
            = Enumerable.Empty<AdminVariantValueDto>();

        public async Task OnGetAsync(int variantId)
        {
            await LoadPageAsync(variantId);
        }

        public async Task<IActionResult> OnPostAddValueAsync(int variantId)
        {
            if (string.IsNullOrWhiteSpace(NewValueName))
            {
                ModelState.AddModelError("", "Value name is required.");
                await LoadPageAsync(variantId);
                return Page();
            }

            try
            {
                await _service.AddVariantValueAsync(variantId, NewValueName.Trim());
                TempData["Success"] = "Variant value added successfully.";
                return RedirectToPage(new { variantId });
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadPageAsync(variantId);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostToggleVariantAsync(int variantId)
        {
            await _service.ToggleVariantAsync(variantId);
            return RedirectToPage(new { variantId });
        }

        public async Task<IActionResult> OnPostToggleValueAsync(int variantId, int valueId)
        {
            await _service.ToggleVariantValueAsync(valueId);
            return RedirectToPage(new { variantId });
        }

        private async Task LoadPageAsync(int variantId)
        {
            Variant = await _service.GetVariantAsync(variantId);
            Values = await _service.GetVariantValuesAsync(variantId);
        }
    }
}
