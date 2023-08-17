using Exepense_Vendor_Management.Interfaces;
using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Repositories
{
    public class ExpenseRepo : IExpense
    {
        private readonly IMedia media;
        private readonly AppDbContext appContext;
        public ExpenseRepo(IMedia media, AppDbContext appContext)
        {
            this.appContext = appContext;
            this.media = media;

        }
        
        public bool AddNewExpense(EmployeeExpense ex)
        {
            try
            {
                ex.isDeleted = false;
                ex.createdOn = DateTime.Now;
                ex.createdBy = "Mir";
                ex.supportingDocid = -1;
                if(ex.SuportingMedia!=null)
                {
                    Media m = new Media();
                    m.mediaFile = ex.SuportingMedia;
                    m.mediaType = "Add Expense";
                    ex.supportingDocid = media.AddMedia(m);
                }
                appContext.EmployeeExpense.Add(ex);
                appContext.SaveChanges();
                return true;
            }

            catch (Exception)
            {
                return false;
  
            }
  
        }
    }
}
