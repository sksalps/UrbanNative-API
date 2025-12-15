using Microsoft.Extensions.DependencyInjection;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminNotificationRepository, AdminNotificationRepository>();

            return services;
        }
    }
}
