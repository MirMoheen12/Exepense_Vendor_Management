using Expense_Vendor_Management.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Vendor_Management.Controllers
{
    public class ActionCenterController : Controller
    {
        private readonly IVendor ivend;
        private readonly IExpense ex;
        private readonly ICostExp costExp;
        public ActionCenterController(IVendor ivend, IExpense ex,ICostExp costExp)
        {
            this.ivend = ivend;
            this.ex = ex;
            this.costExp = costExp;

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
            return RedirectToAction("AllVendorForms");
        }

        public IActionResult AllExpenseForms()
        {
            return View(ex.GetAllExpenses());
        }
        [HttpGet]
        public IActionResult ExpenseactionCenter(int ID)
        {
            var dt = ex.GetExpById(ID);
            return View(dt);
        }
        [HttpPost]
        public IActionResult ExpenseactionCenter(int ID, string Remarks, string Fstatus, IFormFile? file)
        {
            var dt = ex.ChangeExpenseAction(ID, Remarks, Fstatus, file);
            return RedirectToAction("AllExpenseForms");
        }






        public IActionResult AllCostExpense()
        {
            return View(costExp.GetAllCost());
        }
        [HttpGet]
        public IActionResult CostactionCenter(int ID)
        {
            var dt = costExp.GetCostById(ID);
            return View(dt);
        }
        [HttpPost]
        public IActionResult CostactionCenter(int ID, string Remarks, string Fstatus, IFormFile? file)
        {
            var dt = costExp.ChangeCostAction(ID, Remarks, Fstatus, file);
            return RedirectToAction("AllCostExpense");
        }

    }
}
