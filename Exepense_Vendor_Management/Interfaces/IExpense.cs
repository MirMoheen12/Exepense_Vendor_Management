using Expense_Vendor_Management.Models;
using static Expense_Vendor_Management.Repositories.ExpenseRepo;

namespace Expense_Vendor_Management.Interfaces
{
    public interface IExpense
    {
        public Task<bool> AddNewExpense(EmployeeExpense expense);
        public List<EmployeeExpense> GetAllExpenses();
        public Task<bool> PostExpenseAsync(ExpenseData expense);
        public EmployeeExpense GetExpById(int id);
        public decimal monthlylimt();
        public decimal yearlylimt();
        public Task<bool> ChangeExpenseAction(int ID, string Remarks, string Fstatus, IFormFile[]? file,float Amount);
        public void EditExpense(EmployeeExpense e);
    }
}
