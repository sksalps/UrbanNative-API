using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Customers.Security;
using UrbanNative.Customers.Services;
using UrbanNative.Customers.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Razor Pages
// =======================
builder.Services.AddRazorPages();

// =======================
// IHttpContextAccessor
// =======================
builder.Services.AddHttpContextAccessor();

builder.Services.AddMvc();

// =======================
// JWT → Cookie bridge
// =======================
builder.Services.AddTransient<JwtTokenHandler>();

// =======================
// 🔐 Customer Cookie Auth
// =======================
builder.Services.AddAuthentication("CustomerCookie")
    .AddCookie("CustomerCookie", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.Cookie.Name = "UrbanNative.Customer";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;

        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // enable in prod
    });


// =======================
// 🔒 Customer Authorization
// =======================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomerOnly", policy =>
        //policy.RequireAuthenticatedUser())
        policy.RequireAuthenticatedUser().RequireRole("Customer"));
});


// Protect Dashboard
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Dashboard", "CustomerOnly");
});



// =======================
// 🌐 API HttpClient
// =======================
builder.Services.AddHttpClient("ApiClient", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
        baseUrl = "https://localhost:5127"; // same API

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(100);
})
.AddHttpMessageHandler<JwtTokenHandler>();





// =======================
// 🧩 Customer Services
// =======================
builder.Services.AddScoped <CustomerAuthService>();
builder.Services.AddScoped<IAddressEngineService, CustomerAddressService>();
builder.Services.AddScoped<ICustomerDashboardService, CustomerDashboardService>();


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

// Default route → Login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Auth/Login");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();