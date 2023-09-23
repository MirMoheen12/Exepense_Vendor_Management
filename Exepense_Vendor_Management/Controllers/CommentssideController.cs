using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Exepense_Vendor_Management.Controllers
{
    public class CommentssideController : Controller
    {
        private readonly ICommentSide commentSide;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CommentssideController(ICommentSide commentSide, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> roleManager)
        {
            this.commentSide = commentSide;
            this._userManager = _userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var data = commentSide.AllComments(57);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddComments(CommentsSection _commentsSection)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var re = await _userManager.FindByNameAsync(claimsIdentity.Name);
            _commentsSection.Commentsby = re.Id;
            var data = commentSide.AddComments(_commentsSection);
            return RedirectToAction("ExpenseactionCenter", "ActionCenter", new {ID=_commentsSection.ID });

        }
    }
}
