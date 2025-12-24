using System.Text.Json;
using UrbanNative.Application.DTOs.AdminCategory;
using static System.Net.WebRequestMethods;

namespace UrbanNative.Admin.Services
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly HttpClient _http;

        public AdminCategoryService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // =========================
        // Admin – Category Listing
        // =========================
        public async Task<IEnumerable<AdminCategoryListDto>> GetCategoriesAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminCategoryListDto>>(
                "/api/admin/categories")
                ?? Enumerable.Empty<AdminCategoryListDto>();
        }

        public async Task<List<CategoryBreadcrumbDto>> GetBreadcrumbAsync(int categoryId)
        {
            var result = new List<CategoryBreadcrumbDto>();
            await BuildBreadcrumbAsync(categoryId, result);

            result.Reverse(); // root → leaf
            return result;
        }
        //Show Recursive depth of Cat/Subcat

        private async Task BuildBreadcrumbAsync(  int categoryId,  List<CategoryBreadcrumbDto> result)
        {
            var category = await GetByIdAsync(categoryId);
            if (category == null)
                return;

            result.Add(new CategoryBreadcrumbDto
            {
                CategoryID = category.CategoryID,
                Name = category.CategoryName
            });

            if (category.ParentCategoryID.HasValue)
            {
                await BuildBreadcrumbAsync(
                    category.ParentCategoryID.Value,
                    result);
            }
        }


        // =========================
        // Admin – Single Category
        // =========================
        public async Task<AdminCategoryDetailDto?> GetByIdAsync(int categoryId)
        {
            var res = await _http.GetAsync($"/api/admin/categories/{categoryId}");

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<AdminCategoryDetailDto>();
        }
        public async Task<AdminCategoryDetailsDto> GetCategoryDetailsAsync(int categoryId)
        {
            var category = await GetByIdAsync(categoryId);
            if (category == null)
                throw new Exception("Category not found");

            return new AdminCategoryDetailsDto
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Level = category.Level,
                IsActive = category.IsActive,
                ParentCategoryID = category.ParentCategoryID,

                Breadcrumb = await GetBreadcrumbAsync(categoryId)
            };
        }

        private void BuildFlatTree(
            IEnumerable<AdminCategoryListDto> categories,
            int? parentId,
            string prefix,
            List<AdminCategoryFlatDto> result)
        {
            var children = categories
                .Where(c => c.ParentCategoryID == parentId)
                .OrderBy(c => c.SortOrder);

            foreach (var cat in children)
            {
                result.Add(new AdminCategoryFlatDto
                {
                    CategoryID = cat.CategoryID,
                    DisplayName = prefix + cat.CategoryName
                });

                BuildFlatTree(
                    categories,
                    cat.CategoryID,
                    prefix + "— ",
                    result);
            }
        }
        public async Task<List<AdminCategoryFlatDto>> GetFlatCategoriesAsync()
        {
            var categories = await GetCategoriesAsync(); // DTO ✅

            var result = new List<AdminCategoryFlatDto>();
            BuildFlatTree(categories, null, "", result);

            return result;
        }


        // =========================
        // Admin – Create Category
        // =========================
        public async Task<string?> CreateAsync(AdminCategorySaveDto dto)
        {
            var res = await _http.PostAsJsonAsync("/api/admin/categories", dto);

            if (res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(json)
                ? "Unable to create category"
                : JsonDocument.Parse(json).RootElement.GetProperty("message").GetString();
        }



        // =========================
        // Admin – Update Category
        // =========================

        public async Task<string?> UpdateAsync(int categoryId, AdminCategorySaveDto dto)
        {
            var res = await _http.PutAsJsonAsync(
                $"/api/admin/categories/{categoryId}", dto);

            if (res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(json)
                ? "Unable to update category"
                : JsonDocument.Parse(json).RootElement.GetProperty("message").GetString();
        }


        // =========================
        // Admin – Activate / Deactivate Category
        // =========================

        public async Task<string?> ToggleActiveAsync(int categoryId)
        {
            var res = await _http.PostAsJsonAsync(
                "/api/admin/categories/activate", categoryId);

            if (res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(json)
                ? "Unable to update category status"
                : JsonDocument.Parse(json).RootElement.GetProperty("message").GetString();
        }




    }
}
