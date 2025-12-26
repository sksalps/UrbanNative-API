using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;

namespace UrbanNative.Admin.Pages.VariantSets
{
    public class CreateEditVariantSetModel : PageModel
    {
        private readonly IAdminVariantSetService _service;

        public CreateEditVariantSetModel(IAdminVariantSetService service)
        {
            _service = service;
        }

        [BindProperty]
        public string VariantSetName { get; set; } = string.Empty;

        public int? VariantSetId { get; set; }

        public async Task OnGetAsync(int? variantSetId)
        {
            VariantSetId = variantSetId;

            if (variantSetId.HasValue)
            {
                var set = await _service.GetDetailsAsync(variantSetId.Value);
                VariantSetName = set!.VariantSetName;
            }
        }

        public async Task<IActionResult> OnPostAsync(int? variantSetId)
        {
            if (string.IsNullOrWhiteSpace(VariantSetName))
            {
                ModelState.AddModelError("", "Variant Set name is required.");
                return Page();
            }

            try
            {
                if (variantSetId.HasValue)
                    await _service.UpdateAsync(variantSetId.Value, VariantSetName.Trim());
                else
                    await _service.CreateAsync(VariantSetName.Trim());

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
