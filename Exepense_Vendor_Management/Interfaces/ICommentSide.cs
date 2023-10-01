using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Interfaces
{
    public interface ICommentSide
    {
        public bool AddComments(CommentsSection comment);
        public List<CommentsSection> AllComments(int id);
    }
}
