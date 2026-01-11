using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminReturnsOrder;

namespace UrbanNative.Admin.Pages.ReturnsOrder
{
    public class IndexModel : PageModel
    {
        private readonly IAdminReturnsService _returnsService;

        public IndexModel(IAdminReturnsService returnsService)
        {
            _returnsService = returnsService;
        }

        public IEnumerable<AdminReturnListDto> Returns { get; set; }   = new List<AdminReturnListDto>();

        public async Task OnGetAsync(   string? status,         DateTime? fromDate,       DateTime? toDate)
        {
            Returns = await _returnsService.GetReturnsAsync( status,     null,       // Vendor filter later
                fromDate,
                toDate);
        }
    }
}
