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
using UrbanNative.Domain.Entities;
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
        // ========== Adjust Inventory for Single SKU ==========
        public async Task<AdjustInventoryResultDto> ExecuteAdjustAsync(AdjustInventoryRequestDto request,int vendorUserId, int productId)
        {
            // ===============================
            // Business Validations
            // ===============================
            if (request.SKUId <= 0)
                throw new DomainValidationException("ERR01", "Invalid SKU.");

            if (request.WarehouseId <= 0)
                throw new DomainValidationException("ERR01", "Invalid warehouse.");

            if (request.Quantity <= 0)
                throw new DomainValidationException("ERR01","Quantity must be greater than zero.");

            if (string.IsNullOrWhiteSpace(request.Reason))
                throw new DomainValidationException("ERR01", "Reason is required.");

            if (request.ChangeType != "IN" && request.ChangeType != "OUT")
                throw new DomainValidationException("ERR01", "Invalid ChangeType.");

            // ===============================
            // SKU INITIATION VALIDATION (AUTHORITATIVE)
            // =============================== 
            //var summary = await _inventoryRepo.GetSkuStockSummaryAsync(            vendorId,        sku.ProductId,  skuId,           warehouseId;
            var summary = await _inventoryRepo.GetSkuStockSummaryAsync(vendorUserId,productId,  request.SKUId.Value, request.WarehouseId.Value);
            if (!summary.IsInitiated)
                throw new DomainValidationException("ERR01",
                    "SKU is not initiated. Please complete SKU entry before adding inventory."
                );
            // ===============================
            // Execute Repository (SP)
            // ===============================
            var result = await _inventoryRepo.AdjustInventoryAsync(
                request.SKUId,
                request.WarehouseId,
                request.ChangeType,
                request.Quantity,
                request.Reason,
                vendorUserId
            );

            if (result == null)
                throw new DomainValidationException("ERR01", "Inventory adjustment failed.");

            return result;
        }

        // ======================================================
        // GET Single SKUS FOR ADDING INVENTORY
        // ======================================================
        public async Task<SkuInventoryStockSummaryDto> ExecuteAsyncSummary(
    int vendorId,
    int skuId,
    int warehouseId)
        {
            try
            {
                // ===============================
                // SKU VALIDATION (DOMAIN)
                // ===============================
                var sku = await _skuFilterRepo.GetSkuContextAsync(skuId, vendorId);
                if (sku == null)
                    throw new DomainValidationException("ERR01",
                        "Invalid or inactive SKU."
                    );

                // ===============================
                // WAREHOUSE VALIDATION (DOMAIN)
                // ===============================
                if (warehouseId > 0)
                {
                    var warehouse = await _skuFilterRepo.GetWarehouseByIdAsync(warehouseId);

                    if (warehouse == null || warehouse.VendorId != vendorId)
                        throw new DomainValidationException("ERR01",
                            "Invalid warehouse selection."
                        );
                }

                // ===============================
                // FETCH SUMMARY (INFRA CALL)
                // ===============================
                return await _inventoryRepo.GetSkuStockSummaryAsync(
                    vendorId,
                    sku.ProductId,
                    skuId,
                    warehouseId
                );
            }
            catch (DomainValidationException)
            {
                // ✅ business rule → bubble unchanged
                throw;
            }
            catch (Exception)
            {
                // 🚫 infra / sql / wrong SP / dapper / network
                throw new DomainValidationException("ERR01",
                    "Internal server error. Please try again."
                );
            }
        }


        public async Task<AddInventoryResultDto> ExecuteAsyncAddStockSKU(
    int vendorId,
    int skuId,
    int warehouseId,
    int quantity,
    string? remarks)
        {
            // ===============================
            // BASIC GUARD
            // ===============================
            if (quantity <= 0)
                throw new DomainValidationException("ERR01",
                    "Quantity must be greater than zero."
                );

            // ===============================
            // SKU VALIDATION + PRODUCT RESOLVE
            // ===============================
            var sku = await _skuFilterRepo.GetSkuContextAsync(skuId, vendorId);
            if (sku == null)
                throw new DomainValidationException("ERR01",
                    "Invalid or inactive SKU."   );

            // ===============================
            // SKU INITIATION VALIDATION (AUTHORITATIVE)
            // ===============================
            var summary = await _inventoryRepo.GetSkuStockSummaryAsync(
                vendorId,
                sku.ProductId,
                skuId,
                warehouseId
            );

            if (!summary.IsInitiated)
                throw new DomainValidationException("ERR01",
                    "SKU is not initiated. Please complete SKU entry before adding inventory."
                );

            // ===============================
            // WAREHOUSE VALIDATION
            // ===============================
            var warehouse = await _skuFilterRepo.GetWarehouseByIdAsync(warehouseId);

            if (warehouse == null || warehouse.VendorId != vendorId)
                throw new DomainValidationException("ERR01",
                    "Invalid warehouse selection."
                );

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
                remarks
            );

            return new AddInventoryResultDto
            {
                Success = true,
                Message = "Inventory Adjusted successfully."
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
                throw new DomainValidationException("ERR01", "Invalid request");

            if (request.Items == null || !request.Items.Any())
                throw new DomainValidationException("ERR1","No inventory items provided");
            if (!request.ProductId.HasValue)
                throw new DomainValidationException("ERR1", "Product is required");

            if (!request.WarehouseId.HasValue)
                throw new DomainValidationException("ERR1", "Warehouse is required");
            int productId = request.ProductId.Value;
            int warehouseId = request.WarehouseId.Value;


            // Normalize remarks
            request.Remarks = string.IsNullOrWhiteSpace(request.Remarks)
                ? null
                : request.Remarks.Trim();

            if (request.Remarks?.Length > 255)
                throw new DomainValidationException("ERR1", "Remarks cannot exceed 255 characters");

            // Validate warehouse
            var warehouse = await _skuFilterRepo.GetWarehouseByIdAsync(warehouseId);
            if (warehouse == null ||
                warehouse.VendorId != vendorId ||
                !warehouse.IsActive)
                throw new DomainValidationException("ERR1", "Invalid warehouse selected");

            // Validate product
            var product = await _productRepo.GetProductForEditAsync(
                vendorId, productId);

            if (product == null)
                throw new DomainValidationException("ERR1", "Invalid product selection");

            // Filter valid quantities
            var validItems = request.Items
                .Where(x => x.Quantity > 0)
                .ToList();

            if (!validItems.Any())
                throw new DomainValidationException("ERR1", "Enter quantity for at least one SKU");

            // Load SKU snapshot
            var skuSnapshots = await _inventoryRepo.GetProductInventorySkusAsync(vendorId, productId, warehouseId);

            foreach (var item in validItems)
            {
                var sku = skuSnapshots.FirstOrDefault(x => x.SKUId == item.SKUId);

                if (sku == null)
                    throw new DomainValidationException("ERR1", "Invalid SKU detected");

                //if (!sku.IsActive)
                 //   throw new DomainValidationException("Inactive SKU cannot receive stock");

                if (!sku.IsInitiated)
                    throw new DomainValidationException("ERR1", "Initiate from SKU Entry");
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
