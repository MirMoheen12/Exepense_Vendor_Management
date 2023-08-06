using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Exepense_Vendor_Management.Controllers
{
    [AllowAnonymous]
    public class AccountsController : Controller
    {
      
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> UserManager;
        public AccountsController( SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
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
        //public async Task<IActionResult> AddnewUser(AllUser allUser, ValidationResponse recaptchaResponse)
        //{
        //    var user = new ApplicationUser { UserName = allUser.UserName, Email = allUser.UserEmail, Purpose = "NormalUser", PhoneNumber = allUser.ContactNo, PasswordHash = allUser.UserPass };

        //    await UserManager.CreateAsync(user);
        //    await signInManager.SignInAsync(user, isPersistent: false);
        //    var dt = accountside.AddnewUser(allUser);
        //    return View();
        //}
   
        public async Task<IActionResult> Login(string returnUrl="sfd")
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
            var redirectUri = Url.Action("ExternalLoginCallBack", "AccountsSide", new { RetunrUrl = returnUrl });
            var prop = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            return new ChallengeResult(provider, prop);
        }

        public async Task<IActionResult> ExternalLoginCallBack(string RetunrUrl = null, string remoteError = null)
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
        //public async Task GoogleLogin()
        //{
        //    await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        //    {

        //        RedirectUri = Url.Action("GoogleResponse")
        //    });

        //}

        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //        {
        //            claim.Issuer,
        //            claim.OriginalIssuer,
        //            claim.Type,
        //            claim.Value
        //        });
        //    return Json(claims);
        //}
        //public IActionResult FacebookLogin()
        //{
        //    var prop = new AuthenticationProperties
        //    {
        //        RedirectUri = Url.Action("FacebookResponse")
        //    };
        //    return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        //}
        //public async Task<IActionResult> FacebookResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //    {
        //        claim.Issuer,
        //        claim.OriginalIssuer,
        //        claim.Type,
        //        claim.Value
        //    });
        //    return Json(claims);
        //}
        //public IActionResult LinkedLogin()
        //{
        //             //  return Challenge(LinkedResponse);
        //    return Challenge(LinkedInAuthenticationDefaults.AuthenticationScheme);
        //}
        //public async Task<IActionResult> LinkedResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //    {
        //        claim.Issuer,
        //        claim.OriginalIssuer,
        //        claim.Type,
        //        claim.Value
        //    });
        //    return Json(claims);
        //}

    }
}
