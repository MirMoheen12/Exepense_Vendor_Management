using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Expense_Vendor_Management.Repositories;
using System.Xml.Linq;
using Exepense_Vendor_Management.Repositories;
using Exepense_Vendor_Management.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().

                AddDefaultTokenProviders().
             AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddControllersWithViews(opt => {

    var policy = new AuthorizationPolicyBuilder()
               .RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
    opt.Filters.Add(new RequestSizeLimitAttribute(100 * 1024 * 1024)); // 100MB

});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});
builder.Services.AddAuthentication().AddOpenIdConnect("AzureAD", "Azure AD", options =>
{
    options.ClientId = builder.Configuration["Authentication2:AzureAD:ClientId"];
    options.Authority = "https://login.microsoftonline.com/"+builder.Configuration["Authentication2:AzureAD:Authority"];
    options.ClientSecret = builder.Configuration["Authentication2:AzureAD:ClientSecret"];
    options.ResponseType = "code";
    options.CallbackPath = "/Accounts/ExternalLoginCallBack/";
    options.SignedOutCallbackPath = "/Accounts/Logout";
    options.TokenValidationParameters.ValidateIssuer = false;
    options.SignInScheme = IdentityConstants.ExternalScheme;
});
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();


builder.Services.AddTransient<IVendor, VendorRepo>();
builder.Services.AddTransient<IMedia, MediaRepo>();
builder.Services.AddTransient<IExpense, ExpenseRepo>();
builder.Services.AddTransient<ICostExp, CostExpenseRepo>();
builder.Services.AddTransient<ILogs, LogRepo>();
builder.Services.AddTransient<IUser, UserRepo>();
builder.Services.AddTransient<IDashboard, DashboardRepo>();
builder.Services.AddTransient<IDashboard, DashboardRepo>();
builder.Services.AddTransient<ISharePoint, SharePointRepo>();
builder.Services.AddTransient<ICommentSide, CommentsRepo>();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews(opt => {

    var policy = new AuthorizationPolicyBuilder()
               .RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/UniversalError");
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
    //app.UseStatusCodePagesWithReExecute("/Home/UniversalError", "?statusCode={0}");
}
else
{
    app.UseExceptionHandler("/Home/UniversalError");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}");

app.Run();