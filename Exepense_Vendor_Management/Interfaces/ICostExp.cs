
using Exepense_Vendor_Management.Models;

namespace Exepense_Vendor_Management.Interfaces
{
    public interface ICostExp
    {
        public bool AddNewCostExp(CostCenterExpense ce);
        public List<CostCenterExpense> GetAllCost();
        public CostCenterExpense GetCostById(int vendorId);
        public bool ChangeCostAction(int ID, string Remarks, string Fstatus, IFormFile? file);
    }
}
