using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrbanNative.Api.Services;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.UseCases.Vendors;
using UrbanNative.Infrastructure;
using UrbanNative.Infrastructure.Caching;
using UrbanNative.Infrastructure.Database;
using UrbanNative.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// =======================
// MVC + Swagger
// =======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

// =======================
// Database
// =======================
builder.Services.AddSingleton<SqlConnectionFactory>();

// =======================
// Repositories
// =======================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IVariantMasterRepository, VariantMasterRepository>();
builder.Services.AddScoped<IVariantValueRepository, VariantValueRepository>();
builder.Services.AddScoped<IProductVariantSetRepository, ProductVariantSetRepository>();
builder.Services.AddScoped<IProductVariantValuesRepository, ProductVariantValuesRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminNotificationRepository, AdminNotificationRepository>();
builder.Services.AddScoped<VendorChangePasswordUseCase>();

// =======================
// Services
// =======================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IVariantMasterService, VariantMasterService>();
builder.Services.AddScoped<IVariantValueService, VariantValueService>();
builder.Services.AddScoped<IProductVariantSetService, ProductVariantSetService>();
builder.Services.AddScoped<IProductVariantValuesService, ProductVariantValuesService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

// =======================
// Infrastructure
// =======================
builder.Services.AddInfrastructure(builder.Configuration);

// =======================
// JWT Authentication
// =======================
var jwt = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwt["Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// =======================
// Cache Loader (Preload)
// =======================
builder.Services.AddScoped<VariantMasterCacheLoader>();
builder.Services.AddSingleton<VariantMasterCacheLoader>();


/* at time of Change password I have changed this code to below block
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var loader = scope.ServiceProvider.GetRequiredService<VariantMasterCacheLoader>();
    var cache = await loader.LoadAsync();
    builder.Services.AddSingleton(cache);
}

*/




// =======================
// CORS (Admin + Vendor)
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("DashboardCors", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:5145", // Admin UI
                "https://localhost:7116"  // Vendor UI
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// =======================
// Build App
// =======================
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var loader = scope.ServiceProvider.GetRequiredService<VariantMasterCacheLoader>();
    var cache = await loader.LoadAsync();
    // store cache somewhere static or in IMemoryCache
}

// =======================
// Middleware Pipeline (ORDER IS CRITICAL)
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();              // 🔥 REQUIRED

app.UseCors("DashboardCors");

app.UseAuthentication();       // 🔐 JWT
app.UseAuthorization();

app.MapControllers();          // 🔥 REQUIRED



app.Run();
