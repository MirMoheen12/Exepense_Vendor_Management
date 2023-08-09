using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class CostCenterExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet] 
        public IActionResult AddCostCenterExpense()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddCostCenterExpense(CostCenterExpense centerExpense)
        {
            return View();
        }
    }
}
