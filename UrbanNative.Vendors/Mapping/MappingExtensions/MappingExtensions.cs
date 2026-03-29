using AutoMapper;
using System.Reflection;

namespace UrbanNative.Vendors.Mapping
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddVendorMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}