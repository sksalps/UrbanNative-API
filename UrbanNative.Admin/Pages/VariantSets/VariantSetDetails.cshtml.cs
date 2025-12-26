using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminVariantSet;
using UrbanNative.Application.DTOs.AdminVariant;
using UrbanNative.Application.DTOs.AdminCategory;


namespace UrbanNative.Admin.Pages.VariantSets
{
    public class VariantSetDetailsModel : PageModel
    {
        private readonly IAdminVariantSetService _service;
        private readonly IAdminVariantService _variantService;
        private readonly IAdminCategoryService _categoryService;


        public VariantSetDetailsModel(
            IAdminVariantSetService service,
            IAdminVariantService variantService,
            IAdminCategoryService categoryService)
        {
            _service = service;
            _variantService = variantService;
            _categoryService = categoryService;
        }


        public AdminVariantSetDetailsDto VariantSet { get; set; } = null!;
        public IEnumerable<AdminVariantInsideSetDto> Variants { get; set; } = [];
        public IEnumerable<AdminVariantSetCategoryDto> AssignedCategories { get; set; } = [];
        public IEnumerable<AdminVariantListDto> AvailableVariants { get; set; } = [];
        public IEnumerable<AdminCategoryListDto> AvailableCategories { get; set; } = [];
        public async Task<IActionResult> OnGetAsync(int variantSetId)
        {
            await LoadAsync(variantSetId);

            if (VariantSet == null)
            {
                TempData["Error"] = "Variant Set not found or has been deleted.";
                return RedirectToPage("Index");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAddVariantAsync(int variantSetId, int variantId)
        {
            await _service.AddVariantAsync(variantSetId, variantId);
            return RedirectToPage(new { variantSetId });
        }

        public async Task<IActionResult> OnPostAssignCategoryAsync( int variantSetId, int categoryId)
        {
            try
            {
                await _service.AssignCategoryAsync(variantSetId, categoryId);
                return RedirectToPage(new { variantSetId });
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadAsync(variantSetId);
                return Page();
            }
        }


        public async Task<IActionResult> OnPostRemoveVariantAsync(int variantSetVariantId, int variantSetId)
        {
            await _service.RemoveVariantAsync(variantSetVariantId);

            return RedirectToPage(new { variantSetId });
        }
        public async Task<IActionResult> OnPostMoveVariantAsync( int variantSetVariantId,string direction,  int variantSetId)
        {
            await _service.MoveVariantAsync(variantSetVariantId, direction);
            return RedirectToPage(new { variantSetId });
        }


        public async Task<IActionResult> OnPostRemoveCategoryAsync(   int categoryVariantSetId,  int variantSetId)
        {
            await _service.RemoveCategoryAsync(categoryVariantSetId);
            return RedirectToPage(new { variantSetId });
        }


        private async Task LoadAsync(int variantSetId)
        {
            VariantSet = (await _service.GetDetailsAsync(variantSetId))!;
            Variants = await _service.GetVariantsAsync(variantSetId);
            AssignedCategories = await _service.GetAssignedCategoriesAsync(variantSetId);

            var allVariants = await _variantService.GetVariantsAsync(null, true);
            AvailableVariants = allVariants
                .Where(v => !Variants.Any(x => x.VariantID == v.VariantID));

            // ✅ FIX: assign to PROPERTY, not local variable
            AvailableCategories = await _service.GetAvailableCategoriesAsync(variantSetId);
        }

    }
}
