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
            if (v.catagory == "Other")
            {
                v.catagory = othercat;
            }
            if (v.poductType == "Other")
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
        public async Task<IActionResult> ExpenseForm(EmployeeExpense e, string newform, string Othercat, string[] ecatagory)
        {
            var yearly = expense.monthlylimt();
            var monthly = expense.monthlylimt();
            if (yearly != null)
            {
                if (yearly >= 36000)
                {
                    ViewBag.year = "Yearly Limit exceeded";
                    return View();
                }
            }
            if (monthly != null)
            {
                if (monthly >= 3000)
                {
                    ViewBag.year = "Monthly Limit exceeded";
                    return View();
                }
            }
            string ecat = "";
            for (int i = 0; i < ecatagory.Count(); i++)
            {
                ecat = ecat + ecatagory[i];
                if (i != ecatagory.Count() - 1)
                {
                    ecat = ecat + ",";
                }
            }
            if (Othercat != null)
            {
                ecat = ecat + "," + Othercat;
            }
            e.expenseCategory = ecat;
            e.expenseOccurred = DateTime.Now;
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
        public async Task<IActionResult> CostExpenseForm(CostCenterExpense ce, string newform,string Othercat, string[] ecatagory)
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
            if (Othercat != null)
            {
                ecat = ecat + "," + Othercat;
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
        public IActionResult EditCostExpenseForm(CostCenterExpense ce, string[] ecatagory,string othercat)
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
            if (othercat != null)
            {
                ecat = ecat + "," + othercat;
            }
            ce.expenseCategory = ecat;
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
        public IActionResult EditExpenseForm(EmployeeExpense e, string[] ecatagory, string othercat)
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
            if (othercat != null)
            {
                ecat = ecat + "," + othercat;
            }
            e.expenseCategory = ecat;
            expense.EditExpense(e);
            TempData["SuccessMessage"] = "Form Edited successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
