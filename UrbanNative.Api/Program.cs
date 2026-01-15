using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrbanNative.Api.Services;
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
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;


// Database factory
builder.Services.AddSingleton<SqlConnectionFactory>();


// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();

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


// build provider temporarily
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var loader = scope.ServiceProvider
        .GetRequiredService<VariantMasterCacheLoader>();

    var cache = await loader.LoadAsync();

    builder.Services.AddSingleton(cache);
}


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









