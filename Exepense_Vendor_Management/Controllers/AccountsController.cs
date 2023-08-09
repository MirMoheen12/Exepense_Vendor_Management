using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Controllers
{
    public class AccountsController : Controller
    {

        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> UserManager;
        public AccountsController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {

            this.signInManager = signInManager;
            this.UserManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult AddnewUser()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> AddnewUser(AllUser allUser)
        //{
        //    var user = new IdentityUser { UserName = allUser.UserName, Email = allUser.UserEmail, PhoneNumber = allUser.ContactNo, PasswordHash = allUser.UserPass };

        //    await UserManager.CreateAsync(user);
        //    await signInManager.SignInAsync(user, isPersistent: false);
        //    var dt = accountside.AddnewUser(allUser);
        //    return View();
        //}
        public async Task<IActionResult> Login(string returnUrl)
        {
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
            var signinresult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signinresult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
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
                    return LocalRedirect(RetunrUrl);
                }
                ViewBag.ErrorTtle = $"Email not Found:{info.LoginProvider}";
                return View("Error");

            }

        }


    }
}
