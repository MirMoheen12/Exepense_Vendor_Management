using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class VendorSideController : Controller
    {
        private readonly IVendor vendor;
        public VendorSideController(IVendor vendor)
        {
            this.vendor = vendor;
        }
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
            vendor.AddNewVendor(v);
            return View();
        }
    }
}
