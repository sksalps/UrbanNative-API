using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminVariant;

namespace UrbanNative.Admin.Pages.Variants
{
    public class IndexModel : PageModel
    {
        private readonly IAdminVariantService _service;

        public IndexModel(IAdminVariantService service)
        {
            _service = service;
        }

        public IEnumerable<AdminVariantListDto> Variants { get; set; }
            = Enumerable.Empty<AdminVariantListDto>();

        public async Task OnGetAsync(string? search, bool? isActive)
        {
            Variants = await _service.GetVariantsAsync(search, isActive);
        }

        public async Task<IActionResult> OnPostToggleAsync(int id)
        {
            await _service.ToggleVariantAsync(id);
            return RedirectToPage();
        }
    }
}
