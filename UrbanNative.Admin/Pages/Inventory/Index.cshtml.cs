using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminInventory;

namespace UrbanNative.Admin.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly IAdminInventoryService _inventoryService;

        public IndexModel(IAdminInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public IEnumerable<AdminInventoryListDto> Items { get; set; }
            = Enumerable.Empty<AdminInventoryListDto>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? LowStockOnly { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        public async Task OnGetAsync()
        {
            Items = await _inventoryService.GetInventoryAsync(
                Search,
                CategoryId,
                LowStockOnly,
                IsActive
            );
        }
    }
}
