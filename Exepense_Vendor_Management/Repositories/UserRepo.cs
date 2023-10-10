using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using Expense_Vendor_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Kiota.Abstractions;
using System.Security.Claims;

namespace Exepense_Vendor_Management.Repositories
{
    public class UserRepo: IUser
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext _context,IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> _userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            this._userManager = _userManager;
            this._context= _context;
        }
        public async Task<string> GetUserName(string userid)
        {
            try
            {
                //return "Not Found";
                var user = await _userManager.FindByIdAsync(userid);
                return user.DisplayName;
            }
            catch (Exception)
            {

                return "Not Found";
            }
           
        }
        public async Task<string> ActiveUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = await _userManager.FindByIdAsync(userId);
            return res.Id;
        }
        public async Task<IdentityUser> getUser(string id)
        {
            var dt = await _userManager.FindByIdAsync(id);
            return dt;
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
        public List<CostCenter> CostCentersbyid(string id)
        {
            return _context.costCenter.Where(x => x.Userid == id).ToList();
        }
        public bool AddCostCenter(string id, string userid)
        {
            try
            {
                CostCenter costCenter = new CostCenter();
                costCenter.CostCenterID = id;
                costCenter.Userid = userid;
                costCenter.IsDelete = false;
                _context.costCenter.Add(costCenter);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteCostCenter(string id, string userid)
        {
            try
            {
                var data = _context.costCenter.Where(x => x.Userid == userid && x.CostCenterID == id).FirstOrDefault();
                if (data != null)
                {
                    data.IsDelete = true;
                    _context.costCenter.Update(data);
                    _context.SaveChanges();
                }
                   
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
