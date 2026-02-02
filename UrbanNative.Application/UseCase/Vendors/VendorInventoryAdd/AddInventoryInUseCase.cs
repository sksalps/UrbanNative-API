using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.UseCases.VendorInventoryAdd;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Application.Interfaces.Vendors.InventoryAdd;
using UrbanNative.Domain.Exceptions;


namespace UrbanNative.Application.UseCase.Vendors.VendorInventoryAdd
{
    public class AddInventoryInUseCase : IAddInventoryInUseCase
    {
        private readonly IInventoryAddRepository _inventoryRepo;
        private readonly IVendorProductRepository _productRepo;
        private readonly ISkuFilterRepository _skuFilterRepo;

        public AddInventoryInUseCase(
            IInventoryAddRepository inventoryRepo,
            IVendorProductRepository productRepo,
            ISkuFilterRepository warehouseRepo)
        {
            _inventoryRepo = inventoryRepo;
            _productRepo = productRepo;
            _skuFilterRepo = warehouseRepo;
        }

        // ======================================================
        // GET Single SKUS FOR ADDING INVENTORY
        // ======================================================
        public async Task<SkuInventoryStockSummaryDto> ExecuteAsyncSummary(
        int vendorId,
        int skuId,
        int warehouseId)
        {
            // ===============================
            // SKU VALIDATION
            // ===============================
            var sku = await _skuFilterRepo.GetSkuContextAsync(skuId, vendorId);
            if (sku == null)
                return new SkuInventoryStockSummaryDto();

            // ===============================
            // WAREHOUSE VALIDATION (OPTIONAL)
            // ===============================
            if (warehouseId > 0)
            {
                var warehouse = await _skuFilterRepo.GetWarehouseByIdAsync(warehouseId);

                // invalid warehouse OR warehouse not belonging to vendor
                if (warehouse == null || warehouse.VendorId != vendorId)
                    return new SkuInventoryStockSummaryDto();
            }

            // ===============================
            // FETCH SUMMARY
            // ===============================
            return await _inventoryRepo.GetSkuStockSummaryAsync(
                vendorId,
                sku.ProductId,
                skuId,
                warehouseId
            );

        }
        public async Task<AddInventoryResultDto> ExecuteAsyncAddStockSKU(int vendorId,
        int skuId,
        int warehouseId,
        int quantity,
        string? remarks)
        {
            // ===============================
            // BASIC GUARD
            // ===============================
            if (quantity <= 0)
                return Fail("Quantity must be greater than zero.");

            // ===============================
            // SKU VALIDATION + PRODUCT RESOLVE
            // ===============================
            var sku = await _skuFilterRepo.GetSkuContextAsync(skuId, vendorId);
            if (sku == null)
                return Fail("Invalid or inactive SKU.");

            // ===============================
            // WAREHOUSE VALIDATION
            // ===============================
            var warehouseValid =
                await _skuFilterRepo.GetWarehouseByIdAsync(warehouseId);

            if (warehouseValid.VendorId!=vendorId)
                return Fail("Invalid warehouse selection.");

            // ===============================
            // BUILD TVP (SINGLE SKU)
            // ===============================
            var items = new List<ProductInventoryInItemDto>
            {
                new ProductInventoryInItemDto
                {
                    SKUId = skuId,
                    Quantity = quantity
                }
            };

            // ===============================
            // EXECUTE SP
            // ===============================
            await _inventoryRepo.AddProductInventoryInAsync(
                vendorId,
                sku.ProductId,
                warehouseId,
                items,
                remarks);

            return new AddInventoryResultDto
            {
                Success = true,
                Message = "Inventory added successfully."
            };
        }

        private static AddInventoryResultDto Fail(string message) => new()
            {
                Success = false,
                Message = message
            };


        //===============End GET  Single SKUS FOR ADDING INVENTORY=================

        public async Task<IReadOnlyList<ProductAddInventorySkuGridDto>> ExecuteAsync(int vendorId, int productId, int warehouseId)
        {
            return await _inventoryRepo          .GetProductInventorySkusAsync(
                    vendorId,
                    productId,
                    warehouseId);
        }


        public async Task<InventoryInResponseDto> ExecuteAsync(int vendorId,ProductInventoryInRequestDto request)
        {
            if (request == null)
                throw new DomainValidationException("Invalid request");

            if (request.Items == null || !request.Items.Any())
                throw new DomainValidationException("No inventory items provided");
            if (!request.ProductId.HasValue)
                throw new DomainValidationException("Product is required");

            if (!request.WarehouseId.HasValue)
                throw new DomainValidationException("Warehouse is required");
            int productId = request.ProductId.Value;
            int warehouseId = request.WarehouseId.Value;


            // Normalize remarks
            request.Remarks = string.IsNullOrWhiteSpace(request.Remarks)
                ? null
                : request.Remarks.Trim();

            if (request.Remarks?.Length > 255)
                throw new DomainValidationException("Remarks cannot exceed 255 characters");

            // Validate warehouse
            var warehouse = await _skuFilterRepo.GetWarehouseByIdAsync(warehouseId);
            if (warehouse == null ||
                warehouse.VendorId != vendorId ||
                !warehouse.IsActive)
                throw new DomainValidationException("Invalid warehouse selected");

            // Validate product
            var product = await _productRepo.GetProductForEditAsync(
                vendorId, productId);

            if (product == null)
                throw new DomainValidationException("Invalid product selection");

            // Filter valid quantities
            var validItems = request.Items
                .Where(x => x.Quantity > 0)
                .ToList();

            if (!validItems.Any())
                throw new DomainValidationException("Enter quantity for at least one SKU");

            // Load SKU snapshot
            var skuSnapshots = await _inventoryRepo.GetProductInventorySkusAsync(vendorId, productId, warehouseId);

            foreach (var item in validItems)
            {
                var sku = skuSnapshots.FirstOrDefault(x => x.SKUId == item.SKUId);

                if (sku == null)
                    throw new DomainValidationException("Invalid SKU detected");

                //if (!sku.IsActive)
                 //   throw new DomainValidationException("Inactive SKU cannot receive stock");

                if (!sku.IsInitiated)
                    throw new DomainValidationException("Initiate from SKU Entry");
            }

            await _inventoryRepo.AddProductInventoryInAsync(
                vendorId,
                productId,
                warehouseId,
                validItems,
                request.Remarks);

            return new InventoryInResponseDto
            {
                Success = true,
                Message = "Inventory added successfully",
                ProductId = productId,
                TotalSkusProcessed = validItems.Count,
                TotalQuantityAdded = validItems.Sum(x => x.Quantity ?? 0)

            };
        }

    }

}
