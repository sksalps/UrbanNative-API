using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Admin.Pages.Categories
{
    public class CategoryDetailsModel : PageModel
    {
        private readonly IAdminCategoryService _service;

        public AdminCategoryDetailDto Category { get; set; } = null!;
        public List<CategoryBreadcrumbDto> Breadcrumb { get; set; } = new();

        public CategoryDetailsModel(IAdminCategoryService service)
        {
            _service = service;
        }


  
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return NotFound();

            Category = data;
            var category = await _service.GetCategoryDetailsAsync(id);
            Breadcrumb = category.Breadcrumb;
            return Page();
        }



        public async Task<IActionResult> OnPostCreateAsync(AdminCategorySaveDto dto)
        {
            var error = await _service.CreateAsync(dto);

            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return RedirectToPage(new { id = dto.ParentCategoryID });
            }

            TempData["Success"] = "Child category created successfully";
            return RedirectToPage(new { id = dto.ParentCategoryID });
        }

        // ✅ UPDATE HANDLER (THIS BELONGS HERE)


        public async Task<IActionResult> OnPostUpdateAsync( AdminCategorySaveDto dto, int categoryId)
        {
            var error = await _service.UpdateAsync(categoryId, dto);

            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return RedirectToPage(new { id = categoryId });
            }

            TempData["Success"] = "Category updated successfully";
            return RedirectToPage(new { id = categoryId });
        }


        // ✅ TOGGLE HANDLER

        public async Task<IActionResult> OnPostToggleActiveAsync(int categoryId)
        {
            var error = await _service.ToggleActiveAsync(categoryId);

            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return RedirectToPage(new { id = categoryId });
            }

            TempData["Success"] = "Category status updated successfully";
            return RedirectToPage(new { id = categoryId });
        }

    }
}
