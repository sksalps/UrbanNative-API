using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrbanNative.Admin.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            await HttpContext.SignOutAsync("AdminCookie");

            return RedirectToPage("/Login");
        }
    }
}