
using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using Exepense_Vendor_Management.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using NuGet.Protocol.Core.Types;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IVendor, VendorRepo>();
builder.Services.AddTransient<IMedia, MediaRepo>();
builder.Services.AddDbContextPool<AppDbContext>(item => item.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().
                AddDefaultUI().
                  AddDefaultTokenProviders().
               AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication().AddOpenIdConnect("AzureAD", "Azure AD", options =>
{
    options.ClientId = builder.Configuration["Authentication2:AzureAD:ClientId"];
    options.Authority = "https://login.microsoftonline.com/9a97e28b-a403-4c26-a921-a5b40f2bbcb4"; // Replace with your Azure AD tenant ID
    options.ClientSecret = builder.Configuration["Authentication2:AzureAD:ClientSecret"];
    options.ResponseType = "code";
    options.CallbackPath = "/Accounts/ExternalLoginCallBack/";
    options.SignedOutCallbackPath = "/Accounts/Logout";
    options.TokenValidationParameters.ValidateIssuer = false;
    options.SignInScheme = IdentityConstants.ExternalScheme;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    IdentityModelEventSource.ShowPII = true;
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EmployeeExpense}/{action=Index}/{id?}");

app.Run();
