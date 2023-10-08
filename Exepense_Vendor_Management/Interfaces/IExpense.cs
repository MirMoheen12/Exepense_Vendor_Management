using Expense_Vendor_Management.Models;

namespace Expense_Vendor_Management.Interfaces
{
    public interface IExpense
    {
        public Task<bool> AddNewExpense(EmployeeExpense expense);
        public List<EmployeeExpense> GetAllExpenses();
        public EmployeeExpense GetExpById(int id);
        public Task<bool> ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile? file,float Amount);
        public void EditExpense(EmployeeExpense e);
    }
}
