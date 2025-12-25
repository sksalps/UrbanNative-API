using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Admin.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IAdminCategoryService _service;

        public IndexModel(IAdminCategoryService service)
        {
            _service = service;
        }

        // =========================
        // DATA FOR UI
        // =========================
        public IEnumerable<AdminCategoryListDto> Categories { get; set; } = [];
        public IEnumerable<AdminCategoryListDto> AllCategories { get; set; } = [];

        // =========================
        // FILTER INPUTS
        // =========================
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }   // dropdown selection

        [BindProperty(SupportsGet = true)]
        public int? Level { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        // =========================
        // GET
        // =========================
        public async Task OnGetAsync()
        {
            // Load once
            AllCategories = (await _service.GetCategoriesAsync()).ToList();

            IEnumerable<AdminCategoryListDto> result = AllCategories;

            // =========================
            // CATEGORY DROPDOWN LOGIC
            // =========================

            if (CategoryId.HasValue && CategoryId.Value > 0)
            {
                var selected = AllCategories
                    .FirstOrDefault(x => x.CategoryID == CategoryId.Value);

                if (selected != null)
                {
                    int baseLevel = selected.Level;

                    // relative depth = max 2
                    result = AllCategories.Where(x =>
                        x.CategoryID == selected.CategoryID ||
                        (x.ParentCategoryID == selected.CategoryID) ||
                        (x.Level == baseLevel + 2 &&
                         AllCategories.Any(p =>
                             p.CategoryID == x.ParentCategoryID &&
                             p.ParentCategoryID == selected.CategoryID))
                    );
                }
                else
                {
                    result = Enumerable.Empty<AdminCategoryListDto>();
                }
            }
            else
            {
                // All Categories selected → flat mode
                result = AllCategories;
            }

            // =========================
            // SEARCH FILTER
            // =========================
            if (!string.IsNullOrWhiteSpace(Search))
            {
                result = result.Where(x =>
                    x.CategoryName.Contains(Search,
                        StringComparison.OrdinalIgnoreCase));
            }

            // =========================
            // ACTIVE FILTER
            // =========================
            if (IsActive.HasValue)
            {
                result = result.Where(x => x.IsActive == IsActive.Value);
            }

            // =========================
            // LEVEL FILTER (contextual)
            // =========================
            if (Level.HasValue)
            {
                result = result.Where(x => x.Level == Level.Value);
            }

            // =========================
            // FINAL ORDERING
            // =========================
            Categories = result
                .OrderBy(x => x.Level)
                .ThenBy(x => x.SortOrder)
                .ToList();
        }

        // =========================
        // POST: CREATE CATEGORY
        // =========================
        public async Task<IActionResult> OnPostCreateAsync(AdminCategorySaveDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid category data";
                return RedirectToPage();
            }

            await _service.CreateAsync(dto);

            TempData["Success"] = "Category created successfully";
            return RedirectToPage();
        }

        // =========================
        // POST: TOGGLE ACTIVE
        // =========================
        public async Task<IActionResult> OnPostToggleActiveAsync(int categoryId)
        {
            var error = await _service.ToggleActiveAsync(categoryId);

            if (!string.IsNullOrWhiteSpace(error))
                TempData["Error"] = error;
            else
                TempData["Success"] = "Category status updated";

            return RedirectToPage();
        }
    }
}
