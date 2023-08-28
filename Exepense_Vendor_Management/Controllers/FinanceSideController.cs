using Microsoft.AspNetCore.Mvc;

namespace Expense_Vendor_Management.Controllers
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
