using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class VendorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddVendor()
        {
            return View();
        }


        [HttpPost]
        public IActionResult  AddVendor(Vendor v)
        {
            return View();
        }
    }
}
