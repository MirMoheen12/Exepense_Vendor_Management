using Exepense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Interfaces;
using Expense_Vendor_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense_Vendor_Management.Repositories
{
    public class ExpenseRepo : IExpense
    {
        private readonly IMedia media;
        private readonly AppDbContext appContext;
        private readonly IUser user;
        public ExpenseRepo(IMedia media, AppDbContext appContext, IUser user)
        {
            this.user = user;   
            this.appContext = appContext;
            this.media = media;

        }
        
        public bool AddNewExpense(EmployeeExpense ex)
        {
            try
            {
                ex.isDeleted = false;
                ex.createdOn = DateTime.Now;
                ex.createdBy = user.ActiveUserId();
                ex.modifiedBy = user.ActiveUserId();
                ex.status = "Submitted";
                appContext.EmployeeExpense.Add(ex);
                appContext.SaveChanges();
                if (ex.SuportingMedia != null)
                {
                    Media m = new Media();
                    m.mediaFile = ex.SuportingMedia;
                    m.mediaType = "Add Expense";
                    m.belongTo = "Expense";
                    media.AddMedia(m,ex.id.ToString());
                }
                return true;
            }

            catch (Exception)
            {
                return false;
  
            }
  
        }

        public List<EmployeeExpense> GetAllExpenses()
        {
            var data = appContext.EmployeeExpense.ToList();
            return data;
        }

        public EmployeeExpense GetExpById(int id)
        {
            var data = appContext.EmployeeExpense.Where(x => x.id == id).FirstOrDefault();
            return data;
        }
        public bool ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile? file)
        {
            try
            {
                var data = appContext.EmployeeExpense.Where(x => x.id == ID).FirstOrDefault();
                data.status = Fstatus;
                data.modifiedBy = "SAdmin/Finance";
                data.notes = Remarks;
                appContext.EmployeeExpense.Update(data);
                appContext.SaveChanges();
                if (file != null)
                {
                    Media m = new Media();
                    m.mediaFile = file;
                    m.mediaType = "Approve";
                    m.createdBy = "";
                    m.belongTo = "Expense";
                    media.AddMedia(m, ID.ToString());
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void EditExpense(EmployeeExpense e)
        {
            appContext.EmployeeExpense.Update(e); 
            appContext.SaveChanges();
        }
    }
}
