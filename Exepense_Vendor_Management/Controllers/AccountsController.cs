using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Expense_Vendor_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;
using Azure.Identity;
using System.Text.Json;
using Newtonsoft.Json;
using Exepense_Vendor_Management.Models;
using System.Configuration;
using Exepense_Vendor_Management.Interfaces;
using AngleSharp.Css;

namespace Expense_Vendor_Management.Controllers
{
    [AllowAnonymous]
    public class AccountsController : Controller
    {

        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> UserManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUser user;
        private readonly IConfiguration configuration;
        public AccountsController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> _roleManager,IConfiguration configuration,IUser user)
        {
           
            this.signInManager = signInManager;
            this.UserManager = userManager;
            this.configuration = configuration;
            this._roleManager = _roleManager;
            this.user = user;
        }

        public IActionResult Index()
        {

            return View();
        }
     
        public async Task<IActionResult> CreateRole()
        {
            await CreateRoles();
            return View();
        }



        public async Task<IActionResult> Login(string returnUrl)
        {
            await signInManager.SignOutAsync();
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Externallogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };
            return View(loginViewModel);

        }
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUri = Url.Action("ExternalLoginCallBack", "Accounts", new { RetunrUrl = returnUrl });
            var prop = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            return new ChallengeResult(provider, prop);
        }
        public async Task<IActionResult> ExternalLoginCallBack(string? RetunrUrl = null, string? remoteError = null)
        {
            RetunrUrl = RetunrUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = RetunrUrl,
                Externallogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };
            if (remoteError != null)
            {
                ModelState.AddModelError(String.Empty, $"Error from External Provide:{remoteError}");
                return View("Login", loginViewModel);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(String.Empty, $"Error from External Login info:{remoteError}");
                return View("Login", loginViewModel);
            }
            var signinresult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
            if (signinresult.Succeeded)
            {
                try
                {
                    var email = info.Principal.FindFirstValue(ClaimTypes.Upn);
                    var user = await UserManager.FindByEmailAsync(email);
                    var data = getInfo(email).Result;
                    if (data != null)
                    {
                        if (data.DisplayName != null)
                        {
                            user.UserName=data.DisplayName;
                            var r = await UserManager.UpdateAsync(user);
                        }
                        var defaultrole = _roleManager.FindByNameAsync(data.RolesForVendorAndExpenseMgt).Result;
                        var roleresult = await UserManager.AddToRoleAsync(user, defaultrole.Name);
                        var stat = await UserManager.GetRolesAsync(user);
                        await UserManager.AddLoginAsync(user, info);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }

                catch(Exception ex)
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Upn);
                if (email != null)
                {
                    var user = await UserManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Upn),

                        };
                        await UserManager.CreateAsync(user);
                    }
                    var data = getInfo(user.Email).Result;
                    if (data != null)
                    {
                        if (data.DisplayName != null)
                        {
                            user.UserName = data.DisplayName;
                            var r = await UserManager.UpdateAsync(user);
                        }
                        var defaultrole = _roleManager.FindByNameAsync(data.RolesForVendorAndExpenseMgt).Result;
                        var roleresult = await UserManager.AddToRoleAsync(user, defaultrole.Name);
                        var stat = await UserManager.GetRolesAsync(user);
                        await UserManager.AddLoginAsync(user, info);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");

                    }
                    return RedirectToAction("Index", "Home");
                }
                email= info.Principal.FindFirstValue(ClaimTypes.Email);
                if(email!=null)
                {
                    var user = await UserManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),

                        };
                        await UserManager.CreateAsync(user);
                    }
                  
                    //change Email
                    var data= getInfo(user.Email).Result;
                    if (data != null)
                    {
                        
                        var defaultrole = _roleManager.FindByNameAsync(data.RolesForVendorAndExpenseMgt).Result;
                        var roleresult = await UserManager.AddToRoleAsync(user, defaultrole.Name);
                        var stat = await UserManager.GetRolesAsync(user);
                        await UserManager.AddLoginAsync(user, info);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");

                    }

                    return RedirectToAction("NoAccess", "Home");
                }
                else
                {
                    ViewBag.ErrorTtle = $"Email not Found:{info.LoginProvider}";
                    return View("Error");
                }
            }

        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Accounts");
        }
     


        private async Task CreateRoles()
        {
            string[] roles = { "Super Admin", "Accouting Team", "Divisional Manager", "Regional Manager", "Area Manager", "Branch Manager\r\n" }; // Add more roles as needed

            foreach (var role in roles)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task<CustomSecurityAttributeValue> getInfo(string Email)
        {
            try
            {
                
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                var options = new ClientSecretCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                };

                var clientSecretCredential = new ClientSecretCredential(
                        configuration.GetSection("Authentication2:AzureAD:Authority").Value, configuration.GetSection("Authentication2:AzureAD:ClientId").Value, configuration.GetSection("Authentication2:AzureAD:ClientSecret").Value, options);
                var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

                //var result1 = await graphClient.Users["consulting_personal_outlook.com#EXT#@NETORGFT11843804.onmicrosoft.com"].GetAsync();
                var result = await graphClient.Users[Email].GetAsync((requestConfiguration) =>
                {
                    requestConfiguration.QueryParameters.Select = new string[] { "customSecurityAttributes" };
                });
                var Username = await graphClient.Users[Email].GetAsync((requestConfiguration) =>
                {
                    requestConfiguration.QueryParameters.Select = new string[] { "displayName" };
                });
                if (result?.CustomSecurityAttributes != null)
                {
                    var element = result.CustomSecurityAttributes.AdditionalData["UniversalAttributesForRize"];                             //add this to app settings.
                    var jsonString = element.ToString();

                    var customSecurityAttribute = System.Text.Json.JsonSerializer.Deserialize<CustomSecurityAttributeValue>(jsonString);
                    List<string> CostCenter = customSecurityAttribute.CostCenter;                                                           // cost center for dashboard
                    var department = customSecurityAttribute.Department;                                                                    // Department
                    var role = customSecurityAttribute.RolesForVendorAndExpenseMgt;
                    customSecurityAttribute.DisplayName = Username.DisplayName;
                    return customSecurityAttribute;
                        //Roles for all purposes
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IActionResult NoRole()
        {

            return View();
        }
    }
}
