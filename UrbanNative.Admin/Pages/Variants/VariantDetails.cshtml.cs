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

        public AdminVariantDetailsDto Variant { get; set; } = null!;
        public IEnumerable<AdminVariantValueDto> Values { get; set; } = [];

        [BindProperty] public int? EditingValueId { get; set; }
        [BindProperty] public bool IsEditMode { get; set; }
        [BindProperty] public string NewValueName { get; set; } = string.Empty;

        // =========================
        // GET
        // =========================
        public async Task OnGetAsync(int variantId)
        {
            await LoadPageAsync(variantId);
        }

        // =========================
        // EDIT VALUE CLICK
        // =========================

        public async Task<IActionResult> OnPostEditValueAsync(int variantId,int valueId, string valueName)
        {
            // ✅ CLEAR OLD MESSAGES
            ModelState.Clear();
            TempData.Remove("Success");

            EditingValueId = valueId;
            NewValueName = valueName;
            IsEditMode = true;

            await LoadPageAsync(variantId);
            return Page();
        }


        // =========================
        // ADD / UPDATE VALUE
        // =========================


        public async Task<IActionResult> OnPostAddValueAsync(int variantId)
        {
            if (string.IsNullOrWhiteSpace(NewValueName))
            {
                ModelState.AddModelError("", "Value name is required.");

                IsEditMode = EditingValueId.HasValue;
                await LoadPageAsync(variantId); // ✅ REQUIRED
                return Page();
            }

            try
            {
                if (EditingValueId.HasValue)
                {
                    await _service.UpdateVariantValueAsync(
                        EditingValueId.Value,
                        NewValueName.Trim());

                    TempData["Success"] = "Variant value updated successfully.";
                }
                else
                {
                    await _service.AddVariantValueAsync(
                        variantId,
                        NewValueName.Trim());

                    TempData["Success"] = "Variant value added successfully.";
                }

                // clear edit state
                EditingValueId = null;
                IsEditMode = false;
                NewValueName = string.Empty;

                return RedirectToPage(new { variantId });
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);

                IsEditMode = EditingValueId.HasValue;
                await LoadPageAsync(variantId); // ✅ REQUIRED
                return Page();
            }
        }

        // =========================
        // TOGGLE VALUE
        // =========================
        public async Task<IActionResult> OnPostToggleValueAsync(int variantId, int valueId)
        {
            await _service.ToggleVariantValueAsync(valueId);

            return RedirectToPage(new { variantId });
        }

        // =========================
        // LOAD DATA
        // =========================
        private async Task LoadPageAsync(int variantId)
        {
            Variant = await _service.GetVariantAsync(variantId);
            Values = await _service.GetVariantValuesAsync(variantId);
        }
    }

}


