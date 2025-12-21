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
        public async Task OnGetAsync(
    string? search,
    int? level,
    bool? isActive)
        {
            var all = await _service.GetCategoriesAsync();

            if (!string.IsNullOrWhiteSpace(search))
                all = all.Where(x =>
                    x.CategoryName.Contains(search,
                    StringComparison.OrdinalIgnoreCase));

            if (level.HasValue)
                all = all.Where(x => x.Level == level.Value);

            if (isActive.HasValue)
                all = all.Where(x => x.IsActive == isActive.Value);

            // IMPORTANT
            Categories = level.HasValue
                ? all.OrderBy(x => x.Level).ThenBy(x => x.SortOrder)
                : BuildTree(all);
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
        public async Task<IActionResult> OnPostCreateAsync(AdminCategorySaveDto dto)
        {
            var error = await _service.CreateAsync(dto);

            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return RedirectToPage();
            }

            TempData["Success"] = "Category created successfully";
            return RedirectToPage(); // reload list
        }

        private IEnumerable<AdminCategoryListDto> BuildTree(
    IEnumerable<AdminCategoryListDto> source)
        {
            var lookup = source.ToLookup(x => x.ParentCategoryID);
            var result = new List<AdminCategoryListDto>();

            void AddChildren(int? parentId)
            {
                foreach (var item in lookup[parentId].OrderBy(x => x.SortOrder))
                {
                    result.Add(item);
                    AddChildren(item.CategoryID);
                }
            }

            AddChildren(null); // roots first
            return result;
        }

    }
}
