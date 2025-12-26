using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminVariantSet;


namespace UrbanNative.Admin.Pages.VariantSets
{
    public class IndexModel : PageModel
    {
        private readonly IAdminVariantSetService _service;

        public IndexModel(IAdminVariantSetService service)
        {
            _service = service;
        }

        public IEnumerable<AdminVariantSetListDto> VariantSets { get; set; } = [];

        public async Task OnGetAsync()
        {
            VariantSets = await _service.GetAllAsync();
        }

        public async Task<IActionResult> OnPostToggleAsync(int variantSetId)
        {
            await _service.ToggleStatusAsync(variantSetId);
            return RedirectToPage();
        }
    }
}