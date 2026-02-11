using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.UseCases.VendorInventoryAdd;
using UrbanNative.Application.Interfaces.UseCase.Logistics;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Application.Interfaces.Vendors.InventoryAdd;
using UrbanNative.Application.Interfaces.Vendors.Orders;


using UrbanNative.Application.UseCase;
using UrbanNative.Application.UseCase.Vendors.VendorInventoryAdd;
using UrbanNative.Application.UseCases.Vendors;
using UrbanNative.Application.UseCase.Vendors.Orders;
using UrbanNative.Application.UseCase.Vendors.Logistics;
using UrbanNative.Infrastructure.Repositories;
using UrbanNative.Infrastructure.Repositories.CommonCrossDashboard;
using UrbanNative.Infrastructure.Repositories.Vendors;
using UrbanNative.Infrastructure.Repositories.Vendors.InventoryAdd;
using UrbanNative.Infrastructure.Repositories.Vendors.Orders;

using UrbanNative.Infrastructure.Repository;
using UrbanNative.Infrastructure.Services;

namespace UrbanNative.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminNotificationRepository, AdminNotificationRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAdminVendorRepository, AdminVendorRepository>();
            services.AddScoped<IAdminCategoryRepository, AdminCategoryRepository>();
            services.AddScoped<IAdminVariantRepository, AdminVariantRepository>();
            services.AddScoped<IAdminVariantSetRepository, AdminVariantSetRepository>();
            services.AddScoped<IAdminSkuRepository, AdminSkuRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
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
            services.AddScoped<IVendorProfileUseCase, VendorProfileUseCase>();
            services.AddScoped<IVendorProfileRepository, VendorProfileRepository>();
            

            // UseCases
            services.AddScoped<IVendorSettingsUseCase, VendorSettingsUseCase>();
            services.AddScoped<IVendorProductsUseCase, VendorProductsUseCase>();
            services.AddScoped<IVendorSkuUseCase, VendorSkuUseCase>();
            services.AddScoped<IVendorInventoryUseCase, VendorInventoryUseCase>();
            services.AddScoped<IAddInventoryInUseCase, AddInventoryInUseCase>();
            services.AddScoped<IVendorOrderUseCase, VendorOrderUseCase>();
            services.AddScoped<IVendorLogisticsUseCase, VendorLogisticsUseCase>();

            // Repositories
            services.AddScoped<IVendorSettingsRepository, VendorSettingsRepository>();
            services.AddScoped<IVendorChangePasswordUseCase, VendorChangePasswordUseCase>();
            services.AddScoped<IVendorProductRepository, VendorProductRepository>();
            services.AddScoped<IVendorSkuRepository, VendorSkuRepository>();
            services.AddScoped<IVendorInventoryRepository, VendorInventoryRepository>();
            services.AddScoped<ISkuFilterRepository, SkuFilterRepository>();
            services.AddScoped<IInventoryAddRepository, InventoryAddRepository>();
            services.AddScoped<IVendorOrderRepository, VendorOrderRepository>();
            services.AddScoped<IVendorLogisticsRepository, VendorLogisticsRepository>();
            return services;
        }
    }
}

