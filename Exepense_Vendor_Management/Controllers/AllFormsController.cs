using Exepense_Vendor_Management.Interfaces;
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
        private readonly IUser _user;
        public AllFormsController(IVendor vendor, IExpense expense, ICostExp costExp, IMedia media, IUser _user)
        {
            this.vendor = vendor;
            this.expense = expense;
            this.costExp = costExp;
            this.media = media; 
            this._user = _user;
        }

        [HttpGet]
        public IActionResult VendorForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VendorForm(Vendor v,string newform,string otherserv,string othercat)
        {
            if (v.catagory == "Others")
            {
                v.catagory = othercat;
            }
            if (v.poductType == "Others")
            {
                v.poductType = otherserv;
            }
            await vendor.AddNewVendor(v);
            TempData["SuccessMessage"] = "Form submitted successfully!";
            if (newform == "New Val")
            {
                return RedirectToAction("VendorForm", "AllForms");
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult ExpenseForm()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ExpenseForm(EmployeeExpense e, string newform)
        {
            var res = expense.AddNewExpense(e);
            TempData["SuccessMessage"] = "Form submitted successfully!";
            if (newform == "New Val")
            {
                return RedirectToAction("ExpenseForm", "AllForms");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult CostExpenseForm()
        {
            ViewBag.costexpense = _user.CostCentersbyid(_user.ActiveUserId().Result);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CostExpenseForm(CostCenterExpense ce, string newform, string[] ecatagory)
        {
            string ecat = "";
            for (int i = 0; i < ecatagory.Count(); i++)
            {
                ecat = ecat + ecatagory[i];
                if (i != ecatagory.Count() - 1)
                {
                    ecat = ecat + ",";
                }
            }
            ce.expenseCategory = ecat;
            ce.submissionDate = DateTime.Now;
            var res = await costExp.AddNewCostExp(ce);
            TempData["SuccessMessage"] = "Form submitted successfully!";
            if (newform == "New Val")
            {
                return RedirectToAction("CostExpenseForm", "AllForms");
            }
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
            ViewBag.costexpense = _user.CostCentersbyid(_user.ActiveUserId().Result);
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
