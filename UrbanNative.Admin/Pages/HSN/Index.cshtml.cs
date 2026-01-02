using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminHSN;
using UrbanNative.Application.DTOs.AdminGST;


namespace UrbanNative.Admin.Pages.HSN
{
    public class IndexModel : PageModel
    {
        private readonly IAdminHSNService _hsnService;
        private readonly IAdminGSTService _gstService;
        public IndexModel(IAdminHSNService hsnService, IAdminGSTService gstService)
        {
            _hsnService = hsnService;
            _gstService = gstService;
        }

        public IEnumerable<AdminHSNListDto> Items { get; set; } = [];

        // Filters
        public IEnumerable<SelectListItem> GstList { get; set; } = [];

        public string? Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? GstId { get; set; } 

        public bool? IsActive { get; set; }

        /*  public async Task OnGetAsync(string? search,int? gstId, bool? isActive)
          {
              Search = search;
              GSTId = gstId;
              IsActive = isActive;

              //var all = await _hsnService.GetAllAsync();
              Items = await _hsnService.GetFilterAsync(Search, GSTId, IsActive);
              // UI-level filtering (Phase-1)
             /* Items = all.Where(x =>            (string.IsNullOrEmpty(search)
                      || x.HSNCode.Contains(search, StringComparison.OrdinalIgnoreCase)
                      || (x.Description ?? "").Contains(search, StringComparison.OrdinalIgnoreCase))
                  && (!gstId.HasValue || x.GSTId == gstId)
                  && (!isActive.HasValue || x.IsActive == isActive)
              );


          }*/
        public async Task OnGetAsync(string? search, int? gstId, bool? isActive)
        {
            Search = search;
            GstId = gstId;
            IsActive = isActive;
            Console.WriteLine($"GST FILTER VALUE = {GstId}");
            Items = await _hsnService.GetFilterAsync(Search, GstId, IsActive);

            var gstList = await _gstService.GetGSTAsync(null, true);

            GstList = gstList.Select(g => new SelectListItem
            {
                Value = g.GSTID.ToString(),
                Text = $"{g.GSTPercentage}%",
                Selected = GstId.HasValue && g.GSTID == GstId.Value
            }).ToList();
        }

        public async Task<IActionResult> OnPostToggleAsync(int hsnId)
        {
            await _hsnService.ToggleActiveAsync(hsnId);
            return RedirectToPage();
        }

    }
}
