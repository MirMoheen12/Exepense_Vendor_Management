using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using Expense_Vendor_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exepense_Vendor_Management.Repositories
{
    public class CommentsRepo:ICommentSide
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        public CommentsRepo(AppDbContext context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {

            this._context = context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;

        }

        public bool AddComments(CommentsSection comment)
        {
            try
            {
                comment.CommentsDate = DateTime.Now;
                _context.CommentsSection.Add(comment);
                _context.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }


        }

        public List<CommentsSection> AllComments(int id)
        {
            List<CommentsSection> comlist = new List<CommentsSection>();

            var data = _context.CommentsSection.Where(x => x.ID == id).OrderByDescending(x => x.CommentsDate).ToList();
            foreach (var comment in data)
            {
                var dat = getUserid(comment.Commentsby);
                comment.Commentsby = dat.Result;
                comlist.Add(comment);

            }
            return comlist;
        }
        private async Task<string> getUserid(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return user.UserName;
            }
            else
            {
                return "Finance";
            }
        }
    }
}
