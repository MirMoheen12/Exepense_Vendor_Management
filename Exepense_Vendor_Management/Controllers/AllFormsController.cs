using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Vendor_Management.Controllers
{
    [Authorize(Roles ="Super Admin")]
    public class AllFormsController : Controller
    {
        private readonly IVendor vendor;
        private readonly IExpense expense;
        private readonly ICostExp costExp;
        public AllFormsController(IVendor vendor, IExpense expense, ICostExp costExp)
        {
            this.vendor = vendor;
            this.expense = expense;
            this.costExp = costExp;
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
            TempData["SuccessMessage"] = "Form submitted successfully!";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult ExpenseForm()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ExpenseForm(EmployeeExpense e)
        {
            var res = expense.AddNewExpense(e);
            TempData["SuccessMessage"] = "Form submitted successfully!";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult CostExpenseForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CostExpenseForm(CostCenterExpense ce)
        {
            var res = costExp.AddNewCostExp(ce);
            TempData["SuccessMessage"] = "Form submitted successfully!";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult submittedForm()
        {
            return View();
        }





        [HttpGet]
        public IActionResult EditCostExpenseForm(int id)
        {
            var data = costExp.GetCostById(id);
            return View(data);
        }
        [HttpPost]
        public IActionResult EditCostExpenseForm(CostCenterExpense ce)
        {
            costExp.EditCostExp(ce);
            TempData["SuccessMessage"] = "Form Edited successfully!";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult EditVendorForm(int id)
        {
            var data = vendor.GetVendorById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult EditVendorForm(Vendor v)
        {
            vendor.EditVendor(v);
            TempData["SuccessMessage"] = "Form Edited successfully!";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult EditExpenseForm(int id)
        {
            var data = expense.GetExpById(id);
            return View(data);
        }
        [HttpPost]
        public IActionResult EditExpenseForm(EmployeeExpense e)
        {
            expense.EditExpense(e);
            TempData["SuccessMessage"] = "Form Edited successfully!";
            return RedirectToAction("Index", "Home");
        }



    }
}
