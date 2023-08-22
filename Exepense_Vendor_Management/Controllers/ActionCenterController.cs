using Exepense_Vendor_Management.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class ActionCenterController : Controller
    {
        private readonly IVendor ivend;
        public ActionCenterController(IVendor ivend)
        {
            this.ivend = ivend;
        }
        public IActionResult AllVendorForms(int ID)
        {
            return View(ivend.GetActiveVendorsForms());
        }
    }
}
