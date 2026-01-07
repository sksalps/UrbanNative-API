//Admin project for UrbanNative application Program.cs
using UrbanNative.Admin.Security;
using UrbanNative.Admin.Services;
using UrbanNative.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();

// make IHttpContextAccessor available to views/partials
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtTokenHandler>();


// Authentication: cookie for Admin Razor UI
builder.Services.AddAuthentication("AdminCookie")
    .AddCookie("AdminCookie", options =>
    {
        // if your Login page is at /Login (root Pages/Login.cshtml), keep it as below
        options.LoginPath = "/Login";
        options.Cookie.Name = "UrbanNative.Admin";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // enable in prod
    });


// Authorization policy for Admin
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Admin"));
});
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Products", "AdminOnly");
});

builder.Services.AddTransient<JwtTokenHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
        baseUrl = "https://localhost:5127";

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<JwtTokenHandler>();



// register your domain services
builder.Services.AddScoped<IAdminService, AdminService>();
// register AdminProductService which implements IAdminProductService
builder.Services.AddScoped<IAdminProductService, AdminProductService>();
// register AdminVendorService which implements IAdminVendorService
builder.Services.AddScoped<IAdminVendorService, AdminVendorService>();
// register AdminCategoryService which implements IAdminCategoryService
builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
builder.Services.AddScoped<IAdminVariantService, AdminVariantService>();
builder.Services.AddScoped<IAdminVariantSetService, AdminVariantSetService>();
builder.Services.AddScoped<AdminGSTService>();
builder.Services.AddScoped<IAdminGSTService, AdminGSTService>();
builder.Services.AddScoped<IAdminHSNService, AdminHSNService>();
builder.Services.AddScoped<IAdminCategoryHSNService, AdminCategoryHSNService>();
builder.Services.AddScoped<IAdminInventoryService, AdminInventoryService>();
builder.Services.AddScoped<IAdminInventoryLogService, AdminInventoryLogService>();
builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();

// register SqlHelpers example


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();   // must be before UseAuthorization
app.UseAuthorization();

// map razor pages (Admin pages are in /Pages)
app.MapRazorPages();

// If you also have controllers for API, keep MapControllers() here:
// app.MapControllers();

app.Run();