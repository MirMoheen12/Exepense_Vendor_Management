using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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
    options.CallbackPath = "/AccountsSide/ExternalLoginCallBack/";
    options.SignedOutCallbackPath = "/AccountsSide/Logout";
    options.TokenValidationParameters.ValidateIssuer = false;
    options.SignInScheme = IdentityConstants.ExternalScheme;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
    pattern: "{controller=Accounts}/{action=Login}/{id?}");

app.Run();
