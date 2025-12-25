using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;

namespace UrbanNative.Admin.Pages.Variants
{
    public class CreateEditVariantModel : PageModel
    {
        private readonly IAdminVariantService _service;

        public CreateEditVariantModel(IAdminVariantService service)
        {
            _service = service;
        }

        [BindProperty]
        public string VariantName { get; set; } = string.Empty;

        public int? VariantId { get; set; }

        public async Task OnGetAsync(int? variantId)
        {
            VariantId = variantId;

            if (variantId.HasValue)
            {
                var variant = await _service.GetVariantAsync(variantId.Value);
                VariantName = variant.VariantName;
            }
        }

        public async Task<IActionResult> OnPostAsync(int? variantId)
        {
            if (string.IsNullOrWhiteSpace(VariantName))
            {
                ModelState.AddModelError("", "Variant name is required.");
                return Page();
            }

            try
            {
                if (variantId.HasValue)
                {
                    await _service.UpdateVariantAsync(variantId.Value, VariantName.Trim());
                    TempData["Success"] = "Variant updated successfully.";
                }
                else
                {
                    await _service.CreateVariantAsync(VariantName.Trim());
                    TempData["Success"] = "Variant created successfully.";
                }

                return RedirectToPage("Index");
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }
    }

}


