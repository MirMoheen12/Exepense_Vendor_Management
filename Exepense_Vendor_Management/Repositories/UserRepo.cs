using Exepense_Vendor_Management.Interfaces;
using System.Security.Claims;

namespace Exepense_Vendor_Management.Repositories
{
    public class UserRepo : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogs logs;

        public UserRepo(IHttpContextAccessor httpContextAccessor, ILogs logs)
        {
            _httpContextAccessor = httpContextAccessor;
            this.logs = logs;
        }

        public string ActiveUserId()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                logs.AddLog("ActiveUserId" + "Retrieved active user ID.");
                return userId;
            }
            catch (Exception ex)
            {
                logs.ErrorLog($"Error getting active user ID: {ex.Message}", "ActiveUserId");
                return string.Empty;
            }
        }
    }
}
