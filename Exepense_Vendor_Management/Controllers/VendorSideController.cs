using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class VendorSideController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendorForm()
        {
            return View();
        }


        [HttpPost]
        public IActionResult VendorForm(Vendor v)
        {
            return View();
        }
    }
}
