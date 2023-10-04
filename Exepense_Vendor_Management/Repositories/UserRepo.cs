using Exepense_Vendor_Management.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Exepense_Vendor_Management.Repositories
{
    public class UserRepo: IUser
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> UserManager;

        public UserRepo(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> UserManager)
        {
            _httpContextAccessor = httpContextAccessor;
            UserManager = UserManager;
        }
        public async Task<string> GetUserName(string userid)
        {
            var user = await UserManager.FindByIdAsync(userid);
            return user.UserName;
        }
        public string ActiveUserId()
        {
        
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<IEnumerable<string>> GetUserRolesAsync(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                // Handle the case where the user doesn't exist
                return null;
            }

            var userRoles = await UserManager.GetRolesAsync(user);

            return userRoles;
        }

    }
}
