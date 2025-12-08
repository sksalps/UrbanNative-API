namespace UrbanNative.Infrastructure.Database
{
    public interface IConfiguration
    {
        string? GetConnectionString(string v);
    }
}