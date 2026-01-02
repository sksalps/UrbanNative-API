using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminGST;

namespace UrbanNative.Admin.Pages.GST
{

    public class IndexModel : PageModel
    {
        private readonly AdminGSTService _service;

        public IndexModel(AdminGSTService service)
        {
            _service = service;
        }

        public IEnumerable<AdminGSTListDto> GSTList { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? GSTPercentage { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        public async Task OnGetAsync()
        {
            GSTList = await _service.GetGSTAsync(GSTPercentage, IsActive);

            foreach (var g in GSTList)
            {
                Console.WriteLine($"GST {g.GSTPercentage}% → HSNCount = {g.HSNCount}");
            }

        }

        public async Task<IActionResult> OnPostToggleAsync(
            int gstId,
            bool isActive)
        {
            await _service.ToggleGSTAsync(gstId, isActive);
          

            return RedirectToPage();
        }
    }
}