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

namespace Expense_Vendor_Management.Controllers
{
    [AllowAnonymous]
    public class AccountsController : Controller
    {

        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> UserManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountsController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> _roleManager)
        {
           
            this.signInManager = signInManager;
            this.UserManager = userManager;
            this._roleManager = _roleManager;
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
                    // Code snippets are only available for the latest version. Current version is 5.x
                    var scopes = new[] { "https://graph.microsoft.com/.default" };

                    // Values from app registration
                    var clientId = "76e5585d-f738-4163-82eb-a1651776b761";                                          //get this from app settings.
                    var tenantId = "711f2702-c004-4887-af6e-961f941df9ec";                                          //add this to app settings.
                    var clientSecret = "nP88Q~rAB_AZNpkBlbc.vmKetOS16UPjz51Sfdtj";                                  //add this from app settings.

                    // using Azure.Identity;
                    var options = new ClientSecretCredentialOptions
                    {
                        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                    };

                    var clientSecretCredential = new ClientSecretCredential(
                        tenantId, clientId, clientSecret, options);
                    var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

                    var result = await graphClient.Users["consulting@rizemtg.com"].GetAsync((requestConfiguration) =>                           //add current user email here
                    {
                        requestConfiguration.QueryParameters.Select = new string[] { "customSecurityAttributes" };                              
                    });
                    if(result?.CustomSecurityAttributes != null)
                    {
                        var element = result.CustomSecurityAttributes.AdditionalData["UniversalAttributesForRize"];                             //add this to app settings.
                        var jsonString = element.ToString();

                        var customSecurityAttribute = System.Text.Json.JsonSerializer.Deserialize<CustomSecurityAttributeValue>(jsonString);
                        List<string> CostCenter = customSecurityAttribute.CostCenter;                                                           // cost center for dashboard
                        var department = customSecurityAttribute.Department;                                                                    // Department
                        var role =customSecurityAttribute.RolesForVendorAndExpenseMgt;                                                          //Roles for all purposes
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
                    await UserManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);
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
                    await UserManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
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
        public class CustomSecurityAttributeValue
        {
            public string ODataType { get; set; }

            public string CostCenterODataType { get; set; }

            public List<string> CostCenter { get; set; }

            public string Department { get; set; }

            public string RolesForVendorAndExpenseMgt { get; set; }
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


    }
}
