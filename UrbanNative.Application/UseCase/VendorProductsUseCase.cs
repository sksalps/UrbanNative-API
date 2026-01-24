using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Application.UseCase
{
    public class VendorProductsUseCase : IVendorProductsUseCase
    {
        private readonly IVendorProductRepository _repo;

        public VendorProductsUseCase(IVendorProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<VendorProductListDto>> ExecuteAsync(int vendorId, string? search, int? categoryId, int? hsnId)
        {
            return await _repo.GetVendorProductsAsync(vendorId, search, categoryId, hsnId);
        }

        public Task<List<CategoryLookupDto>> GetVendorCategoriesAsync(int? vendorId, bool? isActive)
        => _repo.GetVendorCategoriesAsync(vendorId,isActive);

        public Task<List<HsnLookupDto>> GetVendorHsnListAsync()
            => _repo.GetVendorHsnListAsync();
        public Task<int> ExecuteAsync(int vendorId, VendorProductCreateDto dto)
        => _repo.CreateProductAsync(vendorId, dto);
        public async Task ExecuteAsync(int vendorId, VendorProductUpdateDto dto)
        {
            var existing = await _repo.GetProductForEditAsync(vendorId, dto.ProductID);

            if (existing.ApprovalStatus == "APPROVED"
                && existing.CategoryID != dto.CategoryID)
            {
                throw new InvalidOperationException(
                    "Category cannot be changed after approval."
                );
            }

            await _repo.UpdateProductAsync(vendorId, dto);
        }
        public Task<VendorProductEditDto> ExecuteAsync(int vendorId, int productId)
        => _repo.GetProductForEditAsync(vendorId, productId);
        public Task<List<VendorWarehouseDto>> ExecuteAsync(int? vendorId,bool?isActive)
        => _repo.GetVendorWarehousesAsync(vendorId,isActive);
        public Task<List<ReturnPolicyDto>> GetReturnPoliciesAsync(bool? isActive)
        => _repo.GetReturnPoliciesAsync(isActive);

        public Task<HsnLookupDto> GetHsnByCategoryAsync(int categoryId)
            => _repo.GetHsnByCategoryAsync(categoryId);

        public Task<VendorWarehouseDto> GetWarehouseByIdAsync(int id)
            => _repo.GetWarehouseByIdAsync(id);

    }
        


    }
