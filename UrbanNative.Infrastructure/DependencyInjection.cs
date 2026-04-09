using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//Repostitory Interfaces
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCase.Logistics;
//Interfaces UseCases
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCases.VendorInventoryAdd;
using UrbanNative.Application.Interfaces.UseCases.Wallet;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Application.Interfaces.Vendors.InventoryAdd;
using UrbanNative.Application.Interfaces.Vendors.Orders;
using UrbanNative.Application.Interfaces.Vendors.Wallet;
// UseCases Implementation
using UrbanNative.Application.UseCase;
using UrbanNative.Application.UseCase.CommonCroshDashboard.Compliance;
using UrbanNative.Application.UseCase.CommonCrossDashboard;
using UrbanNative.Application.UseCase.Vendors;
using UrbanNative.Application.UseCase.Vendors.Logistics;
using UrbanNative.Application.UseCase.Vendors.Orders;
using UrbanNative.Application.UseCase.Vendors.VendorInventoryAdd;
using UrbanNative.Application.UseCase.Vendors.Wallet;
using UrbanNative.Application.UseCases.Vendors;

// Repositories Implementation
using UrbanNative.Infrastructure.Repositories;
using UrbanNative.Infrastructure.Repositories.CommonCrossDashboard;
using UrbanNative.Infrastructure.Repositories.CommonCrossDashboard.Compliance;
using UrbanNative.Infrastructure.Repositories.Vendors;
using UrbanNative.Infrastructure.Repositories.Vendors.InventoryAdd;
using UrbanNative.Infrastructure.Repositories.Vendors.Orders;
using UrbanNative.Infrastructure.Repositories.Vendors.Wallet;
using UrbanNative.Infrastructure.Repository;
using UrbanNative.Infrastructure.Services;
using UrbanNative.Infrastructure.Services.OcrService;

namespace UrbanNative.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminNotificationRepository, AdminNotificationRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAdminVendorRepository, AdminVendorRepository>();
            services.AddScoped<IAdminCategoryRepository, AdminCategoryRepository>();
            services.AddScoped<IAdminVariantRepository, AdminVariantRepository>();
            services.AddScoped<IAdminVariantSetRepository, AdminVariantSetRepository>();
            services.AddScoped<IAdminSkuRepository, AdminSkuRepository>();
            services.AddScoped<IAddressRepository, XXXAddressRepository>();
            services.AddScoped<IAdminGSTRepository, AdminGSTRepository>();
            services.AddScoped<IAdminHSNRepository, AdminHSNRepository>();
            services.AddScoped<IAdminCategoryHSNRepository, AdminCategoryHSNRepository>();
            services.AddScoped<IAdminInventoryRepository, AdminInventoryRepository>();
            services.AddScoped<IAdminInventoryLogRepository, AdminInventoryLogRepository>();
            services.AddScoped<IAdminOrderRepository, AdminOrderRepository>();
            services.AddScoped<IAdminReturnsRepository, AdminReturnsRepository>();
            services.AddScoped<IAdminWalletRepository, AdminWalletRepository>();
            services.AddScoped<IVendorAuthRepository, VendorAuthRepository>();
            services.AddScoped<IVendorDashboardRepository, VendorDashboardRepository>();
            //services.AddScoped<IVendorLoginRepository, VendorLoginRepository>();
            services.AddScoped<IVendorProfileRepository, VendorProfileRepository>();
            services.AddScoped<IVendorWalletRepository, VendorWalletRepository>();
            services.AddScoped<ISettlementPayoutRepository, SettlementPayoutRepository>();
            services.AddScoped<IComplianceRepository, ComplianceRepository>();
            services.AddScoped<IVendorWarehouseRepository, VendorWarehouseRepository>();
            services.AddScoped<IBankRepository,BankRepository>();
            services.AddScoped<IAddressEngineRepository, AddressEngineRepository>();
            
            // UseCases
            services.AddScoped<IVendorProfileUseCase, VendorProfileUseCase>();

            services.AddScoped<IVendorSettingsUseCase, VendorSettingsUseCase>();
            services.AddScoped<IVendorProductsUseCase, VendorProductsUseCase>();
            services.AddScoped<IVendorSkuUseCase, VendorSkuUseCase>();
            services.AddScoped<IVendorInventoryUseCase, VendorInventoryUseCase>();
            services.AddScoped<IAddInventoryInUseCase, AddInventoryInUseCase>();
            services.AddScoped<IVendorOrderUseCase, VendorOrderUseCase>();
            services.AddScoped<IVendorLogisticsUseCase, VendorLogisticsUseCase>();
            services.AddScoped<IVendorWalletUseCase, VendorWalletUseCase>();
            services.AddScoped<ISettlementPayoutUseCase, SettlementPayoutUseCase>();
            services.AddScoped<IComplianceUseCase, ComplianceUseCase>();
            services.AddScoped<IVendorChangePasswordUseCase, VendorChangePasswordUseCase>();
            services.AddScoped<IOcrComplianceUseCase, OcrComplianceUseCase>();
            services.AddScoped<IVendorWarehouseUseCase, VendorWarehouseUseCase>();
            services.AddScoped<IAddressEngineUseCase, AddressEngineUseCase>();

            //services.AddHttpClient<OcrSpaceService>();
            //services.AddHttpClient<TesseractCliService>();
            services.AddHttpClient<IOcrService, OcrService>();
            
            services.AddScoped<IBankUseCase, BankUseCase>();
            // Repositories
            services.AddScoped<IVendorSettingsRepository, VendorSettingsRepository>();
            services.AddScoped<IVendorProductRepository, VendorProductRepository>();
            services.AddScoped<IVendorSkuRepository, VendorSkuRepository>();
            services.AddScoped<IVendorInventoryRepository, VendorInventoryRepository>();
            services.AddScoped<ISkuFilterRepository, SkuFilterRepository>();
            services.AddScoped<IInventoryAddRepository, InventoryAddRepository>();
            services.AddScoped<IVendorOrderRepository, VendorOrderRepository>();
            services.AddScoped<IVendorLogisticsRepository, VendorLogisticsRepository>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IOcrComplianceInfraService, OcrComplianceInfraService>();
            return services;
        }
    }
}

