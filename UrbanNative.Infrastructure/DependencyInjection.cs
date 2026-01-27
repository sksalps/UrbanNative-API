using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.UseCases; 
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Application.UseCase;
using UrbanNative.Application.UseCases.Vendors;
using UrbanNative.Infrastructure.Repositories;
using UrbanNative.Infrastructure.Repositories.CommonCrossDashboard;
using UrbanNative.Infrastructure.Repositories.Vendors;
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

            // Repositories
            services.AddScoped<IVendorSettingsRepository, VendorSettingsRepository>();
            services.AddScoped<IVendorChangePasswordUseCase, VendorChangePasswordUseCase>();
            services.AddScoped<IVendorProductRepository, VendorProductRepository>();
            services.AddScoped<IVendorSkuRepository, VendorSkuRepository>();
            services.AddScoped<IVendorInventoryRepository, VendorInventoryRepository>();
            services.AddScoped<ISkuFilterRepository, SkuFilterRepository>();

            return services;
        }
    }
}

