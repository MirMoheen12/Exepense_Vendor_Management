namespace Exepense_Vendor_Management.Interfaces
{
    public interface IUser
    {
        public string ActiveUserId();
        public Task<IEnumerable<string>> GetUserRolesAsync(string id);
        public Task<string> GetUserName(string userid);
      
    }
}
