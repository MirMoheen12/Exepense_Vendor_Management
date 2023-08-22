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
        public IActionResult AllVendorForms()
        {
            return View(ivend.GetActiveVendorsForms());
        }
        [HttpGet]
        public IActionResult VendorActionCenter(int ID)
        {
            var dt=ivend.GetVendorById(ID);
            return View(dt);
        }
        [HttpPost]
        public IActionResult VendorActionCenter(int ID,string Remarks,string Fstatus,IFormFile? file)
        {
            var dt = ivend.ChangeVendorAction(ID,Remarks,Fstatus,file);
            return View(dt);
        }
    }
}
