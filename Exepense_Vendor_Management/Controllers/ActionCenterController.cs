using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class ActionCenterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
