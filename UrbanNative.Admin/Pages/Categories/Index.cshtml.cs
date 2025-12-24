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

        public int? ParentCategoryID { get; set; }

        public List<CategoryBreadcrumbDto> Breadcrumb { get; set; } = new();
        public List<AdminCategoryFlatDto> FlatCategories { get; set; } = new();


        public bool OpenCategoryModal { get; set; }

        public IndexModel(IAdminCategoryService service)
        {
            _service = service;
        }

        // ======================
        // NORMAL PAGE LOAD
        // ======================
        public async Task OnGetAsync(
            string? search, int? level,bool? isActive,int? categoryId)
        {
            ParentCategoryID = null;
            Breadcrumb = new();
            OpenCategoryModal = false;
            FlatCategories = await _service.GetFlatCategoriesAsync();
            await LoadCategoriesAsync(search, level, isActive, categoryId);
        }



        // ====================== 
        // ADD ROOT CATEGORY
        // ======================
        public async Task<IActionResult> OnGetAddRootAsync()
        {
            ParentCategoryID = null;
            Breadcrumb = new();
            OpenCategoryModal = true;

            await LoadCategoriesAsync(null,null, null, null);
            return Page();
        }

        // ======================
        // ADD SUB CATEGORY
        // ======================
        public async Task<IActionResult> OnGetAddSubAsync(int id)
        {
            ParentCategoryID = id;
            Breadcrumb = await _service.GetBreadcrumbAsync(id);
            OpenCategoryModal = true;

            await LoadCategoriesAsync(null,null, null, null);
            return Page();
        }

        // ======================
        // CREATE CATEGORY
        // ======================
        public async Task<IActionResult> OnPostCreateAsync(AdminCategorySaveDto dto)
        {
            var error = await _service.CreateAsync(dto);

            if (!string.IsNullOrEmpty(error))
                TempData["Error"] = error;
            else
                TempData["Success"] = "Category created successfully";

            return RedirectToPage();
        }

        // ======================
        // TOGGLE ACTIVE
        // ======================
        public async Task<IActionResult> OnPostToggleActiveAsync(int categoryId)
        {
            var error = await _service.ToggleActiveAsync(categoryId);

            if (!string.IsNullOrEmpty(error))
                TempData["Error"] = error;
            else
                TempData["Success"] = "Category status updated successfully";

            return RedirectToPage();
        }

        // ======================
        // HELPERS
        // ======================
        private async Task LoadCategoriesAsync(
            string? search,
            int? level,
            bool? isActive,
            int? categoryId)
        {
            var all = await _service.GetCategoriesAsync();

            if (!string.IsNullOrWhiteSpace(search))
                all = all.Where(x =>
                    x.CategoryName.Contains(search,
                    StringComparison.OrdinalIgnoreCase));

            if (isActive.HasValue)
                all = all.Where(x => x.IsActive == isActive.Value);

            //  CATEGORY FILTER (KEY PART)
            if (categoryId.HasValue)
            {
                // FLAT MODE
                Categories = all
                    .Where(x =>
                        x.CategoryID == categoryId.Value ||
                        x.ParentCategoryID == categoryId.Value)
                    .OrderBy(x => x.SortOrder);

                return; // NO TREE
            }

            // DEFAULT → TREE MODE
            Categories = level.HasValue
                ? all.OrderBy(x => x.Level).ThenBy(x => x.SortOrder)
                : BuildTree(all);
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

            AddChildren(null);
            return result;
        }
    }
}
