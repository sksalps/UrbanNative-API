using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminHSN;

namespace UrbanNative.Admin.Pages.HSN
{
    public class IndexModel : PageModel
    {
        private readonly IAdminHSNService _hsnService;

        public IndexModel(IAdminHSNService hsnService)
        {
            _hsnService = hsnService;
        }

        public IEnumerable<AdminHSNListDto> Items { get; set; } = [];

        // Filters
        public string? Search { get; set; }
        public int? GSTId { get; set; }
        public bool? IsActive { get; set; }

        public async Task OnGetAsync(
            string? search,
            int? gstId,
            bool? isActive)
        {
            Search = search;
            GSTId = gstId;
            IsActive = isActive;

            var all = await _hsnService.GetAllAsync();

            // UI-level filtering (Phase-1)
            Items = all.Where(x =>
                (string.IsNullOrEmpty(search)
                    || x.HSNCode.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || (x.Description ?? "").Contains(search, StringComparison.OrdinalIgnoreCase))
                && (!gstId.HasValue || x.GSTId == gstId)
                && (!isActive.HasValue || x.IsActive == isActive)
            );
        }
        public async Task<IActionResult> OnPostToggleAsync(int hsnId)
        {
            await _hsnService.ToggleActiveAsync(hsnId);
            return RedirectToPage();
        }

    }
}
