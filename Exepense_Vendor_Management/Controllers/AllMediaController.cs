using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    [AllowAnonymous]
    public class AllMediaController : Controller
    {
        public IActionResult MediaByid(int id,string reqtype)
        {
            return View();
        }
    }
}
