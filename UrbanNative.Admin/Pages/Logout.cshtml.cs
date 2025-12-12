using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace YourAppNamespace.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            // show a simple page or redirect directly on GET if desired
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync("AdminCookie");
            return RedirectToPage("/Login");
        }
    }
}