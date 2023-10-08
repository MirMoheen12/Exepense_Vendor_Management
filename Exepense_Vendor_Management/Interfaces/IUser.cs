using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Interfaces
{
    public interface IUser
    {
        public Task<string> ActiveUserId();
        public Task<IEnumerable<string>> GetUserRolesAsync(string id);
        public Task<string> GetUserName(string userid);
        public List<CostCenter> CostCentersbyid(string id);
        public bool AddCostCenter(string id, string userid);


    }
}
