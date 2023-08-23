using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Interfaces
{
    public interface IExpense
    {
        public bool AddNewExpense(EmployeeExpense expense);
        public List<EmployeeExpense> GetAllExpenses();
        public EmployeeExpense GetExpById(int id);
        public bool ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile? file);
    }
}
