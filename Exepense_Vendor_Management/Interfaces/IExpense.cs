using Expense_Vendor_Management.Models;

namespace Expense_Vendor_Management.Interfaces
{
    public interface IExpense
    {
        public bool AddNewExpense(EmployeeExpense expense);
        public List<EmployeeExpense> GetAllExpenses();
        public EmployeeExpense GetExpById(int id);
        public bool ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile? file);
        public void EditExpense(EmployeeExpense e);
    }
}
