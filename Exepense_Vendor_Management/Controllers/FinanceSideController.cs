using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class FinanceSideController : Controller
    {
        public IActionResult AllExpenses()
        {
            return View();
        }
        public IActionResult AllVendors()
        {
            return View();
        }
    }
}
