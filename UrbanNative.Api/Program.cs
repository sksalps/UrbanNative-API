using UrbanNative.Infrastructure.Database;
using UrbanNative.Infrastructure.Repositories;
using UrbanNative.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database factory
builder.Services.AddSingleton<SqlConnectionFactory>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IVendorService, VendorService>();

builder.Services.AddScoped<IVariantMasterService, VariantMasterService>();
builder.Services.AddScoped<IVariantValueService, VariantValueService>();
builder.Services.AddScoped<IProductVariantSetService, ProductVariantSetService>();
builder.Services.AddScoped<IProductVariantValuesService, ProductVariantValuesService>();

builder.Services.AddScoped<IVariantMasterRepository, VariantMasterRepository>();
builder.Services.AddScoped<IVariantValueRepository, VariantValueRepository>();
builder.Services.AddScoped<IProductVariantSetRepository, ProductVariantSetRepository>();
builder.Services.AddScoped<IProductVariantValuesRepository, ProductVariantValuesRepository>();

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();

var app = builder.Build();   // ✔ Build only once

// Configure HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();



