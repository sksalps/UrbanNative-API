
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using UrbanNative.Admin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();

// make IHttpContextAccessor available to views/partials
builder.Services.AddHttpContextAccessor();

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

builder.Services.AddHttpClient("ApiClient", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        // fallback - for safety during dev
        baseUrl = "https://localhost:5128";
    }
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// register your domain services
builder.Services.AddScoped<IAdminService, AdminService>();
// register SqlHelpers example


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
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