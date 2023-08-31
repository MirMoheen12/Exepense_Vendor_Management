using Exepense_Vendor_Management.Interfaces;
using System.Security.Claims;

namespace Exepense_Vendor_Management.Repositories
{
    public class UserRepo: IUser
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
      
        public UserRepo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
         
        }
        public string ActiveUserId()
        {
        
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
