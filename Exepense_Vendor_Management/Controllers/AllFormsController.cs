using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class AllFormsController : Controller
    {
        private readonly IVendor vendor;
        public AllFormsController(IVendor vendor)
        {
            this.vendor = vendor;
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
        [HttpGet]
        public IActionResult ExpenseForm()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ExpenseForm(EmployeeExpense e)
        {
           
            return View();
        }
    }
}
