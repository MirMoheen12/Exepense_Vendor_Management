using Expense_Vendor_Management.Models;

namespace Expense_Vendor_Management.Interfaces
{
    public interface IMedia
    {
        public List<Media> getAllMediaByID(int id, string belongTo);
        public int AddMedia(Media medias, string ReqID);
    }

}
