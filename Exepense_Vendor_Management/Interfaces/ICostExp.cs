
using Expense_Vendor_Management.Models;

namespace Expense_Vendor_Management.Interfaces
{
    public interface ICostExp
    {
        public Task<bool> AddNewCostExp(CostCenterExpense ce);
        public List<CostCenterExpense> GetAllCost();
        public CostCenterExpense GetCostById(int vendorId);
        public Task<bool> ChangeCostAction(int ID, string Remarks, string Fstatus, IFormFile[]? file,float Amount);
        public void EditCostExp(CostCenterExpense ce);
    }
}
