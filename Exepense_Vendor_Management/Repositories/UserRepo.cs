using Exepense_Vendor_Management.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Kiota.Abstractions;
using System.Security.Claims;

namespace Exepense_Vendor_Management.Repositories
{
    public class UserRepo: IUser
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepo(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> _userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            this._userManager = _userManager;
        }
        public async Task<string> GetUserName(string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            return user.UserName;
        }
        public string ActiveUserId()
        {
        
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<IEnumerable<string>> GetUserRolesAsync(string id)
        {
            var Userinfo = await _userManager.FindByIdAsync(id);

            if (Userinfo == null)
            {
                // Handle the case where the user doesn't exist
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(Userinfo);

            return userRoles;
        }

    }
}
