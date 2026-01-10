using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminReturnsOrder;

namespace UrbanNative.Admin.Pages.ReturnsOrder
{
    public class ReturnDetailsModel : PageModel
    {
        private readonly IAdminReturnsService _returnsService;

        public ReturnDetailsModel(IAdminReturnsService returnsService)
        {
            _returnsService = returnsService;
        }

        public AdminReturnDetailsDto Return { get; set; } = null!;
        public List<LogisticsProviderDto> LogisticsProviders { get; set; } = new();

        public async Task OnGetAsync(int returnId)
        {
            Return = await _returnsService.GetReturnDetailsAsync(returnId)
                     ?? throw new Exception("Return not found");

            // 🔥 Load courier list for dropdown
            LogisticsProviders = (await _returnsService.GetLogisticsProvidersAsync()).ToList();
        }

        public async Task<IActionResult> OnPostDecisionAsync(
            int returnId,
            string newStatus,
            string adminComment,
            string remarkText)
        {
            await _returnsService.UpdateStatusAsync(
                returnId,
                newStatus,
                adminComment,
                remarkText);

            return RedirectToPage(new { returnId });
        }

        public async Task<IActionResult> OnPostCreateShipmentAsync(
            int returnId,
            int logisticsProviderID,
            string trackingNumber)
        {
            await _returnsService.CreateReturnShipmentAsync(
                returnId,
                logisticsProviderID,
                trackingNumber);

            return RedirectToPage(new { returnId });
        }
    }
}
