using Expense_Vendor_Management.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    [AllowAnonymous]
    public class AllMediaController : Controller
    {
        private readonly IMedia media;
        public AllMediaController(IMedia media)
        {
            this.media = media;
        }
        public IActionResult MediaByid(int id, string reqtype)
        {
            return View(media.getAllMediaByID(id, reqtype));
        }
    }
}
