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
        private readonly IUser user;

        public CommentssideController(ICommentSide commentSide,IUser user)
        {
            this.commentSide = commentSide;
            this.user = user;
        
        }
        public IActionResult Index()
        {
            var data = commentSide.AllComments(57);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddComments(CommentsSection _commentsSection)
        {
         
            _commentsSection.Commentsby = user.ActiveUserId().Result;
            var data = commentSide.AddComments(_commentsSection);
            if (_commentsSection.Comfor == "Expense")
            {
                return RedirectToAction("ExpenseactionCenter", "ActionCenter", new { ID = _commentsSection.ID });
            }
            else if((_commentsSection.Comfor == "Cost"))
                {
                    return RedirectToAction("CostactionCenter", "ActionCenter", new { ID = _commentsSection.ID });
                }
            return View();

        }
    }
}
