using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminHSN;

namespace UrbanNative.Admin.Pages.HSN
{
    public class CreateHSNModel : PageModel
    {
        private readonly IAdminHSNService _hsnService;
        private readonly IAdminGSTService _gstService;

        public CreateHSNModel(
            IAdminHSNService hsnService,
            IAdminGSTService gstService)
        {
            _hsnService = hsnService;
            _gstService = gstService;
        }

        [BindProperty]
        public AdminHSNCreateDto HSN { get; set; } = new();

        public IEnumerable<SelectListItem> GSTList { get; set; } = [];

        public async Task OnGetAsync()
        {
            await LoadGSTAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadGSTAsync();
                return Page();
            }

            await _hsnService.CreateAsync(HSN);
            TempData["Success"] = "HSN created successfully";
            return RedirectToPage("Index");
        }

        private async Task LoadGSTAsync()
        {
            var gst = await _gstService.GetGSTAsync(null, true);

            GSTList = gst.Select(x => new SelectListItem
            {
                Value = x.GSTID.ToString(),
                Text = $"{x.GSTPercentage}%"
            });
        }

    }
}
