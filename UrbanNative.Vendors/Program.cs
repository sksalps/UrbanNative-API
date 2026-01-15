using UrbanNative.Application.Interfaces;
using UrbanNative.Vendors.Security;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Razor Pages
// =======================
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// JWT → Cookie bridge
builder.Services.AddTransient<JwtTokenHandler>();


// =======================
// 🔐 Vendor Cookie Auth (ONE scheme only)
// =======================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "VendorCookie";
    options.DefaultSignInScheme = "VendorCookie";
    options.DefaultChallengeScheme = "VendorCookie";
})
.AddCookie("VendorCookie", options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Login";
    options.Cookie.Name = "UrbanNative.Vendor";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});


// =======================
// 🔒 Vendor Authorization
// =======================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("VendorOnly", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Vendor"));
});


// =======================
// Razor authorization
// =======================
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
        baseUrl = "https://localhost:5127";

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<JwtTokenHandler>();


// =======================
// 🧩 Vendor Services
// =======================
//builder.Services.AddScoped<IVendorAuthService, VendorAuthService>();
//builder.Services.AddScoped<IVendorDashboardService, VendorDashboardService>();
//builder.Services.AddScoped<IVendorService, VendorService>();


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
