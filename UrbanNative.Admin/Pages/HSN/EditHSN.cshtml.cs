using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminHSN;

namespace UrbanNative.Admin.Pages.HSN
{
    public class EditHSNModel : PageModel
    {
        private readonly IAdminHSNService _hsnService;
        private readonly IAdminGSTService _gstService;

        public EditHSNModel(
            IAdminHSNService hsnService,
            IAdminGSTService gstService)
        {
            _hsnService = hsnService;
            _gstService = gstService;
        }

        [BindProperty]
        public AdminHSNUpdateDto HSN { get; set; } = new();

        public IEnumerable<SelectListItem> GSTList { get; set; } = [];

        public async Task<IActionResult> OnGetAsync(int hsnId)
        {
            var data = await _hsnService.GetByIdAsync(hsnId);
            if (data == null)
                return RedirectToPage("Index");

            HSN = new AdminHSNUpdateDto
            {
                HSNId = data.HSNId,
                HSNCode = data.HSNCode,
                Description = data.Description,
                GSTId = data.GSTId
            };

            await LoadGSTAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadGSTAsync();
                return Page();
            }

            await _hsnService.UpdateAsync(HSN.HSNId, HSN);
            TempData["Success"] = "HSN updated successfully";
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
