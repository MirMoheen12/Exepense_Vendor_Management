using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Interfaces
{
    public interface IExpense
    {
        public bool AddNewExpense(EmployeeExpense expense);
    }
}
