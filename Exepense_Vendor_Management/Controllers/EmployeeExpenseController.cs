using Exepense_Vendor_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exepense_Vendor_Management.Controllers
{
    public class EmployeeExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddEmployeeExpense()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddEmployeeExpense(EmployeeExpense centerExpense)
        {
            return View();
        }
    }
}
