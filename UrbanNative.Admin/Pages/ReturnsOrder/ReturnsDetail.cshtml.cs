using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminReturnsOrder;

namespace UrbanNative.Admin.Pages.ReturnsOrder
{
    public class ReturnsDetailsModel : PageModel
    {
        private readonly IAdminReturnsService _returnsService;

        public ReturnsDetailsModel(IAdminReturnsService returnsService)
        {
            _returnsService = returnsService;
        }

        public AdminReturnDetailsDto Return { get; set; } = null!;

        public async Task OnGetAsync(int returnId)
        {
            Return = await _returnsService.GetReturnDetailsAsync(returnId)
                     ?? throw new Exception("Return not found");
        }

        public async Task<IActionResult> OnPostDecisionAsync(
            int returnId,
            bool isApproved,
            string adminComment,
            string remarkText)
        {
            await _returnsService.ApproveRejectAsync(
                returnId, isApproved, adminComment, remarkText);

            return RedirectToPage(new { returnId });
        }

        public async Task<IActionResult> OnPostCreateShipmentAsync(
            int returnId,
            string courierName,
            string trackingNumber,
            string pickupAddress,
            string deliveryAddress)
        {
            await _returnsService.CreateReturnShipmentAsync(
                returnId, courierName, trackingNumber, pickupAddress, deliveryAddress);

            return RedirectToPage(new { returnId });
        }
    }
}
