using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Repositories;
using UrbanNative.Infrastructure.Repository;
using UrbanNative.Infrastructure.Services;

namespace UrbanNative.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
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



            return services;
        }
    }
}

