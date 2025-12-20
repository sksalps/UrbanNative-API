using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Admin.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IAdminCategoryService _service;

        public IEnumerable<AdminCategoryListDto> Categories { get; set; } = [];

        public IndexModel(IAdminCategoryService service)
        {
            _service = service;
        }
        public async Task OnGetAsync(    string? search,    int? level,    bool? isActive)
        {
            var data = await _service.GetCategoriesAsync();

            if (!string.IsNullOrWhiteSpace(search))
                data = data.Where(x => x.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase));

            if (level.HasValue)
                data = data.Where(x => x.Level == level.Value);

            if (isActive.HasValue)
                data = data.Where(x => x.IsActive == isActive.Value);

            Categories = data
                .OrderBy(x => x.Level)
                .ThenBy(x => x.SortOrder);
        }


        public async Task<IActionResult> OnPostToggleActiveAsync(int categoryId)
        {
            var error = await _service.ToggleActiveAsync(categoryId);

            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return RedirectToPage();
            }

            TempData["Success"] = "Category status updated successfully";
            return RedirectToPage();
        }

    }

}
