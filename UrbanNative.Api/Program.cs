using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrbanNative.Api.Services;
using UrbanNative.Application.GlobalCall.VariantValueSignature;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure;
using UrbanNative.Infrastructure.Caching;
using UrbanNative.Infrastructure.Database;
using UrbanNative.Infrastructure.Repositories;




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
builder.Services.AddScoped<IAdminNotificationRepository, AdminNotificationRepository>();

// register loader
builder.Services.AddScoped<VariantMasterCacheLoader>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

var jwtKey = jwtSettings["Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception("JWT Key is missing in appsettings.json (JwtSettings:Key)");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddCookie(options =>
    {
        options.Cookie.Name = "UrbanNative.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;   // 🔑 REQUIRED
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 🔑 REQUIRED
    })

    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };


        // 🔥 THIS IS THE KEY FIX
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Read JWT from cookie instead of header
                if (context.Request.Cookies.ContainsKey("jwt"))
                {
                    context.Token = context.Request.Cookies["jwt"];
                }
                return Task.CompletedTask;
            }
        };
    });

// build provider temporarily
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var loader = scope.ServiceProvider
        .GetRequiredService<VariantMasterCacheLoader>();

    var cache = await loader.LoadAsync();

    builder.Services.AddSingleton(cache);
}
// decoder
builder.Services.AddSingleton<IVariantSignatureDecoder, VariantSignatureDecoder>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AdminCors", policy =>
    {
        policy
            .WithOrigins("https://localhost:5145") // Admin UI
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // 🔑 REQUIRED
    });
});


builder.Services.AddAuthorization();


var app = builder.Build();   // ✔ Build only once 
app.UseCors("AdminCors");
app.UseAuthentication();   // ⬅️ MUST COME FIRST
app.UseAuthorization();

app.MapControllers();

// Configure HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();







