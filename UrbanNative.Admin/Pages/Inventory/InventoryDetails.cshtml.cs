using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminInventory;

namespace UrbanNative.Admin.Pages.Inventory
{
    public class InventoryDetailsModel : PageModel
    {
        private readonly IAdminInventoryService _inventoryService;

        public InventoryDetailsModel(IAdminInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public AdminInventorySummaryDto? Summary { get; set; }
        public IEnumerable<AdminInventorySkuDto> Skus { get; set; }
            = Enumerable.Empty<AdminInventorySkuDto>();

        public async Task<IActionResult> OnGetAsync(int productId)
        {
            Summary = await _inventoryService.GetInventorySummaryAsync(productId);

            if (Summary == null)
                return NotFound();

            Skus = await _inventoryService.GetInventorySkusAsync(productId);

            return Page();
        }
    }
}
