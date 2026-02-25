using System.Net.Http.Headers;
using UrbanNative.Application.Interfaces;
using UrbanNative.Vendors.Security;
using UrbanNative.Vendors.Services;
using UrbanNative.Vendors.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Make IHttpContextAccessor available
builder.Services.AddHttpContextAccessor();

// JWT → Cookie bridge (same pattern as Admin)
builder.Services.AddTransient<JwtTokenHandler>();

// =======================
// 🔐 Vendor Cookie Auth
// =======================
builder.Services.AddAuthentication("VendorCookie")
    .AddCookie("VendorCookie", options =>
    {
        options.LoginPath = "/Login";
        options.Cookie.Name = "UrbanNative.Vendor";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // enable in prod
    });


// =======================
// 🔒 Vendor Authorization
// =======================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("VendorOnly", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Vendor"));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Dashboard", "VendorOnly");
});


// =======================
// 🌐 API HttpClient
// =======================
builder.Services.AddHttpClient("ApiClient", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
        baseUrl = "https://localhost:5127";   // same API as Admin

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<JwtTokenHandler>();



// =======================
// 🧩 Vendor Services
// =======================
builder.Services.AddScoped<IVendorAuthService, VendorAuthService>();
builder.Services.AddScoped<IVendorDashboardService, VendorDashboardService>();
builder.Services.AddScoped<IVendorProfileService, VendorProfileService>();
builder.Services.AddScoped<IVendorSettingsService, VendorSettingsService>();
builder.Services.AddScoped<IVendorProductService,VendorProductService>();
builder.Services.AddScoped<VendorSkuService, VendorSkuService>();
builder.Services.AddScoped<IVendorInventoryService, VendorInventoryService>();
builder.Services.AddScoped<ISkuFilterService, SkuFilterService>();
builder.Services.AddScoped<IVendorAddInventoryService, VendorAddInventoryService>();
builder.Services.AddScoped<IVendorOrderService, VendorOrderService>();
builder.Services.AddScoped<IVendorLogisticsService, VendorLogisticsService>();
builder.Services.AddScoped<IVendorWalletService, VendorWalletService>();
builder.Services.AddScoped<IVendorSettlementPayoutService, VendorSettlementPayoutService>();
builder.Services.AddScoped<IVendorComplianceService, VendorComplianceService>();
// Later: Orders, Inventory, Wallet, SKUs, etc.


// =======================
// Pipeline
// =======================
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();
