using Exepense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Vendor_Management.Controllers
{
    public class ActionCenterController : Controller
    {
        private readonly ICommentSide commentSide;
        private readonly IVendor ivend;
        private readonly IExpense ex;
        private readonly ICostExp costExp;
        private readonly IMedia media;
        private readonly IUser user;
        public ActionCenterController(IVendor ivend, IExpense ex,ICostExp costExp, ICommentSide commentSide, IMedia media, IUser user)
        {
            this.ivend = ivend;
            this.user=user;
            this.ex = ex;
            this.costExp = costExp;
            this.commentSide = commentSide;
            this.media = media;
        }
        public IActionResult AllVendorForms()
        {
            return View(ivend.GetActiveVendorsForms());
        }
        [HttpGet]
        public IActionResult VendorActionCenter(int ID)
        {
            var dt=ivend.GetVendorById(ID);
            ViewBag.media = media.getAllMediaByID(ID, "Vendor");
            return View(dt);
        }
        [HttpPost]
        public IActionResult VendorActionCenter(int ID,string Remarks,string Fstatus,IFormFile? file, string[] RNotfication,string criticalVendor)
        {
            string Notfi = "";
            for (int i = 0; i < RNotfication.Count(); i++)
            {
                Notfi = Notfi + RNotfication[i];
                if (i != RNotfication.Count()-1)
                {
                    Notfi = Notfi + ",";
                }
            }
            var dt = ivend.ChangeVendorAction(ID,Remarks,Fstatus,file, Notfi, criticalVendor);
            return RedirectToAction("AllVendorForms");
        }

        public IActionResult AllExpenseForms()
        {

            ViewBag.Name = user.GetUserName(user.ActiveUserId().Result).Result.Split(" ").FirstOrDefault();
            return View(ex.GetAllExpenses());
        }
        [HttpGet]
        public IActionResult ExpenseactionCenter(int ID)
        {
            ViewBag.media = media.getAllMediaByID(ID, "Expense");
            var cmt = commentSide.AllComments(ID);
            ViewBag.cmt = cmt;
            var dt = ex.GetExpById(ID);
            return View(dt);
        }
        [HttpPost]
        public async Task<IActionResult> ExpenseactionCenter(int ID, string Remarks, float Amount, string Fstatus, IFormFile? file)
        {
            var dt = await ex.ChangeExpenseAction(ID, Remarks, Fstatus, file,Amount);
            return RedirectToAction("AllExpenseForms");
        }






        public IActionResult AllCostExpense()
        {
            return View(costExp.GetAllCost());
        }
        [HttpGet]
        public IActionResult CostactionCenter(int ID)
        {
            ViewBag.media = media.getAllMediaByID(ID, "Cost");
            var cmt = commentSide.AllComments(ID);
            ViewBag.cmt = cmt;
            var dt = costExp.GetCostById(ID);
            return View(dt);
        }
        [HttpPost]
        public async Task<IActionResult> CostactionCenter(int ID, string Remarks, float Amount, string Fstatus, IFormFile? file)
        {
            var dt = await costExp.ChangeCostAction(ID, Remarks, Fstatus, file,Amount);
            return RedirectToAction("AllCostExpense");
        }

    }
}
