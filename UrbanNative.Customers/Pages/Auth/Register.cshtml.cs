using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrbanNative.Customers.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        public int TempID { get; set; }
        public string Token { get; set; }

        public void OnGet(int tempId, string token)
        {
            TempID = tempId;
            Token = token;
        }

        public IActionResult OnPost()
        {
            // Call registration API next step
            return RedirectToPage("/Dashboard/Index");
        }
    }
}