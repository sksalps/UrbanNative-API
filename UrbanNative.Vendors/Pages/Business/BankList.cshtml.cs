using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Business
{
    public class BankListModel : PageModel
    {
        private readonly IBankService _bankService;

        public BankListModel(IBankService bankService)
        {
            _bankService = bankService;
        }

        // 🔷 Grid Binding
        public List<BankListDto> Banks { get; set; } = new();

        // 🔷 Temp Message (Success/Error UI)
        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        

        public async Task OnGetAsync()
        {
             await LoadBanksAsync();
        }


        // 🔷 Load Bank List
        private async Task LoadBanksAsync()
        {
            try
            {
                var result = await _bankService.GetBankListAsync();

                if (result != null)
                    Banks = result;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Unable to load bank list.";
                // Optional: log ex
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int bankId)
        {
            try
            {
                var result = await _bankService.DeleteBankAsync(bankId);

                return new JsonResult(new
                {
                    success = result.IsSuccess,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        public async Task<IActionResult> OnPostSetPrimaryAsync(int bankId)
        {
            try
            {
                var result = await _bankService.SetPrimaryBankAsync(bankId);

                return new JsonResult(new
                {
                    success = result.IsSuccess,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}