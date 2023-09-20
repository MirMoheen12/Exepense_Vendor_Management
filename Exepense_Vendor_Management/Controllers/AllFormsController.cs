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
        private readonly IMedia media;
        public AllFormsController(IVendor vendor, IExpense expense, ICostExp costExp, IMedia media)
        {
            this.vendor = vendor;
            this.expense = expense;
            this.costExp = costExp;
            this.media = media; 
        }

        [HttpGet]
        public IActionResult VendorForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VendorForm(Vendor v)
        {
            await vendor.AddNewVendor(v);
            TempData["SuccessMessage"] = "Form submitted successfully!";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult ExpenseForm()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ExpenseForm(EmployeeExpense e)
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
        public async Task<IActionResult> CostExpenseForm(CostCenterExpense ce)
        {
            ce.submissionDate = DateTime.Now;
            var res = await costExp.AddNewCostExp(ce);
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
            ViewBag.media = media.getAllMediaByID(id,"Cost");
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
            ViewBag.media = media.getAllMediaByID(id, "Vendor");
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
            ViewBag.media = media.getAllMediaByID(id, "Expense");
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
